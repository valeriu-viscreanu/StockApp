import React from 'react';

function Dashboard({ user, data }) {
  return (
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
          <div className="stat-value">${data.balance.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</div>
        </div>
      </div>
    </main>
  );
}

export default Dashboard;
