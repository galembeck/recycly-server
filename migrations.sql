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
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE TABLE [TBUser] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Document] nvarchar(max) NOT NULL,
        [BirthDate] date NULL,
        [Phones] nvarchar(max) NULL,
        [ProfileType] int NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [LastAccessAt] datetimeoffset NULL,
        [PasswordChangeToken] nvarchar(max) NULL,
        [PasswordChangeTokenExpiresAt] datetimeoffset NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBUser] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE TABLE [TBUserHistoric] (
        [Id] nvarchar(450) NOT NULL,
        [IdUser] nvarchar(max) NOT NULL,
        [DateStart] datetime2 NOT NULL,
        [DateEnd] datetime2 NULL,
        [Name] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Document] nvarchar(max) NOT NULL,
        [BirthDate] date NULL,
        [Phones] nvarchar(max) NULL,
        [ProfileType] int NULL,
        [Password] nvarchar(max) NOT NULL,
        [LastAccessAt] datetimeoffset NULL,
        [PasswordChangeToken] nvarchar(max) NULL,
        [PasswordChangeTokenExpiresAt] datetimeoffset NULL,
        [UpdatedColumn] nvarchar(max) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBUserHistoric] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE TABLE [TBAccessToken] (
        [Id] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [ExpiresAt] datetimeoffset NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBAccessToken] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TBAccessToken_TBUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [TBUser] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE TABLE [TBRefreshToken] (
        [Id] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [ExpiresAt] datetimeoffset NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBRefreshToken] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TBRefreshToken_TBUser_User~] FOREIGN KEY ([UserId]) REFERENCES [TBUser] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE TABLE [TBUserSecurityInfo] (
        [Id] nvarchar(450) NOT NULL,
        [Ip] nvarchar(max) NOT NULL,
        [MacAdress] nvarchar(max) NOT NULL,
        [Browser] nvarchar(max) NOT NULL,
        [Hash] nvarchar(max) NOT NULL,
        [Moment] int NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBUserSecurityInfo] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TBUserSecurityInfo_TBUser_~] FOREIGN KEY ([UserId]) REFERENCES [TBUser] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_TBAccessToken_UserId] ON [TBAccessToken] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_TBRefreshToken_UserId] ON [TBRefreshToken] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_TBUserSecurityInfo_UserId] ON [TBUserSecurityInfo] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260425210102_InitialMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260425210102_InitialMigration', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426121106_AddDocumentUploadUrl'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426121106_AddDocumentUploadUrl', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426122738_UpdateMigrationsV5'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426122738_UpdateMigrationsV5', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426130000_AddDocumentUrlToUser'
)
BEGIN
    ALTER TABLE [TBUser] ADD [DocumentUrl] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426130000_AddDocumentUrlToUser'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426130000_AddDocumentUrlToUser', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE TABLE [TBCollectionPoint] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [ZipCode] nvarchar(10) NOT NULL,
        [Address] nvarchar(250) NOT NULL,
        [Number] nvarchar(20) NOT NULL,
        [Complement] nvarchar(100) NULL,
        [Neighborhood] nvarchar(150) NOT NULL,
        [City] nvarchar(150) NOT NULL,
        [State] nvarchar(2) NOT NULL,
        [Latitude] nvarchar(30) NOT NULL,
        [Longitude] nvarchar(30) NOT NULL,
        [OpeningTime] time NOT NULL,
        [ClosingTime] time NOT NULL,
        [Phone] nvarchar(20) NOT NULL,
        [CooperativeId] nvarchar(450) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBCollectionPoint] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TBCollectionPoint_TBUser_C~] FOREIGN KEY ([CooperativeId]) REFERENCES [TBUser] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE TABLE [TBMaterial] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(150) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [Color] nvarchar(20) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBMaterial] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE TABLE [TBCollect] (
        [Id] nvarchar(450) NOT NULL,
        [CollectionPointId] nvarchar(450) NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [MaterialId] nvarchar(450) NOT NULL,
        [WeightKg] decimal(10,3) NOT NULL,
        [CollectedAt] datetimeoffset NOT NULL,
        [Notes] nvarchar(500) NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBCollect] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TBCollect_TBCollectionPoin~] FOREIGN KEY ([CollectionPointId]) REFERENCES [TBCollectionPoint] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TBCollect_TBMaterial_Mater~] FOREIGN KEY ([MaterialId]) REFERENCES [TBMaterial] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TBCollect_TBUser_UserId] FOREIGN KEY ([UserId]) REFERENCES [TBUser] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE TABLE [TBCollectionPointMaterial] (
        [CollectionPointId] nvarchar(450) NOT NULL,
        [MaterialId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_TBCollectionPointMaterial] PRIMARY KEY ([CollectionPointId], [MaterialId]),
        CONSTRAINT [FK_TBCollectionPointMaterial_~] FOREIGN KEY ([CollectionPointId]) REFERENCES [TBCollectionPoint] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TBCollectionPointMaterial~1] FOREIGN KEY ([MaterialId]) REFERENCES [TBMaterial] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE INDEX [IX_TBCollect_CollectionPointId] ON [TBCollect] ([CollectionPointId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE INDEX [IX_TBCollect_MaterialId] ON [TBCollect] ([MaterialId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE INDEX [IX_TBCollect_UserId] ON [TBCollect] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE INDEX [IX_TBCollectionPoint_Cooperat~] ON [TBCollectionPoint] ([CooperativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    CREATE INDEX [IX_TBCollectionPointMaterial_~] ON [TBCollectionPointMaterial] ([MaterialId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260426161722_AddMaterialCollectionPointCollect'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260426161722_AddMaterialCollectionPointCollect', N'10.0.3');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427170831_AddSaleEntity'
)
BEGIN
    CREATE TABLE [TBSale] (
        [Id] nvarchar(450) NOT NULL,
        [BuyerName] nvarchar(200) NOT NULL,
        [WeightKg] decimal(10,3) NOT NULL,
        [Price] decimal(10,2) NOT NULL,
        [SoldAt] datetimeoffset NOT NULL,
        [Notes] nvarchar(500) NULL,
        [CooperativeId] nvarchar(450) NOT NULL,
        [CreatedBy] nvarchar(max) NOT NULL,
        [UpdatedBy] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [UpdatedAt] datetimeoffset NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_TBSale] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TBSale_TBUser_CooperativeId] FOREIGN KEY ([CooperativeId]) REFERENCES [TBUser] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427170831_AddSaleEntity'
)
BEGIN
    CREATE TABLE [TBSaleMaterial] (
        [MaterialId] nvarchar(450) NOT NULL,
        [SaleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_TBSaleMaterial] PRIMARY KEY ([MaterialId], [SaleId]),
        CONSTRAINT [FK_TBSaleMaterial_TBMaterial_~] FOREIGN KEY ([MaterialId]) REFERENCES [TBMaterial] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TBSaleMaterial_TBSale_Sale~] FOREIGN KEY ([SaleId]) REFERENCES [TBSale] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427170831_AddSaleEntity'
)
BEGIN
    CREATE INDEX [IX_TBSale_CooperativeId] ON [TBSale] ([CooperativeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427170831_AddSaleEntity'
)
BEGIN
    CREATE INDEX [IX_TBSaleMaterial_SaleId] ON [TBSaleMaterial] ([SaleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260427170831_AddSaleEntity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260427170831_AddSaleEntity', N'10.0.3');
END;

COMMIT;
GO

