import React from 'react';

function Register({ 
  handleRegister, 
  registerName, 
  setRegisterName, 
  registerEmail, 
  setRegisterEmail, 
  registerPassword, 
  setRegisterPassword, 
  error, 
  onSwitchToLogin 
}) {
  return (
    <div className="login-container">
      <div className="login-card">
        <h1>Register</h1>
        <p className="login-subtitle">Create a new account to start trading.</p>

        <form onSubmit={handleRegister}>
          <div className="form-group">
            <label>Full Name</label>
            <input
              type="text"
              value={registerName}
              onChange={(e) => setRegisterName(e.target.value)}
              className="login-input"
              required
            />
          </div>
          <div className="form-group">
            <label>Email</label>
            <input
              type="email"
              value={registerEmail}
              onChange={(e) => setRegisterEmail(e.target.value)}
              className="login-input"
              required
            />
          </div>
          <div className="form-group">
            <label>Password</label>
            <input
              type="password"
              value={registerPassword}
              onChange={(e) => setRegisterPassword(e.target.value)}
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
