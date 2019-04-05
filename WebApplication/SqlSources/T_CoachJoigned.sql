CREATE TRIGGER [T_CoachJoigned]
ON dbo.CoachEvents
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @eventIdInserted INT = 0;

    SELECT TOP(1) @eventIdInserted = EventId FROM Inserted
            
    EXEC P_CheckQueued 
    @eventId = @eventIdInserted
END

