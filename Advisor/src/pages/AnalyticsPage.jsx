import {
  LineChart, Line, BarChart, Bar, RadarChart, Radar, PolarGrid,
  PolarAngleAxis, XAxis, YAxis, ResponsiveContainer, Tooltip,
  CartesianGrid, Legend
} from 'recharts';
import { clients } from '../data/mockData';

const riskReturnData = clients.map(c => ({
  name: c.name.split(' ')[0],
  risk: c.risk === 'Conservative' ? 1 : c.risk === 'Moderate' ? 2 : 3,
  return: c.returns,
  aum: c.aum / 1_000_000,
}));

const benchmarkData = [
  { month: 'Nov', portfolio: 8.4, sp500: 6.1 },
  { month: 'Dec', portfolio: 9.2, sp500: 7.0 },
  { month: 'Jan', portfolio: 10.1, sp500: 7.8 },
  { month: 'Feb', portfolio: 12.3, sp500: 8.5 },
  { month: 'Mar', portfolio: 13.8, sp500: 9.0 },
  { month: 'Apr', portfolio: 13.2, sp500: 9.8 },
  { month: 'May', portfolio: 15.6, sp500: 10.3 },
];

const radarData = [
  { metric: 'Returns', value: 88 },
  { metric: 'Risk Mgmt', value: 72 },
  { metric: 'Diversification', value: 81 },
  { metric: 'Client Sat.', value: 95 },
  { metric: 'Compliance', value: 99 },
  { metric: 'ESG Score', value: 65 },
];

const aumByClient = clients.map(c => ({
  name: c.name.split(' ')[0],
  aum: +(c.aum / 1_000_000).toFixed(2),
}));

const CustomTooltip = ({ active, payload, label }) => {
  if (!active || !payload?.length) return null;
  return (
    <div style={{
      background: 'var(--bg-card)', border: '1px solid var(--border)',
      borderRadius: 8, padding: '10px 14px', fontSize: '0.8rem'
    }}>
      <p style={{ color: 'var(--text-muted)', marginBottom: 6 }}>{label}</p>
      {payload.map((p, i) => (
        <p key={i} style={{ color: p.color, fontWeight: 700 }}>
          {p.name}: {typeof p.value === 'number' ? (Number.isInteger(p.value) ? p.value : p.value.toFixed(2)) : p.value}
          {p.name === 'aum' ? 'M' : '%'}
        </p>
      ))}
    </div>
  );
};

export default function AnalyticsPage() {
  return (
    <>
      {/* Benchmark Comparison */}
      <div className="card" style={{ marginBottom: 20 }}>
        <div className="card-header">
          <div>
            <div className="card-title">Portfolio vs S&P 500</div>
            <div className="card-subtitle">Cumulative returns — 7 month comparison</div>
          </div>
          <div className="tabs">
            <button className="tab-btn active">YTD</button>
            <button className="tab-btn">1Y</button>
            <button className="tab-btn">3Y</button>
          </div>
        </div>
        <ResponsiveContainer width="100%" height={250}>
          <LineChart data={benchmarkData} margin={{ top: 4, right: 8, left: -20, bottom: 0 }}>
            <CartesianGrid stroke="rgba(255,255,255,0.04)" strokeDasharray="3 3" vertical={false} />
            <XAxis dataKey="month" tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} />
            <YAxis tick={{ fill: 'var(--text-muted)', fontSize: 11 }} axisLine={false} tickLine={false} tickFormatter={v => `${v}%`} />
            <Tooltip content={<CustomTooltip />} />
            <Legend wrapperStyle={{ fontSize: '0.8rem', color: 'var(--text-muted)', paddingTop: 12 }} />
            <Line type="monotone" dataKey="portfolio" name="Your Portfolio" stroke="#6366f1" strokeWidth={2.5} dot={false} />
            <Line type="monotone" dataKey="sp500" name="S&P 500" stroke="#94a3b8" strokeWidth={2} strokeDasharray="5 5" dot={false} />
          </LineChart>
        </ResponsiveContainer>
      </div>

      <div className="charts-row-3">
        {/* AUM by Client */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">AUM by Client</div>
              <div className="card-subtitle">In millions (USD)</div>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={200}>
            <BarChart data={aumByClient} margin={{ top: 4, right: 4, left: -24, bottom: 0 }}>
              <CartesianGrid stroke="rgba(255,255,255,0.04)" vertical={false} />
              <XAxis dataKey="name" tick={{ fill: 'var(--text-muted)', fontSize: 10 }} axisLine={false} tickLine={false} />
              <YAxis tick={{ fill: 'var(--text-muted)', fontSize: 10 }} axisLine={false} tickLine={false} />
              <Tooltip content={<CustomTooltip />} />
              <Bar dataKey="aum" fill="#6366f1" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Advisor Radar */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Advisor Score</div>
              <div className="card-subtitle">Performance across 6 dimensions</div>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={200}>
            <RadarChart data={radarData} margin={{ top: 4, right: 20, left: 20, bottom: 4 }}>
              <PolarGrid stroke="rgba(255,255,255,0.07)" />
              <PolarAngleAxis dataKey="metric" tick={{ fill: 'var(--text-muted)', fontSize: 10 }} />
              <Radar name="Score" dataKey="value" stroke="#6366f1" fill="#6366f1" fillOpacity={0.25} />
              <Tooltip contentStyle={{
                background: 'var(--bg-card)', border: '1px solid var(--border)',
                borderRadius: 8, fontSize: '0.8rem'
              }} />
            </RadarChart>
          </ResponsiveContainer>
        </div>

        {/* Risk vs Return */}
        <div className="card">
          <div className="card-header">
            <div>
              <div className="card-title">Return by Risk</div>
              <div className="card-subtitle">Client segmentation overview</div>
            </div>
          </div>
          <div style={{ padding: '12px 0' }}>
            {riskReturnData.map((c) => (
              <div key={c.name} style={{
                display: 'flex', alignItems: 'center', gap: 10,
                padding: '7px 0', borderBottom: '1px solid var(--border)'
              }}>
                <div className="avatar sm">{c.name[0]}</div>
                <span style={{ flex: 1, fontSize: '0.8rem', color: 'var(--text-muted)' }}>{c.name}</span>
                <div style={{ flex: 2 }}>
                  <div style={{
                    height: 5, background: 'rgba(255,255,255,0.06)',
                    borderRadius: 99, overflow: 'hidden'
                  }}>
                    <div style={{
                      width: `${(c.return / 35) * 100}%`,
                      height: '100%',
                      background: c.risk === 1 ? 'var(--info)' : c.risk === 2 ? 'var(--warning)' : 'var(--danger)',
                      borderRadius: 99,
                      transition: '0.4s ease'
                    }} />
                  </div>
                </div>
                <span style={{ fontSize: '0.8rem', fontWeight: 700, minWidth: 42, textAlign: 'right' }}
                  className={c.return > 0 ? 'change-positive' : 'change-negative'}>
                  +{c.return}%
                </span>
              </div>
            ))}
          </div>
        </div>
      </div>
    </>
  );
}
