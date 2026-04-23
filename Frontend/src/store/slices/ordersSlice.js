import { createSlice } from '@reduxjs/toolkit';

const initialState = {
  buyOrders: [],
  sellOrders: [],
  isSynced: false,
};

const ordersSlice = createSlice({
  name: 'orders',
  initialState,
  reducers: {
    setOrders: (state, action) => {
      const { buyOrders, sellOrders } = action.payload;
      state.buyOrders = buyOrders || [];
      state.sellOrders = sellOrders || [];
      state.isSynced = true;
    },
    resetOrders: () => initialState,
  },
});

export const { setOrders, resetOrders } = ordersSlice.actions;
export default ordersSlice.reducer;
