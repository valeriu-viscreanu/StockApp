import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  popularStocks: [],
  selectedStock: { symbol: 'AAPL', name: 'Apple', price: 0 },
  stockChartData: null,
};

const marketSlice = createSlice({
  name: 'market',
  initialState,
  reducers: {
    setPopularStocks: (state, action) => {
      state.popularStocks = action.payload;
    },
    updateStockPrices: (state, action) => {
      const quotes = action.payload;
      state.popularStocks = state.popularStocks.map(stock => ({
        ...stock,
        price: quotes[stock.symbol]?.c || stock.price
      }));
      
      if (state.selectedStock && quotes[state.selectedStock.symbol]?.c) {
        state.selectedStock.price = quotes[state.selectedStock.symbol].c;
      }
    },
    setSelectedStock: (state, action) => {
      state.selectedStock = action.payload;
    },
    setStockChartData: (state, action) => {
      state.stockChartData = action.payload;
    }
  },
});

export const { 
  setPopularStocks, 
  updateStockPrices, 
  setSelectedStock, 
  setStockChartData 
} = marketSlice.actions;

export default marketSlice.reducer;
