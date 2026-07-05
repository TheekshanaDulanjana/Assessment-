# SPIL Labs — Sales Order Web Application

Full-stack implementation of the UI/UX Designer / Developer internship assignment:
a **Sales Order** entry screen (Screen 1) and a **Home** grid screen (Screen 2),
built with **.NET Core Web API (Clean Architecture)** on the backend and
**React + Redux Toolkit + Tailwind CSS** on the frontend, backed by **SQL Server**.

---

## 1. Architecture overview

### Backend — Clean Architecture, 4 projects

```
Backend/
  SalesOrderApp.Domain            Entities only (Client, Item, SalesOrder, SalesOrderItem)
  SalesOrderApp.Application        Interfaces, DTOs, business logic (Services), AutoMapper profiles
  SalesOrderApp.Infrastructure      EF Core DbContext, entity configurations, repositories, PDF report generator
  SalesOrderApp.Api                 Controllers, Program.cs, middleware, appsettings
```

Dependency direction is strictly inward: `Api → Application + Infrastructure`,
`Infrastructure → Application`, `Application → Domain`. Domain has zero
dependencies. This means the business rules (the Excl/Tax/Incl calculations,
validation, invoice numbering) live in one place — `SalesOrderService` — and
are **completely independent of EF Core, SQL Server, or ASP.NET**, so they can
be unit-tested in isolation and the persistence technology could be swapped
without touching business logic.

**Key design decisions / non-obvious choices, explained:**

- **Line-item amounts are always recalculated server-side.** The frontend
  computes and *displays* Excl/Tax/Incl amounts live for a responsive UX, but
  the `SaveSalesOrderDto` sent to the API deliberately does **not** include
  those calculated fields — only `Quantity`, `Price`, `TaxRate`. This closes
  an obvious integrity hole: a modified network request can never persist
  incorrect totals, because `SalesOrderService.SaveAsync` recomputes
  everything from scratch before saving.
- **Repository + Unit of Work pattern** wraps EF Core so `Application` never
  references `Microsoft.EntityFrameworkCore` directly — only interfaces
  (`IClientRepository`, `IItemRepository`, `ISalesOrderRepository`,
  `IUnitOfWork`) that `Infrastructure` implements.
- **A single global `ExceptionHandlingMiddleware`** converts
  `NotFoundException` → 404 and `ArgumentException` → 400 with a consistent
  `{ "message": "..." }` JSON body, instead of scattering try/catch blocks
  across every controller action.
- **Invoice numbers** are auto-suggested (`GET /api/salesorders/next-invoice-no`,
  format `INV-{year}-{00001}`) but remain a free-text field the user can
  overwrite, per instruction #3 ("these fields can be filled by user as wish").
- **PDF printing** (requirement #8) uses **QuestPDF** (MIT/Community license,
  actively maintained, no native dependencies) rather than a legacy reporting
  stack — `GET /api/salesorders/{id}/print` streams the PDF back to the browser.

### Frontend — React + Redux Toolkit + Tailwind

```
Frontend/src/
  pages/Home.jsx            Screen 2 — grid + "Add New"
  pages/SalesOrder.jsx       Screen 1 — full order form
  components/                Reusable DataGrid, FormField, OrderItemsTable
  redux/slices/               ordersSlice, clientsSlice, itemsSlice (Redux Toolkit, async thunks)
  services/                   Axios wrappers per resource (clientService, itemService, orderService)
```

- **React Router** wires `/` → Home, `/sales-order` → new order,
  `/sales-order/:id` → edit an existing order (double-click a row on Home).
- **Redux Toolkit** holds server-derived reference data (clients, items,
  orders list) so it isn't re-fetched needlessly across navigations; the
  in-progress order **form state itself lives in local component state**
  (`useState`) since it's transient, per-screen data — a deliberate choice to
  avoid over-engineering global state for something only one component reads.
- **Tailwind CSS** utility classes only, no component library, per the brief.

---

## 2. What was implemented (requirement-by-requirement)

**Screen 1 — Sales Order**
1. Customer Name dropdown lists customers from the `Client` table (`GET /api/clients`). ✅
2. Selecting a customer auto-fills Address 1–3, Suburb, State, Post Code (read-only, derived from the selected client). ✅
3. Invoice No., Invoice Date, Reference No., Note are free-text/typeable fields. ✅
4. Item Code column is a dropdown populated from the Item catalog (`GET /api/items`). ✅
5. Description is also a dropdown over the same catalog; selecting either Item Code or Description fills the other plus Note/Quantity/Tax Rate inputs, and calculates `Excl = Qty * Price`, `Tax = Excl * TaxRate / 100`, `Incl = Excl + Tax` live. Multiple item rows can be added/removed. ✅
6. Each line's Excl/Tax/Incl amounts are shown per row and summed into Total Excl / Total Tax / Total Incl. ✅
7. `SalesOrders` and `SalesOrderItems` tables persist the header and line items (EF Core Code-First migrations). ✅
8. "Print" button (visible once an order is saved) opens a generated PDF via `/api/salesorders/{id}/print`. ✅

**Screen 2 — Home**
1. `/` is the application's default route — first screen shown on load. ✅
2. "Add New" navigates to the Sales Order screen. ✅
3. Grid lists saved orders with definable columns; double-clicking a row reopens that order (pre-filled) in the Sales Order screen for editing and re-saving. ✅

---

## 3. Assumptions made (explicitly called out, as requested)

- The wireframes don't specify a `Client` or `Item` maintenance screen — the
  brief only says the Customer dropdown reads from a `Client` table and items
  populate from an item catalog, so those tables are **seeded automatically**
  on first run (3 sample clients, 5 sample items in `DbInitializer.cs`) rather
  than built out with their own CRUD screens, since none were mocked up.
- "Grid columns you can define as you wish" (Screen 2, #3) — implemented as
  Invoice No., Invoice Date, Customer Name, Reference No., Total (Incl),
  since these are the fields most useful for identifying/opening an order.
- No authentication/authorization was shown in the wireframes or mentioned in
  the instructions, so none was implemented — this would be the first
  addition before any real production deployment (see §6).
- Tax rate is entered per line (as shown in the "Tax" column of the wireframe)
  rather than being a fixed global rate, matching the formula given
  (`Tax Amount = Excl Amount * Tax Rate / 100`).
- Invoice numbers are unique; a suggested next number is auto-filled for
  convenience but remains editable, since the spec lists Invoice No. as a
  user-typeable field.

---

## 4. Running the backend

**Prerequisites:** .NET 8 SDK, SQL Server (or SQL Server LocalDB / Express),
`dotnet-ef` tool (`dotnet tool install --global dotnet-ef` if you don't have it).

```bash
cd Backend

# 1. Create a solution and wire up the four projects (one-time setup)
dotnet new sln -n SalesOrderApp
dotnet sln add SalesOrderApp.Domain/SalesOrderApp.Domain.csproj
dotnet sln add SalesOrderApp.Application/SalesOrderApp.Application.csproj
dotnet sln add SalesOrderApp.Infrastructure/SalesOrderApp.Infrastructure.csproj
dotnet sln add SalesOrderApp.Api/SalesOrderApp.Api.csproj

# 2. Restore packages
dotnet restore

# 3. Adjust the connection string if needed
#    Backend/SalesOrderApp.Api/appsettings.json -> ConnectionStrings:DefaultConnection

# 4. Create the initial migration (from the Api project, referencing Infrastructure)
cd SalesOrderApp.Api
dotnet ef migrations add InitialCreate --project ../SalesOrderApp.Infrastructure --startup-project .

# 5. Run — this applies the migration AND seeds sample Clients/Items automatically
dotnet run
```

The API listens on the HTTPS/HTTP ports printed in the console (typically
`https://localhost:7000` / `http://localhost:5000`), with Swagger UI available
at `/swagger` in Development. Update `Frontend/.env` (see below) to match.

## 5. Running the frontend

**Prerequisites:** Node.js 18+.

```bash
cd Frontend
cp .env.example .env
# edit .env if your API is running on a different port
npm install
npm run dev
```

Open `http://localhost:5173`. CORS is already configured on the API to allow
this origin (`Program.cs` → `AllowReactApp` policy).

---

## 6. Suggested next steps for a production-grade deployment

These weren't required by the brief but are worth flagging honestly:

- Add authentication/authorization (e.g. JWT + role-based access) before any
  real customer data is involved.
- Add FluentValidation or DataAnnotations on the DTOs for richer input
  validation (currently only quantity > 0 and "at least one item" are enforced).
- Add xUnit test projects for `SalesOrderService` (the calculation and
  save/update logic is the highest-value thing to unit test) and integration
  tests for the controllers.
- Add pagination/filtering to `GET /api/salesorders` once order volume grows.
- Containerize with Docker Compose (API + SQL Server) for easier grading/demo.
#   A s s e s s m e n t -  
 