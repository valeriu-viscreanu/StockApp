import { useState } from 'react';
import { Save } from 'lucide-react';

export default function SettingsPage() {
  const [savedMessage, setSavedMessage] = useState('');
  
  // State for General Profile details only
  const [profile, setProfile] = useState({
    name: 'Alex Rivera',
    email: 'alex.rivera@advisor.com',
    role: 'Senior Wealth Advisor',
    phone: '+1 (555) 234-5678',
    avatarLetter: 'A'
  });

  const handleProfileChange = (e) => {
    const { name, value } = e.target;
    setProfile(prev => ({ ...prev, [name]: value }));
  };

  const saveSettings = () => {
    setSavedMessage('Profile settings saved successfully!');
    setTimeout(() => setSavedMessage(''), 3000);
  };

  return (
    <div className="settings-container">
      {savedMessage && (
        <div className="settings-toast success">
          <span>{savedMessage}</span>
        </div>
      )}

      <div className="settings-content">
        <div className="card">
          <div className="card-header">
            <div>
              <h3 className="card-title">Advisor Information</h3>
              <p className="card-subtitle">Manage your personal details and public profile info</p>
            </div>
          </div>
          
          <div className="settings-form">
            <div className="avatar-upload-section">
              <div className="user-avatar" style={{ width: 64, height: 64, fontSize: '1.5rem' }}>
                {profile.avatarLetter}
              </div>
              <div className="avatar-upload-info">
                <h4 style={{ fontSize: '0.9rem', fontWeight: 600, color: 'var(--text)' }}>Profile Picture</h4>
                <p style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Avatar generated from initials. Uploads disabled in test mode.</p>
              </div>
            </div>

            <div className="form-grid">
              <div className="form-group">
                <label>Full Name</label>
                <input 
                  type="text" 
                  name="name" 
                  value={profile.name} 
                  onChange={handleProfileChange}
                  className="search-input"
                  style={{ paddingLeft: 12 }}
                />
              </div>
              
              <div className="form-group">
                <label>Email Address</label>
                <input 
                  type="email" 
                  name="email" 
                  value={profile.email} 
                  onChange={handleProfileChange}
                  className="search-input"
                  style={{ paddingLeft: 12 }}
                  disabled
                />
                <span className="input-hint">Email address cannot be changed. Contact admin.</span>
              </div>

              <div className="form-group">
                <label>Job Title</label>
                <input 
                  type="text" 
                  name="role" 
                  value={profile.role} 
                  onChange={handleProfileChange}
                  className="search-input"
                  style={{ paddingLeft: 12 }}
                />
              </div>

              <div className="form-group">
                <label>Phone Number</label>
                <input 
                  type="text" 
                  name="phone" 
                  value={profile.phone} 
                  onChange={handleProfileChange}
                  className="search-input"
                  style={{ paddingLeft: 12 }}
                />
              </div>
            </div>

            <div style={{ marginTop: 24, display: 'flex', justifyContent: 'flex-end' }}>
              <button className="btn btn-primary" onClick={saveSettings}>
                <Save size={16} /> Save Changes
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
