
    CREATE DATABASE DISTRIBUIDORA_LICORES;
GO
USE DISTRIBUIDORA_LICORES;
GO

/* =========================================================
   1) ROLES
   ========================================================= */

CREATE TABLE Roles(
    RolId       INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(50) NOT NULL,
    Descripcion VARCHAR(200) NULL
);

INSERT INTO Roles (Nombre, Descripcion) VALUES
('Administrador','Acceso total al sistema'),
('Vendedor','Registra ventas y clientes'),
('Bodeguero','Gestiona inventario y lotes'),
('Compras','Gestiona proveedores y compras'),
('Contador','Consulta reportes y finanzas');

/* =========================================================
   2) USUARIOS
   ========================================================= */

CREATE TABLE Usuarios(
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    RolId     INT NOT NULL REFERENCES dbo.Roles(RolId),
    Username  VARCHAR(50) NOT NULL UNIQUE,
    HashPass  VARCHAR(200) NOT NULL,
    Activo    BIT NOT NULL DEFAULT 1
);

INSERT INTO Usuarios (RolId, Username, HashPass, Activo) VALUES
(1,'admin','hash_admin',1),
(2,'vendedor1','hash_vend1',1),
(2,'vendedor2','hash_vend2',1),
(3,'bodega1','hash_bod1',1),
(4,'compras1','hash_comp1',1);

/* =========================================================
   3) SUCURSALES
   ========================================================= */
CREATE TABLE Sucursales(
    SucursalId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre     VARCHAR(80) NOT NULL,
    Ciudad     VARCHAR(80) NOT NULL,
    Direccion  VARCHAR(120) NOT NULL
);

INSERT INTO Sucursales (Nombre, Ciudad, Direccion) VALUES
('Central Medellín','Medellín','Cra 50 #45-10'),
('Sur Envigado','Envigado','Cl 30 Sur #44-21'),
('Norte Bello','Bello','Av 50 #40-05'),
('Centro Bogotá','Bogotá','Cl 13 #7-20'),
('Occidente Cali','Cali','Av 4 Oeste #6-30');

/* =========================================================
   4) EMPLEADOS
   ========================================================= */
CREATE TABLE Empleados(
    EmpleadoId INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId INT NOT NULL REFERENCES dbo.Sucursales(SucursalId),
    UsuarioId  INT NULL REFERENCES dbo.Usuarios(UsuarioId),
    Nombres    VARCHAR(80) NOT NULL,
    Apellidos  VARCHAR(80) NOT NULL,
    Cargo      VARCHAR(60) NOT NULL
);

INSERT INTO Empleados (SucursalId, UsuarioId, Nombres, Apellidos, Cargo) VALUES
(1,1,'Laura','Gómez','Administrador'),
(1,2,'Carlos','Pérez','Vendedor'),
(2,3,'Diana','López','Vendedor'),
(1,4,'Sergio','Ramírez','Bodeguero'),
(1,5,'Paula','Quintero','Compras');

/* =========================================================
   5) PROVEEDORES
   ========================================================= */
CREATE TABLE Proveedores(
    ProveedorId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(100) NOT NULL,
    Ciudad      VARCHAR(80) NULL,
    Telefono    VARCHAR(30) NULL
);

INSERT INTO Proveedores (Nombre, Ciudad, Telefono) VALUES
('Destilería Andina','Medellín','3001112222'),
('Viñedos del Sol','Cali','3002223333'),
('Ron Caribe S.A.','Cartagena','3003334444'),
('Cervecera Andina','Bogotá','3004445555'),
('Importadora Spirits','Barranquilla','3005556666');

/* =========================================================
   6) CATEGORIAS
   ========================================================= */

CREATE TABLE Categorias(
    CategoriaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre      VARCHAR(60) NOT NULL,
    Descripcion VARCHAR(200) NULL
);

INSERT INTO Categorias (Nombre, Descripcion) VALUES
('Cerveza','Bebidas fermentadas de cereal'),
('Vino','Vinos tinto, blanco y rosado'),
('Ron','Destilado de caña'),
('Whisky','Destilado de cereal envejecido'),
('Aguardiente','Anís tradicional');

/* =========================================================
   7) MARCAS
   ========================================================= */

CREATE TABLE Marcas(
    MarcaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre  VARCHAR(80) NOT NULL
);

INSERT INTO Marcas (Nombre) VALUES
('Andina'),
('Sol Dorado'),
('Caribeño'),
('Highland Oak'),
('Antioqueño');

/* =========================================================
   8) PRODUCTOS
   ========================================================= */

CREATE TABLE Productos(
    ProductoId   INT IDENTITY(1,1) PRIMARY KEY,
    CategoriaId  INT NOT NULL REFERENCES dbo.Categorias(CategoriaId),
    MarcaId      INT NOT NULL REFERENCES dbo.Marcas(MarcaId),
    ProveedorId  INT NOT NULL REFERENCES dbo.Proveedores(ProveedorId),
    Nombre       VARCHAR(120) NOT NULL,
    Unidad       VARCHAR(20) NOT NULL, -- Botella, Caja, SixPack
    ContenidoML  INT NOT NULL,
    PrecioUnit   DECIMAL(10,2) NOT NULL
);

INSERT INTO Productos (CategoriaId, MarcaId, ProveedorId, Nombre, Unidad, ContenidoML, PrecioUnit) VALUES
(1,1,4,'Cerveza Andina Lager 330ml','SixPack',330,18000),
(2,2,2,'Vino Sol Dorado Tinto 750ml','Botella',750,45000),
(3,3,3,'Ron Caribeño Añejo 750ml','Botella',750,65000),
(4,4,5,'Whisky Highland Oak 700ml','Botella',700,120000),
(5,5,1,'Aguardiente Antioqueño 750ml','Botella',750,40000);

/* =========================================================
   9) CLIENTES
   ========================================================= */

CREATE TABLE Clientes(
    ClienteId  INT IDENTITY(1,1) PRIMARY KEY,
    Tipo       VARCHAR(20) NOT NULL, -- 'Minorista','Mayorista','Bar','Restaurante'
    RazonSocial VARCHAR(120) NOT NULL,
    NIT        VARCHAR(40) NULL,
    Telefono   VARCHAR(30) NULL,
    Ciudad     VARCHAR(80) NULL
);

INSERT INTO Clientes (Tipo, RazonSocial, NIT, Telefono, Ciudad) VALUES
('Bar','Bar La Esquina','900111001-1','3111111111','Medellín'),
('Restaurante','Restaurante El Roble','900222002-2','3222222222','Envigado'),
('Mayorista','Distribuciones Norte','900333003-3','3333333333','Bello'),
('Minorista','Tienda Don Pepe','900444004-4','3444444444','Medellín'),
('Bar','Pub Central','900555005-5','3555555555','Bogotá');

/* =========================================================
   10) INVENTARIO (por sucursal y producto)
   ========================================================= */

CREATE TABLE Inventario(
    InventarioId INT IDENTITY(1,1) PRIMARY KEY,
    SucursalId   INT NOT NULL REFERENCES dbo.Sucursales(SucursalId),
    ProductoId   INT NOT NULL REFERENCES dbo.Productos(ProductoId),
    Stock        INT NOT NULL,
    UNIQUE(SucursalId, ProductoId)
);

INSERT INTO Inventario (SucursalId, ProductoId, Stock) VALUES
(1,1,100),
(1,2,60),
(1,3,40),
(2,1,50),
(3,5,30);

/* =========================================================
   11) LOTES
   ========================================================= */

CREATE TABLE Lotes(
    LoteId      INT IDENTITY(1,1) PRIMARY KEY,
    ProductoId  INT NOT NULL REFERENCES dbo.Productos(ProductoId),
    ProveedorId INT NOT NULL REFERENCES dbo.Proveedores(ProveedorId),
    CodigoLote  VARCHAR(40) NOT NULL,
    Vencimiento DATE NULL,
    Cantidad    INT NOT NULL
);

INSERT INTO Lotes (ProductoId, ProveedorId, CodigoLote, Vencimiento, Cantidad) VALUES
(1,4,'AND-330-20250901','2026-09-01',200),
(2,2,'SOL-TIN-20250815','2028-08-15',100),
(3,3,'CAR-ANJ-20250710','2027-07-10',80),
(4,5,'HOK-700-20250130','2030-01-30',50),
(5,1,'ANT-750-20250620','2027-06-20',120);

/* =========================================================
   12) COMPRAS (encabezado)
   ========================================================= */

CREATE TABLE Compras(
    CompraId    INT IDENTITY(1,1) PRIMARY KEY,
    ProveedorId INT NOT NULL REFERENCES dbo.Proveedores(ProveedorId),
    SucursalId  INT NOT NULL REFERENCES dbo.Sucursales(SucursalId),
    Fecha       DATE NOT NULL,
    Total       DECIMAL(12,2) NOT NULL DEFAULT 0
);

INSERT INTO Compras (ProveedorId, SucursalId, Fecha, Total) VALUES
(4,1,'2025-09-01',0),
(2,1,'2025-09-05',0),
(3,1,'2025-09-06',0),
(5,1,'2025-09-07',0),
(1,2,'2025-09-08',0);

/* =========================================================
   13) COMPRA DETALLE
   ========================================================= */

CREATE TABLE CompraDetalle(
    CompraDetId INT IDENTITY(1,1) PRIMARY KEY,
    CompraId    INT NOT NULL REFERENCES dbo.Compras(CompraId),
    ProductoId  INT NOT NULL REFERENCES dbo.Productos(ProductoId),
    Cantidad    INT NOT NULL,
    PrecioCompra DECIMAL(10,2) NOT NULL,
    Subtotal    AS (Cantidad * PrecioCompra) PERSISTED
);

INSERT INTO CompraDetalle (CompraId, ProductoId, Cantidad, PrecioCompra) VALUES
(1,1,120,2500),
(2,2,60,28000),
(3,3,40,42000),
(4,4,30,90000),
(5,5,80,26000);

/* Actualiza totales de compras */
UPDATE c
SET Total = d.SumSub
FROM dbo.Compras c
CROSS APPLY (
    SELECT SUM(Subtotal) AS SumSub
    FROM dbo.CompraDetalle d
    WHERE d.CompraId = c.CompraId
) d;

/* =========================================================
   14) VENTAS (encabezado)
   ========================================================= */

CREATE TABLE Ventas(
    VentaId    INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId  INT NOT NULL REFERENCES dbo.Clientes(ClienteId),
    SucursalId INT NOT NULL REFERENCES dbo.Sucursales(SucursalId),
    EmpleadoId INT NOT NULL REFERENCES dbo.Empleados(EmpleadoId),
    Fecha      DATE NOT NULL,
    Total      DECIMAL(12,2) NOT NULL DEFAULT 0
);

INSERT INTO Ventas (ClienteId, SucursalId, EmpleadoId, Fecha, Total) VALUES
(1,1,2,'2025-09-10',0),
(2,1,2,'2025-09-10',0),
(3,1,2,'2025-09-11',0),
(4,1,2,'2025-09-11',0),
(5,1,2,'2025-09-12',0);

/* =========================================================
   15) VENTA DETALLE
   ========================================================= */

CREATE TABLE VentaDetalle(
    VentaDetId  INT IDENTITY(1,1) PRIMARY KEY,
    VentaId     INT NOT NULL REFERENCES dbo.Ventas(VentaId),
    ProductoId  INT NOT NULL REFERENCES dbo.Productos(ProductoId),
    Cantidad    INT NOT NULL,
    PrecioVenta DECIMAL(10,2) NOT NULL,
    Subtotal    AS (Cantidad * PrecioVenta) PERSISTED
);

INSERT INTO VentaDetalle (VentaId, ProductoId, Cantidad, PrecioVenta) VALUES
(1,1,10,3000),
(2,2,4,50000),
(3,3,2,75000),
(4,5,6,42000),
(5,4,1,130000);

/*
_______________________

LA TABLA DE AUDITORIAS
--------------------------

*/
CREATE TABLE Auditoria(
  AuditoriaId   INT IDENTITY(1,1) PRIMARY KEY,
  Fecha         DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  Usuario       VARCHAR(80) NULL,
  Tabla         VARCHAR(128) NOT NULL,
  Llave         VARCHAR(200) NOT NULL,   -- PK(s) de la fila afectada
  Accion        VARCHAR(10) NOT NULL,    -- Added/Modified/Deleted
  ValoresAntes  NVARCHAR(MAX) NULL,      -- JSON
  ValoresDespues NVARCHAR(MAX) NULL      -- JSON
);
