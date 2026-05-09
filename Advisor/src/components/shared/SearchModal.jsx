import { Search, X } from 'lucide-react';
import { useState, useEffect } from 'react';

export default function SearchModal({ isOpen, onClose }) {
  const [query, setQuery] = useState('');

  // Handle ESC key to close
  useEffect(() => {
    const handleEsc = (e) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', handleEsc);
    return () => window.removeEventListener('keydown', handleEsc);
  }, [onClose]);

  if (!isOpen) return null;

  return (
    <div className="search-overlay" onClick={onClose}>
      <div className="search-modal" onClick={(e) => e.stopPropagation()}>
        <div className="search-modal-header">
          <Search size={18} className="search-modal-icon" />
          <input
            type="text"
            className="search-modal-input"
            placeholder="Search for a client by name, email or ID..."
            autoFocus
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button className="search-modal-close" onClick={onClose}>
            <X size={18} />
          </button>
        </div>
        
        <div className="search-modal-body">
          {query.trim().length === 0 ? (
            <div className="search-empty">
              <p>Type to start searching...</p>
              <div className="search-shortcuts">
                <span className="shortcut">ESC</span> to close
              </div>
            </div>
          ) : (
            <div className="search-results-draft">
              <p className="results-label">Searching for "{query}"...</p>
              <div className="draft-items">
                <div className="draft-item">
                  <div className="item-avatar">JD</div>
                  <div className="item-info">
                    <div className="item-name">John Doe (Draft Result)</div>
                    <div className="item-meta">john.doe@example.com</div>
                  </div>
                  <button className="btn btn-ghost btn-xs">View</button>
                </div>
                <div className="draft-item">
                  <div className="item-avatar">AS</div>
                  <div className="item-info">
                    <div className="item-name">Alice Smith (Draft Result)</div>
                    <div className="item-meta">alice.s@example.com</div>
                  </div>
                  <button className="btn btn-ghost btn-xs">View</button>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
