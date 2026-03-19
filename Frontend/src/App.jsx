import { useState, useEffect } from 'react';
import Dashboard from './components/Dashboard';
import Trade from './components/Trade';
import Orders from './components/Orders';
import Cash from './components/Cash';
import Login from './components/Login';

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
  const [selectedStock, setSelectedStock] = useState({ symbol: 'AAPL', name: 'Apple', price: 175.50 });
  const [data, setData] = useState({
    balance: 0.00,
    stocks: 15,
    totalValue: 12450.75
  });

  useEffect(() => {
    if (isLoggedIn && token) {
      fetchBalance();
      // Mocking user email from token for now
      setUser(loginEmail || 'User');
    }
  }, [isLoggedIn, token]);

  const fetchBalance = async () => {
    try {
      const response = await fetch('http://localhost:5002/api/v1/CashApi/balance', {
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });
      if (response.ok) {
        const balance = await response.json();
        setData(prev => ({ ...prev, balance }));
      } else if (response.status === 401) {
        handleLogout();
      }
    } catch (err) {
      console.error('Failed to fetch balance', err);
    }
  };

  const handleCashAction = async (type) => {
    try {
      const endpoint = type === 'add' ? 'add-funds' : 'withdraw';
      const response = await fetch(`http://localhost:5002/api/v1/CashApi/${endpoint}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
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

  const handleLogin = async (e) => {
    e.preventDefault();
    setError('');
    try {
      const response = await fetch('http://localhost:5002/api/Auth/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
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

  // Static Data
  const [popularStocks] = useState([
    { symbol: "MSFT", name: "Microsoft", price: 420.50 },
    { symbol: "AAPL", name: "Apple", price: 175.50 },
    { symbol: "GOOGL", name: "Alphabet", price: 145.20 },
    { symbol: "AMZN", name: "Amazon", price: 178.10 },
    { symbol: "NVDA", name: "Nvidia", price: 875.40 },
    { symbol: "META", name: "Meta Platforms", price: 490.30 },
    { symbol: "TSLA", name: "Tesla", price: 165.20 },
    { symbol: "AMD", name: "Advanced Micro Devices", price: 180.50 },
    { symbol: "JPM", name: "JPMorgan Chase", price: 195.40 },
    { symbol: "V", name: "Visa", price: 280.10 },
  ]);

  const [buyOrders] = useState([
    { id: 1, stockName: "Microsoft", stockSymbol: "MSFT", quantity: 5, price: 415.20, tradeAmount: 2076.00, date: "16 March 2026 10:20:15 AM" },
    { id: 2, stockName: "Apple", stockSymbol: "AAPL", quantity: 10, price: 172.50, tradeAmount: 1725.00, date: "15 March 2026 02:45:10 PM" },
  ]);

  const [sellOrders] = useState([
    { id: 3, stockName: "Tesla", stockSymbol: "TSLA", quantity: 2, price: 168.40, tradeAmount: 336.80, date: "16 March 2026 11:15:22 AM" },
  ]);

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
          setSelectedStock={setSelectedStock}
          quantity={quantity}
          setQuantity={setQuantity}
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
