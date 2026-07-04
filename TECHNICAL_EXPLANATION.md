# Technical Explanation: V2 Phone Arcade - Inventory Management System

## Overview
This document provides a comprehensive technical overview of the V2 Phone Arcade Inventory Management System. The system is designed using a decoupled, modern architecture comprising a React-based frontend and a .NET Core API backend.

---

## 1. System Architecture
The application follows a client-server architecture:
- **Frontend**: Single Page Application (SPA) built with React 18 and Vite.
- **Backend**: RESTful Web API built with ASP.NET Core 8.
- **Database**: Microsoft SQL Server, accessed via Entity Framework (EF) Core.

---

## 2. Frontend (React + Vite)

### Technology Stack
- **Framework**: React 18
- **Build Tool**: Vite (for fast Hot Module Replacement and optimized production builds)
- **Language**: TypeScript (ensures type safety across components)
- **Styling**: Tailwind CSS 4 for utility-first styling, utilizing a custom theme configuration for a modern aesthetic.
- **State Management & Data Fetching**: 
  - `Context API` for global app state (like Theme and Auth).
  - `@tanstack/react-query` for asynchronous server state management (caching, background retries, fetching).
- **Routing**: React Router (v6+ using the `createBrowserRouter` data router).
- **UI & Forms**: 
  - Radix UI primitives for accessible, unstyled components.
  - React Hook Form + Zod for strict, schema-based form validation.
  - Recharts for data visualization on the dashboard.

### Authentication & Routing Flow
- **Session Management**: A simple but robust `AuthContext` manages the login session using `sessionStorage`. This ensures the session is completely cleared when the browser tab is closed.
- **Guarded Routes**: A `ProtectedRoute` component wraps the entire Dashboard layout. It intercepts unauthorized access and redirects users to the cleanly designed `/login` page.

### Key Modules
- **Dashboard**: Aggregates system data for quick insights (low stock items, recent stock movements, key metrics).
- **Products, Categories, Suppliers**: CRUD interfaces for managing the core catalog and vendor relationships.
- **Stock Movement**: Dedicated, streamlined interfaces for recording incoming stock (`Stock In`) and outbound deductions (`Stock Out` for Sales, Damages, Returns, etc.).

---

## 3. Backend (.NET Core 8 Web API)

The backend follows **Clean Architecture** principles to ensure a maintainable and testable codebase.

### Solution Structure
The solution (`MiniInventorySystem.sln`) is divided into distinct layers to enforce separation of concerns:
- **`MiniInventory.API`**: Presentation layer. Contains the Controllers, Swagger setup, and Dependency Injection bootstrapping.
- **`MiniInventory.Application`**: Business logic layer. Contains Services, Interfaces, and Data Transfer Objects (DTOs).
- **`MiniInventory.Domain`**: Core domain layer. Contains only the business entities (Models) with no external dependencies.
- **`MiniInventory.Infrastructure`**: Data access layer. Contains the EF Core `DbContext` and Repositories.
- **`MiniInventory.Shared`**: Common utilities layer. Contains the standardized `ApiResponse<T>` wrapper.

### Design Principles & Best Practices
- **No Business Logic in Controllers**: Controllers strictly handle incoming HTTP requests and route them to Application Services.
- **No Raw SQL**: Data access is handled entirely via EF Core LINQ queries within the Repositories, ensuring database agnosticism and type safety.
- **DTO Pattern**: Database entities are never exposed directly to the client. DTOs are mapped to and from entities to strictly control the data payload over the wire.
- **Standardized API Responses**: Every API endpoint wraps its return data in a consistent `ApiResponse<T>` envelope containing `success`, `message`, `data`, and `errors` fields. This makes frontend consumption highly predictable.
- **Dependency Injection (DI)**: Services and repositories are decoupled and injected via the built-in IoC container (`InfrastructureServiceExtensions`).
- **Asynchronous Execution**: `async/await` is used comprehensively across all layers to ensure non-blocking I/O operations.

---

## 4. Database Schema
The database (`MiniInventoryDB` in SQL Server) is managed via **EF Core Code-First Migrations**. Key tables include:
- **Category**: Classifies products.
- **Supplier**: Details vendors who supply the stock.
- **Item**: The core product catalog, linked via Foreign Keys to both Category and Supplier.
- **StockTransaction**: An immutable ledger recording every movement (In/Out) to maintain a strict audit trail. 
  - *Stock Out Reasons*: Supported values are `Sale`, `Damage`, `Internal Use`, and `Return`.
- **StockBalance**: Aggregate tables/queries representing current, real-time stock levels calculated from the transactions.

---

## 5. Deployment & Execution Flow
- **CORS Configuration**: The backend explicitly allows Cross-Origin requests from the frontend development server (`http://localhost:5173`).
- **Startup**: 
  - The backend (`dotnet run`) automatically applies pending EF Core migrations to ensure the SQL database schema is always up-to-date.
  - The frontend (`npm run dev`) proxies API requests to the backend server and renders the interactive UI.
