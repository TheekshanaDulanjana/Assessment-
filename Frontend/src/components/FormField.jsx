export default function FormField({ label, children, className = '' }) {
  return (
    <div className={`flex items-center gap-3 ${className}`}>
      <label className="w-32 shrink-0 text-sm font-medium text-gray-700">{label}</label>
      <div className="flex-1">{children}</div>
    </div>
  )
}
