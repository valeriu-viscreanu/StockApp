import { Link, useLocation } from 'react-router-dom';
import {
  LayoutDashboard, Users, PieChart, BarChart3,
  Settings, Bell, LogOut, TrendingUp
} from 'lucide-react';

const NAV = [
  { path: '/', label: 'Dashboard', icon: LayoutDashboard },
  { path: '/clients', label: 'Clients', icon: Users, badge: 6 },
  { path: '/portfolio', label: 'Portfolio', icon: PieChart },
  { path: '/analytics', label: 'Analytics', icon: BarChart3 },
];

export default function Sidebar({ user, onLogout }) {
  const location = useLocation();

  return (
    <aside className="sidebar">
      <Link to="/" className="sidebar-logo">
        <div className="sidebar-logo-icon">
          <TrendingUp size={18} color="#fff" />
        </div>
        <span className="sidebar-logo-text">Advisor</span>
      </Link>

      <span className="sidebar-section-label">Main Menu</span>

      <nav className="sidebar-nav">
        {NAV.map(({ path, label, icon: Icon, badge }) => (
          <Link
            key={path}
            to={path}
            className={`nav-item ${location.pathname === path ? 'active' : ''}`}
          >
            <Icon size={17} className="nav-icon" />
            {label}
            {badge && <span className="nav-badge">{badge}</span>}
          </Link>
        ))}
      </nav>

      <span className="sidebar-section-label" style={{ marginTop: 'auto' }}>Account</span>
      <nav className="sidebar-nav" style={{ flex: 'none', marginBottom: 12 }}>
        <button className="nav-item">
          <Bell size={17} className="nav-icon" /> Notifications <span className="nav-badge">3</span>
        </button>
        <button className="nav-item">
          <Settings size={17} className="nav-icon" /> Settings
        </button>
        <button className="nav-item" style={{ color: 'var(--danger)' }} onClick={onLogout}>
          <LogOut size={17} className="nav-icon" /> Sign Out
        </button>
      </nav>

      <div className="sidebar-footer">
        <div className="sidebar-user">
          <div className="user-avatar">
            {user?.email?.charAt(0).toUpperCase() || 'A'}
          </div>
          <div className="user-info">
            <div className="user-name">{user?.email?.split('@')[0] || 'Advisor'}</div>
            <div className="user-role">Senior Advisor</div>
          </div>
        </div>
      </div>
    </aside>
  );
}
