CREATE PROCEDURE [dbo].[P_CheckUsers2Add]
	@eventId int
AS
	/**
	 * THIS STORED PROCEDURE IS CALLED WHEN A COACH JOIN AN EVENT
	 * IT WILL RECALCULATE HOW MANY MAX PEOPLE ALLOWED FOR THE EVENT
	 * THEN MOVE PEOPLE
	 */
	 DECLARE @maxPeople INT = 0
	 DECLARE @numPeople INT = 0 --numPeople = people register in the event then people to add to the event
	 DECLARE @selectedFamily INT = NULL
	 DECLARE @selectedUser NVARCHAR(128) = NULL
	 DECLARE @people2Add UserListTableType
	 DECLARE @familyList FamilyQueuedListTableType

	-- Get the number of people register to this event
	SELECT @numPeople = COUNT(UserId)
	FROM MemberEvents
	WHERE EventId = @eventId

	-- Determine the number of maximum people for this event
	SELECT @maxPeople = COUNT(UserId)*8 - @numPeople
	FROM CoachEvents
	WHERE EventId = @eventId
	IF(@maxPeople > 0)
	BEGIN
		-- Select the list of family in this event
		-- We choose MAX function to penalize late members family (join the event seperate with other) 
		INSERT INTO @familyList
		SELECT ANU.Family_Id, NULL, MAX(ME.[Time]), COUNT(0)
		FROM MemberEvents AS ME
		INNER JOIN AspNetUsers AS ANU ON ANU.Id = ME.UserId
		WHERE EventId = @eventId
		AND ANU.Family_Id IS NOT NULL
		GROUP BY ANU.Family_Id
		HAVING COUNT(0) <= @maxPeople
	
		-- Select People W/OUT family
		INSERT INTO @familyList
		SELECT 0, ME.UserId, ME.[Time], 1
		FROM MemberEvents AS ME
		INNER JOIN AspNetUsers AS ANU ON ANU.Id = ME.UserId
		WHERE EventId = @eventId
		AND ANU.Family_Id IS NULL

		--SELECT OUR FIRST FAMILY
		SELECT TOP(1) @selectedFamily = FamilyId, @selectedUser = UserId
		FROM @familyList
		WHERE MembersCount <= @maxPeople
		ORDER BY [Time] ASC, MembersCount DESC


		--START WHILE --> LET'S DECREASE THAT QUEUED
		WHILE(@selectedFamily IS NOT NULL AND @maxPeople > 0)
		BEGIN
			IF(@selectedFamily = 0) -- People W/OUT Family is selected
				BEGIN
					INSERT INTO @people2Add VALUES (@selectedUser)
				
					SELECT @maxPeople -= 1 -- Decrease maxPeople
				
					DELETE FROM @familyList WHERE UserId = @selectedUser -- Clean
				END
			ELSE -- Family is selected
				BEGIN
					INSERT INTO @people2Add
					SELECT TOP(@maxPeople - @numPeople) UserId
					FROM MemberEvents AS ME
					INNER JOIN AspNetUsers AS ANU ON ANU.Id = ME.UserId
					WHERE EventId = @eventId
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
				ORDER BY [Time] ASC, MembersCount DESC
		END
	
		SELECT @numPeople = COUNT(0)
		FROM @people2Add

		IF (@numPeople > 0)
		BEGIN
			UPDATE MemberEvents SET isRegistered = 1
			WHERE UserId IN (SELECT UserId FROM @people2Add)
		END
	END
RETURN 0

