import React, { useState } from 'react';
import { useAuth } from '../context/AuthContext';

function Login({ onSwitchToRegister }) {
  const [email, setEmail] = useState('admin@test.com');
  const [password, setPassword] = useState('123');
  const { handleLogin, error } = useAuth();

  const onSubmit = (e) => {
    e.preventDefault();
    handleLogin(email, password);
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Login</h1>
        <p className="login-subtitle">Welcome back! Please login to your account.</p>

        <form onSubmit={onSubmit}>
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
            Login
          </button>
        </form>

        <p className="auth-switch">
          Don't have an account? <button onClick={onSwitchToRegister} className="link-button">Register</button>
        </p>
      </div>
    </div>
  );
}

export default Login;
