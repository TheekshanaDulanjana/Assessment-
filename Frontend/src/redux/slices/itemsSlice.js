import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import { fetchItems } from '../../services/itemService.js'

export const loadItems = createAsyncThunk('items/load', async () => {
  return await fetchItems()
})

const itemsSlice = createSlice({
  name: 'items',
  initialState: { list: [], status: 'idle' },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(loadItems.pending, (state) => {
        state.status = 'loading'
      })
      .addCase(loadItems.fulfilled, (state, action) => {
        state.status = 'succeeded'
        state.list = action.payload
      })
  }
})

export default itemsSlice.reducer
