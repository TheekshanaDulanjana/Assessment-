export default function DataGrid({ columns, rows, onRowDoubleClick }) {
  return (
    <div className="overflow-x-auto border border-gray-300 rounded-md shadow-sm">
      <table className="min-w-full text-sm text-left text-gray-700">
        <thead className="bg-gray-100 text-gray-800">
          <tr>
            {columns.map((col) => (
              <th key={col.key} className="px-4 py-2 font-semibold border-b border-gray-300">
                {col.label}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.length === 0 ? (
            <tr>
              <td colSpan={columns.length} className="px-4 py-6 text-center text-gray-400">
                No sales orders yet. Click "Add New" to create one.
              </td>
            </tr>
          ) : (
            rows.map((row, idx) => (
              <tr
                key={row.id}
                onDoubleClick={() => onRowDoubleClick && onRowDoubleClick(row)}
                title="Double-click to open"
                className={`cursor-pointer hover:bg-blue-50 ${idx % 2 === 0 ? 'bg-white' : 'bg-gray-50'}`}
              >
                {columns.map((col) => (
                  <td key={col.key} className="px-4 py-2 border-b border-gray-200">
                    {col.render ? col.render(row) : row[col.key]}
                  </td>
                ))}
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  )
}
