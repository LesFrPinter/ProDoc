USE [ProVisionDev]
GO

/****** Object:  Table [dbo].[CRC_CU10]    Script Date: 2/17/2023 9:53:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CRC_CU10](
	[CUST_NUMB] [nchar](5) NOT NULL,
	[CUST_NAME] [nchar](25) NOT NULL,
	[ADDRESS1] [nchar](25) NULL,
	[ADDRESS2] [nchar](25) NULL,
	[CITY] [nchar](15) NULL,
	[STATE] [nchar](2) NULL,
	[ZIP] [nchar](10) NULL,
	[ATTENTION] [nchar](25) NULL,
	[PHONE] [nchar](12) NULL
) ON [PRIMARY]
GO

