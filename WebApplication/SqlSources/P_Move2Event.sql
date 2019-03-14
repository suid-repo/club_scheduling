CREATE PROCEDURE [dbo].[P_Move2Event]
	@eventId INT,
	@userList [UserListTableType] READONLY
AS
	INSERT INTO ApplicationUserEvents (ApplicationUser_Id, Event_Id)
	SELECT UL.Id, @eventId
	FROM @userList AS UL

	DELETE FROM	QueuedItems
	WHERE QueuedId = @eventId
	AND
	UserId IN (SELECT Id FROM @userList)
RETURN 0

