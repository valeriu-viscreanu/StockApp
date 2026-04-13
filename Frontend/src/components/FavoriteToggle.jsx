import React from 'react';

function FavoriteToggle({ symbol, isFavorite, onToggle }) {
  return (
    <span 
      className="favorite-toggle" 
      onClick={(e) => {
        e.preventDefault();
        e.stopPropagation();
        onToggle(symbol);
      }}
      style={{
        cursor: 'pointer',
        fontSize: '1.2rem',
        marginLeft: '8px',
        color: isFavorite ? '#f1c40f' : '#ccc',
        transition: 'color 0.2s ease',
        display: 'inline-flex',
        alignItems: 'center',
        justifyContent: 'center'
      }}
    >
      {isFavorite ? '★' : '☆'}
    </span>
  );
}

export default FavoriteToggle;
