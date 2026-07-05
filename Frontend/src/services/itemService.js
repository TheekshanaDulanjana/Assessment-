import apiClient from './apiClient.js'

export const fetchItems = async () => {
  const { data } = await apiClient.get('/items')
  return data
}
