CREATE TRIGGER [T_CoachJoigned]
ON dbo.CoachEvents
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @eventIdInserted INT = 0;
    DECLARE @i INT = 0;
    SET TRANSACTION ISOLATION LEVEL REPEATABLE READ

	 WHILE (@i < 5)
	 BEGIN
		BEGIN TRY
			SET @i = @i+1;
			BEGIN TRANSACTION
            SELECT TOP(1) @eventIdInserted = EventId FROM Inserted
            
            EXEC P_CheckQueued 
            @eventId = @eventIdInserted
            COMMIT TRANSACTION
			BREAK		
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			IF(ERROR_NUMBER() = 1205)
				CONTINUE;
			ELSE
				BREAK;
				THROW
		END CATCH
	END
END

