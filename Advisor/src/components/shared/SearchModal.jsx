import { Search, X, Loader2, AlertCircle } from 'lucide-react';
import { useState, useEffect } from 'react';
import { fetchClients } from '../../services/api';

export default function SearchModal({ isOpen, onClose, token }) {
  const [query, setQuery] = useState('');
  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Load clients when modal opens
  useEffect(() => {
    if (isOpen) {
      const loadClients = async () => {
        setLoading(true);
        setError(null);
        try {
          const data = await fetchClients(token);
          if (data) {
            setClients(data);
          } else {
            setError('Could not load clients. Check your connection.');
          }
        } catch (err) {
          setError('An unexpected error occurred.');
          console.error(err);
        } finally {
          setLoading(false);
        }
      };
      loadClients();
    }
  }, [isOpen, token]);

  // Handle ESC key to close
  useEffect(() => {
    const handleEsc = (e) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', handleEsc);
    return () => window.removeEventListener('keydown', handleEsc);
  }, [onClose]);

  if (!isOpen) return null;

  const filtered = clients.filter(c =>
    c.email.toLowerCase().includes(query.toLowerCase()) ||
    c.clientID.toLowerCase().includes(query.toLowerCase())
  );

  return (
    <div className="search-overlay" onClick={onClose}>
      <div className="search-modal" onClick={(e) => e.stopPropagation()}>
        <div className="search-modal-header">
          <Search size={18} className="search-modal-icon" />
          <input
            type="text"
            className="search-modal-input"
            placeholder="Search for a client by email or ID..."
            autoFocus
            value={query}
            onChange={(e) => setQuery(e.target.value)}
          />
          <button className="search-modal-close" onClick={onClose}>
            <X size={18} />
          </button>
        </div>

        <div className="search-modal-body">
          {loading ? (
            <div className="search-loading">
              <Loader2 className="animate-spin" size={24} />
              <p>Fetching your clients...</p>
            </div>
          ) : error ? (
            <div className="search-error">
              <AlertCircle size={24} />
              <p>{error}</p>
            </div>
          ) : query.trim().length === 0 ? (
            <div className="search-empty">
              <p>Type to start searching among your {clients.length} clients...</p>
              <div className="search-shortcuts">
                <span className="shortcut">ESC</span> to close
              </div>
            </div>
          ) : (
            <div className="search-results-list">
              <p className="results-label">{filtered.length} matching clients</p>
              <div className="search-items">
                {filtered.map(client => (
                  <div className="draft-item" key={client.clientID}>
                    <div className="item-avatar">
                      {client.email.charAt(0).toUpperCase()}
                    </div>
                    <div className="item-info">
                      <div className="item-name">{client.email}</div>
                    </div>
                    <button className="btn btn-ghost btn-xs">View</button>
                  </div>
                ))}
                {filtered.length === 0 && (
                  <div className="search-no-results">
                    <p>No matches found for "{query}"</p>
                  </div>
                )}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

