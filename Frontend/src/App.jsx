import { useState, useEffect } from 'react';
import Dashboard from './components/Dashboard';
import Trade from './components/Trade';
import Orders from './components/Orders';
import Cash from './components/Cash';
import Login from './components/Login';

const API_BASE = 'http://localhost:5002/api/v1';

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
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));
  const [loginEmail, setLoginEmail] = useState('admin@test.com');
  const [loginPassword, setLoginPassword] = useState('123');
  const [error, setError] = useState('');
  
  const [theme, setTheme] = useState(localStorage.getItem('theme') || 'light');
  const [page, setPage] = useState('dashboard');

  useEffect(() => {
    document.documentElement.setAttribute('data-theme', theme);
    localStorage.setItem('theme', theme);
  }, [theme]);

  const toggleTheme = () => {
    setTheme(prev => prev === 'light' ? 'dark' : 'light');
  };

  const [amount, setAmount] = useState(100);
  const [quantity, setQuantity] = useState(1);
  const [popularStocks, setPopularStocks] = useState(
    POPULAR_STOCKS.map(s => ({ ...s, price: 0 }))
  );
  const [selectedStock, setSelectedStock] = useState({ symbol: 'AAPL', name: 'Apple', price: 0 });
  const [data, setData] = useState({
    balance: 0.00,
    stocks: 0,
    totalValue: 0
  });

  const [buyOrders, setBuyOrders] = useState([]);
  const [sellOrders, setSellOrders] = useState([]);
  const [tradeMessage, setTradeMessage] = useState('');

  // Auth helper for API calls
  const authHeaders = () => ({
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  });

  const handleApiError = (response) => {
    if (response.status === 401) {
      handleLogout();
      return true;
    }
    return false;
  };

  // ---- Data Fetching ----

  useEffect(() => {
    if (isLoggedIn && token) {
      fetchBalance();
      fetchOrders();
      fetchStockPrices();
      setUser(loginEmail || 'User');
    }
  }, [isLoggedIn, token]);

  const fetchBalance = async () => {
    try {
      const response = await fetch(`${API_BASE}/CashApi/balance`, {
        headers: authHeaders()
      });
      if (handleApiError(response)) return;
      if (response.ok) {
        const balance = await response.json();
        setData(prev => ({ ...prev, balance }));
      }
    } catch (err) {
      console.error('Failed to fetch balance', err);
    }
  };

  const fetchOrders = async () => {
    try {
      const response = await fetch(`${API_BASE}/TradeApi/orders`, {
        headers: authHeaders()
      });
      if (handleApiError(response)) return;
      if (response.ok) {
        const orders = await response.json();
        const mapOrder = (o) => ({
          id: o.buyOrderID || o.sellOrderID,
          stockName: o.stockName,
          stockSymbol: o.stockSymbol,
          quantity: o.quantity,
          price: o.price,
          tradeAmount: o.tradeAmount,
          date: new Date(o.dateAndTimeOfOrder).toLocaleString()
        });
        setBuyOrders((orders.buyOrders || []).map(mapOrder));
        setSellOrders((orders.sellOrders || []).map(mapOrder));

        // Update stocks held & total value from orders
        const totalBought = (orders.buyOrders || []).reduce((sum, o) => sum + o.quantity, 0);
        const totalSold = (orders.sellOrders || []).reduce((sum, o) => sum + o.quantity, 0);
        const totalBuyValue = (orders.buyOrders || []).reduce((sum, o) => sum + o.tradeAmount, 0);
        const totalSellValue = (orders.sellOrders || []).reduce((sum, o) => sum + o.tradeAmount, 0);
        setData(prev => ({
          ...prev,
          stocks: totalBought - totalSold,
          totalValue: prev.balance + (totalBuyValue - totalSellValue)
        }));
      }
    } catch (err) {
      console.error('Failed to fetch orders', err);
    }
  };

  const fetchStockPrices = async () => {
    try {
      const symbols = POPULAR_STOCKS.map(s => s.symbol);
      const params = symbols.map(s => `symbols=${s}`).join('&');
      const response = await fetch(`${API_BASE}/TradeApi/quotes?${params}`, {
        headers: authHeaders()
      });
      if (handleApiError(response)) return;
      if (response.ok) {
        const quotes = await response.json();
        setPopularStocks(prev => prev.map(stock => {
          const quote = quotes[stock.symbol];
          return quote ? { ...stock, price: quote.c || 0 } : stock;
        }));
        // Update selected stock price if available
        setSelectedStock(prev => {
          const quote = quotes[prev.symbol];
          return quote ? { ...prev, price: quote.c || 0 } : prev;
        });
      }
    } catch (err) {
      console.error('Failed to fetch stock prices', err);
    }
  };

  // When user selects a stock, fetch its latest price
  const handleSelectStock = async (stock) => {
    setSelectedStock(stock);
    try {
      const response = await fetch(`${API_BASE}/TradeApi/quote/${stock.symbol}`, {
        headers: authHeaders()
      });
      if (handleApiError(response)) return;
      if (response.ok) {
        const quote = await response.json();
        if (quote.c) {
          setSelectedStock(prev => ({ ...prev, price: quote.c }));
        }
      }
    } catch (err) {
      console.error('Failed to fetch quote', err);
    }
  };

  // ---- Trading ----

  const handleBuyOrder = async () => {
    setTradeMessage('');
    try {
      const response = await fetch(`${API_BASE}/TradeApi/buy-order`, {
        method: 'POST',
        headers: authHeaders(),
        body: JSON.stringify({
          stockSymbol: selectedStock.symbol,
          stockName: selectedStock.name,
          dateAndTimeOfOrder: new Date().toISOString(),
          quantity: quantity,
          price: selectedStock.price
        })
      });
      if (handleApiError(response)) return;
      if (response.ok) {
        setTradeMessage(`Bought ${quantity} share(s) of ${selectedStock.symbol}`);
        fetchBalance();
        fetchOrders();
      } else {
        const err = await response.json();
        setTradeMessage(err.message || 'Buy order failed');
      }
    } catch (err) {
      setTradeMessage('Buy order failed');
    }
  };

  const handleSellOrder = async () => {
    setTradeMessage('');
    try {
      const response = await fetch(`${API_BASE}/TradeApi/sell-order`, {
        method: 'POST',
        headers: authHeaders(),
        body: JSON.stringify({
          stockSymbol: selectedStock.symbol,
          stockName: selectedStock.name,
          dateAndTimeOfOrder: new Date().toISOString(),
          quantity: quantity,
          price: selectedStock.price
        })
      });
      if (handleApiError(response)) return;
      if (response.ok) {
        setTradeMessage(`Sold ${quantity} share(s) of ${selectedStock.symbol}`);
        fetchBalance();
        fetchOrders();
      } else {
        const err = await response.json();
        setTradeMessage(err.message || 'Sell order failed');
      }
    } catch (err) {
      setTradeMessage('Sell order failed');
    }
  };

  // ---- Cash ----

  const handleCashAction = async (type) => {
    try {
      const endpoint = type === 'add' ? 'add-funds' : 'withdraw';
      const response = await fetch(`${API_BASE}/CashApi/${endpoint}`, {
        method: 'POST',
        headers: authHeaders(),
        body: JSON.stringify(amount)
      });

      if (response.ok) {
        const result = await response.json();
        setData(prev => ({ ...prev, balance: result.balance }));
        setAmount(100);
      } else if (response.status === 401) {
        handleLogout();
      } else {
        const errorData = await response.json();
        alert(errorData.message || `${type} failed`);
      }
    } catch (err) {
      alert(`An error occurred during the ${type} action.`);
    }
  };

  // ---- Auth ----

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const response = await fetch(`${API_BASE.replace('/v1', '')}/Auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email: loginEmail, password: loginPassword })
      });

      if (response.ok) {
        const result = await response.json();
        const accessToken = result.token;
        localStorage.setItem('token', accessToken);
        setToken(accessToken);
        setIsLoggedIn(true);
        setUser(loginEmail);
      } else {
        setError('Invalid email or password');
      }
    } catch (err) {
      setError('Login failed. Please check if the server is running.');
    }
  };

  const handleLogout = () => {
    localStorage.removeItem('token');
    setToken(null);
    setIsLoggedIn(false);
    setUser(null);
  };

  // ---- Computed ----

  const totalBuyAmount = buyOrders.reduce((sum, order) => sum + order.tradeAmount, 0);
  const totalSellAmount = sellOrders.reduce((sum, order) => sum + order.tradeAmount, 0);

  if (!isLoggedIn) {
    return (
      <Login 
        handleLogin={handleLogin}
        loginEmail={loginEmail}
        setLoginEmail={setLoginEmail}
        loginPassword={loginPassword}
        setLoginPassword={setLoginPassword}
        error={error}
      />
    );
  }

  return (
    <div className="layout">
      <nav className="navbar">
        <a href="/" className="brand" onClick={(e) => { e.preventDefault(); setPage('dashboard'); }}>
          <span>&#128200;</span> Stocks
        </a>
        <div className="nav-links">
          <a href="/" className={page === 'dashboard' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('dashboard'); }}>Dashboard</a>
          <a href="/trade" className={page === 'trade' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('trade'); }}>Trade</a>
          <a href="/orders" className={page === 'orders' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('orders'); }}>Orders</a>
          <a href="/cash" className={page === 'cash' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('cash'); }}>Cash</a>
        </div>
        <div className="nav-right">
          {user}
          <button className="theme-toggle" onClick={toggleTheme} title="Toggle Theme">
            {theme === 'light' ? '🌙' : '☀️'}
          </button>
          <button
            onClick={handleLogout}
            style={{ background: '#dc3545', color: '#fff', border: 'none', padding: '8px 16px', borderRadius: '8px', cursor: 'pointer', fontWeight: 600, marginLeft: '12px' }}
          >
            Logout
          </button>
        </div>
      </nav>

      {page === 'dashboard' && <Dashboard user={user} data={data} setPage={setPage} />}
      
      {page === 'trade' && (
        <Trade 
          popularStocks={popularStocks}
          selectedStock={selectedStock}
          setSelectedStock={handleSelectStock}
          quantity={quantity}
          setQuantity={setQuantity}
          onBuy={handleBuyOrder}
          onSell={handleSellOrder}
          tradeMessage={tradeMessage}
        />
      )}

      {page === 'orders' && (
        <Orders 
          buyOrders={buyOrders}
          sellOrders={sellOrders}
          totalBuyAmount={totalBuyAmount}
          totalSellAmount={totalSellAmount}
        />
      )}

      {page === 'cash' && (
        <Cash 
          data={data}
          amount={amount}
          setAmount={setAmount}
          handleCashAction={handleCashAction}
        />
      )}
    </div>
  );
}

export default App;
