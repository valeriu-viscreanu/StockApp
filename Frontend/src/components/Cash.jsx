import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import AddCashModal from './AddCashModal';
import WithdrawCashModal from './WithdrawCashModal';

function Cash({ handleCashAction }) {
  const balance = useSelector((state) => state.portfolio.balance);
  const [isAddModalOpen, setIsAddModalOpen] = useState(false);
  const [isWithdrawModalOpen, setIsWithdrawModalOpen] = useState(false);

  return (
    <div className="cash-page">
      <div className="cash-card">
        <div className="cash-icon">&#128176;</div>
        <h1 className="cash-title">Your Cash Balance</h1>
        <p className="cash-subtitle">Manage your trading funds</p>

        <div className="cash-balance-display">
          <span className="cash-currency">$</span>
          <span className="cash-amount" id="cash-amount">{balance.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
        </div>

        <div className="cash-message">
          <p>Add or withdraw funds to manage your account balance.</p>
        </div>

        <div className="cash-actions">
          <button type="button" className="btn-add-cash" onClick={() => setIsAddModalOpen(true)}>
            <span className="btn-icon">&#43;</span> Add Funds
          </button>

          <button type="button" className="btn-withdraw-cash" onClick={() => setIsWithdrawModalOpen(true)}>
            <span className="btn-icon">&#8722;</span> Withdraw
          </button>
        </div>
      </div>

      <AddCashModal
        isOpen={isAddModalOpen}
        onClose={() => setIsAddModalOpen(false)}
        onConfirm={(amount) => handleCashAction('add', amount)}
        currentBalance={balance}
      />

      <WithdrawCashModal
        isOpen={isWithdrawModalOpen}
        onClose={() => setIsWithdrawModalOpen(false)}
        onConfirm={(amount) => handleCashAction('withdraw', amount)}
        currentBalance={balance}
      />
    </div>
  );
}

export default Cash;
