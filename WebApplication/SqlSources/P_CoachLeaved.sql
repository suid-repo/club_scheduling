CREATE PROCEDURE [dbo].[P_CoachLeaved]
	@eventId int
AS
	/**
	 * THIS STORED PROCEDURE IS CALLED WHEN A COACH LEAVE AN EVENT
	 * IT WILL RECALCULATE HOW MANY MAX PEOPLE ALLOWED FOR THE EVENT
	 * THEN MOVE POEPLE
	 */
	 DECLARE @maxPeople INT = 0
	 DECLARE @numPeople INT = 0 --numPeople = people register in the event then people to add to the event
	 DECLARE @selectedFamily INT = NULL
	 DECLARE @selectedUser NVARCHAR(128) = NULL
	 DECLARE @insertedTime BIGINT = 0
	 DECLARE @people2Kick UserListTableType
	 DECLARE @familyList FamilyQueuedListTableType

	-- Get the number of people register to this event
	SELECT @numPeople = COUNT(ApplicationUser_Id)
	FROM ApplicationUserEvents
	WHERE Event_Id = @eventId

	-- Determine the number of people to kick
	SELECT @maxPeople =  @numPeople - COUNT(UserId)*8
	FROM CoachEvents
	WHERE EventId = @eventId

	IF(@maxPeople) > 0
	BEGIN
		-- Select the list of family registered in this event
		-- We choose MAX function to penalize late members family (join the event seperate with other) 
		INSERT INTO @familyList
		SELECT ANU.Family_Id, NULL, 0, COUNT(0)
		FROM ApplicationUserEvents AS AUE
		INNER JOIN AspNetUsers AS ANU ON ANU.Id = AUE.ApplicationUser_Id
		WHERE Event_Id = @eventId
		AND ANU.Family_Id IS NOT NULL
		GROUP BY ANU.Family_Id
		HAVING COUNT(0) <= @maxPeople
	
		-- Select People W/OUT family
		INSERT INTO @familyList
		SELECT 0, AUE.ApplicationUser_Id, 0, 1
		FROM ApplicationUserEvents AS AUE
		INNER JOIN AspNetUsers AS ANU ON ANU.Id = AUE.ApplicationUser_Id
		WHERE Event_Id = @eventId
		AND ANU.Family_Id IS NULL

		--SELECT OUR FIRST FAMILY
		SELECT TOP(1) @selectedFamily = FamilyId, @selectedUser = UserId
		FROM @familyList
		WHERE MembersCount <= @maxPeople
		ORDER BY newid()


		--START WHILE --> LET'S DECREASE THAT QUEUED
		WHILE(@selectedFamily IS NOT NULL AND @maxPeople > 0)
		BEGIN
			IF(@selectedFamily = 0) -- People W/OUT Family is selected
				BEGIN
					INSERT INTO @people2Kick VALUES (@selectedUser)
				
					SELECT @maxPeople -= 1 -- Decrease maxPeople
				
					DELETE FROM @familyList WHERE UserId = @selectedUser -- Clean
				END
			ELSE -- Family is selected
				BEGIN
					INSERT INTO @people2Kick
					SELECT ApplicationUser_Id
					FROM ApplicationUserEvents AS AUE
					INNER JOIN AspNetUsers AS ANU ON ANU.Id = AUE.ApplicationUser_Id
					WHERE Event_Id = @eventId
					AND ANU.Family_Id = @selectedFamily

					SELECT @maxPeople -= MembersCount FROM @familyList WHERE FamilyId = @selectedFamily -- Decrease maxPeople
				
					DELETE FROM @familyList WHERE FamilyId = @selectedFamily -- Clean
				END

				--Clean Variables
				SELECT @selectedFamily = NULL, @selectedUser = NULL

				--Select again a new item
				SELECT TOP(1) @selectedFamily = FamilyId, @selectedUser = UserId
				FROM @familyList
				WHERE MembersCount <= @maxPeople
				ORDER BY newid()
		END
	
		SELECT @numPeople = COUNT(0)
		FROM @people2Kick

		IF (@numPeople > 0)
		BEGIN
			EXEC P_Move2Queued 
			@eventId = @eventId,
			@insertedTime = @insertedTime,
			@userList = @people2kick
		END
	END
RETURN 0

