import apiClient from './apiClient.js'

export const fetchClients = async () => {
  const { data } = await apiClient.get('/clients')
  return data
}

export const fetchClientById = async (id) => {
  const { data } = await apiClient.get(`/clients/${id}`)
  return data
}
