import React from 'react';
import { useNavigate } from 'react-router-dom';
import StatCard from './StatCard';

function StatsGrid({ data }) {
  const navigate = useNavigate();
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
        onClick={() => navigate('/cash')}
      />
    </div>
  );
}

export default StatsGrid;
