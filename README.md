# Mini Inventory Management System – Backend

> **ASP.NET Core 8 Web API** | Clean Architecture | Repository Pattern  
> Ceylon Innovation Services (PVT) LTD – Intern Challenge 01

---

## Solution Structure

```
MiniInventorySystem.sln
├── MiniInventory.API              ← Controllers + Program.cs (ASP.NET Core Web API)
├── MiniInventory.Application      ← DTOs, Interfaces, Services, Validation
├── MiniInventory.Domain           ← Domain Entities only (no dependencies)
├── MiniInventory.Infrastructure   ← EF Core DbContext, Repositories, DI Setup
└── MiniInventory.Shared           ← ApiResponse<T> wrapper
```

---

## Prerequisites

| Requirement | Version |
|-------------|---------|
| .NET SDK    | 8.0+    |
| SQL Server  | 2019+   |

---

## Quick Start

### 1. Clone the repository
```bash
git clone https://github.com/rush-25/Inventory-Management-System-backend.git
cd Inventory-Management-System-backend
```

### 2. Configure Database & Connection String
Ensure you have **Microsoft SQL Server** installed and running on your machine.
Edit `MiniInventory.API/appsettings.json` to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MiniInventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> **Default Instance:** Use `Server=.` if you installed the default instance of SQL Server.  
> **SQL Express:** Use `Server=.\SQLEXPRESS` if you installed SQL Server Express.  
> **Docker:** Use `Server=localhost,1433;User Id=sa;Password=YourPassword;` if running SQL Server via Docker.

### 3. Run the Backend (API)

**Using .NET CLI:**
Open a terminal and navigate to the API project folder:
```powershell
cd MiniInventory.API
```
Then start the application:
```powershell
dotnet run
```
> **Note:** If you get an error that `dotnet` is not recognized, make sure you have the .NET 8 SDK installed. If you just installed it, restart your terminal. You can also run it via its absolute path:
> ```powershell
> & "C:\Program Files\dotnet\dotnet.exe" run
> ```

**Using Visual Studio:**
1. Open `MiniInventorySystem.sln` in Visual Studio 2022.
2. Ensure `MiniInventory.API` is set as the Startup Project (right-click on the project -> Set as Startup Project).
3. Press `F5` or click the "Start" button to run with debugging.

The backend will:
- **Automatically apply EF Core migrations** on first startup (creates the database and tables)
- Serve the Swagger UI automatically at `http://localhost:<PORT>/swagger`

> Alternatively, you can run the SQL script manually if you prefer not to use EF Core Migrations:  
> `Database/schema.sql` → open in SSMS and execute

---

## API Endpoints

### Category (`/api/category`)
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/category` | List all categories |
| `GET` | `/api/category/{id}` | Get category by ID |
| `GET` | `/api/category/search?keyword=` | Search categories |
| `POST` | `/api/category` | Create category |
| `PUT` | `/api/category/{id}` | Update category |
| `DELETE` | `/api/category/{id}` | Delete category |

### Supplier (`/api/supplier`)
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/supplier` | List all suppliers |
| `GET` | `/api/supplier/{id}` | Get supplier by ID |
| `POST` | `/api/supplier` | Create supplier |
| `PUT` | `/api/supplier/{id}` | Update supplier |
| `DELETE` | `/api/supplier/{id}` | Delete supplier |

### Item (`/api/item`)
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/item` | List all items |
| `GET` | `/api/item/{id}` | Get item by ID |
| `GET` | `/api/item/search?keyword=phone` | Search by name/code/barcode |
| `POST` | `/api/item` | Create item |
| `PUT` | `/api/item/{id}` | Update item |
| `DELETE` | `/api/item/{id}` | Delete item |

### Stock (`/api/stock`)
| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/stock/in` | Record stock in |
| `POST` | `/api/stock/out` | Record stock out |
| `GET` | `/api/stock/balance` | Full stock balance report |
| `GET` | `/api/stock/low-stock` | Low stock / out of stock items |

### Dashboard (`/api/dashboard`)
| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/dashboard/stats` | Summary stats for dashboard |

---

## Response Format

All endpoints return `ApiResponse<T>`:

```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": { ... },
  "errors": null
}
```

---

## Stock Out Reason Values

Valid values for the `reason` field in a Stock Out request:
- `Sale`
- `Damage`
- `Internal Use`
- `Return`

---

## Architecture Notes

- **No business logic in controllers** – controllers only call service methods
- **No raw SQL in repositories** – EF Core LINQ used throughout
- **DTOs** used for all API input/output (entities never returned directly)
- **Dependency Injection** wired via `InfrastructureServiceExtensions.AddInfrastructure()`
- **async/await** used throughout all layers
- **CORS** configured for `http://localhost:5173` (Vite), `http://localhost:3000` (CRA)

---

## EF Core Migrations (manual)

```bash
# From solution root
dotnet ef migrations add InitialCreate --project MiniInventory.Infrastructure --startup-project MiniInventory.API
dotnet ef database update --project MiniInventory.Infrastructure --startup-project MiniInventory.API
```

---

## Frontend

React frontend: https://github.com/rush-25/Inventory-Management-System-Frontend
