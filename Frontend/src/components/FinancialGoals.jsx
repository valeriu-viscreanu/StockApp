import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getGoals, getGoalTypes, addGoal } from '../store/slices/goalsSlice';
import { logout } from '../store/slices/authSlice';

function FinancialGoals() {
  const dispatch = useDispatch();
  const { token } = useSelector(state => state.auth);
  const { goals, types, loading, error } = useSelector(state => state.goals);
  
  const [showAddModal, setShowAddModal] = useState(false);
  const [newGoal, setNewGoal] = useState({
    title: '',
    goalTypeID: '',
    targetAmount: 0,
    initialAmount: 0,
    monthlyContribution: 0,
    targetDate: null
  });

  useEffect(() => {
    if (token) {
      const logoutFunc = () => dispatch(logout());
      dispatch(getGoals({ token, logout: logoutFunc }));
      dispatch(getGoalTypes({ token, logout: logoutFunc }));
    }
  }, [dispatch, token]);

  const handleAddGoal = async (e) => {
    e.preventDefault();
    const logoutFunc = () => dispatch(logout());
    await dispatch(addGoal({ goalData: newGoal, token, logout: logoutFunc }));
    setShowAddModal(false);
    setNewGoal({
      title: '',
      goalTypeID: '',
      targetAmount: 0,
      initialAmount: 0,
      monthlyContribution: 0,
      targetDate: null
    });
  };

  if (loading && goals.length === 0) {
    return <div className="container">Loading goals...</div>;
  }

  return (
    <div className="container">
      <div className="header-section">
        <h1>Financial Goals</h1>
        <p>Track your progress towards your financial milestones.</p>
      </div>

      {error && <div className="error-message" style={{ color: 'red', marginBottom: '1rem' }}>{error}</div>}

      <div className="stats-grid">
        {goals.map(goal => (
          <div key={goal.financialGoalID} className="stat-card">
            <h3>{goal.title}</h3>
            <div className="goal-info">
              <span className="label">Type:</span> <span>{goal.goalTypeName}</span>
            </div>
            <div className="goal-details">
              <div className="detail-item">
                <span className="label">Monthly:</span>
                <span>${goal.monthlyContribution}/mo</span>
              </div>
              <div className="detail-item">
                <span className="label">Balance:</span>
                <span>${goal.currentAmount.toLocaleString()} / ${goal.targetAmount.toLocaleString()}</span>
              </div>
            </div>
          </div>
        ))}
        
        <div 
          className="stat-card add-goal-card" 
          onClick={() => setShowAddModal(true)}
          style={{ border: '2px dashed var(--border-color)', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer' }}
        >
           <div style={{ textAlign: 'center' }}>
             <span style={{ fontSize: '2rem' }}>+</span>
             <p>Add New Goal</p>
           </div>
        </div>
      </div>

      {showAddModal && (
        <div className="modal-overlay">
          <div className="modal-content">
            <h2>Add New Financial Goal</h2>
            <form onSubmit={handleAddGoal}>
              <div className="form-group">
                <label>Title</label>
                <input 
                  type="text" 
                  value={newGoal.title} 
                  onChange={e => setNewGoal({...newGoal, title: e.target.value})} 
                  required 
                />
              </div>
              <div className="form-group">
                <label>Goal Type</label>
                <select 
                  value={newGoal.goalTypeID} 
                  onChange={e => setNewGoal({...newGoal, goalTypeID: e.target.value})}
                  required
                >
                  <option value="">Select Type</option>
                  {types.map(t => (
                    <option key={t.goalTypeID} value={t.goalTypeID}>{t.name}</option>
                  ))}
                </select>
              </div>
              <div className="form-group">
                <label>Target Amount</label>
                <input 
                  type="number" 
                  value={newGoal.targetAmount} 
                  onChange={e => setNewGoal({...newGoal, targetAmount: parseFloat(e.target.value)})} 
                  required 
                />
              </div>
              <div className="form-group">
                <label>Initial Amount</label>
                <input 
                  type="number" 
                  value={newGoal.initialAmount} 
                  onChange={e => setNewGoal({...newGoal, initialAmount: parseFloat(e.target.value)})} 
                />
              </div>
              <div className="form-group">
                <label>Monthly Contribution</label>
                <input 
                  type="number" 
                  value={newGoal.monthlyContribution} 
                  onChange={e => setNewGoal({...newGoal, monthlyContribution: parseFloat(e.target.value)})} 
                />
              </div>
              <div className="modal-actions">
                <button type="button" onClick={() => setShowAddModal(false)}>Cancel</button>
                <button type="submit" className="primary-btn">Create Goal</button>
              </div>
            </form>
          </div>
        </div>
      )}

      <style jsx>{`
        .goal-details {
          display: flex;
          justify-content: space-between;
          padding-top: 1rem;
          border-top: 1px solid var(--border-color);
        }
        .detail-item {
          display: flex;
          flex-direction: column;
        }
        .label {
          font-size: 0.75rem;
          color: var(--text-secondary);
          text-transform: uppercase;
        }
        .add-goal-card:hover {
          background: var(--bg-secondary);
        }
        .modal-overlay {
          position: fixed;
          top: 0;
          left: 0;
          right: 0;
          bottom: 0;
          background: rgba(0,0,0,0.5);
          display: flex;
          align-items: center;
          justify-content: center;
          z-index: 1000;
        }
        .modal-content {
          background: var(--bg-primary);
          padding: 2rem;
          border-radius: 8px;
          width: 100%;
          max-width: 500px;
          box-shadow: 0 4px 12px rgba(0,0,0,0.2);
        }
        .form-group {
          margin-bottom: 1rem;
        }
        .form-group label {
          display: block;
          margin-bottom: 0.5rem;
          font-size: 0.9rem;
        }
        .form-group input, .form-group select {
          width: 100%;
          padding: 0.6rem;
          border: 1px solid var(--border-color);
          background: var(--bg-secondary);
          color: var(--text-primary);
          border-radius: 4px;
          outline: none;
        }
        .form-group select option {
          background: var(--bg-secondary);
          color: var(--text-primary);
        }
        .modal-actions {
          display: flex;
          justify-content: flex-end;
          gap: 1rem;
          margin-top: 1.5rem;
        }
        .primary-btn {
          background: var(--primary-color);
          color: white;
          border: none;
          padding: 0.5rem 1.5rem;
          border-radius: 4px;
          cursor: pointer;
        }
        .primary-btn:hover {
          opacity: 0.9;
        }
        button[type="button"] {
          background: transparent;
          border: 1px solid var(--border-color);
          color: var(--text-primary);
          padding: 0.5rem 1.5rem;
          border-radius: 4px;
          cursor: pointer;
        }
      `}</style>
    </div>
  );
}

export default FinancialGoals;
