import { useState } from 'react'

function App() {
  const [user] = useState('admin');
  const [data] = useState({
    balance: 1000.00,
    stocks: 15,
    totalValue: 12450.75
  });

  return (
    <div className="layout">
      <nav className="navbar">
        <a href="/" className="brand">
          <span>&#128200;</span> Stocks
        </a>
        <div className="nav-links">
          <a href="/" className="active">Dashboard</a>
          <a href="/trade">Trade</a>
          <a href="/orders">Orders</a>
        </div>
        <div className="nav-right">
          {user}
          <button style={{ background: '#dc3545', color: '#fff', border: 'none', padding: '8px 16px', borderRadius: '8px', cursor: 'pointer', fontWeight: 600 }}>Logout</button>
        </div>
      </nav>

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
    </div>
  )
}

export default App
