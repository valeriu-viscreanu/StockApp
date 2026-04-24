import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  user: localStorage.getItem('email') || null,
  token: localStorage.getItem('token') || null,
  isLoggedIn: !!localStorage.getItem('token'),
  error: '',
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginSuccess: (state, action) => {
      const { email, token } = action.payload;
      state.user = email;
      state.token = token;
      state.isLoggedIn = true;
      state.error = '';
      localStorage.setItem('token', token);
      localStorage.setItem('email', email);
    },
    logout: (state) => {
      state.user = null;
      state.token = null;
      state.isLoggedIn = false;
      state.error = '';
      localStorage.removeItem('token');
      localStorage.removeItem('email');
    },
    setError: (state, action) => {
      state.error = action.payload;
    },
    clearError: (state) => {
      state.error = '';
    },
    setUser: (state, action) => {
      state.user = action.payload;
      localStorage.setItem('email', action.payload);
    }
  },
});

export const { loginSuccess, logout, setError, clearError, setUser } = authSlice.actions;
export default authSlice.reducer;
