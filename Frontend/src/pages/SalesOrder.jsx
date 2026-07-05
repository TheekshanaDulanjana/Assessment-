import { useEffect, useMemo, useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate, useParams } from 'react-router-dom'
import FormField from '../components/FormField.jsx'
import OrderItemsTable from '../components/OrderItemsTable.jsx'
import { loadClients } from '../redux/slices/clientsSlice.js'
import { loadItems } from '../redux/slices/itemsSlice.js'
import {
  createOrder,
  updateOrder,
  fetchOrderById,
  fetchNextInvoiceNo,
  printOrder
} from '../services/orderService.js'

const emptyLine = () => ({
  itemCode: '', description: '', note: '', quantity: '', price: '', taxRate: ''
})

export default function SalesOrder() {
  const dispatch = useDispatch()
  const navigate = useNavigate()
  const { id } = useParams()
  const isEdit = Boolean(id)

  const clients = useSelector((state) => state.clients.list)
  const items = useSelector((state) => state.items.list)

  const [clientId, setClientId] = useState('')
  const [address, setAddress] = useState({
    address1: '', address2: '', address3: '', suburb: '', state: '', postCode: ''
  })
  const [invoiceNo, setInvoiceNo] = useState('')
  const [invoiceDate, setInvoiceDate] = useState(new Date().toISOString().slice(0, 10))
  const [referenceNo, setReferenceNo] = useState('')
  const [note, setNote] = useState('')
  const [orderItems, setOrderItems] = useState([emptyLine()])
  const [saving, setSaving] = useState(false)
  const [errorMsg, setErrorMsg] = useState('')

  // Populate the Customer Name and Item Code/Description dropdowns.
  useEffect(() => {
    dispatch(loadClients())
    dispatch(loadItems())
  }, [dispatch])

  // Requirement (Screen 2 #3): opening an existing order loads it back into this form.
  useEffect(() => {
    if (isEdit) {
      fetchOrderById(id).then((order) => {
        setClientId(order.clientId)
        setAddress({
          address1: order.address1 || '',
          address2: order.address2 || '',
          address3: order.address3 || '',
          suburb: order.suburb || '',
          state: order.state || '',
          postCode: order.postCode || ''
        })
        setInvoiceNo(order.invoiceNo)
        setInvoiceDate(order.invoiceDate.slice(0, 10))
        setReferenceNo(order.referenceNo || '')
        setNote(order.note || '')
        setOrderItems(
          order.items.map((i) => ({
            itemCode: i.itemCode,
            description: i.description,
            note: i.note || '',
            quantity: i.quantity,
            price: i.price,
            taxRate: i.taxRate
          }))
        )
      })
    } else {
      // Convenience default for a new order; still fully editable by the user (requirement 3).
      fetchNextInvoiceNo().then(setInvoiceNo).catch(() => {})
    }
  }, [id, isEdit])

  // Requirement 2: selecting a customer auto-fills the address block.
  const handleClientChange = (e) => {
    const selectedId = e.target.value
    setClientId(selectedId)
    const client = clients.find((c) => String(c.id) === String(selectedId))
    if (client) {
      setAddress({
        address1: client.address1 || '',
        address2: client.address2 || '',
        address3: client.address3 || '',
        suburb: client.suburb || '',
        state: client.state || '',
        postCode: client.postCode || ''
      })
    }
  }

  const handleItemChange = (index, updated) => {
    setOrderItems((prev) => prev.map((row, i) => (i === index ? updated : row)))
  }

  const handleAddRow = () => setOrderItems((prev) => [...prev, emptyLine()])
  const handleRemoveRow = (index) =>
    setOrderItems((prev) => (prev.length === 1 ? prev : prev.filter((_, i) => i !== index)))

  // Requirement 6: line + grand totals, recalculated live as the user types.
  const totals = useMemo(() => {
    return orderItems.reduce(
      (acc, item) => {
        const qty = parseFloat(item.quantity) || 0
        const price = parseFloat(item.price) || 0
        const taxRate = parseFloat(item.taxRate) || 0
        const excl = qty * price
        const tax = (excl * taxRate) / 100
        acc.totalExcl += excl
        acc.totalTax += tax
        acc.totalIncl += excl + tax
        return acc
      },
      { totalExcl: 0, totalTax: 0, totalIncl: 0 }
    )
  }, [orderItems])

  const validate = () => {
    if (!clientId) return 'Please select a customer.'
    const activeLines = orderItems.filter((i) => i.itemCode)
    if (activeLines.length === 0) return 'Please add at least one item.'
    for (const item of activeLines) {
      if (!item.quantity || Number(item.quantity) <= 0) {
        return `Please enter a valid quantity for item ${item.itemCode}.`
      }
    }
    return ''
  }

  const handleSave = async () => {
    const validationError = validate()
    if (validationError) {
      setErrorMsg(validationError)
      return
    }
    setErrorMsg('')
    setSaving(true)

    const payload = {
      clientId: Number(clientId),
      invoiceNo,
      invoiceDate,
      referenceNo,
      note,
      items: orderItems
        .filter((i) => i.itemCode)
        .map((i) => ({
          itemCode: i.itemCode,
          description: i.description,
          note: i.note,
          quantity: Number(i.quantity) || 0,
          price: Number(i.price) || 0,
          taxRate: Number(i.taxRate) || 0
        }))
    }

    try {
      const result = isEdit ? await updateOrder(id, payload) : await createOrder(payload)
      setSaving(false)
      navigate(`/sales-order/${result.id}`)
    } catch (err) {
      setSaving(false)
      setErrorMsg(err.response?.data?.message || 'Failed to save the sales order.')
    }
  }

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-5xl mx-auto bg-white shadow rounded-lg border border-gray-200">
        <div className="flex items-center justify-between px-6 py-4 border-b border-gray-200">
          <h1 className="text-lg font-semibold text-gray-800">Sales Order</h1>
          <button onClick={() => navigate('/')} className="text-sm text-gray-500 hover:text-gray-700">
            ← Back to Home
          </button>
        </div>

        <div className="px-6 py-4 flex items-center gap-3">
          <button
            onClick={handleSave}
            disabled={saving}
            className="inline-flex items-center px-4 py-2 bg-green-600 text-white text-sm font-medium rounded-md shadow-sm hover:bg-green-700 transition disabled:opacity-50"
          >
            {saving ? 'Saving...' : '✓ Save Order'}
          </button>

          {/* Requirement 8: print option, available once the order has been saved. */}
          {isEdit && (
            <button
              onClick={() => printOrder(id)}
              className="inline-flex items-center px-4 py-2 bg-gray-700 text-white text-sm font-medium rounded-md shadow-sm hover:bg-gray-800 transition"
            >
              Print
            </button>
          )}

          {errorMsg && <span className="text-sm text-red-600">{errorMsg}</span>}
        </div>

        <div className="px-6 pb-4 grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="space-y-2">
            <FormField label="Customer Name">
              <select
                value={clientId}
                onChange={handleClientChange}
                className="w-full border border-gray-300 rounded px-2 py-1"
              >
                <option value="">Select customer</option>
                {clients.map((c) => (
                  <option key={c.id} value={c.id}>{c.customerName}</option>
                ))}
              </select>
            </FormField>
            <FormField label="Address 1">
              <input value={address.address1} readOnly className="w-full border border-gray-200 bg-gray-100 rounded px-2 py-1" />
            </FormField>
            <FormField label="Address 2">
              <input value={address.address2} readOnly className="w-full border border-gray-200 bg-gray-100 rounded px-2 py-1" />
            </FormField>
            <FormField label="Address 3">
              <input value={address.address3} readOnly className="w-full border border-gray-200 bg-gray-100 rounded px-2 py-1" />
            </FormField>
            <FormField label="Suburb">
              <input value={address.suburb} readOnly className="w-full border border-gray-200 bg-gray-100 rounded px-2 py-1" />
            </FormField>
            <FormField label="State">
              <input value={address.state} readOnly className="w-full border border-gray-200 bg-gray-100 rounded px-2 py-1" />
            </FormField>
            <FormField label="Post Code">
              <input value={address.postCode} readOnly className="w-full border border-gray-200 bg-gray-100 rounded px-2 py-1" />
            </FormField>
          </div>

          <div className="space-y-2">
            <FormField label="Invoice No.">
              <input
                value={invoiceNo}
                onChange={(e) => setInvoiceNo(e.target.value)}
                className="w-full border border-gray-300 rounded px-2 py-1"
              />
            </FormField>
            <FormField label="Invoice Date">
              <input
                type="date"
                value={invoiceDate}
                onChange={(e) => setInvoiceDate(e.target.value)}
                className="w-full border border-gray-300 rounded px-2 py-1"
              />
            </FormField>
            <FormField label="Reference no">
              <input
                value={referenceNo}
                onChange={(e) => setReferenceNo(e.target.value)}
                className="w-full border border-gray-300 rounded px-2 py-1"
              />
            </FormField>
            <FormField label="Note">
              <textarea
                value={note}
                onChange={(e) => setNote(e.target.value)}
                rows={4}
                className="w-full border border-gray-300 rounded px-2 py-1"
              />
            </FormField>
          </div>
        </div>

        <div className="px-6 pb-6">
          <OrderItemsTable
            items={orderItems}
            itemOptions={items}
            onChange={handleItemChange}
            onAddRow={handleAddRow}
            onRemoveRow={handleRemoveRow}
          />

          <div className="flex justify-end mt-4">
            <div className="w-64 space-y-1">
              <div className="flex justify-between text-sm">
                <span className="text-gray-600">Total Excl</span>
                <span className="font-medium">{totals.totalExcl.toFixed(2)}</span>
              </div>
              <div className="flex justify-between text-sm">
                <span className="text-gray-600">Total Tax</span>
                <span className="font-medium">{totals.totalTax.toFixed(2)}</span>
              </div>
              <div className="flex justify-between text-base border-t border-gray-300 pt-1">
                <span className="font-semibold text-gray-800">Total Incl</span>
                <span className="font-semibold">{totals.totalIncl.toFixed(2)}</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
