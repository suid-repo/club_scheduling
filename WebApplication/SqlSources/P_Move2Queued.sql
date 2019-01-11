CREATE PROCEDURE [dbo].[P_Move2Queued]
	@eventId INT,
	@insertedTime BIGINT,
	@userList [UserListTableType] READONLY
AS
	INSERT INTO QueuedItems (UserId, QueuedId, [Time])
	SELECT UL.Id, @eventId, @insertedTime
	FROM @userList AS UL

	DELETE FROM	ApplicationUserEvents
	WHERE Event_Id = @eventId
	AND
	ApplicationUser_Id IN (SELECT Id FROM @userList)
RETURN 0

