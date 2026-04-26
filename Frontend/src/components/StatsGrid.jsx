import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import StatCard from './StatCard';
import AddCashModal from './AddCashModal';

function StatsGrid({ handleCashAction }) {
  const { totalValue, balance, stocksCount } = useSelector((state) => state.portfolio);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const navigate = useNavigate();

  return (
    <div className="stats-grid">
      <StatCard
        label="Total Value"
        value={`$${totalValue.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`}
        footer=""
      />

      <StatCard
        label="Stocks Held"
        value={stocksCount}
        isClickable={true}
        onClick={() => navigate('/holdings')}
      />

      <StatCard
        label="Available Cash"
        value={`$${balance.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`}
        isClickable={true}
        onClick={() => setIsModalOpen(true)}
      />

      <AddCashModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onConfirm={(amount) => handleCashAction('add', amount)}
        currentBalance={balance}
      />
    </div>
  );
}

export default StatsGrid;
