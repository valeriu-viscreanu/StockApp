import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';

function Register({ onSwitchToLogin }) {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { handleRegister, error } = useAuth();

  const onSubmit = (e) => {
    e.preventDefault();
    handleRegister(name, email, password);
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Register</h1>
        <p className="login-subtitle">Create a new account to start trading.</p>

        <form onSubmit={onSubmit}>
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
          <button type="submit" className="login-button">
            Register
          </button>
        </form>
        
        <p className="auth-switch">
          Already have an account? <button onClick={onSwitchToLogin} className="link-button">Login</button>
        </p>
      </div>
    </div>
  );
}

export default Register;
