import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import { fetchOrders } from '../../services/orderService.js'

export const loadOrders = createAsyncThunk('orders/load', async () => {
  return await fetchOrders()
})

const ordersSlice = createSlice({
  name: 'orders',
  initialState: { list: [], status: 'idle', error: null },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(loadOrders.pending, (state) => {
        state.status = 'loading'
      })
      .addCase(loadOrders.fulfilled, (state, action) => {
        state.status = 'succeeded'
        state.list = action.payload
      })
      .addCase(loadOrders.rejected, (state, action) => {
        state.status = 'failed'
        state.error = action.error.message
      })
  }
})

export default ordersSlice.reducer
