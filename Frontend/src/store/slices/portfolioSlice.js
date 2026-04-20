import { createSlice } from '@reduxjs/toolkit';

const initialState = {
    holdings: [],
    balance: 0,
    totalValue: 0,
    stockValue: 0,
    stocksCount: 0,
    isSynced: false,
};

const portfolioSlice = createSlice({
    name: 'portfolio',
    initialState,
    reducers: {
        setPortfolioData: (state, action) => {
            const { holdings, balance, totalValue, stockValue, stocksCount } = action.payload;
            state.holdings = holdings !== undefined ? holdings : state.holdings;
            state.balance = balance !== undefined ? balance : state.balance;
            state.totalValue = totalValue !== undefined ? totalValue : state.totalValue;
            state.stockValue = stockValue !== undefined ? stockValue : state.stockValue;
            state.stocksCount = stocksCount !== undefined ? stocksCount : state.stocksCount;
            state.isSynced = true;
        },
        updateHoldings: (state, action) => {
            state.holdings = action.payload;
            state.isSynced = true;
        },
        updateBalance: (state, action) => {
            state.balance = action.payload;
            state.isSynced = true;
        },
        resetPortfolio: () => initialState,
    },
});

export const { setPortfolioData, updateHoldings, updateBalance, resetPortfolio } = portfolioSlice.actions;
export default portfolioSlice.reducer;
