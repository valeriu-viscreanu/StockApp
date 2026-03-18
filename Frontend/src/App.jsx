import { useState, useEffect } from 'react'

function App() {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));
  const [loginEmail, setLoginEmail] = useState('admin@test.com');
  const [loginPassword, setLoginPassword] = useState('123');
  const [error, setError] = useState('');
  
  const [page, setPage] = useState('dashboard');
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
        setAmount(100); // Reset amount
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

  if (!isLoggedIn) {
    return (
      <div className="login-container" style={{ 
        display: 'flex', 
        justifyContent: 'center', 
        alignItems: 'center', 
        height: '100vh', 
        background: '#f8f9fa' 
      }}>
        <div className="login-card" style={{ 
          background: 'white', 
          padding: '40px', 
          borderRadius: '16px', 
          boxShadow: '0 4px 24px rgba(0,0,0,0.1)',
          width: '100%',
          maxWidth: '400px'
        }}>
          <h1 style={{ textAlign: 'center', marginBottom: '8px' }}>Login</h1>
          <p style={{ textAlign: 'center', color: '#666', marginBottom: '24px' }}>Welcome back! Please login to your account.</p>
          
          <form onSubmit={handleLogin}>
            <div style={{ marginBottom: '16px' }}>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: 600 }}>Email</label>
              <input 
                type="email" 
                value={loginEmail} 
                onChange={(e) => setLoginEmail(e.target.value)}
                style={{ 
                  width: '100%', 
                  padding: '12px', 
                  borderRadius: '8px', 
                  border: '1px solid #ddd',
                  boxSizing: 'border-box'
                }}
                required 
              />
            </div>
            <div style={{ marginBottom: '24px' }}>
              <label style={{ display: 'block', marginBottom: '8px', fontWeight: 600 }}>Password</label>
              <input 
                type="password" 
                value={loginPassword} 
                onChange={(e) => setLoginPassword(e.target.value)}
                style={{ 
                  width: '100%', 
                  padding: '12px', 
                  borderRadius: '8px', 
                  border: '1px solid #ddd',
                  boxSizing: 'border-box'
                }}
                required 
              />
            </div>
            {error && <p style={{ color: '#dc3545', marginBottom: '16px', textAlign: 'center' }}>{error}</p>}
            <button type="submit" style={{ 
              width: '100%', 
              padding: '12px', 
              borderRadius: '8px', 
              border: 'none', 
              background: '#28a745', 
              color: 'white', 
              fontWeight: 600, 
              cursor: 'pointer' 
            }}>
              Login
            </button>
          </form>
        </div>
      </div>
    );
  }

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
          <button 
            onClick={handleLogout}
            style={{ background: '#dc3545', color: '#fff', border: 'none', padding: '8px 16px', borderRadius: '8px', cursor: 'pointer', fontWeight: 600, marginLeft: '12px' }}
          >
            Logout
          </button>
        </div>
      </nav>

      {page === 'dashboard' && (
        <main className="container">
          <div className="header">
            <h1>Welcome, {user}</h1>
            <p style={{ color: '#666' }}>Here's your portfolio overview for today.</p>
          </div>

          <div className="stats-grid">
            <div className="stat-card">
              <div className="stat-label">Total Value</div>
              <div className="stat-value">${data.totalValue.toLocaleString()}</div>
              <div className="stat-footer">↑ 2.4% today</div>
            </div>

            <div className="stat-card">
              <div className="stat-label">Stocks Held</div>
              <div className="stat-value">{data.stocks}</div>
            </div>

            <div className="stat-card">
              <div className="stat-label">Available Cash</div>
              <div className="stat-value">${data.balance.toLocaleString()}</div>
            </div>
          </div>
        </main>
      )}

      {page === 'trade' && (
        <div className="advanced-dashboard">
          <div className="sidebar-panel">
            <div className="sidebar-header">
              <h3>Market Watch</h3>
            </div>
            <div className="stock-list">
              {popularStocks.map(stock => (
                <a 
                  key={stock.symbol}
                  href="#"
                  className={`stock-item ${selectedStock.symbol === stock.symbol ? 'active' : ''}`}
                  onClick={(e) => { e.preventDefault(); setSelectedStock(stock); }}
                >
                  <div className="stock-icon">{stock.symbol.substring(0, 1)}</div>
                  <div className="stock-details">
                    <span className="stock-symbol">{stock.symbol}</span>
                    <span className="stock-company">{stock.name}</span>
                  </div>
                  <div className="stock-mini-chart">
                    <span className="stock-mini-price">${stock.price.toFixed(2)}</span>
                  </div>
                </a>
              ))}
            </div>
          </div>

          <div className="main-view-panel">
            <div className="trade-container">
              <div className="stock-info">
                <h1 className="stock-name">{selectedStock.name} ({selectedStock.symbol})</h1>
                <div className="stock-price">
                  <span className="currency">$</span>
                  <span className="price-value">{selectedStock.price.toFixed(2)}</span>
                </div>
                <div className="stock-price-label">Live Market Price</div>
              </div>

              <div className="order-panel">
                <h3 className="order-title">New Order</h3>
                <div className="order-form">
                  <label>Quantity:</label>
                  <input 
                    type="number" 
                    className="quantity-input" 
                    min="1" 
                    value={quantity} 
                    onChange={(e) => setQuantity(parseInt(e.target.value) || 1)}
                  />
                </div>

                <div className="order-buttons">
                  <button type="button" className="btn-sell">&#8595; Sell</button>
                  <button type="button" className="btn-buy">&#8593; Buy</button>
                </div>
              </div>

              <div className="graph-panel" style={{ flex: 2, padding: '20px', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', borderLeft: '1px solid #eee' }}>
                <div className="timeframe-buttons" style={{ marginBottom: '15px' }}>
                  <button style={{ padding: '6px 16px', margin: '0 5px', cursor: 'pointer', border: '1px solid #28a745', background: 'transparent', color: '#28a745', borderRadius: '4px', fontWeight: 'bold' }}>Day</button>
                  <button style={{ padding: '6px 16px', margin: '0 5px', cursor: 'pointer', border: '1px solid #28a745', background: '#28a745', color: 'white', borderRadius: '4px', fontWeight: 'bold' }}>Month</button>
                  <button style={{ padding: '6px 16px', margin: '0 5px', cursor: 'pointer', border: '1px solid #28a745', background: 'transparent', color: '#28a745', borderRadius: '4px', fontWeight: 'bold' }}>Year</button>
                </div>
                <div style={{ width: '100%', height: '300px', display: 'flex', alignItems: 'center', justifyContent: 'center', background: '#fcfcfc', border: '1px dashed #ddd', borderRadius: '12px', color: '#888' }}>
                  [ Chart Visualization Placeholder ]
                </div>
              </div>
            </div>
          </div>
        </div>
      )}

      {page === 'orders' && (
        <>
          <div className="breadcrumb">
            Stocks &#9654; Orders
          </div>
          <div className="orders-container">
            <div className="orders-column">
              <h3 className="orders-heading">Buy Orders</h3>
              {buyOrders.length === 0 ? (
                <p className="no-orders">No buy orders yet.</p>
              ) : (
                buyOrders.map(order => (
                  <div key={order.id} className="order-card">
                    <div className="order-stock-name">{order.stockName} ({order.stockSymbol})</div>
                    <div className="order-details">
                      <span className="order-quantity">{order.quantity} shares</span> at <span className="order-price">${order.price.toFixed(2)}</span>
                    </div>
                    <div className="order-trade-amount">
                      Trade Amount: <strong>${order.tradeAmount.toFixed(2)}</strong>
                    </div>
                    <hr />
                    <div className="order-date">{order.date}</div>
                  </div>
                ))
              )}
            </div>

            <div className="orders-column">
              <h3 className="orders-heading">Sell Orders</h3>
              {sellOrders.length === 0 ? (
                <p className="no-orders">No sell orders yet.</p>
              ) : (
                sellOrders.map(order => (
                  <div key={order.id} className="order-card">
                    <div className="order-stock-name">{order.stockName} ({order.stockSymbol})</div>
                    <div className="order-details">
                      <span className="order-quantity">{order.quantity} shares</span> at <span className="order-price">${order.price.toFixed(2)}</span>
                    </div>
                    <div className="order-trade-amount">
                      Trade Amount: <strong>${order.tradeAmount.toFixed(2)}</strong>
                    </div>
                    <hr />
                    <div className="order-date">{order.date}</div>
                  </div>
                ))
              )}
            </div>
          </div>
          <div className="orders-total-footer">
            <h3>Total Buy Amounts: ${totalBuyAmount.toLocaleString('en-US', { minimumFractionDigits: 2 })}</h3>
            <h3>Total Sell Amounts: ${totalSellAmount.toLocaleString('en-US', { minimumFractionDigits: 2 })}</h3>
          </div>
          <div style={{ height: '80px' }}></div>
        </>
      )}

      {page === 'cash' && (
        <div className="cash-page">
          <div className="cash-card">
              <div className="cash-icon">&#128176;</div>
              <h1 className="cash-title">Your Cash Balance</h1>
              <p className="cash-subtitle">Manage your trading funds</p>

              <div className="cash-balance-display">
                  <span className="cash-currency">$</span>
                  <span className="cash-amount" id="cash-amount">{data.balance.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
              </div>

              <div className="cash-message">
                  <p>Add or withdraw funds to manage your account balance. Enter the amount below.</p>
              </div>

              <div className="amount-selector">
                  <button type="button" className="btn-adjust minus" onClick={() => setAmount(Math.max(10, amount - 10))}>-</button>
                  <input type="number" name="amount" id="cash-amount-input" value={amount} onChange={(e) => setAmount(parseInt(e.target.value) || 0)} min="10" step="10" className="input-amount" />
                  <button type="button" className="btn-adjust plus" onClick={() => setAmount(amount + 10)}>+</button>
              </div>

              <div className="cash-actions">
                  <button type="button" className="btn-add-cash" onClick={() => handleCashAction('add')}>
                      <span className="btn-icon">&#43;</span> Add Funds
                  </button>

                  <button type="button" className="btn-withdraw-cash" onClick={() => handleCashAction('withdraw')}>
                      <span className="btn-icon">&#8722;</span> Withdraw
                  </button>
              </div>
          </div>
        </div>
      )}
    </div>
  )
}

export default App
