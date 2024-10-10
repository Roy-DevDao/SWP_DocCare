﻿-- Create a new database
CREATE DATABASE DocCare;
GO

USE DocCare;
GO

CREATE TABLE Contact (
    ContactId NVARCHAR(50) PRIMARY KEY,    -- checked
    Name NVARCHAR(255),
    Email NVARCHAR(255),
    Title NVARCHAR(255),
    Description NVARCHAR(MAX),
    Status NVARCHAR(50)
);
-- Account table
CREATE TABLE Account (                  -- checked
    Id NVARCHAR(255) PRIMARY KEY,
    Username NVARCHAR(255),
    Password NVARCHAR(255),
	Email NVARCHAR(255),
    Role INT,
    Status BIT
);

-- Patient table - Using Id from Account
CREATE TABLE Patient (   -- checked
    PId NVARCHAR(255) PRIMARY KEY,
    Name NVARCHAR(255),
	PatientImg NVARCHAR(255),
    Phone NVARCHAR(255),
    Gender NVARCHAR(50),
    DOB DATE,
    FOREIGN KEY (PId) REFERENCES Account(Id) -- Using Id as foreign key
);

-- Specialty table
CREATE TABLE Specialty (   
    SpecialtyId NVARCHAR(255) PRIMARY KEY,
    SpecialtyName NVARCHAR(255),
    SpecialtyImg NVARCHAR(255),
	ShortDescription Nvarchar(255),
	LongDescription Nvarchar(1000)
);

-- Doctor table - Using Id from Account
CREATE TABLE Doctor (
    DId NVARCHAR(255) PRIMARY KEY,
	Name NVARCHAR(255),
    DoctorImg NVARCHAR(255),
	Position NVARCHAR(255),
    Phone NVARCHAR(255),
    Gender NVARCHAR(50),
    DOB DATE,
    Description NVARCHAR(MAX),
    Price FLOAT,
	SpecialtyId NVARCHAR(255),
    FOREIGN KEY (DId) REFERENCES Account(Id), -- Using Id as foreign key
	FOREIGN KEY (SpecialtyId) REFERENCES Specialty(SpecialtyId)
);

-- DetailDoctor table
CREATE TABLE DetailDoctor (
    DetailId NVARCHAR(255) PRIMARY KEY,
    DId NVARCHAR(255),
    Title NVARCHAR(255),
    Content NVARCHAR(MAX),
    FOREIGN KEY (DId) REFERENCES Doctor(DId)
);

-- Feedback table
CREATE TABLE Feedback (
    FeedbackId NVARCHAR(255) PRIMARY KEY,
    DId NVARCHAR(255),
    PId NVARCHAR(255),
    Name NVARCHAR(255),
    DateCmt DATETIME,
    Star INT,
    Description NVARCHAR(MAX),
    FOREIGN KEY (DId) REFERENCES Doctor(DId),
    FOREIGN KEY (PId) REFERENCES Patient(PId)
);

-- HealthRecord table
CREATE TABLE HealthRecord (
    RecordId NVARCHAR(255) PRIMARY KEY,
    PId NVARCHAR(255),
    DId NVARCHAR(255),
	OId NVARCHAR(255),
    Diagnosis NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    Note NVARCHAR(MAX),
    DateExam DATETIME,
    FOREIGN KEY (PId) REFERENCES Patient(PId),
    FOREIGN KEY (DId) REFERENCES Doctor(DId),
	FOREIGN KEY (OId) REFERENCES [Order](OId)
);

-- Schedule table
CREATE TABLE Schedule (
    ScheduleId NVARCHAR(255) PRIMARY KEY,
    DId NVARCHAR(255),
    DateWork DATE,
    TimeStart TIME,
	TimeEnd Time,
    Status NVARCHAR(255),
    FOREIGN KEY (DId) REFERENCES Doctor(DId)
);

-- Option table
CREATE TABLE [Option] (
    OptionId NVARCHAR(255) PRIMARY KEY,
    DId NVARCHAR(255),
    DateExam DATETIME,
    FOREIGN KEY (DId) REFERENCES Doctor(DId)
);

-- Order table
CREATE TABLE [Order] (
    OId NVARCHAR(255) PRIMARY KEY,
    PId NVARCHAR(255),
    OptionId NVARCHAR(255),
    Status NVARCHAR(255),
    DateOrder DATETIME,
    Symptom NVARCHAR(MAX),
    FOREIGN KEY (PId) REFERENCES Patient(PId),
    FOREIGN KEY (OptionId) REFERENCES [Option](OptionId),
);

-- Payment table (updated)
CREATE TABLE Payment (
    PayId NVARCHAR(255) PRIMARY KEY,    
    OId NVARCHAR(255),                  
    Method NVARCHAR(255),               
    PayImg NVARCHAR(255),              
    DatePay DATETIME,                 
    FOREIGN KEY (OId) REFERENCES [Order](OId)
);

