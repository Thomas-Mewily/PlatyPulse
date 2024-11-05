﻿CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(256) NOT NULL,
    Email NVARCHAR(100),
    XP INT DEFAULT 0,
    BirthDay DATETIME DEFAULT GETDATE(),
    DateJoined DATETIME DEFAULT GETDATE()
);
