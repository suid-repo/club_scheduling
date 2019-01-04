CREATE PROCEDURE [dbo].[P_CoachJoined]
	@eventId int
AS
	/**
	 * THIS STORED PROCEDURE IS CALLED WHEN A COACH LEAVE AN EVENT
	 * IT WILL RECALCULATE HOW MANY MAX PEOPLE ALLOWED FOR THE EVENT
	 * THEN MOVE POEPLE
	 */
	 DECLARE @maxPeople INT = 0;
	 DECLARE @numPeople INT = 0;
	 DECLARE @people2Add UserListTableType;
	 
	-- Determine the number of maximum people for this event
	SELECT @maxPeople = COUNT(UserId)*8
	FROM CoachEvents
	WHERE EventId = @eventId

	SELECT @numPeople = COUNT(ApplicationUser_Id)
	FROM ApplicationUserEvents
	WHERE Event_Id = @eventId

	-- SELECT THE NUMBER OF PEOPLE TO ADD
	IF ((@maxPeople - @numPeople) > 0)
	BEGIN
		INSERT INTO @people2Add
		SELECT TOP(@maxPeople - @numPeople) UserId
		FROM QueuedItems
		WHERE QueuedId = @eventId
		ORDER BY [Time] ASC

		EXEC P_Move2Event
		@eventId = @eventId,
		@userList = @people2Add
	END
RETURN 0

