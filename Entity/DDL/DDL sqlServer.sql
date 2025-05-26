-- Script DDL para SQL Server
-- Versión: 1.0
-- Autor: bscl
-- Fecha: 17/05/2025
-- Descripción: Script para crear las tablas del sistema Encarte1817

-- Desactivar la verificación de claves foráneas durante la creación de tablas
SET NOCOUNT ON;
GO

-- Eliminar tablas si existen para garantizar una creación limpia
IF OBJECT_ID('dbo.RolUsers', 'U') IS NOT NULL
    DROP TABLE dbo.RolUsers;
GO

IF OBJECT_ID('dbo.Roles', 'U') IS NOT NULL
    DROP TABLE dbo.Roles;
GO

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO

-- Crear tabla de Usuarios
CREATE TABLE dbo.Users (
    Id NVARCHAR(100) PRIMARY KEY,
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Status BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NULL,
    UpdatedAt DATETIME NULL,
    DeleteAt DATETIME NULL
);
GO

-- Crear índice para optimizar búsquedas por Email
CREATE UNIQUE INDEX IX_Users_Email ON dbo.Users(Email) WHERE Status = 1;
GO

-- Crear tabla de Roles
CREATE TABLE dbo.Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NOT NULL,
    Status BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NULL,
    UpdatedAt DATETIME NULL,
    DeleteAt DATETIME NULL
);
GO

-- Crear índice para optimizar búsquedas por Nombre de Rol
CREATE UNIQUE INDEX IX_Roles_Name ON dbo.Roles(Name) WHERE Status = 1;
GO

-- Crear tabla de relación entre Roles y Usuarios
CREATE TABLE dbo.RolUsers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RolId INT NOT NULL,
    UserId NVARCHAR(100) NOT NULL,
    Status BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME NULL,
    UpdatedAt DATETIME NULL,
    DeleteAt DATETIME NULL,
    CONSTRAINT FK_RolUsers_Roles FOREIGN KEY (RolId) REFERENCES dbo.Roles(Id),
    CONSTRAINT FK_RolUsers_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
);
GO

-- Crear índice para optimizar búsquedas de roles por usuario
CREATE INDEX IX_RolUsers_UserId ON dbo.RolUsers(UserId);
GO

-- Crear índice para optimizar búsquedas de usuarios por rol
CREATE INDEX IX_RolUsers_RolId ON dbo.RolUsers(RolId);
GO

-- Crear índice único para evitar duplicados de asignaciones rol-usuario
CREATE UNIQUE INDEX IX_RolUsers_Unique ON dbo.RolUsers(RolId, UserId) WHERE Status = 1;
GO

-- Insertar roles básicos para el sistema
INSERT INTO dbo.Roles (Name, Description, Status, CreatedAt)
VALUES 
    ('Admin', 'Administrador del sistema con acceso completo', 1, GETUTCDATE()),
    ('Usuario', 'Usuario regular con acceso limitado', 1, GETUTCDATE());
GO

-- Insertar un usuario administrador por defecto (contraseña: Admin123!)
-- Nota: En un sistema real, la contraseña debe estar cifrada
INSERT INTO dbo.Users (Id, Email, Password, Status, CreatedAt)
VALUES 
    (NEWID(), 'admin@encarte1817.com', 'Admin123!', 1, GETUTCDATE());
GO

-- Obtener el ID del usuario admin creado
DECLARE @adminUserId NVARCHAR(100);
SELECT @adminUserId = Id FROM dbo.Users WHERE Email = 'admin@encarte1817.com';

-- Asignar rol de administrador al usuario admin
INSERT INTO dbo.RolUsers (RolId, UserId, Status, CreatedAt)
VALUES 
    (1, @adminUserId, 1, GETUTCDATE());
GO

-- Crear vistas para simplificar consultas comunes

-- Vista para obtener usuarios con sus roles
CREATE OR ALTER VIEW dbo.vw_UsersWithRoles
AS
SELECT 
    u.Id AS UserId,
    u.Email,
    u.Status AS UserStatus,
    r.Id AS RolId,
    r.Name AS RolName,
    r.Description AS RolDescription,
    ru.Status AS AssignmentStatus
FROM 
    dbo.Users u
    LEFT JOIN dbo.RolUsers ru ON u.Id = ru.UserId AND ru.Status = 1
    LEFT JOIN dbo.Roles r ON ru.RolId = r.Id AND r.Status = 1
WHERE 
    u.Status = 1;
GO

-- Vista para obtener roles con conteo de usuarios
CREATE OR ALTER VIEW dbo.vw_RolesWithUserCount
AS
SELECT 
    r.Id,
    r.Name,
    r.Description,
    r.Status,
    COUNT(DISTINCT ru.UserId) AS UserCount
FROM 
    dbo.Roles r
    LEFT JOIN dbo.RolUsers ru ON r.Id = ru.RolId AND ru.Status = 1
GROUP BY 
    r.Id, r.Name, r.Description, r.Status;
GO

-- Crear procedimientos almacenados para operaciones comunes

-- Procedimiento para activar/desactivar un usuario
CREATE OR ALTER PROCEDURE dbo.sp_UpdateUserStatus
    @UserId NVARCHAR(100),
    @Status BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.Users
    SET 
        Status = @Status,
        UpdatedAt = GETUTCDATE(),
        DeleteAt = CASE WHEN @Status = 0 THEN GETUTCDATE() ELSE NULL END
    WHERE Id = @UserId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO

-- Procedimiento para activar/desactivar un rol
CREATE OR ALTER PROCEDURE dbo.sp_UpdateRolStatus
    @RolId INT,
    @Status BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.Roles
    SET 
        Status = @Status,
        UpdatedAt = GETUTCDATE(),
        DeleteAt = CASE WHEN @Status = 0 THEN GETUTCDATE() ELSE NULL END
    WHERE Id = @RolId;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO

-- Procedimiento para buscar usuarios con paginación
CREATE OR ALTER PROCEDURE dbo.sp_SearchUsers
    @SearchTerm NVARCHAR(255) = NULL,
    @Status BIT = 1,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Calcular el número total de registros (para la paginación)
    SELECT COUNT(*) AS TotalCount
    FROM dbo.Users
    WHERE 
        (@SearchTerm IS NULL OR Email LIKE '%' + @SearchTerm + '%') AND
        (@Status IS NULL OR Status = @Status);
    
    -- Obtener los resultados paginados
    SELECT 
        Id, Email, Status, CreatedAt, UpdatedAt, DeleteAt
    FROM dbo.Users
    WHERE 
        (@SearchTerm IS NULL OR Email LIKE '%' + @SearchTerm + '%') AND
        (@Status IS NULL OR Status = @Status)
    ORDER BY CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Procedimiento para buscar roles con paginación
CREATE OR ALTER PROCEDURE dbo.sp_SearchRoles
    @SearchTerm NVARCHAR(255) = NULL,
    @Status BIT = 1,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Calcular el número total de registros (para la paginación)
    SELECT COUNT(*) AS TotalCount
    FROM dbo.Roles
    WHERE 
        (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%' OR Description LIKE '%' + @SearchTerm + '%') AND
        (@Status IS NULL OR Status = @Status);
    
    -- Obtener los resultados paginados
    SELECT 
        Id, Name, Description, Status, CreatedAt, UpdatedAt, DeleteAt
    FROM dbo.Roles
    WHERE 
        (@SearchTerm IS NULL OR Name LIKE '%' + @SearchTerm + '%' OR Description LIKE '%' + @SearchTerm + '%') AND
        (@Status IS NULL OR Status = @Status)
    ORDER BY CreatedAt DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Procedimiento para asignar un rol a un usuario
CREATE OR ALTER PROCEDURE dbo.sp_AssignRoleToUser
    @RolId INT,
    @UserId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Comprobar si ya existe una asignación activa
    IF EXISTS (SELECT 1 FROM dbo.RolUsers WHERE RolId = @RolId AND UserId = @UserId AND Status = 1)
    BEGIN
        SELECT 0 AS Result, 'La asignación ya existe y está activa' AS Message;
        RETURN;
    END
    
    -- Comprobar si existe una asignación inactiva
    IF EXISTS (SELECT 1 FROM dbo.RolUsers WHERE RolId = @RolId AND UserId = @UserId AND Status = 0)
    BEGIN
        -- Reactivar la asignación existente
        UPDATE dbo.RolUsers
        SET 
            Status = 1,
            UpdatedAt = GETUTCDATE(),
            DeleteAt = NULL
        WHERE RolId = @RolId AND UserId = @UserId;
        
        SELECT 1 AS Result, 'Asignación reactivada correctamente' AS Message;
    END
    ELSE
    BEGIN
        -- Crear una nueva asignación
        INSERT INTO dbo.RolUsers (RolId, UserId, Status, CreatedAt)
        VALUES (@RolId, @UserId, 1, GETUTCDATE());
        
        SELECT 1 AS Result, 'Asignación creada correctamente' AS Message;
    END
END;
GO

-- Procedimiento para revocar un rol de un usuario
CREATE OR ALTER PROCEDURE dbo.sp_RevokeRoleFromUser
    @RolId INT,
    @UserId NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE dbo.RolUsers
    SET 
        Status = 0,
        UpdatedAt = GETUTCDATE(),
        DeleteAt = GETUTCDATE()
    WHERE RolId = @RolId AND UserId = @UserId AND Status = 1;
    
    SELECT @@ROWCOUNT AS AffectedRows;
END;
GO

PRINT 'Script DDL ejecutado correctamente.';
GO