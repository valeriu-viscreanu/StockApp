import { useState, useEffect, useCallback } from 'react';
import Dashboard from './components/Dashboard';
import Trade from './components/Trade';
import Orders from './components/Orders';
import Cash from './components/Cash';
import Login from './components/Login';
import Register from './components/Register';
import Navbar from './components/Navbar';
import * as api from './services/api';
import { useAuth } from './context/AuthContext';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';

const POPULAR_STOCKS = [
  { symbol: "MSFT", name: "Microsoft" },
  { symbol: "AAPL", name: "Apple" },
  { symbol: "GOOGL", name: "Alphabet" },
  { symbol: "AMZN", name: "Amazon" },
  { symbol: "NVDA", name: "Nvidia" },
  { symbol: "META", name: "Meta Platforms" },
  { symbol: "TSLA", name: "Tesla" },
  { symbol: "AMD", name: "Advanced Micro Devices" },
  { symbol: "JPM", name: "JPMorgan Chase" },
  { symbol: "V", name: "Visa" },
];

function App() {
  const { isLoggedIn, token, user, setUser, handleLogout } = useAuth();
  const [theme, setTheme] = useState(localStorage.getItem('theme') || 'light');
  const [showRegister, setShowRegister] = useState(false);

  const [amount, setAmount] = useState(100);
  const [quantity, setQuantity] = useState(1);
  const [popularStocks, setPopularStocks] = useState(POPULAR_STOCKS.map(s => ({ ...s, price: 0 })));
  const [selectedStock, setSelectedStock] = useState({ symbol: 'AAPL', name: 'Apple', price: 0 });
  const [data, setData] = useState({ balance: 0.00, stocks: 0, totalValue: 0 });
  const [buyOrders, setBuyOrders] = useState([]);
  const [sellOrders, setSellOrders] = useState([]);
  const [tradeMessage, setTradeMessage] = useState('');
  const [stockChartData, setStockChartData] = useState(null);

  // Theme setup
  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }, [theme]);

  // Data Refresh Helpers
  const refreshUserData = useCallback(async () => {
    if (!token) return;

    try {
      const balance = await api.fetchBalance(token, handleLogout);
      if (balance !== null) setData(prev => ({ ...prev, balance }));

      const ordersData = await api.fetchOrders(token, handleLogout);
      if (ordersData) {
        const mapOrder = (o) => ({
          id: o.buyOrderID || o.sellOrderID,
          stockName: o.stockName,
          stockSymbol: o.stockSymbol,
          quantity: o.quantity,
          price: o.price,
          tradeAmount: o.tradeAmount,
          date: new Date(o.dateAndTimeOfOrder).toLocaleString()
        });
        const bOrders = (ordersData.buyOrders || []).map(mapOrder);
        const sOrders = (ordersData.sellOrders || []).map(mapOrder);
        setBuyOrders(bOrders);
        setSellOrders(sOrders);

        const totalBought = bOrders.reduce((sum, o) => sum + o.quantity, 0);
        const totalSold = sOrders.reduce((sum, o) => sum + o.quantity, 0);
        const totalBuyValue = bOrders.reduce((sum, o) => sum + o.tradeAmount, 0);
        const totalSellValue = sOrders.reduce((sum, o) => sum + o.tradeAmount, 0);
        
        setData(prev => ({
          ...prev,
          stocks: totalBought - totalSold,
          totalValue: (balance ?? prev.balance) + (totalBuyValue - totalSellValue)
        }));
      }

      const quotes = await api.fetchStockPrices(POPULAR_STOCKS.map(s => s.symbol), token, handleLogout);
      if (quotes) {
        setPopularStocks(prev => prev.map(stock => ({
          ...stock,
          price: quotes[stock.symbol]?.c || stock.price
        })));
        setSelectedStock(prev => ({
          ...prev,
          price: quotes[prev.symbol]?.c || prev.price
        }));
      }
    } catch (err) {
      console.error('Refresh data error:', err);
    }
  }, [token, handleLogout]);

  useEffect(() => {
    if (isLoggedIn && token) {
      refreshUserData();
      if (!user) {
        setUser('User'); // Fallback if user name not set
      }
    }
  }, [isLoggedIn, token, refreshUserData, user, setUser]);

  // Handlers
  const fetchStockChartData = async (symbol, timeframe) => {
    const data = await api.fetchStockData(symbol, timeframe, token, handleLogout);
    setStockChartData(data);
  };

  const handleSelectStock = async (stock) => {
    setSelectedStock(stock);
    const quote = await api.fetchStockQuote(stock.symbol, token, handleLogout);
    if (quote?.c) setSelectedStock(prev => ({ ...prev, price: quote.c }));
    fetchStockChartData(stock.symbol, 'month');
  };

  const executeOrder = async (type) => {
    setTradeMessage('');
    const orderData = {
      stockSymbol: selectedStock.symbol,
      stockName: selectedStock.name,
      dateAndTimeOfOrder: new Date().toISOString(),
      quantity,
      price: selectedStock.price
    };
    
    const result = await api.createOrder(type, orderData, token, handleLogout);
    if (result.data) {
      setTradeMessage(`${type === 'buy' ? 'Bought' : 'Sold'} ${quantity} share(s) of ${selectedStock.symbol}`);
      refreshUserData();
    } else {
      setTradeMessage(result.error);
    }
  };

  const handleCashAction = async (type) => {
    try {
      const result = await api.handleCashActionApi(type, amount, token, handleLogout);
      if (result) {
        setData(prev => ({ ...prev, balance: result.balance }));
        setAmount(100);
      }
    } catch (err) {
      alert(err.message);
    }
  };

  if (!isLoggedIn) {
    return showRegister ? (
      <Register 
        onSwitchToLogin={() => { setShowRegister(false); }}
      />
    ) : (
      <Login 
        onSwitchToRegister={() => { setShowRegister(true); }}
      />
    );
  }

  const totals = { buy: buyOrders.reduce((sum, o) => sum + o.tradeAmount, 0), sell: sellOrders.reduce((sum, o) => sum + o.tradeAmount, 0) };

  return (
    <Router>
      <div className="layout">
        <Navbar theme={theme} toggleTheme={() => setTheme(p => p === 'light' ? 'dark' : 'light')} />
        <Routes>
          <Route path="/" element={<Dashboard user={user} data={data} />} />
          <Route path="/dashboard" element={<Dashboard user={user} data={data} />} />
          <Route 
            path="/trade" 
            element={
              <Trade 
                popularStocks={popularStocks} 
                selectedStock={selectedStock} 
                setSelectedStock={handleSelectStock} 
                quantity={quantity} 
                setQuantity={setQuantity} 
                onBuy={() => executeOrder('buy')} 
                onSell={() => executeOrder('sell')} 
                tradeMessage={tradeMessage} 
                stockChartData={stockChartData} 
                fetchStockChartData={fetchStockChartData} 
              />
            } 
          />
          <Route path="/orders" element={<Orders buyOrders={buyOrders} sellOrders={sellOrders} totalBuyAmount={totals.buy} totalSellAmount={totals.sell} />} />
          <Route path="/cash" element={<Cash data={data} amount={amount} setAmount={setAmount} handleCashAction={handleCashAction} />} />
          <Route path="*" element={<Navigate to="/dashboard" replace />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
