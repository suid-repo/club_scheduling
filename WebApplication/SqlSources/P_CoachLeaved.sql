CREATE PROCEDURE [dbo].[P_CoachLeaved]
	@eventId int
AS
	/**
	 * THIS STORED PROCEDURE IS CALLED WHEN A COACH LEAVE AN EVENT
	 * IT WILL RECALCULATE HOW MANY MAX PEOPLE ALLOWED FOR THE EVENT
	 * THEN MOVE POEPLE
	 */
	 DECLARE @maxPeople INT = 0;
	 DECLARE @numPeople INT = 0;
	 DECLARE @people2Kick UserListTableType;
	 
				-- Determine the number of maximum people for this event
				SELECT @maxPeople = COUNT(UserId)*8
				FROM CoachEvents
				WHERE EventId = @eventId

				SELECT @numPeople = COUNT(ApplicationUser_Id)
				FROM ApplicationUserEvents
				WHERE Event_Id = @eventId

				-- SELECT THE NUMBER OF PEOPLE TO KICK
				IF ((@numPeople - @maxPeople) > 0)
				BEGIN
					INSERT INTO @people2Kick
					SELECT TOP(@numPeople - @maxPeople) ApplicationUser_Id
					FROM ApplicationUserEvents
					WHERE Event_Id = @eventId
					ORDER BY newid()
			
					EXEC P_Move2Queued 
					@eventId = @eventId,
					@userList = @people2kick
				END
RETURN 0
GO