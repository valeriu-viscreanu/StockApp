import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import { fetchFinancialGoals, fetchGoalTypes, createFinancialGoal } from '../../services/api';

export const getGoals = createAsyncThunk(
  'goals/getGoals',
  async ({ token, logout }, { rejectWithValue }) => {
    const data = await fetchFinancialGoals(token, logout);
    if (!data) return rejectWithValue('Failed to fetch goals');
    return data;
  }
);

export const getGoalTypes = createAsyncThunk(
  'goals/getGoalTypes',
  async ({ token, logout }, { rejectWithValue }) => {
    const data = await fetchGoalTypes(token, logout);
    if (!data) return rejectWithValue('Failed to fetch goal types');
    return data;
  }
);

export const addGoal = createAsyncThunk(
  'goals/addGoal',
  async ({ goalData, token, logout }, { rejectWithValue }) => {
    const data = await createFinancialGoal(goalData, token, logout);
    if (!data) return rejectWithValue('Failed to create goal');
    return data;
  }
);

const goalsSlice = createSlice({
  name: 'goals',
  initialState: {
    goals: [],
    types: [],
    loading: false,
    error: null
  },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(getGoals.pending, (state) => {
        state.loading = true;
      })
      .addCase(getGoals.fulfilled, (state, action) => {
        state.loading = false;
        state.goals = action.payload;
      })
      .addCase(getGoals.rejected, (state, action) => {
        state.loading = false;
        state.error = action.payload;
      })
      .addCase(getGoalTypes.fulfilled, (state, action) => {
        state.types = action.payload;
      })
      .addCase(addGoal.fulfilled, (state, action) => {
        state.goals.push(action.payload);
      });
  }
});

export default goalsSlice.reducer;
