import { useState } from 'react';
import { loginApi } from '../services/api';
import { Lock, Mail, ArrowRight, Loader2 } from 'lucide-react';

const LoginPage = ({ onLoginSuccess }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    const result = await loginApi(email, password);
    if (result && result.token) {
      onLoginSuccess(result.token, { email });
    } else {
      setError('Invalid email or password. Use advisor@test.com / 123');
    }
    setLoading(false);
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <div className="login-header">
          <div className="login-logo">
            <div className="logo-icon">
              <Lock size={24} />
            </div>
            <h1>Advisory</h1>
          </div>
          <p>Login to manage your client portfolios</p>
        </div>

        <form onSubmit={handleSubmit} className="login-form">
          <div className="input-group">
            <label htmlFor="email">Email Address</label>
            <div className="input-with-icon">
              <Mail className="field-icon" size={18} />
              <input
                id="email"
                type="email"
                placeholder="advisor@test.com"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
          </div>

          <div className="input-group">
            <label htmlFor="password">Password</label>
            <div className="input-with-icon">
              <Lock className="field-icon" size={18} />
              <input
                id="password"
                type="password"
                placeholder="••••••••"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
          </div>

          {error && <div className="login-error">{error}</div>}

          <button type="submit" className="login-button" disabled={loading}>
            {loading ? (
              <Loader2 className="animate-spin" size={20} />
            ) : (
              <>
                Continue to Dashboard
                <ArrowRight size={20} />
              </>
            )}
          </button>
        </form>

        <div className="login-footer">
          <p>Don't have an advisor account? <span>Contact Support</span></p>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;
