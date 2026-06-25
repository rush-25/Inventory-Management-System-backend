-- =============================================================================
-- Mini Inventory Management System – Database Schema Script
-- Company: Ceylon Innovation Services (PVT) LTD
-- Generated for: SQL Server 2019+
-- Usage: Run this script in SQL Server Management Studio (SSMS) BEFORE
--        running the application (alternatively use EF Core migrations).
-- =============================================================================

USE master;
GO

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'MiniInventoryDB')
BEGIN
    CREATE DATABASE MiniInventoryDB;
END
GO

USE MiniInventoryDB;
GO

-- =============================================================================
-- 1. CategoryTable
-- =============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CategoryTable')
BEGIN
    CREATE TABLE CategoryTable (
        CategoryId      INT             NOT NULL IDENTITY(1,1),
        CategoryName    NVARCHAR(200)   NOT NULL,
        Description     NVARCHAR(500)   NULL,
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedDate     DATETIME2       NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_Category PRIMARY KEY (CategoryId)
    );
END
GO

-- =============================================================================
-- 2. SupplierTable
-- =============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SupplierTable')
BEGIN
    CREATE TABLE SupplierTable (
        SupplierId      INT             NOT NULL IDENTITY(1,1),
        SupplierName    NVARCHAR(200)   NOT NULL,
        ContactNumber   NVARCHAR(50)    NULL,
        Email           NVARCHAR(200)   NULL,
        Address         NVARCHAR(500)   NULL,
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedDate     DATETIME2       NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_Supplier PRIMARY KEY (SupplierId)
    );
END
GO

-- =============================================================================
-- 3. ItemTable
-- =============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ItemTable')
BEGIN
    CREATE TABLE ItemTable (
        ItemId          INT             NOT NULL IDENTITY(1,1),
        ItemCode        NVARCHAR(100)   NOT NULL,
        Barcode         NVARCHAR(100)   NULL,
        ItemName        NVARCHAR(300)   NOT NULL,
        CategoryId      INT             NOT NULL,
        SupplierId      INT             NOT NULL,
        CostPrice       DECIMAL(18,2)   NOT NULL DEFAULT 0,
        SellingPrice    DECIMAL(18,2)   NOT NULL DEFAULT 0,
        ReorderLevel    INT             NOT NULL DEFAULT 0,
        IsActive        BIT             NOT NULL DEFAULT 1,
        CreatedDate     DATETIME2       NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_Item PRIMARY KEY (ItemId),
        CONSTRAINT FK_Item_Category FOREIGN KEY (CategoryId)
            REFERENCES CategoryTable(CategoryId) ON DELETE NO ACTION,
        CONSTRAINT FK_Item_Supplier FOREIGN KEY (SupplierId)
            REFERENCES SupplierTable(SupplierId) ON DELETE NO ACTION
    );

    -- Indexes for search columns (ItemCode, Barcode, ItemName, CategoryId, SupplierId)
    CREATE INDEX IX_Item_ItemCode    ON ItemTable (ItemCode);
    CREATE INDEX IX_Item_Barcode     ON ItemTable (Barcode) WHERE Barcode IS NOT NULL;
    CREATE INDEX IX_Item_ItemName    ON ItemTable (ItemName);
    CREATE INDEX IX_Item_CategoryId  ON ItemTable (CategoryId);
    CREATE INDEX IX_Item_SupplierId  ON ItemTable (SupplierId);
END
GO

-- =============================================================================
-- 4. StockInTable
-- =============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StockInTable')
BEGIN
    CREATE TABLE StockInTable (
        StockInId       INT             NOT NULL IDENTITY(1,1),
        ItemId          INT             NOT NULL,
        SupplierId      INT             NOT NULL,
        Quantity        INT             NOT NULL,
        CostPrice       DECIMAL(18,2)   NOT NULL DEFAULT 0,
        StockInDate     DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        CreatedDate     DATETIME2       NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_StockIn PRIMARY KEY (StockInId),
        CONSTRAINT FK_StockIn_Item FOREIGN KEY (ItemId)
            REFERENCES ItemTable(ItemId) ON DELETE CASCADE,
        CONSTRAINT FK_StockIn_Supplier FOREIGN KEY (SupplierId)
            REFERENCES SupplierTable(SupplierId) ON DELETE NO ACTION
    );

    CREATE INDEX IX_StockIn_ItemId     ON StockInTable (ItemId);
    CREATE INDEX IX_StockIn_SupplierId ON StockInTable (SupplierId);
    CREATE INDEX IX_StockIn_Date       ON StockInTable (StockInDate DESC);
END
GO

-- =============================================================================
-- 5. StockOutTable
-- =============================================================================
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StockOutTable')
BEGIN
    CREATE TABLE StockOutTable (
        StockOutId      INT             NOT NULL IDENTITY(1,1),
        ItemId          INT             NOT NULL,
        Quantity        INT             NOT NULL,
        Reason          NVARCHAR(100)   NOT NULL,  -- Sale | Damage | Internal Use | Return
        StockOutDate    DATETIME2       NOT NULL DEFAULT GETUTCDATE(),
        CreatedDate     DATETIME2       NOT NULL DEFAULT GETUTCDATE(),

        CONSTRAINT PK_StockOut PRIMARY KEY (StockOutId),
        CONSTRAINT FK_StockOut_Item FOREIGN KEY (ItemId)
            REFERENCES ItemTable(ItemId) ON DELETE CASCADE
    );

    CREATE INDEX IX_StockOut_ItemId ON StockOutTable (ItemId);
    CREATE INDEX IX_StockOut_Date   ON StockOutTable (StockOutDate DESC);
END
GO

-- =============================================================================
-- Sample Data (Optional – uncomment to seed)
-- =============================================================================
/*
INSERT INTO CategoryTable (CategoryName, Description) VALUES
    ('Electronics',   'Electronic devices and accessories'),
    ('Clothing',      'Apparel and garments'),
    ('Food & Drinks', 'Consumable products'),
    ('Office Supplies','Stationery and office equipment');

INSERT INTO SupplierTable (SupplierName, ContactNumber, Email, Address) VALUES
    ('TechWorld PVT LTD',  '0112345678', 'info@techworld.lk',   'No 5, Colombo 03'),
    ('FashionHub',          '0119876543', 'sales@fashionhub.lk', 'No 12, Kandy Road'),
    ('FreshMart',           '0113456789', 'fresh@freshmart.lk',  'Galle Road, Moratuwa');
*/

PRINT 'MiniInventoryDB schema created successfully.';
GO
