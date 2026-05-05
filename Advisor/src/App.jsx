import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import Sidebar from './components/layout/Sidebar';
import Topbar from './components/layout/Topbar';
import DashboardPage from './pages/DashboardPage';
import ClientsPage from './pages/ClientsPage';
import PortfolioPage from './pages/PortfolioPage';
import AnalyticsPage from './pages/AnalyticsPage';

const PAGE_META = {
  '/':          { title: 'Dashboard', subtitle: 'Welcome back, Alex — here\'s your overview.' },
  '/clients':   { title: 'Clients',   subtitle: 'Manage and monitor your client roster.' },
  '/portfolio': { title: 'Portfolio', subtitle: 'Aggregate view across all client holdings.' },
  '/analytics': { title: 'Analytics', subtitle: 'Performance insights and trend analysis.' },
};

function App() {
  const location = useLocation();
  const meta = PAGE_META[location.pathname] || PAGE_META['/'];

  return (
    <div className="app-shell">
      <Sidebar />
      <div className="main-content">
        <Topbar title={meta.title} subtitle={meta.subtitle} />
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
