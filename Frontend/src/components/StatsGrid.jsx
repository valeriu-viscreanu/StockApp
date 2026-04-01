import React, { useState } from 'react';
import StatCard from './StatCard';
import AddCashModal from './AddCashModal';

function StatsGrid({ data, handleCashAction }) {
  const [isModalOpen, setIsModalOpen] = useState(false);

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
        onClick={() => setIsModalOpen(true)}
      />

      <AddCashModal 
        isOpen={isModalOpen} 
        onClose={() => setIsModalOpen(false)} 
        onConfirm={(amount) => handleCashAction('add', amount)}
        currentBalance={data.balance}
      />
    </div>
  );
}

export default StatsGrid;
