import React from 'react';

function Advisor() {
  const advisor = {
    name: "Jonathan Specter",
    role: "Senior Financial Advisor",
    specialization: "Wealth Management & Retirement Planning",
    experience: "15+ years",
    email: "j.specter@stockapp.com",
    bio: "Jonathan is a seasoned financial advisor with over 15 years of experience in helping clients achieve their financial goals through strategic wealth management and personalized retirement planning."
  };

  return (
    <div className="container">
      <div className="header-section">
        <h1>Your Advisor</h1>
        <p>Get professional guidance for your financial journey.</p>
      </div>

      <div className="stats-grid">
        <div className="stat-card" style={{ gridColumn: '1 / -1', display: 'flex', gap: '2rem', alignItems: 'flex-start' }}>
          <div className="advisor-avatar" style={{ 
            width: '120px', 
            height: '120px', 
            borderRadius: '50%', 
            background: 'var(--primary)', 
            display: 'flex', 
            alignItems: 'center', 
            justifyContent: 'center',
            fontSize: '3rem',
            color: 'white',
            flexShrink: 0
          }}>
            JS
          </div>
          <div>
            <h2 style={{ marginBottom: '0.5rem' }}>{advisor.name}</h2>
            <p style={{ color: 'var(--primary)', fontWeight: '700', marginBottom: '1rem' }}>{advisor.role}</p>
            <p style={{ color: 'var(--text-muted)', lineHeight: '1.6' }}>{advisor.bio}</p>
            
            <div style={{ marginTop: '2rem', display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1.5rem' }}>
              <div>
                <span className="label" style={{ display: 'block', marginBottom: '0.3rem', fontSize: '0.75rem', textTransform: 'uppercase', color: 'var(--text-muted)' }}>Specialization</span>
                <strong>{advisor.specialization}</strong>
              </div>
              <div>
                <span className="label" style={{ display: 'block', marginBottom: '0.3rem', fontSize: '0.75rem', textTransform: 'uppercase', color: 'var(--text-muted)' }}>Experience</span>
                <strong>{advisor.experience}</strong>
              </div>
              <div>
                <span className="label" style={{ display: 'block', marginBottom: '0.3rem', fontSize: '0.75rem', textTransform: 'uppercase', color: 'var(--text-muted)' }}>Contact</span>
                <strong>{advisor.email}</strong>
              </div>
            </div>
            
            <button className="primary-btn" style={{ marginTop: '2rem' }}>Schedule a Meeting</button>
          </div>
        </div>

        <div className="stat-card">
          <h3>Advisor Message</h3>
          <p style={{ marginTop: '1rem', fontStyle: 'italic', color: 'var(--text-muted)' }}>
            "Hello! I've been reviewing your portfolio's performance this month. We should discuss your upcoming retirement goals to ensure we're staying on track."
          </p>
        </div>

        <div className="stat-card">
          <h3>Next Appointment</h3>
          <div style={{ marginTop: '1rem' }}>
            <p><strong>June 15, 2026</strong></p>
            <p style={{ color: 'var(--text-muted)' }}>2:30 PM - 3:30 PM</p>
            <p style={{ color: 'var(--primary)', marginTop: '0.5rem', fontWeight: '600' }}>Virtual Call</p>
          </div>
        </div>
      </div>

      <style jsx>{`
        .primary-btn {
          background: var(--primary);
          color: white;
          border: none;
          padding: 0.75rem 1.5rem;
          border-radius: 8px;
          cursor: pointer;
          font-weight: 600;
          transition: opacity 0.2s;
        }
        .primary-btn:hover {
          opacity: 0.9;
        }
      `}</style>
    </div>
  );
}

export default Advisor;
