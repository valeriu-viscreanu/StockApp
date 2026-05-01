import React, { useState } from 'react';

function FinancialGoals() {
  const [goals, setGoals] = useState([
    { id: 1, title: 'Retirement', target: 20000, current: 5000, type: 'Retirement', monthly: 200 },
    { id: 2, title: 'University Fund', target: 15000, current: 2000, type: 'University', monthly: 150 },
    { id: 3, title: 'Emergency Fund', target: 10000, current: 8000, type: 'Emergency Fund', monthly: 100 },
  ]);

  return (
    <div className="container">
      <div className="header-section">
        <h1>Financial Goals</h1>
        <p>Track your progress towards your financial milestones.</p>
      </div>

      <div className="stats-grid">
        {goals.map(goal => (
          <div key={goal.id} className="stat-card">
            <h3>{goal.title}</h3>
            <div className="goal-info">
              <span className="label">Type:</span> <span>{goal.type}</span>
            </div>
            <div className="goal-progress-container">
              <div className="goal-progress-bar">
                <div 
                  className="goal-progress-fill" 
                  style={{ width: `${Math.min((goal.current / goal.target) * 100, 100)}%` }}
                ></div>
              </div>
              <div className="goal-progress-labels">
                <span>${goal.current.toLocaleString()}</span>
                <span>${goal.target.toLocaleString()}</span>
              </div>
            </div>
            <div className="goal-details">
              <div className="detail-item">
                <span className="label">Monthly:</span>
                <span>${goal.monthly}/mo</span>
              </div>
              <div className="detail-item">
                <span className="label">Progress:</span>
                <span>{((goal.current / goal.target) * 100).toFixed(1)}%</span>
              </div>
            </div>
          </div>
        ))}
        
        <div className="stat-card add-goal-card" style={{ border: '2px dashed var(--border-color)', display: 'flex', alignItems: 'center', justifyContent: 'center', cursor: 'pointer' }}>
           <div style={{ textAlign: 'center' }}>
             <span style={{ fontSize: '2rem' }}>+</span>
             <p>Add New Goal</p>
           </div>
        </div>
      </div>

      <style jsx>{`
        .goal-progress-container {
          margin: 1.5rem 0;
        }
        .goal-progress-bar {
          height: 10px;
          background: var(--bg-secondary);
          border-radius: 5px;
          overflow: hidden;
          margin-bottom: 0.5rem;
        }
        .goal-progress-fill {
          height: 100%;
          background: var(--primary-color);
          transition: width 0.3s ease;
        }
        .goal-progress-labels {
          display: flex;
          justify-content: space-between;
          font-size: 0.85rem;
          color: var(--text-secondary);
        }
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
      `}</style>
    </div>
  );
}

export default FinancialGoals;
