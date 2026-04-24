import React, { createContext, useContext, useCallback } from 'react';
import * as api from '../services/api';
import { useDispatch, useSelector } from 'react-redux';
import { loginSuccess, logout, setError, clearError, setUser as setReduxUser } from '../store/slices/authSlice';

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const dispatch = useDispatch();
  const { user, token, isLoggedIn, error } = useSelector(state => state.auth);

  const handleLogout = useCallback(() => {
    dispatch(logout());
  }, [dispatch]);

  const handleLogin = async (email, password) => {
    dispatch(clearError());
    const result = await api.loginApi(email, password);
    if (result?.token) {
      dispatch(loginSuccess({ email, token: result.token }));
      return true;
    } else {
      dispatch(setError('Invalid email or password'));
      return false;
    }
  };

  const handleRegister = async (name, email, password, details) => {
    dispatch(clearError());
    const result = await api.registerApi(name, email, password, details);
    if (result.data) {
      // Auto-login after registration
      return await handleLogin(email, password);
    } else {
      dispatch(setError(result.error));
      return false;
    }
  };

  const handleSetUser = (name) => {
    dispatch(setReduxUser(name));
  };

  return (
    <AuthContext.Provider value={{
      user,
      setUser: handleSetUser,
      token,
      isLoggedIn,
      error,
      setError: (msg) => dispatch(setError(msg)),
      clearError: () => dispatch(clearError()),
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
