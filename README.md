# V2 Phone Arcade - Inventory Management System

A comprehensive, full-stack Inventory Management System built to handle products, suppliers, categories, and granular stock movements efficiently.

---

## 🌟 Features
- **Real-Time Dashboard**: View key metrics, low stock alerts, and recent transactions at a glance.
- **Product & Category Management**: Comprehensive cataloging with custom pricing, descriptions, and barcode support.
- **Supplier Tracking**: Manage vendors and their associated supplies.
- **Stock Movements**: Granular tracking for **Stock In** (receivables) and **Stock Out** (sales, damages, internal use).
- **Responsive UI**: A modern, clean theme built with React & Tailwind CSS.
- **Secure Backend**: A robust RESTful API built on ASP.NET Core 8 following Clean Architecture principles.

---

## 🏗️ Architecture Stack

### Frontend
- **Framework**: React 18 & Vite
- **Language**: TypeScript
- **Styling**: Tailwind CSS 4
- **State Management**: React Context API & React Query
- **Authentication**: Route guarding via `sessionStorage`

### Backend
- **Framework**: ASP.NET Core 8 Web API
- **Architecture**: Clean Architecture & Repository Pattern
- **Database**: Microsoft SQL Server (via Entity Framework Core)

---

## 🚀 Getting Started

### 1. Database Setup
Ensure Microsoft SQL Server is installed and running on your machine.
The connection string is configured in the backend's `appsettings.json`.

### 2. Run the Backend
Navigate to the backend directory and start the API:
```bash
cd Inventory-Management-System-backend/MiniInventory.API
dotnet run
```
*Note: The backend will automatically apply EF Core database migrations on startup and serve the Swagger UI at `http://localhost:<PORT>/swagger`.*

### 3. Run the Frontend
Open a new terminal, navigate to the frontend directory, install dependencies, and start the Vite dev server:
```bash
cd Inventory Management System Frontend/Inventory-Management-System-Frontend
npm install
npm run dev
```
*Access the web application at `http://localhost:5173`.*

**Default Admin Credentials:**
- **Username:** `admin`
- **Password:** `abc123`

---

## 🔗 Repository Links
- **Frontend Repository**: [https://github.com/rush-25/Inventory-Management-System-Frontend](https://github.com/rush-25/Inventory-Management-System-Frontend)
- **Backend Repository**: [https://github.com/rush-25/Inventory-Management-System-backend](https://github.com/rush-25/Inventory-Management-System-backend)

---

## 📚 Documentation
For more detailed information about the system internals, please refer to the accompanying documentation files:
- `API_ENDPOINTS.md`: A complete list of all RESTful API endpoints and response formats.
- `TECHNICAL_EXPLANATION.md`: A deep dive into the system architecture, design patterns, and state management.
