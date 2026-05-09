import { Bell, Search } from 'lucide-react';
import { useState } from 'react';
import SearchModal from '../shared/SearchModal';

export default function Topbar({ title, subtitle }) {
  const [isSearchOpen, setIsSearchOpen] = useState(false);

  return (
    <>
      <header className="topbar">
        <div className="topbar-title">
          <h1>{title}</h1>
          {subtitle && <p>{subtitle}</p>}
        </div>
        <div className="topbar-actions">
          <button 
            className="icon-btn" 
            aria-label="Search"
            onClick={() => setIsSearchOpen(true)}
          >
            <Search size={16} />
          </button>
          <button className="icon-btn" aria-label="Notifications">
            <Bell size={16} />
            <span className="notif-dot" />
          </button>
          <button className="btn btn-primary btn-sm">+ New Client</button>
        </div>
      </header>

      <SearchModal 
        isOpen={isSearchOpen} 
        onClose={() => setIsSearchOpen(false)} 
      />
    </>
  );
}

