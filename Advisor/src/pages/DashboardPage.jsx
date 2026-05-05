import {
  AreaChart, Area, ResponsiveContainer, Tooltip, XAxis, YAxis,
  PieChart, Pie, Cell
} from 'recharts';
import {
  Users, DollarSign, TrendingUp, Award,
  ArrowUpRight, RefreshCw, Calendar, Activity
} from 'lucide-react';
import {
  clients, portfolioHistory, allocationData,
  recentActivity, meetings, aumHistory
} from '../data/mockData';

const fmt = (n) => n >= 1_000_000
  ? `$${(n / 1_000_000).toFixed(1)}M`
  : `$${(n / 1000).toFixed(0)}K`;

const totalAUM = clients.reduce((s, c) => s + c.aum, 0);
const avgReturn = (clients.reduce((s, c) => s + c.returns, 0) / clients.length).toFixed(1);

const ACTIVITY_ICON = {
  buy: <ArrowUpRight size={14} />,
  sell: <ArrowUpRight size={14} style={{ transform: 'rotate(90deg)' }} />,
  rebalance: <RefreshCw size={14} />,
  onboard: <Users size={14} />,
  meeting: <Calendar size={14} />,
};

const CustomTooltip = ({ active, payload, label }) => {
  if (!active || !payload?.length) return null;
  return (
    <div style={{
      background: 'var(--bg-card)', border: '1px solid var(--border)',
      borderRadius: 8, padding: '8px 14px', fontSize: '0.8rem'
    }}>
      <p style={{ color: 'var(--text-muted)', marginBottom: 4 }}>{label}</p>
      <p style={{ color: 'var(--accent-light)', fontWeight: 700 }}>
        {payload[0].value}{payload[0].name === 'value' ? '%' : 'M'}
      </p>
    </div>
  );
};

export default function DashboardPage() {
  return (
    <>
      {/* KPI Row */}
      <div className="kpi-grid">
        <div className="kpi-card accent">
          <div className="kpi-top">
            <div className="kpi-icon accent"><DollarSign size={18} /></div>
            <span className="kpi-change up">↑ 12.4%</span>
          </div>
          <div className="kpi-value">{fmt(totalAUM)}</div>
          <div className="kpi-label">Total AUM</div>
        </div>
        <div className="kpi-card success">
          <div className="kpi-top">
            <div className="kpi-icon success"><TrendingUp size={18} /></div>
            <span className="kpi-change up">↑ 3.1%</span>
          </div>
          <div className="kpi-value">{avgReturn}%</div>
          <div className="kpi-label">Avg. Portfolio Return</div>
        </div>
        <div className="kpi-card warning">
          <div className="kpi-top">
            <div className="kpi-icon warning"><Users size={18} /></div>
            <span className="kpi-change up">↑ 2</span>
          </div>
          <div className="kpi-value">{clients.length}</div>
          <div className="kpi-label">Active Clients</div>
        </div>
        <div className="kpi-card info">
          <div className="kpi-top">
            <div className="kpi-icon info"><Award size={18} /></div>
            <span className="kpi-change up">↑ 0.8</span>
          </div>
          <div className="kpi-value">4.9</div>
          <div className="kpi-label">Satisfaction Score</div>
        </div>
      </div>

      {/* Charts Row */}
      <div className="charts-row">
        {/* AUM Over Time */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">AUM Growth</div>
              <div className="card-subtitle">Last 7 months — combined client assets</div>
            </div>
            <div className="tabs">
              <button className="tab-btn active">6M</button>
              <button className="tab-btn">1Y</button>
              <button className="tab-btn">All</button>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={220}>
            <AreaChart data={aumHistory} margin={{ top: 4, right: 4, left: -20, bottom: 0 }}>
              <defs>
                <linearGradient id="aumGrad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="0%" stopColor="#6366f1" stopOpacity={0.35} />
                  <stop offset="100%" stopColor="#6366f1" stopOpacity={0} />
                </linearGradient>
              </defs>
              <XAxis dataKey="month" tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} />
              <YAxis tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} tickFormatter={v => `${v}M`} />
              <Tooltip content={<CustomTooltip />} />
              <Area type="monotone" dataKey="aum" stroke="#6366f1" strokeWidth={2.5} fill="url(#aumGrad)" dot={false} />
            </AreaChart>
          </ResponsiveContainer>
        </div>

        {/* Allocation Donut */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Asset Allocation</div>
              <div className="card-subtitle">Across all clients</div>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={160}>
            <PieChart>
              <Pie data={allocationData} cx="50%" cy="50%" innerRadius={48} outerRadius={72}
                paddingAngle={3} dataKey="value" stroke="none">
                {allocationData.map((entry, i) => (
                  <Cell key={i} fill={entry.color} />
                ))}
              </Pie>
              <Tooltip formatter={(v) => `${v}%`} contentStyle={{
                background: 'var(--bg-card)', border: '1px solid var(--border)',
                borderRadius: 8, fontSize: '0.8rem'
              }} />
            </PieChart>
          </ResponsiveContainer>
          <div className="donut-legend">
            {allocationData.map((d) => (
              <div className="legend-item" key={d.name}>
                <span className="legend-dot" style={{ background: d.color }} />
                <span className="legend-name">{d.name}</span>
                <span className="legend-val">{d.value}%</span>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Bottom Row */}
      <div className="charts-row">
        {/* Recent Activity */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Recent Activity</div>
              <div className="card-subtitle">Latest transactions and events</div>
            </div>
            <button className="btn btn-ghost btn-sm"><Activity size={13} /> View All</button>
          </div>
          <div className="activity-list">
            {recentActivity.map((a) => (
              <div className="activity-item" key={a.id}>
                <div className={`activity-dot ${a.type}`}>
                  {ACTIVITY_ICON[a.type]}
                </div>
                <div className="activity-text">
                  <div className="activity-client">{a.client}</div>
                  <div className="activity-action">{a.action} · {a.detail}</div>
                </div>
                <div className="activity-time">{a.time}</div>
              </div>
            ))}
          </div>
        </div>

        {/* Upcoming Meetings */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Upcoming Meetings</div>
              <div className="card-subtitle">This week's schedule</div>
            </div>
            <button className="btn btn-ghost btn-sm"><Calendar size={13} /> Calendar</button>
          </div>
          <div>
            {meetings.map((m) => {
              const [mon, day] = m.date.split(' ');
              return (
                <div className="meeting-item" key={m.id}>
                  <div className="meeting-date-box">
                    <span className="meeting-day">{day}</span>
                    <span className="meeting-mon">{mon}</span>
                  </div>
                  <div className="meeting-info">
                    <div className="meeting-client">{m.client}</div>
                    <div className="meeting-type">{m.type}</div>
                    <div className="meeting-meta">
                      <span>{m.time}</span>
                      <span>·</span>
                      <span>{m.mode}</span>
                    </div>
                  </div>
                  <button className="btn btn-ghost btn-sm">Join</button>
                </div>
              );
            })}
          </div>
        </div>
      </div>
    </>
  );
}
