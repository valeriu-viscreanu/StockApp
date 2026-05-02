import { configureStore } from '@reduxjs/toolkit';
import portfolioReducer from './slices/portfolioSlice';
import marketReducer from './slices/marketSlice';
import ordersReducer from './slices/ordersSlice';
import authReducer from './slices/authSlice';
import goalsReducer from './slices/goalsSlice';

export const store = configureStore({
    reducer: {
        portfolio: portfolioReducer,
        market: marketReducer,
        orders: ordersReducer,
        auth: authReducer,
        goals: goalsReducer,
    },
});

export default store;
