-- Create the main Quantity Measurement Entity Table
-- Create the database
CREATE DATABASE QuantityMeasurementDB;
GO
-- Switch to the newly created database
USE QuantityMeasurementDB;
GO
CREATE TABLE QuantityMeasurementEntities (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,  -- Unique identifier for each measurement
    Operation NVARCHAR(255) NOT NULL,          -- Operation type (e.g., addition, subtraction)
    OperandOne NVARCHAR(255) NOT NULL,         -- First operand
    OperandTwo NVARCHAR(255) NOT NULL,         -- Second operand
    Result NVARCHAR(255) NOT NULL,             -- Result of the operation
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE() -- Timestamp when the entry is created
);

-- Create the History Table to store the history of all operations for audit purposes
CREATE TABLE QuantityMeasurementHistory (
    HistoryId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, -- Unique identifier for each history record
    QuantityMeasurementId UNIQUEIDENTIFIER NOT NULL, -- Reference to the original measurement entity
    Operation NVARCHAR(255) NOT NULL,                -- Operation performed
    OperandOne NVARCHAR(255) NOT NULL,               -- First operand value
    OperandTwo NVARCHAR(255) NOT NULL,               -- Second operand value
    Result NVARCHAR(255) NOT NULL,                   -- Result of the operation
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),  -- Timestamp when this history record is created
    FOREIGN KEY (QuantityMeasurementId) REFERENCES QuantityMeasurementEntities(Id) -- Linking to original measurement
);

-- Create Indexes for performance optimization
CREATE INDEX IX_Operation ON QuantityMeasurementEntities (Operation);
CREATE INDEX IX_CreatedAt ON QuantityMeasurementEntities (CreatedAt);

-- Create a table to store user or device information (if needed in the future)
CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,    -- Unique identifier for each user
    UserName NVARCHAR(255) NOT NULL,                  -- User's name
    Email NVARCHAR(255) NOT NULL                      -- User's email address
);

-- Optional: A table for Unit Types (Length, Weight, Volume, etc.) for validation purposes
CREATE TABLE UnitTypes (
    UnitTypeId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, -- Unique identifier for each unit type
    UnitName NVARCHAR(255) NOT NULL,                  -- Name of the unit (e.g., meter, kilogram)
    UnitSymbol NVARCHAR(20) NOT NULL,                 -- Symbol for the unit (e.g., m, kg)
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()     -- Timestamp when this unit was created
);
Go
-- Example of a Sample Procedure to log measurement history upon save
CREATE PROCEDURE LogMeasurementHistory
    @QuantityMeasurementId UNIQUEIDENTIFIER,
    @Operation NVARCHAR(255),
    @OperandOne NVARCHAR(255),
    @OperandTwo NVARCHAR(255),
    @Result NVARCHAR(255)
AS
BEGIN
    INSERT INTO QuantityMeasurementHistory (QuantityMeasurementId, Operation, OperandOne, OperandTwo, Result)
    VALUES (@QuantityMeasurementId, @Operation, @OperandOne, @OperandTwo, @Result);
END;