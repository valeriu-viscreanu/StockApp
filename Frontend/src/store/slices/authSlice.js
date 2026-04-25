import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import * as api from '../../services/api';

export const loginUser = createAsyncThunk(
  'auth/login',
  async ({ email, password }, { dispatch, rejectWithValue }) => {
    dispatch(clearError());
    const data = await api.loginApi(email, password);
    if (data) {
      dispatch(loginSuccess({ email, token: data.token }));
      return data;
    } else {
      const errorMsg = 'Invalid email or password';
      dispatch(setError(errorMsg));
      return rejectWithValue(errorMsg);
    }
  }
);

export const registerUser = createAsyncThunk(
  'auth/register',
  async ({ name, email, password, details }, { dispatch, rejectWithValue }) => {
    dispatch(clearError());
    const result = await api.registerApi(name, email, password, details);
    if (result.data) {
      // Auto-login after registration
      return dispatch(loginUser({ email, password }));
    } else {
      const errorMsg = result.error || 'Registration failed';
      dispatch(setError(errorMsg));
      return rejectWithValue(errorMsg);
    }
  }
);

const initialState = {
  user: localStorage.getItem('email') || null,
  token: localStorage.getItem('token') || null,
  isLoggedIn: !!localStorage.getItem('token'),
  error: '',
  status: 'idle', // 'idle' | 'loading' | 'succeeded' | 'failed'
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
  extraReducers: (builder) => {
    builder
      .addCase(loginUser.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(loginUser.fulfilled, (state) => {
        state.status = 'succeeded';
      })
      .addCase(loginUser.rejected, (state) => {
        state.status = 'failed';
      });
  }
});

export const { loginSuccess, logout, setError, clearError, setUser } = authSlice.actions;
export default authSlice.reducer;
