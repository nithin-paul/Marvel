USE [Marvel]
GO
/****** Object:  StoredProcedure [dbo].[SetImageAsPrimary]    Script Date: 9/8/2018 3:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SetImageAsPrimary]
	-- Add the parameters for the stored procedure here
	@Id INT NULL,
	@ItemId INT 
AS
BEGIN
UPDATE images SET is_primary = 1 where id=@Id;
Update images SET is_primary = 0 where item_id = @ItemId and id <> @id;
END
