import { useEffect } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import DataGrid from '../components/DataGrid.jsx'
import { loadOrders } from '../redux/slices/ordersSlice.js'

// Screen 2 requirement 3: "you can define columns in the grid as you wish".
const columns = [
  { key: 'invoiceNo', label: 'Invoice No.' },
  { key: 'invoiceDate', label: 'Invoice Date', render: (r) => new Date(r.invoiceDate).toLocaleDateString() },
  { key: 'customerName', label: 'Customer Name' },
  { key: 'referenceNo', label: 'Reference No.' },
  { key: 'totalIncl', label: 'Total (Incl)', render: (r) => r.totalIncl.toFixed(2) }
]

export default function Home() {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  const { list, status, error } = useSelector((state) => state.orders)

  // Requirement 1: this is the first screen shown when the app runs (see App.jsx route "/").
  useEffect(() => {
    dispatch(loadOrders())
  }, [dispatch])

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-6xl mx-auto bg-white shadow rounded-lg border border-gray-200">
        <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200">
          <h1 className="text-lg font-semibold text-gray-800">Home</h1>
        </div>

        <div className="px-6 py-4">
          {/* Requirement 2: "Add New" opens the Sales Order screen. */}
          <button
            onClick={() => navigate('/sales-order')}
            className="mb-4 inline-flex items-center px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-md shadow-sm hover:bg-blue-700 transition"
          >
            + Add New
          </button>

          {status === 'loading' && <p className="text-gray-500 mb-2">Loading orders...</p>}
          {status === 'failed' && <p className="text-red-500 mb-2">Failed to load orders: {error}</p>}

          {/* Requirement 3: list of added orders; double-click a row reopens it for edit. */}
          <DataGrid
            columns={columns}
            rows={list}
            onRowDoubleClick={(row) => navigate(`/sales-order/${row.id}`)}
          />
        </div>
      </div>
    </div>
  )
}
