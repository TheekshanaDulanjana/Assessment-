export default function OrderItemsTable({ items, itemOptions, onChange, onAddRow, onRemoveRow }) {
  // Requirement 4/5: item code and description are both dropdowns populated from
  // the Item catalog. Selecting either one fills the other plus the unit price.
  const handleItemSelect = (index, field, value) => {
    const selected = itemOptions.find((opt) =>
      field === 'itemCode' ? opt.itemCode === value : opt.description === value
    )
    const updated = { ...items[index] }

    if (selected) {
      updated.itemCode = selected.itemCode
      updated.description = selected.description
      updated.price = selected.price
    } else {
      updated[field] = value
      updated.price = ''
    }

    onChange(index, updated)
  }

  const handleFieldChange = (index, field, value) => {
    onChange(index, { ...items[index], [field]: value })
  }

  // Requirement 5: Excl = Qty * Price, Tax = Excl * TaxRate / 100, Incl = Excl + Tax
  const calcLine = (item) => {
    const qty = parseFloat(item.quantity) || 0
    const price = parseFloat(item.price) || 0
    const taxRate = parseFloat(item.taxRate) || 0
    const exclAmount = qty * price
    const taxAmount = (exclAmount * taxRate) / 100
    const inclAmount = exclAmount + taxAmount
    return { exclAmount, taxAmount, inclAmount }
  }

  return (
    <div className="overflow-x-auto border border-gray-300 rounded-md mt-4">
      <table className="min-w-full text-sm text-left">
        <thead className="bg-gray-100">
          <tr>
            {['Item Code', 'Description', 'Note', 'Quantity', 'Price', 'Tax %', 'Excl Amount', 'Tax Amount', 'Incl Amount', ''].map((h) => (
              <th key={h} className="px-2 py-2 border-b border-gray-300 font-semibold whitespace-nowrap">
                {h}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {items.map((item, index) => {
            const { exclAmount, taxAmount, inclAmount } = calcLine(item)
            return (
              <tr key={index} className={index % 2 === 0 ? 'bg-white' : 'bg-gray-50'}>
                <td className="px-2 py-1">
                  <select
                    value={item.itemCode}
                    onChange={(e) => handleItemSelect(index, 'itemCode', e.target.value)}
                    className="w-28 border border-gray-300 rounded px-1 py-1"
                  >
                    <option value="">Select</option>
                    {itemOptions.map((opt) => (
                      <option key={opt.itemCode} value={opt.itemCode}>{opt.itemCode}</option>
                    ))}
                  </select>
                </td>
                <td className="px-2 py-1">
                  <select
                    value={item.description}
                    onChange={(e) => handleItemSelect(index, 'description', e.target.value)}
                    className="w-48 border border-gray-300 rounded px-1 py-1"
                  >
                    <option value="">Select</option>
                    {itemOptions.map((opt) => (
                      <option key={opt.itemCode} value={opt.description}>{opt.description}</option>
                    ))}
                  </select>
                </td>
                <td className="px-2 py-1">
                  <input
                    type="text"
                    value={item.note}
                    onChange={(e) => handleFieldChange(index, 'note', e.target.value)}
                    className="w-28 border border-gray-300 rounded px-1 py-1"
                  />
                </td>
                <td className="px-2 py-1">
                  <input
                    type="number"
                    min="0"
                    value={item.quantity}
                    onChange={(e) => handleFieldChange(index, 'quantity', e.target.value)}
                    className="w-20 border border-gray-300 rounded px-1 py-1"
                  />
                </td>
                <td className="px-2 py-1">
                  <input
                    type="number"
                    value={item.price}
                    readOnly
                    className="w-24 border border-gray-200 bg-gray-100 rounded px-1 py-1"
                  />
                </td>
                <td className="px-2 py-1">
                  <input
                    type="number"
                    min="0"
                    value={item.taxRate}
                    onChange={(e) => handleFieldChange(index, 'taxRate', e.target.value)}
                    className="w-16 border border-gray-300 rounded px-1 py-1"
                  />
                </td>
                <td className="px-2 py-1 text-right whitespace-nowrap">{exclAmount.toFixed(2)}</td>
                <td className="px-2 py-1 text-right whitespace-nowrap">{taxAmount.toFixed(2)}</td>
                <td className="px-2 py-1 text-right font-medium whitespace-nowrap">{inclAmount.toFixed(2)}</td>
                <td className="px-2 py-1 text-center">
                  <button
                    type="button"
                    onClick={() => onRemoveRow(index)}
                    className="text-red-500 hover:text-red-700 text-xs"
                    aria-label="Remove row"
                  >
                    ✕
                  </button>
                </td>
              </tr>
            )
          })}
        </tbody>
      </table>
      <div className="p-2 border-t border-gray-200">
        <button
          type="button"
          onClick={onAddRow}
          className="text-sm text-blue-600 hover:text-blue-800 font-medium"
        >
          + Add Item
        </button>
      </div>
    </div>
  )
}
