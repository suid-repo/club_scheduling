CREATE TYPE [dbo].[FamilyQueuedListTableType] AS TABLE (
    [FamilyId] INT NULL,
	[UserId] NVARCHAR(128) NULL,
	[Time] BIGINT NULL,
	[MembersCount] TINYINT NULL);

