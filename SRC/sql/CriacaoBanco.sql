IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Banks] (
    [Id] uniqueidentifier NOT NULL,
    [Code] int NOT NULL,
    [Name] varchar(200) NOT NULL,
    CONSTRAINT [PK_Banks] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [BankAccounts] (
    [Id] uniqueidentifier NOT NULL,
    [Type] varchar(200) NOT NULL,
    [AgencyCode] varchar(50) NOT NULL,
    [AccountCode] varchar(100) NULL,
    [BankId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_BankAccounts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BankAccounts_Banks_BankId] FOREIGN KEY ([BankId]) REFERENCES [Banks] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [BankStatements] (
    [Id] uniqueidentifier NOT NULL,
    [Status] varchar(50) NOT NULL,
    [InitialDate] datetime2 NOT NULL,
    [FinalDate] datetime2 NOT NULL,
    [BankAccountId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_BankStatements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BankStatements_BankAccounts_BankAccountId] FOREIGN KEY ([BankAccountId]) REFERENCES [BankAccounts] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [BankTransactions] (
    [Id] uniqueidentifier NOT NULL,
    [Type] varchar(100) NOT NULL,
    [Date] datetime2 NOT NULL,
    [TransactionValue] float NOT NULL,
    [Description] varchar(100) NULL,
    [Checksum] bigint NOT NULL,
    [BankStatementId] uniqueidentifier NULL,
    CONSTRAINT [PK_BankTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BankTransactions_BankStatements_BankStatementId] FOREIGN KEY ([BankStatementId]) REFERENCES [BankStatements] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_BankAccounts_BankId] ON [BankAccounts] ([BankId]);

GO

CREATE INDEX [IX_BankStatements_BankAccountId] ON [BankStatements] ([BankAccountId]);

GO

CREATE INDEX [IX_BankTransactions_BankStatementId] ON [BankTransactions] ([BankStatementId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200304051910_Initial', N'3.1.2');

GO

