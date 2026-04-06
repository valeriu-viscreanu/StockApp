import React, { useState } from 'react';
import AddCashModal from './AddCashModal';

function Cash({ data, handleCashAction }) {
  const [isModalOpen, setIsModalOpen] = useState(false);

  return (
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
          <p>Add or withdraw funds to manage your account balance.</p>
        </div>

        <div className="cash-actions">
          <button type="button" className="btn-add-cash" onClick={() => setIsModalOpen(true)}>
            <span className="btn-icon">&#43;</span> Add Funds
          </button>
        </div>
      </div>

      <AddCashModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onConfirm={(amount) => handleCashAction('add', amount)}
        currentBalance={data.balance}
      />
    </div>
  );
}

export default Cash;
