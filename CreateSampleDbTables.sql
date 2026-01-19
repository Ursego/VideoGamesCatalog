/*******************************************************************************************************
How to run this script in the Terminal (the Docker must be running):

cd "/Users/michael/Programming/Newton/video-game-catalog"
docker exec -i videogames-sql /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'MyStrong@Passw0rd!' -C -d GameCatalog -i /dev/stdin < "DB_STUFF.sql"
cd /Users/michael/Programming/Newton/video-game-catalog/server/VideoGameCatalog.Api
dotnet ef database update
*******************************************************************************************************/

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
SET NUMERIC_ROUNDABORT OFF;
GO

IF OBJECT_ID(N'dbo.Game', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Game;
END;
GO

IF OBJECT_ID(N'dbo.AgeRating', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.AgeRating;
END;
GO

IF OBJECT_ID(N'dbo.GameCategory', N'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.GameCategory;
END;
GO

CREATE TABLE dbo.GameCategory
(
    Id INT IDENTITY(1,1) NOT NULL,
    Description    NVARCHAR(200)      NOT NULL,

    CONSTRAINT PK_GameCategory PRIMARY KEY CLUSTERED (Id)
);
GO

CREATE TABLE dbo.AgeRating
(
    Id           INT IDENTITY(1,1) NOT NULL,
    Description  NVARCHAR(200)     NOT NULL,

    CONSTRAINT PK_AgeRating PRIMARY KEY CLUSTERED (Id)
);
GO

CREATE TABLE dbo.Game
(
    Id             INT IDENTITY(1,1) NOT NULL,
    Name           NVARCHAR(200)     NOT NULL,
    Description    NVARCHAR(MAX)     NULL,
    GameCategoryId INT               NOT NULL,
    AgeRatingId    INT               NOT NULL,
    ReleaseDate    DATE              NULL,
    CoverImageUrl  NVARCHAR(500)     NULL,
    IsActive       CHAR(1)           NOT NULL CONSTRAINT DF_Game_IsActive DEFAULT ('Y'),

    CONSTRAINT PK_Game PRIMARY KEY CLUSTERED (Id),

    CONSTRAINT FK_Game_GameCategory
        FOREIGN KEY (GameCategoryId)
        REFERENCES dbo.GameCategory (Id),

    CONSTRAINT FK_Game_AgeRating
        FOREIGN KEY (AgeRatingId)
        REFERENCES dbo.AgeRating (Id),

    CONSTRAINT CK_Game_IsActive
        CHECK (IsActive IN ('Y', 'N'))
);
GO

CREATE INDEX IX_Game_Name
ON dbo.Game (Name);
GO


INSERT INTO dbo.GameCategory (Description) VALUES
    (N'Action'),
    (N'Adventure'),
    (N'RPG'),
    (N'Shooter'),
    (N'Strategy'),
    (N'Simulation'),
    (N'Sports'),
    (N'Racing'),
    (N'Puzzle'),
    (N'Platformer'),
    (N'Fighting'),
    (N'Indie');

INSERT INTO dbo.AgeRating (Description) VALUES
    (N'RP - Rating Pending'),
    (N'E - Everyone'),
    (N'E10+ - Everyone 10+'),
    (N'T - Teen'),
    (N'M - Mature 17+'),
    (N'AO - Adults Only 18+');

INSERT INTO dbo.Game
(
    Name,
    Description,
    GameCategoryId,
    AgeRatingId,
    ReleaseDate,
    CoverImageUrl,
    IsActive
)
VALUES
(
    N'Harry Potter and HTTP endpoint',
    N'A wizard discovers REST, miscasts CORS, and turns the QA team into endpoints. Magical retries included.',
    5,
    1,
    '2024-06-01',
    N'https://example.com/covers/harry-potter-web-services.jpg',
    'Y'
),
(
    N'Find the last bug in your code',
    N'An endless puzzle where the final bug only appears in production on Friday at 4:59 PM. Good luck, hero!',
    9,
    2,
    '2023-11-15',
    N'https://example.com/covers/find-last-bug.jpg',
    'Y'
),
(
    N'Winnie the Pooh and the Bloody Piglet',
    N'Horrible events in the dark Hundred Acre Wood, where Piglet holds uncommitted transactions captive.',
    2,
    2,
    '2025-03-20',
    N'https://example.com/covers/pooh-bloody-piglet.jpg',
    'Y'
);
