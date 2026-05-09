import { useState, useEffect } from 'react';
import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import Sidebar from './components/layout/Sidebar';
import Topbar from './components/layout/Topbar';
import DashboardPage from './pages/DashboardPage';
import ClientsPage from './pages/ClientsPage';
import PortfolioPage from './pages/PortfolioPage';
import AnalyticsPage from './pages/AnalyticsPage';
import LoginPage from './pages/LoginPage';

const PAGE_META = {
  '/':          { title: 'Dashboard', subtitle: 'Welcome back, Alex — here\'s your overview.' },
  '/clients':   { title: 'Clients',   subtitle: 'Manage and monitor your client roster.' },
  '/portfolio': { title: 'Portfolio', subtitle: 'Aggregate view across all client holdings.' },
  '/analytics': { title: 'Analytics', subtitle: 'Performance insights and trend analysis.' },
};

function App() {
  const [token, setToken] = useState(localStorage.getItem('advisor_token'));
  const [user, setUser] = useState(JSON.parse(localStorage.getItem('advisor_user') || 'null'));
  const isLoggedIn = !!token;

  const location = useLocation();
  const rawMeta = PAGE_META[location.pathname] || PAGE_META['/'];
  const userName = user?.email?.split('@')[0] || 'Advisor';
  const meta = {
    ...rawMeta,
    subtitle: rawMeta.subtitle.replace('Alex', userName.charAt(0).toUpperCase() + userName.slice(1))
  };

  const handleLoginSuccess = (newToken, userData) => {
    setToken(newToken);
    setUser(userData);
    localStorage.setItem('advisor_token', newToken);
    localStorage.setItem('advisor_user', JSON.stringify(userData));
  };

  const handleLogout = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem('advisor_token');
    localStorage.removeItem('advisor_user');
  };

  if (!isLoggedIn) {
    return <LoginPage onLoginSuccess={handleLoginSuccess} />;
  }

  return (
    <div className="app-shell">
      <Sidebar user={user} onLogout={handleLogout} />
      <div className="main-content">
        <Topbar 
          title={meta.title} 
          subtitle={meta.subtitle} 
          user={user} 
          token={token}
          onLogout={handleLogout}
        />
        <div className="page-body">
          <Routes>
            <Route path="/"          element={<DashboardPage />} />
            <Route path="/clients"   element={<ClientsPage />} />
            <Route path="/portfolio" element={<PortfolioPage />} />
            <Route path="/analytics" element={<AnalyticsPage />} />
            <Route path="*"          element={<Navigate to="/" replace />} />
          </Routes>
        </div>
      </div>
    </div>
  );
}

export default App;
