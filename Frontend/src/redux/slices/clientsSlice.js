import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import { fetchClients } from '../../services/clientService.js'

export const loadClients = createAsyncThunk('clients/load', async () => {
  return await fetchClients()
})

const clientsSlice = createSlice({
  name: 'clients',
  initialState: { list: [], status: 'idle' },
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(loadClients.pending, (state) => {
        state.status = 'loading'
      })
      .addCase(loadClients.fulfilled, (state, action) => {
        state.status = 'succeeded'
        state.list = action.payload
      })
  }
})

export default clientsSlice.reducer
