import { Routes, Route } from 'react-router-dom'
import Home from './pages/Home.jsx'
import SalesOrder from './pages/SalesOrder.jsx'

function App() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/sales-order" element={<SalesOrder />} />
      <Route path="/sales-order/:id" element={<SalesOrder />} />
    </Routes>
  )
}

export default App
