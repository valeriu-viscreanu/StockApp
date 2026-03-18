import React from 'react';
import StatCard from './StatCard';

function StatsGrid({ data, setPage }) {
  return (
    <div className="stats-grid">
      <StatCard
        label="Total Value"
        value={`$${data.totalValue.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`}
        footer="↑ 2.4% today"
      />

      <StatCard
        label="Stocks Held"
        value={data.stocks}
      />

      <StatCard
        label="Available Cash"
        value={`$${data.balance.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`}
        isClickable={true}
        onClick={() => setPage('cash')}
      />
    </div>
  );
}

export default StatsGrid;
