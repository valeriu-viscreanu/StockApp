import { Bell, Search } from 'lucide-react';

export default function Topbar({ title, subtitle }) {
  return (
    <header className="topbar">
      <div className="topbar-title">
        <h1>{title}</h1>
        {subtitle && <p>{subtitle}</p>}
      </div>
      <div className="topbar-actions">
        <button className="icon-btn" aria-label="Search">
          <Search size={16} />
        </button>
        <button className="icon-btn" aria-label="Notifications">
          <Bell size={16} />
          <span className="notif-dot" />
        </button>
        <button className="btn btn-primary btn-sm">+ New Client</button>
      </div>
    </header>
  );
}
