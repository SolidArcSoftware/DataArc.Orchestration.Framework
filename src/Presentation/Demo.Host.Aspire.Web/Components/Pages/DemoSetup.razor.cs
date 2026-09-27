using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Demo.Host.Aspire.Web.Components.Pages;

public partial class DemoSetup
{
    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    protected string SqlScript { get; } = """
-- Database does not exist and will be created
CREATE DATABASE [DataArcOrchestrationDemo];
GO

USE [DataArcOrchestrationDemo];
GO

IF SCHEMA_ID('hr') IS NULL EXEC('CREATE SCHEMA [hr]');
GO

IF SCHEMA_ID('it') IS NULL EXEC('CREATE SCHEMA [it]');
GO

IF SCHEMA_ID('operations') IS NULL EXEC('CREATE SCHEMA [operations]');
GO

IF SCHEMA_ID('finance') IS NULL EXEC('CREATE SCHEMA [finance]');
GO

CREATE TABLE [dbo].[AspNetUsers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Email] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [PasswordHash] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(256) NULL,
    [UserTimeZone] nvarchar(max) NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[AspNetRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[AspNetRoleClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [RoleId] int NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] int NOT NULL,
    PRIMARY KEY ([LoginProvider], [ProviderKey])
);
GO

CREATE TABLE [dbo].[AspNetUserRoles] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    PRIMARY KEY ([UserId], [RoleId])
);
GO

CREATE TABLE [dbo].[AspNetUserTokens] (
    [UserId] int NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    PRIMARY KEY ([UserId], [LoginProvider], [Name])
);
GO

CREATE TABLE [hr].[Department] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [hr].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CreatedUtc] datetime2 NOT NULL,
    [EmployerId] int NOT NULL,
    [IsArchived] bit NOT NULL,
    [LastUpdatedUtc] datetime2 NULL,
    [EmployeeName] varchar(50) NULL,
    [Notes] nvarchar(500) NULL,
    [OnBoardingStatus] varchar(20) NULL,
    [PositionOrder] int NULL,
    [Rating] float NOT NULL,
    [EmployeeSalary] decimal(33,2) NOT NULL,
    [EmployeeNameSurname] varchar(50) NULL,
    [UserId] int NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [hr].[EmployeeDepartment] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DepartmentId] int NOT NULL,
    [EmployeeId] int NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [hr].[Employers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] text NULL,
    [Name] nvarchar(max) NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [it].[AccessRequests] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AccessLevel] nvarchar(max) NOT NULL,
    [CompletedOnUtc] datetimeoffset NULL,
    [EmailAddress] nvarchar(max) NOT NULL,
    [EmployeeId] int NOT NULL,
    [RequestStatus] nvarchar(max) NOT NULL,
    [RequestedOnUtc] datetimeoffset NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [operations].[OnboardingTask] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompletedOnUtc] datetimeoffset NULL,
    [CreatedOnUtc] datetimeoffset NULL,
    [DueDateUtc] datetimeoffset NULL,
    [EmployeeId] int NOT NULL,
    [TaskName] nvarchar(max) NOT NULL,
    [TaskStatus] nvarchar(max) NOT NULL,
    PRIMARY KEY ([Id])
);
GO

CREATE TABLE [finance].[PayrollRecords] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AnnualSalary] decimal(18,2) NOT NULL,
    [CreatedOnUtc] datetimeoffset NOT NULL,
    [CurrencyCode] nvarchar(max) NOT NULL,
    [EmployeeId] int NOT NULL,
    [IsActive] bit NOT NULL,
    PRIMARY KEY ([Id])
);
GO

ALTER TABLE [dbo].[AspNetRoleClaims]
ADD CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
FOREIGN KEY ([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id]);
GO

ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
FOREIGN KEY ([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id]);
GO

ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
FOREIGN KEY ([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id]);
GO

ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
FOREIGN KEY ([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id]);
GO

ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
FOREIGN KEY ([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id]);
GO

ALTER TABLE [dbo].[AspNetUserTokens]
ADD CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
FOREIGN KEY ([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id]);
GO

ALTER TABLE [hr].[Employees]
ADD CONSTRAINT [FK_Employees_Employers_EmployerId]
FOREIGN KEY ([EmployerId])
REFERENCES [hr].[Employers] ([Id]);
GO

ALTER TABLE [hr].[Employees]
ADD CONSTRAINT [FK_Employees_AspNetUsers_UserId]
FOREIGN KEY ([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id]);
GO

ALTER TABLE [hr].[EmployeeDepartment]
ADD CONSTRAINT [FK_EmployeeDepartment_Department_DepartmentId]
FOREIGN KEY ([DepartmentId])
REFERENCES [hr].[Department] ([Id]);
GO

ALTER TABLE [hr].[EmployeeDepartment]
ADD CONSTRAINT [FK_EmployeeDepartment_Employees_EmployeeId]
FOREIGN KEY ([EmployeeId])
REFERENCES [hr].[Employees] ([Id]);
GO

ALTER TABLE [it].[AccessRequests]
ADD CONSTRAINT [FK_AccessRequests_Employees_EmployeeId]
FOREIGN KEY ([EmployeeId])
REFERENCES [hr].[Employees] ([Id]);
GO

ALTER TABLE [operations].[OnboardingTask]
ADD CONSTRAINT [FK_OnboardingTask_Employees_EmployeeId]
FOREIGN KEY ([EmployeeId])
REFERENCES [hr].[Employees] ([Id]);
GO

ALTER TABLE [finance].[PayrollRecords]
ADD CONSTRAINT [FK_PayrollRecords_Employees_EmployeeId]
FOREIGN KEY ([EmployeeId])
REFERENCES [hr].[Employees] ([Id]);
GO
""";

    protected async Task CopySqlAsync()
    {
        await JsRuntime.InvokeVoidAsync(
            "navigator.clipboard.writeText",
            SqlScript);
    }
}