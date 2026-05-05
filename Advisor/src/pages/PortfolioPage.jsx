import {
  AreaChart, Area, BarChart, Bar, XAxis, YAxis,
  ResponsiveContainer, Tooltip, CartesianGrid
} from 'recharts';
import { TrendingUp, TrendingDown, BarChart3 } from 'lucide-react';
import { portfolioHistory, allocationData, topHoldings } from '../data/mockData';

const monthlyReturns = [
  { month: 'Nov', return: 1.8 },
  { month: 'Dec', return: 2.2 },
  { month: 'Jan', return: -0.6 },
  { month: 'Feb', return: 3.1 },
  { month: 'Mar', return: 2.4 },
  { month: 'Apr', return: -0.5 },
  { month: 'May', return: 3.9 },
];

const CustomTooltip = ({ active, payload, label }) => {
  if (!active || !payload?.length) return null;
  return (
    <div style={{
      background: 'var(--bg-card)', border: '1px solid var(--border)',
      borderRadius: 8, padding: '8px 14px', fontSize: '0.8rem'
    }}>
      <p style={{ color: 'var(--text-muted)', marginBottom: 4 }}>{label}</p>
      <p style={{ color: 'var(--accent-light)', fontWeight: 700 }}>
        {payload[0].value >= 0 ? '+' : ''}{payload[0].value}%
      </p>
    </div>
  );
};

export default function PortfolioPage() {
  return (
    <>
      {/* Summary KPIs */}
      <div className="kpi-grid">
        <div className="kpi-card accent">
          <div className="kpi-top">
            <div className="kpi-icon accent"><TrendingUp size={18} /></div>
            <span className="kpi-change up">↑ vs benchmark</span>
          </div>
          <div className="kpi-value">+15.6%</div>
          <div className="kpi-label">YTD Performance</div>
        </div>
        <div className="kpi-card success">
          <div className="kpi-top">
            <div className="kpi-icon success">💼</div>
            <span className="kpi-change up">↑ 3</span>
          </div>
          <div className="kpi-value">$13.96M</div>
          <div className="kpi-label">Total Portfolio Value</div>
        </div>
        <div className="kpi-card warning">
          <div className="kpi-top">
            <div className="kpi-icon warning"><BarChart3 size={18} /></div>
            <span className="kpi-change up">Low</span>
          </div>
          <div className="kpi-value">0.62</div>
          <div className="kpi-label">Beta (Risk)</div>
        </div>
        <div className="kpi-card info">
          <div className="kpi-top">
            <div className="kpi-icon info">📊</div>
            <span className="kpi-change up">Top decile</span>
          </div>
          <div className="kpi-value">1.84</div>
          <div className="kpi-label">Sharpe Ratio</div>
        </div>
      </div>

      <div className="charts-row">
        {/* Cumulative Return Chart */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Cumulative Return</div>
              <div className="card-subtitle">Portfolio vs S&P 500 benchmark</div>
            </div>
            <div className="tabs">
              <button className="tab-btn active">6M</button>
              <button className="tab-btn">1Y</button>
              <button className="tab-btn">3Y</button>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={230}>
            <AreaChart data={portfolioHistory} margin={{ top: 4, right: 4, left: -20, bottom: 0 }}>
              <defs>
                <linearGradient id="retGrad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stopColor="#10b981" stopOpacity={0.3} />
                  <stop offset="100%" stopColor="#10b981" stopOpacity={0} />
                </linearGradient>
              </defs>
              <CartesianGrid stroke="rgba(255,255,255,0.04)" strokeDasharray="3 3" vertical={false} />
              <XAxis dataKey="month" tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} />
              <YAxis tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} tickFormatter={v => `${v}%`} />
              <Tooltip content={<CustomTooltip />} />
              <Area type="monotone" dataKey="value" stroke="#10b981" strokeWidth={2.5} fill="url(#retGrad)" dot={false} />
            </AreaChart>
          </ResponsiveContainer>
        </div>

        {/* Monthly Returns Bar */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Monthly Returns</div>
              <div className="card-subtitle">Gain / loss per month</div>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={230}>
            <BarChart data={monthlyReturns} margin={{ top: 4, right: 4, left: -20, bottom: 0 }}>
              <CartesianGrid stroke="rgba(255,255,255,0.04)" strokeDasharray="3 3" vertical={false} />
              <XAxis dataKey="month" tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} />
              <YAxis tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} tickFormatter={v => `${v}%`} />
              <Tooltip content={<CustomTooltip />} />
              <Bar dataKey="return" radius={[4, 4, 0, 0]}
                fill="#6366f1"
                label={false}
              />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* Top Holdings */}
      <div className="card">
        <div className="card-header">
          <div>
            <div className="card-title">Top Holdings</div>
            <div className="card-subtitle">Largest positions across all client portfolios</div>
          </div>
          <button className="btn btn-ghost btn-sm">View All</button>
        </div>
        <div className="table-wrap">
          <table id="holdings-table">
            <thead>
              <tr>
                <th>Symbol</th>
                <th>Weight</th>
                <th>Market Value</th>
                <th>Daily Change</th>
              </tr>
            </thead>
            <tbody>
              {topHoldings.map((h) => (
                <tr key={h.symbol}>
                  <td>
                    <div className="holding-symbol">{h.symbol}</div>
                    <div className="holding-name">{h.name}</div>
                  </td>
                  <td>
                    <div className="weight-bar-wrap">
                      <div className="weight-bar-bg">
                        <div className="weight-bar-fill" style={{ width: `${(h.weight / 10) * 100}%` }} />
                      </div>
                      <span className="weight-pct">{h.weight}%</span>
                    </div>
                  </td>
                  <td style={{ fontWeight: 700 }}>
                    ${(h.value / 1000).toFixed(0)}K
                  </td>
                  <td>
                    <span className={h.change >= 0 ? 'change-positive' : 'change-negative'}>
                      {h.change >= 0
                        ? <TrendingUp size={13} style={{ marginRight: 4, display: 'inline' }} />
                        : <TrendingDown size={13} style={{ marginRight: 4, display: 'inline' }} />
                      }
                      {h.change >= 0 ? '+' : ''}{h.change}%
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </>
  );
}
