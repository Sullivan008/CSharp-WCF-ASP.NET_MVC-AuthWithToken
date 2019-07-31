CREATE TABLE [dbo].[User] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (250) NOT NULL,
    [Salt]     NVARCHAR (250) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Unique_User] UNIQUE NONCLUSTERED ([Name] ASC)
);
