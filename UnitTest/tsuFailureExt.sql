USE [IFRS]
GO
/****** Object:  Table [dbo].[tsuFailuresExt]    Script Date: 12/05/2008 12:25:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tsuFailuresExt](
	[testResultID] [int] NOT NULL,
	[test] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL,
	[message] [nvarchar](255) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]
