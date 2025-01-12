-- Crear la base de datos EstadoCuenta
IF NOT EXISTS (
    SELECT [name]
        FROM sys.databases
        WHERE [name] = N'EstadoCuenta'
)
CREATE DATABASE EstadoCuenta
GO

-- Usar la base de datos creada
USE EstadoCuenta
GO

-- Tabla Usuarios
CREATE TABLE Usuarios (
    UsuarioID INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL
);
GO
-- Tabla Tarjetas
CREATE TABLE Tarjetas (
    TarjetaID INT IDENTITY(1,1) PRIMARY KEY,
    NumeroTarjeta NVARCHAR(16) UNIQUE NOT NULL,
    SaldoActual DECIMAL(10, 2) NOT NULL,
    LimiteCredito DECIMAL(10, 2) NOT NULL,
    SaldoDisponible AS (LimiteCredito - SaldoActual) PERSISTED,
    UsuarioID INT NOT NULL,
    FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);
GO
-- Tabla Compras
CREATE TABLE Compras (
    CompraID INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    TarjetaID INT NOT NULL,
    FOREIGN KEY (TarjetaID) REFERENCES Tarjetas(TarjetaID)
);
GO
ALTER TABLE Compras
ADD CONSTRAINT DF_Compras_Fecha DEFAULT (GETDATE()) FOR Fecha;
GO
-- Tabla Pagos
CREATE TABLE Pagos (
    PagoID INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATE NOT NULL,
    Monto DECIMAL(10, 2) NOT NULL,
    TarjetaID INT NOT NULL,
    FOREIGN KEY (TarjetaID) REFERENCES Tarjetas(TarjetaID)
);
GO
ALTER TABLE Pagos
ADD CONSTRAINT DF_Pagos_Fecha DEFAULT (GETDATE()) FOR Fecha;
GO
-- Tabla Configuraciones
CREATE TABLE Configuraciones (
    ConfiguracionID INT IDENTITY(1,1) PRIMARY KEY,
    PorcentajeInteres DECIMAL(5, 2) NOT NULL,
    PorcentajeSaldoMinimo DECIMAL(5, 2) NOT NULL
);
GO
-- Crear un trigger para recalcular el SaldoActual cuando se agreguen, eliminen o actualicen compras
CREATE TRIGGER trg_RecalcularSaldoActual
ON Compras
AFTER INSERT, DELETE, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Recalcular el SaldoActual para las tarjetas afectadas
    UPDATE Tarjetas
    SET SaldoActual = (
        SELECT ISNULL(SUM(c.Monto), 0)
        FROM Compras c
        WHERE c.TarjetaID = t.TarjetaID
    )
    FROM Tarjetas t
    INNER JOIN (
        SELECT DISTINCT TarjetaID FROM Inserted
        UNION
        SELECT DISTINCT TarjetaID FROM Deleted
    ) cambios ON t.TarjetaID = cambios.TarjetaID;
END;
GO

-- Crear el trigger para actualizar el saldo después de un pago
CREATE TRIGGER trg_DescontarPago
ON Pagos
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TarjetaID INT, @MontoPago DECIMAL(10, 2), @Interes DECIMAL(10, 2);

    -- Obtener el TarjetaID y MontoPago
    SELECT @TarjetaID = TarjetaID, @MontoPago = Monto
    FROM Inserted;

    -- Calcular el interés basado en la configuración
    SELECT @Interes = PorcentajeInteres
    FROM Configuraciones;

    -- Actualizar el saldo actual de la tarjeta con el monto pagado menos el interés
    UPDATE Tarjetas
    SET SaldoActual = SaldoActual - (@MontoPago - (@MontoPago * @Interes / 100))
    WHERE TarjetaID = @TarjetaID;
END;
GO

-- Insertar configuración predeterminada (Porcentaje de interés y porcentaje de saldo mínimo)
INSERT INTO Configuraciones (PorcentajeInteres, PorcentajeSaldoMinimo)
VALUES (25, 5);
GO
-- Insertar algunos usuarios de ejemplo
INSERT INTO Usuarios (Nombre, Apellido)
VALUES ('Juan', 'Perez');
INSERT INTO Usuarios (Nombre, Apellido)
VALUES ('Alan', 'Brito');
GO
-- Insertar tarjetas de ejemplo
INSERT INTO Tarjetas (NumeroTarjeta, SaldoActual, LimiteCredito, UsuarioID)
VALUES ('1234567890123456', 114.47, 5000.00, 1);
INSERT INTO Tarjetas (NumeroTarjeta, SaldoActual, LimiteCredito, UsuarioID)
VALUES ('9876543210987654', 200.00, 10000.00, 2);
GO
-- Insertar algunas compras de ejemplo
INSERT INTO Compras (Fecha, Descripcion, Monto, TarjetaID)
VALUES 
('2025-01-01', 'Compra en Supermercado', 50.00, 1),
('2025-01-02', 'Pago en Restaurante', 30.00, 1),
('2025-01-03', 'Compra en Tienda Online', 10.00, 1),
('2025-01-03', 'Compra en Tienda Online', 34.47, 1);
GO
INSERT INTO Compras (Fecha, Descripcion, Monto, TarjetaID)
VALUES 
('2025-01-05', 'Compra en Librería', 80.00, 2),
('2025-01-06', 'Compra en Tienda de Ropa', 50.00, 2),
('2025-01-07', 'Pago en Gasolinera', 70.00, 2),
('2025-01-07', 'Pago en Gasolinera', 30.00, 2),
('2025-01-07', 'Pago en Gasolinera', 20.00, 2);

-- Insertar pagos de ejemplo
-- INSERT INTO Pagos (Fecha, Monto, TarjetaID)
-- VALUES ('2025-01-08', 10.00, 1);

-- Consultar el saldo después del pago
-- SELECT 
--     u.Nombre,
--     u.Apellido,
--     t.NumeroTarjeta,
--     t.SaldoActual,
--     t.LimiteCredito,
--     t.SaldoDisponible,
--     ISNULL(SUM(c.Monto), 0) AS ComprasMesActual,
--     (SELECT ISNULL(SUM(Monto), 0) 
--      FROM Compras 
--      WHERE MONTH(Fecha) = MONTH(GETDATE()) - 1 AND TarjetaID = t.TarjetaID) AS ComprasMesAnterior,
--     (t.SaldoActual * (SELECT PorcentajeInteres / 100 FROM Configuraciones)) AS InteresBonificable,
--     (t.SaldoActual * (SELECT PorcentajeSaldoMinimo / 100 FROM Configuraciones)) AS CuotaMinima,
--     (ISNULL(SaldoActual, 0)) AS MontoTotalPagar,
--     (t.SaldoActual + (t.SaldoActual * (SELECT PorcentajeInteres / 100 FROM Configuraciones))) AS PagoContadoConIntereses
-- FROM 
--     Usuarios u
-- JOIN 
--     Tarjetas t ON u.UsuarioID = t.UsuarioID
-- LEFT JOIN 
--     Compras c ON t.TarjetaID = c.TarjetaID AND MONTH(c.Fecha) = MONTH(GETDATE())
-- GROUP BY 
--     u.Nombre, u.Apellido, t.NumeroTarjeta, t.SaldoActual, t.LimiteCredito, t.SaldoDisponible, t.TarjetaID;



-- USE master;
-- GO

-- ALTER DATABASE [EstadoCuenta] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
-- GO

-- DROP DATABASE [EstadoCuenta];
-- GO
GO
CREATE PROCEDURE SP_GetEstadoCuenta
@UsuarioID INT
AS
BEGIN
        SELECT 
        u.Nombre,
        u.Apellido,
        t.TarjetaID,
        t.NumeroTarjeta,
        t.SaldoActual,
        t.LimiteCredito,
        t.SaldoDisponible,
        ISNULL(SUM(c.Monto), 0) AS ComprasMesActual,
        (SELECT ISNULL(SUM(Monto), 0) 
        FROM Compras 
        WHERE MONTH(Fecha) = MONTH(GETDATE()) - 1 AND TarjetaID = t.TarjetaID) AS ComprasMesAnterior,
        (t.SaldoActual * (SELECT PorcentajeInteres / 100 FROM Configuraciones)) AS InteresBonificable,
        (t.SaldoActual * (SELECT PorcentajeSaldoMinimo / 100 FROM Configuraciones)) AS CuotaMinima,
        (ISNULL(SaldoActual, 0)) AS MontoTotalPagar,
        (t.SaldoActual + (t.SaldoActual * (SELECT PorcentajeInteres / 100 FROM Configuraciones))) AS PagoContadoConIntereses
    FROM 
        Usuarios u
    JOIN 
        Tarjetas t ON u.UsuarioID = t.UsuarioID
    LEFT JOIN 
        Compras c ON t.TarjetaID = c.TarjetaID AND MONTH(c.Fecha) = MONTH(GETDATE())
    WHERE u.UsuarioID = @UsuarioID
    GROUP BY 
        u.Nombre, u.Apellido, t.NumeroTarjeta, t.SaldoActual, t.LimiteCredito, t.SaldoDisponible, t.TarjetaID;

END
GO
CREATE PROCEDURE [dbo].[SP_MostrarTransaccionesMesActual]
    @TarjetaID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Mostrar todas las compras y pagos de la tarjeta en el mes actual
    SELECT 
        'Compra' AS TipoTransaccion,
        c.CompraID AS TransaccionID,
        c.Fecha,
        c.Descripcion,
        c.Monto
    FROM 
        Compras c
    WHERE 
        c.TarjetaID = @TarjetaID
        AND MONTH(c.Fecha) = MONTH(GETDATE()) 
        AND YEAR(c.Fecha) = YEAR(GETDATE())

    UNION ALL

    SELECT 
        'Pago' AS TipoTransaccion,
        p.PagoID AS TransaccionID,
        p.Fecha,
        'Pago realizado' AS Descripcion,
        p.Monto
    FROM 
        Pagos p
    WHERE 
        p.TarjetaID = @TarjetaID
        AND MONTH(p.Fecha) = MONTH(GETDATE()) 
        AND YEAR(p.Fecha) = YEAR(GETDATE())

    ORDER BY Fecha DESC;  -- Ordenar por fecha descendente
END;
GO
-- Procedimiento almacenado para insertar pagos
CREATE PROCEDURE [dbo].[SP_InsertarPago]
    @Monto DECIMAL(10, 2),
    @TarjetaID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar el pago en la tabla Pagos
    INSERT INTO Pagos (Monto, TarjetaID)
    VALUES (@Monto, @TarjetaID);

    -- Mensaje de éxito
    PRINT 'Pago registrado exitosamente';
END;
GO
-- Procedimiento almacenado para insertar compras
CREATE PROCEDURE SP_InsertarCompra
    @Descripcion NVARCHAR(255),
    @Monto DECIMAL(10, 2),
    @TarjetaID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar la compra en la tabla Compras
    INSERT INTO Compras (Fecha, Descripcion, Monto, TarjetaID)
    VALUES (GETDATE(), @Descripcion, @Monto, @TarjetaID);

    -- Mensaje de éxito
    PRINT 'Compra registrada exitosamente';
END;
GO
CREATE PROCEDURE [dbo].[SP_GetCompras]
    @TarjetaID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar el pago en la tabla Pagos
    SELECT * FROM Compras WHERE TarjetaID = @TarjetaID
END;
GO
