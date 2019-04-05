CREATE TRIGGER [T_CoachLeaved]
ON dbo.CoachEvents
AFTER DELETE
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @eventIdInserted INT = 0

    SELECT TOP(1) @eventIdInserted = EventId FROM deleted;
                
    EXEC P_CheckQueued
    @eventId = @eventIdInserted
END

