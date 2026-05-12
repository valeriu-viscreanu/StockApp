import { X, User, Mail, Shield, Briefcase, DollarSign, UserPlus, Loader2 } from 'lucide-react';
import { useState, useEffect } from 'react';
import { createPortal } from 'react-dom';

export default function RegisterClientModal({ isOpen, onClose }) {
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    name: '',
    email: '',
    risk: 'Moderate',
    sector: '',
    status: 'onboarding',
    initialAum: ''
  });

  // Handle ESC key to close
  useEffect(() => {
    const handleEsc = (e) => {
      if (e.key === 'Escape') onClose();
    };
    window.addEventListener('keydown', handleEsc);
    return () => window.removeEventListener('keydown', handleEsc);
  }, [onClose]);

  if (!isOpen) return null;

  const handleSubmit = (e) => {
    e.preventDefault();
    setLoading(true);
    // Simulate API call
    setTimeout(() => {
      console.log('Registering client:', formData);
      setLoading(false);
      onClose();
      // Reset form
      setFormData({
        name: '',
        email: '',
        risk: 'Moderate',
        sector: '',
        status: 'onboarding',
        initialAum: ''
      });
    }, 1500);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  return createPortal(
    <div className="modal-overlay" onClick={onClose}>
      <div className="reg-modal" onClick={(e) => e.stopPropagation()}>
        <div className="reg-modal-header">
          <div className="reg-modal-title-area">
            <div className="reg-modal-icon">
              <UserPlus size={20} />
            </div>
            <div>
              <h3>Register New Client</h3>
              <p>Onboard a new high-net-worth individual</p>
            </div>
          </div>
          <button className="search-modal-close" onClick={onClose}>
            <X size={18} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className="reg-modal-body">
          <div className="reg-form-grid">
            {/* Left Column */}
            <div className="reg-form-col">
              <div className="input-group">
                <label htmlFor="reg-name">Full Name</label>
                <div className="input-with-icon">
                  <User size={16} className="field-icon" />
                  <input
                    id="reg-name"
                    name="name"
                    type="text"
                    placeholder="e.g. Alexander Vance"
                    required
                    value={formData.name}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="input-group">
                <label htmlFor="reg-email">Email Address</label>
                <div className="input-with-icon">
                  <Mail size={16} className="field-icon" />
                  <input
                    id="reg-email"
                    name="email"
                    type="email"
                    placeholder="c.vance@example.com"
                    required
                    value={formData.email}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="input-group">
                <label htmlFor="reg-risk">Risk Profile</label>
                <div className="input-with-icon">
                  <Shield size={16} className="field-icon" />
                  <select
                    id="reg-risk"
                    name="risk"
                    className="reg-select"
                    value={formData.risk}
                    onChange={handleChange}
                  >
                    <option>Conservative</option>
                    <option>Moderate</option>
                    <option>Aggressive</option>
                  </select>
                </div>
              </div>
            </div>

            {/* Right Column */}
            <div className="reg-form-col">
              <div className="input-group">
                <label htmlFor="reg-sector">Primary Sector</label>
                <div className="input-with-icon">
                  <Briefcase size={16} className="field-icon" />
                  <input
                    id="reg-sector"
                    name="sector"
                    type="text"
                    placeholder="e.g. Technology"
                    required
                    value={formData.sector}
                    onChange={handleChange}
                  />
                </div>
              </div>

              <div className="input-group">
                <label htmlFor="reg-status">Client Status</label>
                <select
                  id="reg-status"
                  name="status"
                  className="reg-select-no-icon"
                  value={formData.status}
                  onChange={handleChange}
                >
                  <option value="active">Active</option>
                  <option value="review">Review</option>
                  <option value="onboarding">Onboarding</option>
                </select>
              </div>

              <div className="input-group">
                <label htmlFor="reg-aum">Initial AUM ($)</label>
                <div className="input-with-icon">
                  <DollarSign size={16} className="field-icon" />
                  <input
                    id="reg-aum"
                    name="initialAum"
                    type="number"
                    placeholder="e.g. 500000"
                    required
                    value={formData.initialAum}
                    onChange={handleChange}
                  />
                </div>
              </div>
            </div>
          </div>

          <div className="reg-modal-footer">
            <button type="button" className="btn btn-ghost" onClick={onClose} disabled={loading}>
              Cancel
            </button>
            <button type="submit" className="btn btn-primary" disabled={loading}>
              {loading ? (
                <>
                  <Loader2 size={16} className="animate-spin" />
                  Registering...
                </>
              ) : (
                'Confirm Registration'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  , document.body);
}
