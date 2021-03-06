USE [Marvel]
GO
/****** Object:  StoredProcedure [dbo].[CreateUpdateItem]    Script Date: 9/8/2018 3:17:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[CreateUpdateItem]
	-- Add the parameters for the stored procedure here
	@Id INT NULL,
	@catId INT ,
    @Name varchar(50),
    @Description nvarchar(MAX),
	@DetailDescription nvarchar(MAX),
	@Price Numeric(8,2),
    @OfferPercent INT NULL,
	@ImageName varchar(50) NULL
AS
BEGIN
Declare @idValue int
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if @Id IS NULL
		BEGIN
			INSERT INTO dbo.items(
			                    cat_id,
								name,
								price,
								description,
								offer_percent,
								detail_description) 
							VALUES (
								@catId,
								@Name,
								@Price,
								@Description,
								@OfferPercent,
								@DetailDescription
							);

		SET @idValue = SCOPE_IDENTITY();
		END
	ELSE 
		BEGIN
			UPDATE dbo.items SET 
								name = @Name,
								price = @Price,
								description = @Description,
								offer_percent = @OfferPercent,
								detail_description = @DetailDescription where id = @Id;

								SET @idValue = @Id;
			
		END

		if (@ImageName IS NOT NULL)
			BEGIN
				Declare @IsPrimary int
				IF EXISTS(SELECT id
							from images where item_id = @idValue AND is_primary =1)
					BEGIN
						SET @IsPrimary = 0
					END
				Else 
					BEGIN
						SET @IsPrimary = 1
					END
				INSERT INTO images (
									item_id,
									name,
									is_primary
			                   )
							   values
							   (
								   @idValue,
								   @ImageName,
								   @IsPrimary
							   )
			END
		Select i.id As Id, 
			   i.cat_id As CategoryId,
			   i.name as ItemName,
		       i.offer_percent as OfferPercent,
			   i.description as ItemDescription,
			   i.detail_description as DetailDescription,
			   i.price as ItemPrice,
			   img.id as ImageId,
			   img.item_id as ImageItemId,
			   img.name as ImageName,
			   img.is_primary as IsImagePrimary 
			   from items i left join images img on i.id = img.item_id where i.id = @idValue order by i.id ASC;
END
