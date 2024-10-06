-- Create a new database
CREATE DATABASE BookingCare;
GO

USE BookingCare;
GO

-- Account table
CREATE TABLE Account (
    Id NVARCHAR(255) PRIMARY KEY,
    Email NVARCHAR(255),
    Password NVARCHAR(255),
    Role INT,
    Status BIT
);

-- Patient table - Using Id from Account
CREATE TABLE Patient (
    PId NVARCHAR(255) PRIMARY KEY,
    Name NVARCHAR(255),
    Phone NVARCHAR(255),
    Gender NVARCHAR(50),
    DOB DATE,
    FOREIGN KEY (PId) REFERENCES Account(Id) -- Using Id as foreign key
);

-- Doctor table - Using Id from Account
CREATE TABLE Doctor (
    DId NVARCHAR(255) PRIMARY KEY,
	Name NVARCHAR(255),
    DoctorImg NVARCHAR(255),
    Phone NVARCHAR(255),
    Gender NVARCHAR(50),
    DOB DATE,
    Description NVARCHAR(MAX),
    Price FLOAT,
    FOREIGN KEY (DId) REFERENCES Account(Id) -- Using Id as foreign key
);



-- Specialty table
CREATE TABLE Specialty (
    SpecialtyId NVARCHAR(255) PRIMARY KEY,
    SpecialtyName NVARCHAR(255),
    SpecialtyImg NVARCHAR(255),
    ServiceId NVARCHAR(255)
);

-- DoctorSpecialty table (relationship between Doctor and Specialty)
CREATE TABLE DoctorSpecialty (
    DId NVARCHAR(255),
    SpecialtyId NVARCHAR(255),
    PRIMARY KEY (DId, SpecialtyId),
    FOREIGN KEY (DId) REFERENCES Doctor(DId),
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
    Diagnosis NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    Note NVARCHAR(MAX),
    DateExam DATETIME,
    FOREIGN KEY (PId) REFERENCES Patient(PId),
    FOREIGN KEY (DId) REFERENCES Doctor(DId)
);

-- History table
CREATE TABLE History (
    HistoryId NVARCHAR(255) PRIMARY KEY,
    PId NVARCHAR(255),
    DId NVARCHAR(255),
    RecordId NVARCHAR(255),
    DateExam DATETIME,
    FOREIGN KEY (PId) REFERENCES Patient(PId),
    FOREIGN KEY (DId) REFERENCES Doctor(DId),
    FOREIGN KEY (RecordId) REFERENCES HealthRecord(RecordId)
);

-- Schedule table
CREATE TABLE Schedule (
    ScheduleId NVARCHAR(255) PRIMARY KEY,
    DId NVARCHAR(255),
    DateWork DATE,
    TimeWork TIME,
    Status NVARCHAR(255),
    FOREIGN KEY (DId) REFERENCES Doctor(DId)
);

-- Service table
CREATE TABLE Service (
    ServiceId NVARCHAR(255) PRIMARY KEY,
    ServiceName NVARCHAR(255),
    ServiceImg NVARCHAR(255)
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
    ScheduleId NVARCHAR(255),
    Status NVARCHAR(255),
    DateOrder DATETIME,
    Symptom NVARCHAR(MAX),
    FOREIGN KEY (PId) REFERENCES Patient(PId),
    FOREIGN KEY (OptionId) REFERENCES [Option](OptionId),
    FOREIGN KEY (ScheduleId) REFERENCES Schedule(ScheduleId)
);

-- Payment table
CREATE TABLE Payment (
    PayId NVARCHAR(255) PRIMARY KEY,
    OId NVARCHAR(255),
    Method NVARCHAR(255),
    DatePay DATETIME,
    FOREIGN KEY (OId) REFERENCES [Order](OId)
);
