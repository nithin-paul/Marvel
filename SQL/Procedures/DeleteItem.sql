USE [Marvel]
GO
/****** Object:  StoredProcedure [dbo].[DeleteItem]    Script Date: 9/8/2018 3:18:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[DeleteItem]
	-- Add the parameters for the stored procedure here
	@Id INT NULL
AS
BEGIN
DELETE FROM images where item_id IN (Select id from items where id = @id);
DELETE FROM items where id = @id;
END
