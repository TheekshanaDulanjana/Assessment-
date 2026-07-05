import { configureStore } from '@reduxjs/toolkit'
import ordersReducer from './slices/ordersSlice.js'
import clientsReducer from './slices/clientsSlice.js'
import itemsReducer from './slices/itemsSlice.js'

export const store = configureStore({
  reducer: {
    orders: ordersReducer,
    clients: clientsReducer,
    items: itemsReducer
  }
})
