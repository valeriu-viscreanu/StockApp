import React from 'react';
import { useAuth } from '../context/AuthContext';

function Navbar({ page, setPage, theme, toggleTheme }) {
  const { user, handleLogout } = useAuth();
  
  return (
    <nav className="navbar">
      <a href="/" className="brand" onClick={(e) => { e.preventDefault(); setPage('dashboard'); }}>
        <span>&#128200;</span> Stocks
      </a>
      <div className="nav-links">
        <a href="/" className={page === 'dashboard' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('dashboard'); }}>Dashboard</a>
        <a href="/trade" className={page === 'trade' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('trade'); }}>Trade</a>
        <a href="/orders" className={page === 'orders' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('orders'); }}>Orders</a>
        <a href="/cash" className={page === 'cash' ? 'active' : ''} onClick={(e) => { e.preventDefault(); setPage('cash'); }}>Cash</a>
      </div>
      <div className="nav-right">
        {user}
        <button className="theme-toggle" onClick={toggleTheme} title="Toggle Theme">
          {theme === 'light' ? '🌙' : '☀️'}
        </button>
        <button
          onClick={handleLogout}
          style={{ background: '#dc3545', color: '#fff', border: 'none', padding: '8px 16px', borderRadius: '8px', cursor: 'pointer', fontWeight: 600, marginLeft: '12px' }}
        >
          Logout
        </button>
      </div>
    </nav>
  );
}

export default Navbar;
