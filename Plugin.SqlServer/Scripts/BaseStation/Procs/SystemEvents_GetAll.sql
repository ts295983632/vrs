IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA = 'BaseStation' AND ROUTINE_NAME = 'SystemEvents_GetAll')
BEGIN
    EXECUTE sys.sp_executesql N'CREATE PROCEDURE [BaseStation].[SystemEvents_GetAll] AS BEGIN SET NOCOUNT ON; END;';
END;
GO

ALTER PROCEDURE [BaseStation].[SystemEvents_GetAll]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM   [BaseStation].[SystemEvents];
END;
GO

