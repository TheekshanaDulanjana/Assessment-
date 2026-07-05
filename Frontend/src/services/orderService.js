import apiClient from './apiClient.js'

export const fetchOrders = async () => {
  const { data } = await apiClient.get('/salesorders')
  return data
}

export const fetchOrderById = async (id) => {
  const { data } = await apiClient.get(`/salesorders/${id}`)
  return data
}

export const createOrder = async (payload) => {
  const { data } = await apiClient.post('/salesorders', payload)
  return data
}

export const updateOrder = async (id, payload) => {
  const { data } = await apiClient.put(`/salesorders/${id}`, payload)
  return data
}

export const fetchNextInvoiceNo = async () => {
  const { data } = await apiClient.get('/salesorders/next-invoice-no')
  return data.invoiceNo
}

export const printOrder = (id) => {
  window.open(`${apiClient.defaults.baseURL}/salesorders/${id}/print`, '_blank')
}
