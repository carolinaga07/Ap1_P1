IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Estudiantes] (
    [EstudianteId] int NOT NULL IDENTITY,
    [Nombres] nvarchar(max) NOT NULL,
    [Direccion] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [FechaNacimiento] datetime2 NOT NULL,
    CONSTRAINT [PK_Estudiantes] PRIMARY KEY ([EstudianteId])
);

CREATE TABLE [Libros] (
    [LibroId] int NOT NULL IDENTITY,
    [Titulo] nvarchar(max) NOT NULL,
    [Autor] nvarchar(max) NOT NULL,
    [AnioPublicacion] int NOT NULL,
    CONSTRAINT [PK_Libros] PRIMARY KEY ([LibroId])
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'LibroId', N'AnioPublicacion', N'Autor', N'Titulo') AND [object_id] = OBJECT_ID(N'[Libros]'))
    SET IDENTITY_INSERT [Libros] ON;
INSERT INTO [Libros] ([LibroId], [AnioPublicacion], [Autor], [Titulo])
VALUES (1, 1950, N'C.S Lewis', N'Cronicas de Narnia'),
(2, 1999, N'Stephen Chbosky', N'The perks of being a wallflower');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'LibroId', N'AnioPublicacion', N'Autor', N'Titulo') AND [object_id] = OBJECT_ID(N'[Libros]'))
    SET IDENTITY_INSERT [Libros] OFF;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260918034843_Initial', N'10.0.12');

COMMIT;
GO

