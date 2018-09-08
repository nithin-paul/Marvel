USE [Marvel]
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 9/8/2018 3:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[DeleteCategory]
	-- Add the parameters for the stored procedure here
	@Id INT NULL
AS
BEGIN
DELETE FROM images where item_id IN (Select id from items where cat_id = @id);
DELETE FROM items where cat_id = @id;
DELETE from categories where id =@id;
END
