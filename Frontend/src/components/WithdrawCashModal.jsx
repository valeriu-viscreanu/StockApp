import React, { useState } from 'react';
import Modal from './Modal';

function WithdrawCashModal({ isOpen, onClose, onConfirm, currentBalance }) {
  const [amount, setAmount] = useState(100);

  if (!isOpen) return null;

  const handleConfirm = () => {
    onConfirm(amount);
    onClose();
  };

  return (
    <Modal onClose={onClose}>
      <div className="add-cash-modal-content">
        <div className="cash-icon" style={{ fontSize: '3rem', marginBottom: '10px' }}>&#128176;</div>
        <h2 className="cash-title">Withdraw Funds</h2>
        <p className="cash-subtitle">How much would you like to withdraw?</p>
        
        <div className="cash-balance-display" style={{ padding: '15px 0', marginBottom: '20px' }}>
          <span className="cash-currency">$</span>
          <span className="cash-amount" style={{ fontSize: '2.5rem' }}>
            {currentBalance.toLocaleString('en-US', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
          </span>
        </div>

        <div className="amount-selector">
          <button type="button" className="btn-adjust minus" onClick={() => setAmount(Math.max(10, amount - 10))}>-</button>
          <input 
            type="number" 
            value={amount} 
            onChange={(e) => setAmount(parseInt(e.target.value) || 0)} 
            min="10" 
            step="10" 
            className="input-amount" 
          />
          <button type="button" className="btn-adjust plus" onClick={() => setAmount(amount + 10)}>+</button>
        </div>

        <div className="cash-actions" style={{ marginTop: '20px' }}>
          <button type="button" className="btn-withdraw-cash" onClick={handleConfirm} style={{ width: '100%' }}>
            <span className="btn-icon">&#8722;</span> Confirm Withdrawal
          </button>
        </div>
      </div>
    </Modal>
  );
}

export default WithdrawCashModal;
