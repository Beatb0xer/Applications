CREATE DATABASE EventApplicationsDB;
GO

USE EventApplicationsDB;
GO

CREATE TABLE Events (
    EventID INT PRIMARY KEY IDENTITY(1,1),
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATETIME NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE ApplicationStatuses (
    StatusID INT PRIMARY KEY IDENTITY(1,1),
    StatusName NVARCHAR(50) NOT NULL
);

CREATE TABLE Applications (
    ApplicationID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    ContactInfo NVARCHAR(100) NOT NULL,
    EventID INT NOT NULL,
    StatusID INT NOT NULL,
    ApplicationDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (EventID) REFERENCES Events(EventID),
    FOREIGN KEY (StatusID) REFERENCES ApplicationStatuses(StatusID)
);

INSERT INTO ApplicationStatuses (StatusName) VALUES 
('Новая'), ('Подтверждена'), ('Отклонена'), ('Отменена');

INSERT INTO Events (EventName, EventDate, Location, Description) VALUES
('Конференция разработчиков', '2025-05-15', 'Москва', 'Ежегодная конференция для IT-специалистов');
