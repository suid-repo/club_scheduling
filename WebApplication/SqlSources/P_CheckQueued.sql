﻿CREATE PROCEDURE [dbo].[P_CheckQueued]
	@queuedId INT = 0
AS
	DECLARE @queuedList QueuedListTableType
	DECLARE @cursor INT = 0

	SET TRANSACTION ISOLATION LEVEL REPEATABLE READ

	WHILE (@cursor < 10)
	BEGIN
		SET @cursor = @cursor + 1
		BEGIN TRY
			BEGIN TRANSACTION
			-- CHOOSE THE EVENT(S) TO CHECK
			IF @queuedId > 0
				INSERT INTO @queuedList VALUES (@QueuedId)
			ELSE
				INSERT INTO @queuedList
				SELECT Q.EventId
				FROM Queueds AS Q
				INNER JOIN [Events] AS E ON ( Q.EventId = E.Id )
				WHERE E.StartDate >= GETDATE()

			-- DO SOME LOGIC NOW
			DECLARE MY_CURSOR CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
			FOR
			SELECT QueuedId
			FROM @queuedList

			OPEN MY_CURSOR
			FETCH NEXT FROM MY_CURSOR INTO @queuedId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				EXEC P_CoachJoined @queuedId
				FETCH NEXT FROM MY_CURSOR INTO @queuedId
			END 
			CLOSE MY_CURSOR
			DEALLOCATE MY_CURSOR
			COMMIT
			BREAK
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			IF(ERROR_NUMBER() = 1205)
				CONTINUE
			ELSE
				THROW
		END CATCH
	END

RETURN 0