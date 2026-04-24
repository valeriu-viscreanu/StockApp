import { configureStore } from '@reduxjs/toolkit';
import portfolioReducer from './slices/portfolioSlice';
import marketReducer from './slices/marketSlice';
import ordersReducer from './slices/ordersSlice';
import authReducer from './slices/authSlice';

export const store = configureStore({
    reducer: {
        portfolio: portfolioReducer,
        market: marketReducer,
        orders: ordersReducer,
        auth: authReducer,
    },
});

export default store;
