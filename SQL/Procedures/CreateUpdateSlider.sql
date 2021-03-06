USE [Marvel]
GO
/****** Object:  StoredProcedure [dbo].[CreateUpdateSlider]    Script Date: 9/8/2018 3:18:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[CreateUpdateSlider]
	-- Add the parameters for the stored procedure here
	@Id INT NULL,
    @Name varchar(100),
	@Title varchar(200),
    @Description nvarchar(MAX)
AS
BEGIN
Declare @idValue int
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if @Id IS NULL
		BEGIN
			INSERT INTO dbo.slider_images (
								image_name,
								title,
								description) 
							VALUES (
								@Name,
								@Title,
								@Description
							);
	SET @idValue = SCOPE_IDENTITY();
		END
	ELSE 
		BEGIN
			UPDATE dbo.slider_images SET 
								image_name = IsNull(@Name, image_name),
								title = @Title,
								description = @Description
								where id = @Id;
								SET @idValue = @Id;
			
		END
		Select id, image_name,title, description from dbo.slider_images where id = @idValue;
END