import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { NavLink } from 'react-router-dom';
import EditDetailsModal from './EditDetailsModal';

function Navbar({ theme, toggleTheme }) {
  const { user, token, handleLogout } = useAuth();
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);
  
  return (
    <nav className="navbar">
      <NavLink to="/dashboard" className="brand">
        <span>&#128200;</span> Stocks
      </NavLink>
      <div className="nav-links">
        <NavLink to="/dashboard" className={({ isActive }) => isActive ? 'active' : ''}>Dashboard</NavLink>
        <NavLink to="/trade" className={({ isActive }) => isActive ? 'active' : ''}>Trade</NavLink>
        <NavLink to="/orders" className={({ isActive }) => isActive ? 'active' : ''}>Orders</NavLink>
        <NavLink to="/activities" className={({ isActive }) => isActive ? 'active' : ''}>Activities</NavLink>
        <NavLink to="/cash" className={({ isActive }) => isActive ? 'active' : ''}>Cash</NavLink>
      </div>
      <div className="nav-right">
        <button
          className="user-badge"
          onClick={() => setIsDetailsOpen(true)}
          title="Edit profile"
        >
          {user}
        </button>
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
      <EditDetailsModal
        isOpen={isDetailsOpen}
        onClose={() => setIsDetailsOpen(false)}
        token={token}
        logout={handleLogout}
      />
    </nav>
  );
}

export default Navbar;
