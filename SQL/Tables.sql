
CREATE TABLE [dbo].[categories](
	[id] [bigint] NOT NULL,
	[name] [varchar](50) NOT NULL,
	[oid] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[images](
	[id] [bigint] NOT NULL,
	[name] [varchar](100) NOT NULL,
	[type] [tinyint] NOT NULL,
	[description] [varchar](200) NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[items](
	[id] [bigint] NOT NULL,
	[cat_id] [bigint] NOT NULL,
	[img_id] [varchar](500) NOT NULL,
	[price] [numeric](9, 2) NOT NULL,
	[description] [varchar](500) NOT NULL,
	[offer_percent] [numeric](4, 2) NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[user](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[user_name] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[role] [varchar](50) NOT NULL
) ON [PRIMARY]

