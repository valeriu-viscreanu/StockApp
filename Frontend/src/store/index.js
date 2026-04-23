import { configureStore } from '@reduxjs/toolkit';
import portfolioReducer from './slices/portfolioSlice';
import marketReducer from './slices/marketSlice';
import ordersReducer from './slices/ordersSlice';

export const store = configureStore({
    reducer: {
        portfolio: portfolioReducer,
        market: marketReducer,
        orders: ordersReducer,
    },
});

export default store;
