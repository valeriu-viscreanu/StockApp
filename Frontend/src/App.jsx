import { useState, useEffect, useCallback, useRef } from 'react';
import Dashboard from './components/Dashboard';
import Trade from './components/Trade';
import Orders from './components/Orders';
import Cash from './components/Cash';
import Login from './components/Login';
import Register from './components/Register';
import Navbar from './components/Navbar';
import Activities from './components/Activities';
import Holdings from './components/Holdings';
import FinancialGoals from './components/FinancialGoals';
import Advisor from './components/Advisor';
import Settings from './components/Settings';
import * as api from './services/api';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from './store/slices/authSlice';
import { setPortfolioData, updateBalance } from './store/slices/portfolioSlice';
import { setPopularStocks, updateStockPrices, setSelectedStock, setStockChartData } from './store/slices/marketSlice';
import { setOrders } from './store/slices/ordersSlice';
import { Routes, Route, Navigate, useNavigate } from 'react-router-dom';


function App() {
  const dispatch = useDispatch();
  const { isLoggedIn, token, user } = useSelector(state => state.auth);
  const handleLogout = useCallback(() => dispatch(logout()), [dispatch]);
  const navigate = useNavigate();

  // Market state now lives in Redux
  const selectedStock = useSelector(state => state.market.selectedStock);
  const popularStocks = useSelector(state => state.market.popularStocks);
  const popularStocksRef = useRef(popularStocks);
  useEffect(() => { popularStocksRef.current = popularStocks; }, [popularStocks]);
  const stockChartData = useSelector(state => state.market.stockChartData);

  const [theme, setTheme] = useState(localStorage.getItem('theme') || 'light');
  const [showRegister, setShowRegister] = useState(false);
  const [amount, setAmount] = useState(100);
  const [quantity, setQuantity] = useState(1);
  const [data, setData] = useState({ balance: 0.00, stocks: 0, totalValue: 0 });
  const [tradeMessage, setTradeMessage] = useState('');
  const [userHoldings, setUserHoldings] = useState([]);

  // Theme setup
  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }, [theme]);

  // Data Refresh
  const refreshUserData = useCallback(async () => {
    if (!token) return;

    try {
      const balance = await api.fetchBalance(token, handleLogout);
      let currentHoldings = [];

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
        dispatch(setOrders({ buyOrders: bOrders, sellOrders: sOrders }));
        currentHoldings = ordersData.currentHoldings || [];
        setUserHoldings(currentHoldings);

        const totalBought = bOrders.reduce((sum, o) => sum + o.quantity, 0);
        const totalSold = sOrders.reduce((sum, o) => sum + o.quantity, 0);
        setData(prev => ({
          ...prev,
          balance: balance ?? prev.balance,
          stocks: totalBought - totalSold
        }));
      }

      // Fetch popular stocks list only once (when Redux store is empty)
      let currentStocks = popularStocksRef.current;
      if (currentStocks.length === 0) {
        const stocks = await api.fetchPopularStocks(token, handleLogout);
        if (stocks) {
          currentStocks = stocks.map(s => ({ ...s, price: 0 }));
          dispatch(setPopularStocks(currentStocks));
        }
      }

      // Fetch live prices for all relevant symbols
      const symbolsToFetch = [
        ...new Set([
          ...currentStocks.map(s => s.symbol),
          ...currentHoldings.map(h => h.stockSymbol)
        ])
      ];

      if (symbolsToFetch.length > 0) {
        const quotes = await api.fetchStockPrices(symbolsToFetch, token, handleLogout);
        if (quotes) {
          dispatch(updateStockPrices(quotes));

          // Calculate LIVE Stock Value: SUM(Price * Quantity)
          const marketValue = currentHoldings.reduce((sum, h) => {
            const price = quotes[h.stockSymbol]?.c || 0;
            return sum + (price * h.quantity);
          }, 0);

          setData(prev => {
            const currentBalance = balance ?? prev.balance;
            const liveNetWorth = currentBalance + marketValue;

            dispatch(setPortfolioData({
              holdings: currentHoldings,
              balance: currentBalance,
              totalValue: liveNetWorth,
              stockValue: marketValue,
              stocksCount: prev.stocks
            }));

            return {
              ...prev,
              totalValue: marketValue,
              balance: currentBalance
            };
          });
        }
      }
    } catch (err) {
      console.error('Refresh data error:', err);
    }
  }, [token, handleLogout, dispatch]);

  useEffect(() => {
    if (isLoggedIn && token) {
      refreshUserData();
      if (selectedStock.symbol && !stockChartData) {
        fetchStockChartData(selectedStock.symbol, 'month');
      }
    }
  }, [isLoggedIn, token, refreshUserData]);

  // Handlers
  const fetchStockChartData = useCallback(async (symbol, timeframe) => {
    const chartData = await api.fetchStockData(symbol, timeframe, token, handleLogout);
    dispatch(setStockChartData(chartData));
  }, [token, handleLogout, dispatch]);

  const handleSelectStock = useCallback(async (stock) => {
    dispatch(setSelectedStock(stock));
    const quote = await api.fetchStockQuote(stock.symbol, token, handleLogout);
    if (quote?.c) dispatch(setSelectedStock({ ...stock, price: quote.c }));
    fetchStockChartData(stock.symbol, 'month');
  }, [token, handleLogout, dispatch, fetchStockChartData]);

  const executeOrder = useCallback(async (type) => {
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
  }, [selectedStock, quantity, token, handleLogout, refreshUserData]);

  const handleCashAction = useCallback(async (type, overrideAmount) => {
    const finalAmount = overrideAmount !== undefined ? overrideAmount : amount;
    try {
      const result = await api.handleCashActionApi(type, finalAmount, token, handleLogout);
      if (result) {
        const newBalance = result.balance;
        dispatch(updateBalance(newBalance));
        setData(prev => ({ ...prev, balance: newBalance }));
        if (overrideAmount === undefined) setAmount(100);
      }
    } catch (err) {
      alert(err.message);
    }
  }, [amount, token, handleLogout, dispatch]);

  if (!isLoggedIn) {
    return showRegister ? (
      <Register onSwitchToLogin={() => { setShowRegister(false); }} />
    ) : (
      <Login onSwitchToRegister={() => { setShowRegister(true); }} />
    );
  }

  return (
    <div className="layout">
      <Navbar
        theme={theme}
        toggleTheme={() => setTheme(p => p === 'light' ? 'dark' : 'light')}
      />
      <Routes>
        <Route path="/" element={<Dashboard user={user} handleCashAction={handleCashAction} />} />
        <Route path="/dashboard" element={<Dashboard user={user} handleCashAction={handleCashAction} />} />
        <Route
          path="/trade"
          element={
            <Trade
              quantity={quantity}
              setQuantity={setQuantity}
              onBuy={() => executeOrder('buy')}
              onSell={() => executeOrder('sell')}
              tradeMessage={tradeMessage}
              fetchStockChartData={fetchStockChartData}
              handleSelectStock={handleSelectStock}
            />
          }
        />
        <Route path="/holdings" element={<Holdings />} />
        <Route path="/orders" element={<Orders />} />
        <Route path="/activities" element={<Activities />} />
        <Route path="/cash" element={<Cash handleCashAction={handleCashAction} />} />
        <Route path="/goals" element={<FinancialGoals />} />
        <Route path="/advisor" element={<Advisor />} />
        <Route path="/settings" element={<Settings theme={theme} setTheme={setTheme} token={token} logout={handleLogout} />} />
        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </div>
  );
}

export default App;
