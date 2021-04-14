USE [master]

IF db_id('PatientLabResults') IS NULL
  CREATE DATABASE [PatientLabResults]
GO

USE [PatientLabResults]
GO

DROP TABLE IF EXISTS [LabResult];
DROP TABLE IF EXISTS [Patient];

CREATE TABLE [Patient] (
  [Id] integer PRIMARY KEY IDENTITY,
  [FirstName] nvarchar(50) NOT NULL,
  [MiddleName] nvarchar(50),
  [LastName] nvarchar(50) NOT NULL,
  [SexAtBirth] nvarchar(10) NOT NULL,
  [DateOfBirth] datetime NOT NULL,
  [Height] integer NOT NULL,
  [Weight] integer NOT NULL,
  [InsuranceCompany] nvarchar(255),
  [MemberId] nvarchar(25),
  [GroupId] nvarchar(25),
  [IsPolicyHolder] bit
)
GO

CREATE TABLE [LabResult] (
  [Id] integer PRIMARY KEY IDENTITY,
  [TestType] nvarchar(50) NOT NULL,
  [Result] nvarchar(20) NOT NULL,
  [PatientId] integer NOT NULL,
  [TimeOfTest] datetime NOT NULL,
  [EnteredTime] datetime NOT NULL,
  [LabName] nvarchar(255) NOT NULL,
  [OrderedByProvider] nvarchar(255),
  [Measurement] decimal,
  [Unit] nvarchar(255)

  CONSTRAINT [FK_Patent_LabResult] FOREIGN KEY ([PatientId]) REFERENCES [Patient] ([Id])
)
GO