import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { NavLink } from 'react-router-dom';
import EditDetailsModal from './EditDetailsModal';

function Navbar({ theme, toggleTheme, totalValue = 0 }) {
  const { user, token, handleLogout } = useAuth();
  const [isDetailsOpen, setIsDetailsOpen] = useState(false);
  const [menuOpen, setMenuOpen] = useState(false);

  return (
    <nav className="navbar">
      <div className="navbar-left">
        <NavLink to="/dashboard" className="brand">
          <span>&#128200;</span> Stocks
        </NavLink>
        <button className="hamburger" onClick={() => setMenuOpen(!menuOpen)}>
          <span className="bar"></span>
          <span className="bar"></span>
          <span className="bar"></span>
        </button>
      </div>

      <div className={`nav-links ${menuOpen ? 'open' : ''}`}>
        <NavLink to="/dashboard" className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setMenuOpen(false)}>Dashboard</NavLink>
        <NavLink to="/trade" className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setMenuOpen(false)}>Trade</NavLink>
        <NavLink to="/holdings" className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setMenuOpen(false)}>Holdings</NavLink>
        <NavLink to="/orders" className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setMenuOpen(false)}>Orders</NavLink>
        <NavLink to="/activities" className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setMenuOpen(false)}>Activities</NavLink>
        <NavLink to="/cash" className={({ isActive }) => isActive ? 'active' : ''} onClick={() => setMenuOpen(false)}>Cash</NavLink>

        {/* Mobile-only logout/theme (optional, but good for UX) */}
        <div className="mobile-actions">
          <button className="theme-toggle" onClick={toggleTheme}>
            {theme === 'light' ? '🌙 Dark' : '☀️ Light'}
          </button>
          <button onClick={handleLogout} className="btn-logout-mobile">Logout</button>
        </div>
      </div>

      <div className="nav-right">
        <div className="portfolio-badge">
          <span className="portfolio-label">Portfolio:</span>
          <span className="portfolio-amount">${totalValue.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
        </div>
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
          className="btn-logout"
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
