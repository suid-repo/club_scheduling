CREATE PROCEDURE [dbo].[P_Move2Queued]
	@eventId INT,
	@userList [UserListTableType] READONLY
AS
	INSERT INTO QueuedItems (UserId, QueuedId)
	SELECT UL.Id, @eventId
	FROM @userList AS UL

	DELETE FROM	ApplicationUserEvents
	WHERE Event_Id = @eventId
	AND
	ApplicationUser_Id IN (SELECT Id FROM @userList)
RETURN 0

