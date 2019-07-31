CREATE TABLE [dbo].[Token] (
    [Id]         	INT            IDENTITY (1, 1) NOT NULL,
    [SecureToken]	NVARCHAR (250) NOT NULL,
    [UserId]     	INT            NOT NULL,
    [CreateDate] 	DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Unique_Token] UNIQUE NONCLUSTERED ([SecureToken] ASC),
    CONSTRAINT [FK_Token_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);