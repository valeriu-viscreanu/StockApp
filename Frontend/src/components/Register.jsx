import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';

function Register({ onSwitchToLogin }) {
  const [step, setStep] = useState(1);
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  
  // New User Details State
  const [details, setDetails] = useState({
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

  const { handleRegister, error } = useAuth();

  const handleDetailsChange = (e) => {
    const { name, value } = e.target;
    setDetails(prev => ({ ...prev, [name]: value }));
  };

  const onSubmit = (e) => {
    e.preventDefault();
    handleRegister(name, email, password, details);
  };

  const nextStep = (e) => {
    e.preventDefault();
    setStep(2);
  };

  const prevStep = () => setStep(1);

  return (
    <div className="login-container">
      <div className="login-card registration-card">
        <div className="registration-header">
          <h1>Register</h1>
          <div className="step-indicator">Step {step} of 2</div>
        </div>
        
        <p className="login-subtitle">
          {step === 1 ? 'Tell us about yourself.' : 'Secure your account.'}
        </p>

        <form onSubmit={step === 1 ? nextStep : onSubmit}>
          {step === 1 && (
            <div className="form-section fade-in">
              <h3>Address & Contact</h3>
              <div className="form-row">
                <div className="form-group flex-2">
                  <label>Street</label>
                  <input
                    type="text"
                    name="street"
                    value={details.street}
                    onChange={handleDetailsChange}
                    className="login-input"
                    placeholder="Main St"
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Number</label>
                  <input
                    type="text"
                    name="streetNumber"
                    value={details.streetNumber}
                    onChange={handleDetailsChange}
                    className="login-input"
                    placeholder="123"
                    required
                  />
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>Building</label>
                  <input
                    type="text"
                    name="building"
                    value={details.building}
                    onChange={handleDetailsChange}
                    className="login-input"
                  />
                </div>
                <div className="form-group">
                  <label>Unit/Apt</label>
                  <input
                    type="text"
                    name="unit"
                    value={details.unit}
                    onChange={handleDetailsChange}
                    className="login-input"
                  />
                </div>
              </div>

              <div className="form-row">
                <div className="form-group">
                  <label>City</label>
                  <input
                    type="text"
                    name="city"
                    value={details.city}
                    onChange={handleDetailsChange}
                    className="login-input"
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Zip Code</label>
                  <input
                    type="text"
                    name="zipCode"
                    value={details.zipCode}
                    onChange={handleDetailsChange}
                    className="login-input"
                    required
                  />
                </div>
              </div>
              
              <div className="form-row">
                <div className="form-group">
                  <label>Country</label>
                  <input
                    type="text"
                    name="country"
                    value={details.country}
                    onChange={handleDetailsChange}
                    className="login-input"
                    required
                  />
                </div>
                <div className="form-group">
                  <label>Phone Number</label>
                  <input
                    type="text"
                    name="phoneNumber"
                    value={details.phoneNumber}
                    onChange={handleDetailsChange}
                    className="login-input"
                    required
                  />
                </div>
              </div>

              <button type="submit" className="login-button">
                Next Step
              </button>
            </div>
          )}

          {step === 2 && (
            <div className="form-section fade-in">
              <h3>Account Info</h3>
              <div className="form-group">
                <label>Full Name</label>
                <input
                  type="text"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  className="login-input"
                  required
                />
              </div>
              <div className="form-group">
                <label>Email</label>
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="login-input"
                  required
                />
              </div>
              <div className="form-group">
                <label>Password</label>
                <input
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="login-input"
                  required
                />
              </div>

              {error && <p className="login-error">{error}</p>}
              
              <div className="form-row">
                <button type="button" onClick={prevStep} className="login-button secondary-button">
                  Back
                </button>
                <button type="submit" className="login-button">
                  Register
                </button>
              </div>
            </div>
          )}
        </form>
        
        <p className="auth-switch">
          Already have an account? <button onClick={onSwitchToLogin} className="link-button">Login</button>
        </p>
      </div>
    </div>
  );
}

export default Register;
