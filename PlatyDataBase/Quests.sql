CREATE TABLE Quests (
    QuestID INT PRIMARY KEY IDENTITY,
    QuestName NVARCHAR(64) NOT NULL,
    QuestDescription NVARCHAR(4096),
    Category NVARCHAR(64),
    Difficulty INT,
    XP INT DEFAULT 0
);