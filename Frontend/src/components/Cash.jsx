import React from 'react';

function Cash({ data, amount, setAmount, handleCashAction }) {
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
          <p>Add or withdraw funds to manage your account balance. Enter the amount below.</p>
        </div>

        <div className="amount-selector">
          <button type="button" className="btn-adjust minus" onClick={() => setAmount(Math.max(10, amount - 10))}>-</button>
          <input type="number" name="amount" id="cash-amount-input" value={amount} onChange={(e) => setAmount(parseInt(e.target.value) || 0)} min="10" step="10" className="input-amount" />
          <button type="button" className="btn-adjust plus" onClick={() => setAmount(amount + 10)}>+</button>
        </div>

        <div className="cash-actions">
          <button type="button" className="btn-add-cash" onClick={() => handleCashAction('add')}>
            <span className="btn-icon">&#43;</span> Add Funds
          </button>

          <button type="button" className="btn-withdraw-cash" onClick={() => handleCashAction('withdraw')}>
            <span className="btn-icon">&#8722;</span> Withdraw
          </button>
        </div>
      </div>
    </div>
  );
}

export default Cash;
