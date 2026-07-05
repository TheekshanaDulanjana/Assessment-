# SPIL Labs — Sales Order Management System

A full-stack **Sales Order Management System** developed for the **SPIL Labs UI/UX Designer / Developer Internship Assignment**, built using **.NET 8 Web API (Clean Architecture)**, **React**, **Redux Toolkit**, **Tailwind CSS**, and **SQL Server**.

---

## Features

- Create, edit, and manage Sales Orders
- Customer selection with automatic address population
- Dynamic order item management
- Live Excl / Tax / Incl calculations
- Server-side validation and recalculation
- PDF invoice generation
- Home dashboard with order listing
- Double-click to edit existing orders
- SQL Server persistence using Entity Framework Core

---

## Tech Stack

### Backend
- .NET 8 Web API
- Clean Architecture
- Entity Framework Core
- SQL Server
- AutoMapper
- QuestPDF

### Frontend
- React
- Redux Toolkit
- React Router
- Tailwind CSS
- Axios
- Vite

---

## Project Structure

```
Backend/
├── SalesOrderApp.Domain
├── SalesOrderApp.Application
├── SalesOrderApp.Infrastructure
└── SalesOrderApp.Api

Frontend/
├── pages
├── components
├── redux
├── services
└── App.jsx
```

---

## Architecture

- Clean Architecture
- Repository Pattern
- Unit of Work Pattern
- RESTful API
- Global Exception Middleware
- Server-side business logic
- Responsive UI

---

## Business Logic

- Automatic invoice number generation
- Live order calculations
- Grand total calculation
- Customer auto-fill
- Item auto-fill
- Backend recalculates all totals before saving to ensure data integrity

---

## Database

- Clients
- Items
- SalesOrders
- SalesOrderItems

---

## API Endpoints

| Method | Endpoint |
|--------|----------|
| GET | `/api/clients` |
| GET | `/api/items` |
| GET | `/api/salesorders` |
| GET | `/api/salesorders/{id}` |
| POST | `/api/salesorders` |
| PUT | `/api/salesorders/{id}` |
| GET | `/api/salesorders/next-invoice-no` |
| GET | `/api/salesorders/{id}/print` |

---

## Getting Started

### Backend

```bash
cd Backend
dotnet restore

cd SalesOrderApp.Api

dotnet ef migrations add InitialCreate \
--project ../SalesOrderApp.Infrastructure \
--startup-project .

dotnet run
```

---

### Frontend

```bash
cd Frontend

npm install
npm run dev
```

---

## Assignment Requirements Covered

- Customer dropdown with address auto-fill
- Dynamic item selection
- Automatic calculations
- Save & Update Sales Orders
- Home dashboard
- PDF invoice generation
- SQL Server integration
- Responsive UI

---

## Future Improvements

- JWT Authentication
- Role-based Authorization
- Unit Testing
- Docker Support
- Advanced Search & Filtering
- Excel Export
- Email Invoices

---
