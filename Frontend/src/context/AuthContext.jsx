import React, { createContext, useContext, useState, useCallback, useEffect } from 'react';
import * as api from '../services/api';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [user, setUser] = useState(localStorage.getItem('email'));
  const [token, setToken] = useState(localStorage.getItem('token'));
  const [isLoggedIn, setIsLoggedIn] = useState(!!token);
  const [error, setError] = useState('');

  const handleLogout = useCallback(() => {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    setToken(null);
    setIsLoggedIn(false);
    setUser(null);
  }, []);

  const handleLogin = async (email, password) => {
    setError('');
    const result = await api.loginApi(email, password);
    if (result?.token) {
      localStorage.setItem('token', result.token);
      setToken(result.token);
      setIsLoggedIn(true);
      setUser(email);
      localStorage.setItem('email', email);
      return true;
    } else {
      setError('Invalid email or password');
      return false;
    }
  };

  const handleRegister = async (name, email, password, details) => {
    setError('');
    const result = await api.registerApi(name, email, password, details);
    if (result.data) {
      // Auto-login after registration
      return await handleLogin(email, password);
    } else {
      setError(result.error);
      return false;
    }
  };

  const clearError = () => setError('');

  return (
    <AuthContext.Provider value={{
      user,
      setUser,
      token,
      isLoggedIn,
      error,
      setError,
      clearError,
      handleLogin,
      handleRegister,
      handleLogout
    }}>
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);

  return context;
}
