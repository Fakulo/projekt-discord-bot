CREATE TABLE [dbo].[Table]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(100) NOT NULL, 
    [Type] NCHAR(20) NULL, 
    [IsEx] TINYINT NOT NULL DEFAULT 0, 
    [Latitude] FLOAT NOT NULL, 
    [Longitude] FLOAT NOT NULL, 
    [IdCell17] INT NULL, 
	[NeedCheck] TINYINT NULL DEFAULT 0,
    [LastUpdate] DATETIME NULL 
    
)
