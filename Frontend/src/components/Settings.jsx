import React, { useState, useEffect } from 'react';
import * as api from '../services/api';

function Settings({ theme, setTheme, token, logout }) {
  const [activeTab, setActiveTab] = useState('profile');
  
  // Profile form state
  const [profileForm, setProfileForm] = useState({
    street: '',
    streetNumber: '',
    building: '',
    unit: '',
    city: '',
    zipCode: '',
    country: '',
    additionalInfo: '',
    phoneNumber: ''
  });
  const [isProfileLoading, setIsProfileLoading] = useState(false);
  const [profileError, setProfileError] = useState('');
  const [profileSuccess, setProfileSuccess] = useState('');

  // Notifications state (saved in localStorage for mock persistence)
  const [notifications, setNotifications] = useState({
    emailAlerts: true,
    smsAlerts: false,
    pushNotifications: true,
    priceAlerts: true
  });
  const [notifSuccess, setNotifSuccess] = useState('');

  // Investment Profile state (saved in localStorage for mock persistence)
  const [investmentProfile, setInvestmentProfile] = useState({
    riskTolerance: 'moderate',
    horizon: 'medium',
    primaryGoal: 'wealth'
  });
  const [investSuccess, setInvestSuccess] = useState('');

  // Security state
  const [securityForm, setSecurityForm] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: ''
  });
  const [securityError, setSecurityError] = useState('');
  const [securitySuccess, setSecuritySuccess] = useState('');

  // Load profile details from API
  useEffect(() => {
    const loadDetails = async () => {
      setIsProfileLoading(true);
      setProfileError('');
      try {
        const data = await api.fetchUserDetails(token, logout);
        if (data) {
          setFormDetails(data);
        }
      } catch (err) {
        setProfileError('Failed to load profile details.');
      } finally {
        setIsProfileLoading(false);
      }
    };

    if (token) {
      loadDetails();
    }

    // Load mock states from local storage if they exist
    const savedNotifs = localStorage.getItem('mock_settings_notifications');
    if (savedNotifs) setNotifications(JSON.parse(savedNotifs));

    const savedInvest = localStorage.getItem('mock_settings_investment');
    if (savedInvest) setInvestmentProfile(JSON.parse(savedInvest));
  }, [token, logout]);

  const setFormDetails = (data) => {
    setProfileForm({
      street: data.street || '',
      streetNumber: data.streetNumber || '',
      building: data.building || '',
      unit: data.unit || '',
      city: data.city || '',
      zipCode: data.zipCode || '',
      country: data.country || '',
      additionalInfo: data.additionalInfo || '',
      phoneNumber: data.phoneNumber || ''
    });
  };

  // Profile save handler
  const handleProfileSave = async (e) => {
    e.preventDefault();
    setProfileError('');
    setProfileSuccess('');
    const result = await api.updateUserDetails(profileForm, token, logout);
    if (result === true) {
      setProfileSuccess('Profile details saved successfully!');
      setTimeout(() => setProfileSuccess(''), 3000);
    } else {
      setProfileError('Failed to save details. Please try again.');
    }
  };

  // Notification toggles handler
  const handleNotifToggle = (key) => {
    setNotifications(prev => ({
      ...prev,
      [key]: !prev[key]
    }));
  };

  const handleNotifSave = () => {
    localStorage.setItem('mock_settings_notifications', JSON.stringify(notifications));
    setNotifSuccess('Notification preferences updated!');
    setTimeout(() => setNotifSuccess(''), 3000);
  };

  // Investment Profile handler
  const handleInvestChange = (key, val) => {
    setInvestmentProfile(prev => ({
      ...prev,
      [key]: val
    }));
  };

  const handleInvestSave = () => {
    localStorage.setItem('mock_settings_investment', JSON.stringify(investmentProfile));
    setInvestSuccess('Investment profile preferences saved!');
    setTimeout(() => setInvestSuccess(''), 3000);
  };

  // Security change password handler
  const handleSecuritySave = (e) => {
    e.preventDefault();
    setSecurityError('');
    setSecuritySuccess('');

    if (!securityForm.currentPassword || !securityForm.newPassword || !securityForm.confirmPassword) {
      setSecurityError('All password fields are required.');
      return;
    }
    if (securityForm.newPassword !== securityForm.confirmPassword) {
      setSecurityError('New password and confirmation do not match.');
      return;
    }
    if (securityForm.newPassword.length < 6) {
      setSecurityError('New password must be at least 6 characters.');
      return;
    }

    setSecuritySuccess('Password successfully updated (simulation)!');
    setSecurityForm({ currentPassword: '', newPassword: '', confirmPassword: '' });
    setTimeout(() => setSecuritySuccess(''), 3000);
  };

  const handleResetData = () => {
    if (window.confirm('Are you sure you want to reset your local client preferences? This will reset custom notification and investment profiles.')) {
      localStorage.removeItem('mock_settings_notifications');
      localStorage.removeItem('mock_settings_investment');
      setNotifications({
        emailAlerts: true,
        smsAlerts: false,
        pushNotifications: true,
        priceAlerts: true
      });
      setInvestmentProfile({
        riskTolerance: 'moderate',
        horizon: 'medium',
        primaryGoal: 'wealth'
      });
      alert('Preferences reset successfully.');
    }
  };

  return (
    <div className="settings-container">
      <div className="settings-header">
        <h1>Account Settings</h1>
        <p>Manage your profile, theme, and investor preferences.</p>
      </div>

      <div className="settings-layout">
        {/* Navigation Sidebar */}
        <div className="settings-sidebar">
          <button 
            className={`settings-tab-btn ${activeTab === 'profile' ? 'active' : ''}`}
            onClick={() => setActiveTab('profile')}
          >
            👤 Profile & Details
          </button>
          <button 
            className={`settings-tab-btn ${activeTab === 'investment' ? 'active' : ''}`}
            onClick={() => setActiveTab('investment')}
          >
            📈 Investment Profile
          </button>
          <button 
            className={`settings-tab-btn ${activeTab === 'notifications' ? 'active' : ''}`}
            onClick={() => setActiveTab('notifications')}
          >
            🔔 Notifications
          </button>
          <button 
            className={`settings-tab-btn ${activeTab === 'security' ? 'active' : ''}`}
            onClick={() => setActiveTab('security')}
          >
            🔒 Security & Options
          </button>
        </div>

        {/* Settings Panel Content */}
        <div className="settings-content-card">
          
          {/* PROFILE TAB */}
          {activeTab === 'profile' && (
            <div className="settings-panel">
              <h2>Profile Details</h2>
              <p className="settings-panel-desc">Update your mailing address, contact information, and account theme.</p>
              
              <div className="settings-section">
                <h3>App Theme</h3>
                <div className="theme-setting-row">
                  <span className="theme-label">Current theme: <strong>{theme.toUpperCase()}</strong></span>
                  <button 
                    type="button" 
                    className="btn-theme-select"
                    onClick={() => setTheme(prev => prev === 'light' ? 'dark' : 'light')}
                  >
                    Switch to {theme === 'light' ? '🌙 Dark Mode' : '☀️ Light Mode'}
                  </button>
                </div>
              </div>

              <hr className="settings-divider" />

              {isProfileLoading ? (
                <div className="settings-loading">Loading profile data...</div>
              ) : (
                <form onSubmit={handleProfileSave}>
                  <h3>Contact & Address</h3>
                  <div className="settings-grid">
                    <div className="settings-form-group">
                      <label>Street Name</label>
                      <input 
                        type="text"
                        value={profileForm.street}
                        onChange={(e) => setProfileForm({...profileForm, street: e.target.value})}
                        placeholder="Street"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>Number</label>
                      <input 
                        type="text"
                        value={profileForm.streetNumber}
                        onChange={(e) => setProfileForm({...profileForm, streetNumber: e.target.value})}
                        placeholder="Number"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>Building</label>
                      <input 
                        type="text"
                        value={profileForm.building}
                        onChange={(e) => setProfileForm({...profileForm, building: e.target.value})}
                        placeholder="Building"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>Unit/Apartment</label>
                      <input 
                        type="text"
                        value={profileForm.unit}
                        onChange={(e) => setProfileForm({...profileForm, unit: e.target.value})}
                        placeholder="Unit"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>City</label>
                      <input 
                        type="text"
                        value={profileForm.city}
                        onChange={(e) => setProfileForm({...profileForm, city: e.target.value})}
                        placeholder="City"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>Zip Code</label>
                      <input 
                        type="text"
                        value={profileForm.zipCode}
                        onChange={(e) => setProfileForm({...profileForm, zipCode: e.target.value})}
                        placeholder="Zip Code"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>Country</label>
                      <input 
                        type="text"
                        value={profileForm.country}
                        onChange={(e) => setProfileForm({...profileForm, country: e.target.value})}
                        placeholder="Country"
                      />
                    </div>
                    <div className="settings-form-group">
                      <label>Phone Number</label>
                      <input 
                        type="text"
                        value={profileForm.phoneNumber}
                        onChange={(e) => setProfileForm({...profileForm, phoneNumber: e.target.value})}
                        placeholder="Phone Number"
                      />
                    </div>
                  </div>
                  
                  <div className="settings-form-group full-width">
                    <label>Additional Information</label>
                    <textarea 
                      value={profileForm.additionalInfo}
                      onChange={(e) => setProfileForm({...profileForm, additionalInfo: e.target.value})}
                      placeholder="Additional details..."
                      rows="3"
                    />
                  </div>

                  {profileError && <div className="settings-error-msg">{profileError}</div>}
                  {profileSuccess && <div className="settings-success-msg">{profileSuccess}</div>}

                  <div className="settings-actions">
                    <button type="submit" className="settings-btn-save">Save Address Changes</button>
                  </div>
                </form>
              )}
            </div>
          )}

          {/* INVESTMENT TAB */}
          {activeTab === 'investment' && (
            <div className="settings-panel">
              <h2>Investment Profile</h2>
              <p className="settings-panel-desc">Configure your risk tolerance levels and target horizon for custom advice profiling.</p>
              
              <div className="settings-section">
                <h3>Risk Tolerance</h3>
                <div className="settings-options-row">
                  {['conservative', 'moderate', 'aggressive'].map(level => (
                    <button 
                      key={level}
                      type="button"
                      className={`settings-option-btn ${investmentProfile.riskTolerance === level ? 'active' : ''}`}
                      onClick={() => handleInvestChange('riskTolerance', level)}
                    >
                      {level.toUpperCase()}
                    </button>
                  ))}
                </div>
              </div>

              <div className="settings-section">
                <h3>Investment Horizon</h3>
                <select 
                  className="settings-select"
                  value={investmentProfile.horizon}
                  onChange={(e) => handleInvestChange('horizon', e.target.value)}
                >
                  <option value="short">Short-term (Less than 2 years)</option>
                  <option value="medium">Medium-term (2 to 7 years)</option>
                  <option value="long">Long-term (More than 7 years)</option>
                </select>
              </div>

              <div className="settings-section">
                <h3>Primary Goal</h3>
                <select 
                  className="settings-select"
                  value={investmentProfile.primaryGoal}
                  onChange={(e) => handleInvestChange('primaryGoal', e.target.value)}
                >
                  <option value="wealth">Wealth Accumulation & Growth</option>
                  <option value="retirement">Retirement Savings</option>
                  <option value="income">Passive Dividend Income</option>
                  <option value="speculation">Speculative Day Trading</option>
                </select>
              </div>

              {investSuccess && <div className="settings-success-msg">{investSuccess}</div>}

              <div className="settings-actions">
                <button 
                  type="button" 
                  className="settings-btn-save"
                  onClick={handleInvestSave}
                >
                  Save Investment Preferences
                </button>
              </div>
            </div>
          )}

          {/* NOTIFICATIONS TAB */}
          {activeTab === 'notifications' && (
            <div className="settings-panel">
              <h2>Notification Preferences</h2>
              <p className="settings-panel-desc">Stay updated on portfolio events, news alerts, and market updates.</p>

              <div className="toggle-list">
                <div className="toggle-item">
                  <div className="toggle-text">
                    <h4>Email Alerts</h4>
                    <p>Receive weekly summary reports and trade confirmation details by email.</p>
                  </div>
                  <label className="switch">
                    <input 
                      type="checkbox" 
                      checked={notifications.emailAlerts} 
                      onChange={() => handleNotifToggle('emailAlerts')}
                    />
                    <span className="slider round"></span>
                  </label>
                </div>

                <div className="toggle-item">
                  <div className="toggle-text">
                    <h4>SMS Text Alerts</h4>
                    <p>Get instant text alerts on critical margin notifications or portfolio drawdowns.</p>
                  </div>
                  <label className="switch">
                    <input 
                      type="checkbox" 
                      checked={notifications.smsAlerts} 
                      onChange={() => handleNotifToggle('smsAlerts')}
                    />
                    <span className="slider round"></span>
                  </label>
                </div>

                <div className="toggle-item">
                  <div className="toggle-text">
                    <h4>Push Notifications</h4>
                    <p>Get browser notifications when important stock price levels are reached.</p>
                  </div>
                  <label className="switch">
                    <input 
                      type="checkbox" 
                      checked={notifications.pushNotifications} 
                      onChange={() => handleNotifToggle('pushNotifications')}
                    />
                    <span className="slider round"></span>
                  </label>
                </div>

                <div className="toggle-item">
                  <div className="toggle-text">
                    <h4>Price Alert Updates</h4>
                    <p>Receive notifications for sudden price moves (&gt; 5%) of stocks in your Holdings list.</p>
                  </div>
                  <label className="switch">
                    <input 
                      type="checkbox" 
                      checked={notifications.priceAlerts} 
                      onChange={() => handleNotifToggle('priceAlerts')}
                    />
                    <span className="slider round"></span>
                  </label>
                </div>
              </div>

              {notifSuccess && <div className="settings-success-msg">{notifSuccess}</div>}

              <div className="settings-actions">
                <button 
                  type="button" 
                  className="settings-btn-save"
                  onClick={handleNotifSave}
                >
                  Save Notification Setup
                </button>
              </div>
            </div>
          )}

          {/* SECURITY & OPTIONS TAB */}
          {activeTab === 'security' && (
            <div className="settings-panel">
              <h2>Security & System Options</h2>
              <p className="settings-panel-desc">Configure security details and reset local settings configurations.</p>
              
              <form onSubmit={handleSecuritySave} className="settings-section">
                <h3>Update Password</h3>
                <div className="settings-grid">
                  <div className="settings-form-group">
                    <label>Current Password</label>
                    <input 
                      type="password"
                      value={securityForm.currentPassword}
                      onChange={(e) => setSecurityForm({...securityForm, currentPassword: e.target.value})}
                      placeholder="••••••••"
                    />
                  </div>
                  <div className="settings-form-group">
                    <label>New Password</label>
                    <input 
                      type="password"
                      value={securityForm.newPassword}
                      onChange={(e) => setSecurityForm({...securityForm, newPassword: e.target.value})}
                      placeholder="Minimum 6 characters"
                    />
                  </div>
                  <div className="settings-form-group">
                    <label>Confirm New Password</label>
                    <input 
                      type="password"
                      value={securityForm.confirmPassword}
                      onChange={(e) => setSecurityForm({...securityForm, confirmPassword: e.target.value})}
                      placeholder="Confirm new password"
                    />
                  </div>
                </div>

                {securityError && <div className="settings-error-msg">{securityError}</div>}
                {securitySuccess && <div className="settings-success-msg">{securitySuccess}</div>}

                <div className="settings-actions">
                  <button type="submit" className="settings-btn-save">Update Password</button>
                </div>
              </form>

              <hr className="settings-divider" />

              <div className="settings-section">
                <h3>System Reset Actions</h3>
                <p className="warning-text">Below action resets local preferences back to starting settings defaults.</p>
                <div className="settings-actions" style={{ justifyContent: 'flex-start' }}>
                  <button 
                    type="button" 
                    className="btn-danger-outline"
                    onClick={handleResetData}
                  >
                    Reset Client Preferences
                  </button>
                </div>
              </div>
            </div>
          )}

        </div>
      </div>
    </div>
  );
}

export default Settings;
