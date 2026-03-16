import { useState } from 'react'

function App() {
  const [user] = useState('admin');
  const [page, setPage] = useState('dashboard');
  const [amount, setAmount] = useState(100);
  const [data] = useState({
    balance: 1000.00,
    stocks: 15,
    totalValue: 12450.75
  });

  return (
    <div className="layout">
      <nav className="navbar">
        <a href="/" className="brand" onClick={(e) => { e.preventDefault(); setPage('dashboard'); }}>
          <span>&#128200;</span> Stocks
        </a>
        <div className="nav-links">
          <a href="/" className={page === 'dashboard' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('dashboard'); }}>Dashboard</a>
          <a href="/trade">Trade</a>
          <a href="/orders">Orders</a>
          <a href="/cash" className={page === 'cash' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('cash'); }}>Cash</a>
        </div>
        <div className="nav-right">
          {user}
          <button style={{ background: '#dc3545', color: '#fff', border: 'none', padding: '8px 16px', borderRadius: '8px', cursor: 'pointer', fontWeight: 600 }}>Logout</button>
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
                  <button type="button" className="btn-add-cash">
                      <span className="btn-icon">&#43;</span> Add Funds
                  </button>

                  <button type="button" className="btn-withdraw-cash">
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
