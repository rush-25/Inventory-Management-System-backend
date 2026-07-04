# API Endpoints

The V2 Phone Arcade backend exposes a RESTful API. All responses follow a standardized `ApiResponse<T>` format to ensure easy and consistent consumption by the frontend.

---

## Base URL
`http://localhost:<PORT>/api`

## Response Envelope Format
```json
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": { ... },
  "errors": null
}
```

---

## Category Endpoints
Manages product categories.

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/category` | Retrieve a list of all categories |
| `GET` | `/api/category/{id}` | Get details of a specific category by its ID |
| `GET` | `/api/category/search?keyword=` | Search for categories by name |
| `POST` | `/api/category` | Create a new category |
| `PUT` | `/api/category/{id}` | Update an existing category |
| `DELETE` | `/api/category/{id}` | Delete a category |

---

## Supplier Endpoints
Manages vendor and supplier information.

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/supplier` | Retrieve a list of all suppliers |
| `GET` | `/api/supplier/{id}` | Get details of a specific supplier by ID |
| `POST` | `/api/supplier` | Create a new supplier |
| `PUT` | `/api/supplier/{id}` | Update an existing supplier |
| `DELETE` | `/api/supplier/{id}` | Delete a supplier |

---

## Item (Product) Endpoints
Manages the core product catalog. Items are linked to Categories and Suppliers.

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/item` | Retrieve a list of all items |
| `GET` | `/api/item/{id}` | Get details of a specific item by ID |
| `GET` | `/api/item/search?keyword=` | Search items by name, code, or barcode |
| `POST` | `/api/item` | Create a new item |
| `PUT` | `/api/item/{id}` | Update an existing item |
| `DELETE` | `/api/item/{id}` | Delete an item |

---

## Stock Movement Endpoints
Manages inventory levels and stock transactions.

| Method | Route | Description |
|--------|-------|-------------|
| `POST` | `/api/stock/in` | Record incoming stock (receivables from suppliers) |
| `POST` | `/api/stock/out` | Record outbound stock (Valid reasons: `Sale`, `Damage`, `Internal Use`, `Return`) |
| `GET` | `/api/stock/balance` | Retrieve a full stock balance report across all items |
| `GET` | `/api/stock/low-stock` | Retrieve items that are at or below their reorder level |

---

## Dashboard Endpoints
Aggregates data for the high-level system overview.

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/dashboard/stats` | Retrieve summary statistics (total items, total stock value, active alerts, recent movements) |
