import React from 'react';

function StatCard({ label, value, footer, onClick, isClickable }) {
  return (
    <div 
      className={`stat-card ${isClickable ? 'stat-card-clickable' : ''}`}
      onClick={onClick}
      style={isClickable ? { cursor: 'pointer' } : {}}
    >
      <div className="stat-label">{label}</div>
      <div className="stat-value">{value}</div>
      {footer && (
        <div className="stat-footer" style={isClickable ? { color: '#28a745' } : {}}>
          {footer}
        </div>
      )}
    </div>
  );
}

export default StatCard;
