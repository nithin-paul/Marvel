USE [Marvel]
GO

/****** Object:  Table [dbo].[categories]    Script Date: 9/8/2018 3:14:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[categories](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[oid] [int] NOT NULL,
	[description] [nvarchar](max) NULL,
	[image_url] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


USE [Marvel]
GO

/****** Object:  Table [dbo].[images]    Script Date: 9/8/2018 3:16:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[images](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[item_id] [bigint] NOT NULL,
	[name] [varchar](100) NOT NULL,
	[is_primary] [tinyint] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[images] ADD  DEFAULT ((0)) FOR [is_primary]
GO


USE [Marvel]
GO

/****** Object:  Table [dbo].[items]    Script Date: 9/8/2018 3:16:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[items](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[cat_id] [bigint] NOT NULL,
	[name] [varchar](500) NOT NULL,
	[price] [numeric](9, 2) NOT NULL,
	[description] [varchar](500) NOT NULL,
	[offer_percent] [numeric](4, 2) NULL,
	[detail_description] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



USE [Marvel]
GO

/****** Object:  Table [dbo].[slider_images]    Script Date: 9/8/2018 3:16:31 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[slider_images](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[image_name] [varchar](100) NOT NULL,
	[title] [varchar](200) NOT NULL,
	[description] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


USE [Marvel]
GO

/****** Object:  Table [dbo].[user]    Script Date: 9/8/2018 3:16:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[user](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[role] [varchar](50) NOT NULL
) ON [PRIMARY]

GO








