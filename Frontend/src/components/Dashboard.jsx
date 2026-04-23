import StatsGrid from './StatsGrid';
import NewsFeed from './NewsFeed';

function Dashboard({ user, handleCashAction }) {
  return (
    <main className="container">
      <div className="header">
        <h1>Welcome, {user}</h1>
        <p style={{ color: 'var(--text-muted)' }}>Here's your portfolio overview for today.</p>
      </div>

      <StatsGrid handleCashAction={handleCashAction} />

      <NewsFeed />
    </main>
  );
}

export default Dashboard;
