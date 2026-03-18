import React from 'react';
import StatsGrid from './StatsGrid';

function Dashboard({ user, data, setPage }) {
  return (
    <main className="container">
      <div className="header">
        <h1>Welcome, {user}</h1>
        <p style={{ color: '#666' }}>Here's your portfolio overview for today.</p>
      </div>

      <StatsGrid data={data} setPage={setPage} />
    </main>
  );
}

export default Dashboard;
