import React from 'react';

function Login({ handleLogin, loginEmail, setLoginEmail, loginPassword, setLoginPassword, error, onSwitchToRegister }) {
  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Login</h1>
        <p className="login-subtitle">Welcome back! Please login to your account.</p>

        <form onSubmit={handleLogin}>
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              value={loginEmail}
              onChange={(e) => setLoginEmail(e.target.value)}
              className="login-input"
              required
            />
          </div>
          <div className="form-group">
            <label>Password</label>
            <input
              type="password"
              value={loginPassword}
              onChange={(e) => setLoginPassword(e.target.value)}
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
