CREATE PROCEDURE [dbo].[sp_TimKhoaNgoai] (@a INT, @b INT)
AS 
BEGIN
	DECLARE @fkeyid INT;
	DECLARE @rkeyid INT;
    DECLARE @fkey INT;
	DECLARE @rkey INT;

	select @fkeyid = fkeyid, @rkeyid = rkeyid, @fkey = fkey, @rkey = rkey from sys.sysforeignkeys
	where (fkeyid = @a and rkeyid = @b) or (rkeyid = @a and fkeyid = @b)

	(select t.name as table_name, c.name as column_name
	from sys.columns c, sys.tables t
	where c.object_id = t.object_id
	and c.object_id = @fkeyid and c.column_id = @fkey)
	union
	(select t.name as table_name, c.name as column_name
	from sys.columns c, sys.tables t
	where c.object_id = t.object_id
	and c.object_id = @rkeyid and c.column_id = @rkey)
END;
