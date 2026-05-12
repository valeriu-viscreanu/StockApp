import { useState } from 'react';
import { Search, Filter, UserPlus, TrendingUp, TrendingDown } from 'lucide-react';
import { clients } from '../data/mockData';

const fmt = (n) => n >= 1_000_000
  ? `$${(n / 1_000_000).toFixed(2)}M`
  : `$${(n / 1000).toFixed(0)}K`;

const RISK_CLASS = {
  Conservative: 'risk-conservative',
  Moderate: 'risk-moderate',
  Aggressive: 'risk-aggressive',
};

const STATUS_CLASS = {
  active: 'badge-active',
  review: 'badge-review',
  onboarding: 'badge-onboarding',
};

export default function ClientsPage({ onNewClient }) {
  const [search, setSearch] = useState('');
  const [filterRisk, setFilterRisk] = useState('All');
  const [filterStatus, setFilterStatus] = useState('All');

  const filtered = clients.filter((c) => {
    const matchSearch = c.name.toLowerCase().includes(search.toLowerCase()) ||
      c.sector.toLowerCase().includes(search.toLowerCase());
    const matchRisk = filterRisk === 'All' || c.risk === filterRisk;
    const matchStatus = filterStatus === 'All' || c.status === filterStatus;
    return matchSearch && matchRisk && matchStatus;
  });

  return (
    <>
      {/* Summary Cards */}
      {/* Client Table */}
      <div className="card">
        <div className="card-header">
          <div>
            <div className="card-title">Client Roster</div>
            <div className="card-subtitle">{filtered.length} of {clients.length} clients shown</div>
          </div>
          <button 
            className="btn btn-primary btn-sm"
            onClick={onNewClient}
          >
            <UserPlus size={14} /> New Client
          </button>
        </div>

        <div className="filter-bar">
          <div className="search-wrap">
            <Search size={14} className="search-icon" />
            <input
              id="client-search"
              type="text"
              className="search-input"
              placeholder="Search by name or sector…"
              value={search}
              onChange={(e) => setSearch(e.target.value)}
            />
          </div>
          <select
            id="filter-risk"
            className="filter-select"
            value={filterRisk}
            onChange={(e) => setFilterRisk(e.target.value)}
          >
            <option value="All">All Risk Levels</option>
            <option>Conservative</option>
            <option>Moderate</option>
            <option>Aggressive</option>
          </select>
          <select
            id="filter-status"
            className="filter-select"
            value={filterStatus}
            onChange={(e) => setFilterStatus(e.target.value)}
          >
            <option value="All">All Statuses</option>
            <option value="active">Active</option>
            <option value="review">Review</option>
            <option value="onboarding">Onboarding</option>
          </select>
          <button className="btn btn-ghost btn-sm"><Filter size={13} /> More Filters</button>
        </div>

        <div className="table-wrap">
          {filtered.length === 0 ? (
            <div className="empty-state">
              <div className="empty-state-icon">🔍</div>
              <p>No clients match your filters.</p>
            </div>
          ) : (
            <table id="clients-table">
              <thead>
                <tr>
                  <th>Client</th>
                  <th>Risk Profile</th>
                  <th>AUM</th>
                  <th>YTD Return</th>
                  <th>Status</th>
                  <th>Last Contact</th>
                  <th>Joined</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                {filtered.map((c) => (
                  <tr key={c.id}>
                    <td>
                      <div className="client-cell">
                        <div className="avatar">{c.avatar}</div>
                        <div>
                          <div className="client-name">{c.name}</div>
                          <div className="client-sector">{c.sector}</div>
                        </div>
                      </div>
                    </td>
                    <td>
                      <span className={`risk-badge ${RISK_CLASS[c.risk]}`}>{c.risk}</span>
                    </td>
                    <td>
                      <span style={{ fontWeight: 700, color: 'var(--text)' }}>{fmt(c.aum)}</span>
                    </td>
                    <td>
                      <span className={c.returns > 0 ? 'change-positive' : 'change-negative'}>
                        {c.returns > 0
                          ? <TrendingUp size={13} style={{ marginRight: 4, display: 'inline' }} />
                          : <TrendingDown size={13} style={{ marginRight: 4, display: 'inline' }} />
                        }
                        {c.returns}%
                      </span>
                    </td>
                    <td>
                      <span className={`badge ${STATUS_CLASS[c.status]}`}>
                        {c.status.charAt(0).toUpperCase() + c.status.slice(1)}
                      </span>
                    </td>
                    <td style={{ color: 'var(--text-muted)', fontSize: '0.82rem' }}>{c.lastContact}</td>
                    <td style={{ color: 'var(--text-muted)', fontSize: '0.82rem' }}>
                      {new Date(c.joined).toLocaleDateString('en-US', { month: 'short', year: 'numeric' })}
                    </td>
                    <td>
                      <button className="btn btn-ghost btn-sm">View</button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>
      </div>
    </>
  );
}
