USE [Marvel]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.[CreateUpdateCategory]
	-- Add the parameters for the stored procedure here
	@Id INT NULL,
    @Oid INT NULL,
    @Name varchar(50),
    @Description nvarchar(MAX),
    @ImageUrl nvarchar(MAX) NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if @Id IS NULL
		BEGIN
			INSERT INTO dbo.categories (
								oid,
								name,
								description,
								image_url) 
							VALUES (
								@oid,
								@Name,
								@Description,
								@ImageUrl
							)

		END
	ELSE 
		BEGIN
			UPDATE dbo.categories SET 
								oid = @oid,
								name = @Name,
								description = @Description,
								image_url = @ImageUrl where id = @Id
			
		END
		Select id, oid, name, description, image_url from dbo.categories where id =SCOPE_IDENTITY();
END
GO
