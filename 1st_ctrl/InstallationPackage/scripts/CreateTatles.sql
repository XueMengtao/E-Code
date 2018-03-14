CREATE database ParallelTask

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[Antenna]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Antenna](
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[MaxGain] [float] NOT NULL,
	[Polarization] [nvarchar](50) NOT NULL,
	[ReceiverThreshold] [float] NOT NULL,
	[TransmissionLoss] [float] NOT NULL,
	[VSWR] [float] NOT NULL,
	[Temperature] [float] NOT NULL,
	[Radius] [float] NULL,
	[BlockageRadius] [float] NULL,
	[ApertureDistribution] [varchar](50) NULL,
	[EdgeTeper] [float] NULL,
	[Length] [float] NULL,
	[Pitch] [float] NULL,
 CONSTRAINT [PK_AntennaTemplate_1] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[FileInfo]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FileInfo](
	[FileInfoID] [int] NOT NULL,
	[FileName] [varchar](50) NOT NULL,
	[FilePath] [varchar](50) NOT NULL,
 CONSTRAINT [PK_FileInfo] PRIMARY KEY CLUSTERED 
(
	[FileInfoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[LZ_Material]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LZ_Material](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterialName] [nvarchar](50) NOT NULL,
	[LayerNum] [int] NOT NULL,
	[Conductivity] [float] NULL,
	[Permittivity] [float] NULL,
	[Roughness] [float] NULL,
	[Thickness] [float] NULL,
	[Red] [int] NULL,
	[Green] [int] NULL,
	[Blue] [int] NOT NULL,
 CONSTRAINT [PK_LZ_Material] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[Material]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Material](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterialSort] [nvarchar](50) NOT NULL,
	[MaterialName] [nvarchar](50) NOT NULL,
	[Red] [int] NULL,
	[Green] [int] NULL,
	[Blue] [int] NULL,
	[Conductivity] [float] NULL,
	[Permittivity] [float] NULL,
	[Roughness] [float] NULL,
	[Thickness] [float] NULL,
	[LayerOrder] [int] NULL,
	[LayerName] [nvarchar](50) NULL,
	[UpdateMaterial] [int] NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[Project]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Project](
	[ProjectID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ProjectState] [smallint] NOT NULL,
	[Directory] [varchar](50) NOT NULL,
	[ResultDirectory] [varchar](50) NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Percent] [nvarchar](50) NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[T_Material]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[T_Material](
	[MaterialName] [nchar](50) NOT NULL,
	[Conductivity] [float] NULL,
	[Permittivity] [float] NULL,
 CONSTRAINT [PK_T_Material] PRIMARY KEY CLUSTERED 
(
	[MaterialName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[TaskFileRelation]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TaskFileRelation](
	[TaskInfoID] [int] NOT NULL,
	[FileInfoID] [int] NOT NULL,
 CONSTRAINT [PK_TaskInfo] PRIMARY KEY CLUSTERED 
(
	[TaskInfoID] ASC,
	[FileInfoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[TaskInfo]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TaskInfo](
	[TaskInfoID] [int] NOT NULL,
	[TaskState] [smallint] NOT NULL,
	[ProjectID] [int] NOT NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[NodeIP] [char](15) NULL,
 CONSTRAINT [PK_TaskInfo_1] PRIMARY KEY CLUSTERED 
(
	[TaskInfoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[TerInfo]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TerInfo](
	[name] [nvarchar](50) NOT NULL,
	[originX] [float] NOT NULL,
	[originY] [float] NOT NULL,
	[path] [nvarchar](50) NOT NULL,
	[Zmin] [float] NOT NULL,
	[Zmax] [float] NOT NULL,
	[Vertex1X] [float] NOT NULL,
	[Vertex1Y] [float] NOT NULL,
	[Vertex1Z] [float] NOT NULL,
	[Vertex2X] [float] NOT NULL,
	[Vertex2Y] [float] NOT NULL,
	[Vertex2Z] [float] NOT NULL,
	[Vertex3X] [float] NOT NULL,
	[Vertex3Y] [float] NOT NULL,
	[Vertex3Z] [float] NOT NULL,
	[Vertex4X] [float] NOT NULL,
	[Vertex4Y] [float] NOT NULL,
	[Vertex4Z] [float] NOT NULL,
 CONSTRAINT [PK_TerTemplate] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[Transmitter]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Transmitter](
	[Name] [nvarchar](50) NOT NULL,
	[RotateX] [float] NOT NULL,
	[RotateY] [float] NOT NULL,
	[RotateZ] [float] NOT NULL,
	[Power] [float] NOT NULL,
	[AntennaName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Transmiter] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[WaveForm]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WaveForm](
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Frequency] [float] NOT NULL,
	[BandWidth] [float] NOT NULL,
	[Phase] [float] NULL,
	[StartFrequency] [float] NULL,
	[EndFrequency] [float] NULL,
	[RollOffFactor] [float] NULL,
	[FreChangeRate] [nvarchar](50) NULL,
 CONSTRAINT [PK_WaveForm] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[Z_Material]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Z_Material](
	[MaterialName] [nchar](50) NOT NULL,
	[Conductivity] [float] NULL,
	[Permittivity] [float] NULL,
	[Roughness] [float] NULL,
	[Thickness] [float] NULL,
	[Red] [int] NULL,
	[Green] [int] NULL,
	[Blue] [int] NULL,
 CONSTRAINT [PK_Z_Material] PRIMARY KEY CLUSTERED 
(
	[MaterialName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

USE [ParallelTask]
GO

/****** Object:  Table [dbo].[ZDYMaterial]    Script Date: 05/27/2014 12:44:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ZDYMaterial](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaterialName] [nvarchar](50) NOT NULL,
	[LayerNum] [int] NOT NULL,
	[TMaterialName] [nvarchar](50) NOT NULL,
	[Conductivity] [float] NOT NULL,
	[Permittivity] [float] NOT NULL,
	[Roughness] [float] NOT NULL,
	[Thickness] [float] NOT NULL,
	[Red] [int] NULL,
	[Green] [int] NULL,
	[Blue] [int] NULL,
 CONSTRAINT [PK_ZDYMaterial] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TaskFileRelation]  WITH CHECK ADD  CONSTRAINT [FK_TaskFileRelation_TaskInfo] FOREIGN KEY([TaskInfoID])
REFERENCES [dbo].[TaskInfo] ([TaskInfoID])
GO

ALTER TABLE [dbo].[TaskFileRelation] CHECK CONSTRAINT [FK_TaskFileRelation_TaskInfo]
GO

ALTER TABLE [dbo].[TaskFileRelation]  WITH CHECK ADD  CONSTRAINT [FK_TaskInfo_FileInfo] FOREIGN KEY([FileInfoID])
REFERENCES [dbo].[FileInfo] ([FileInfoID])
GO

ALTER TABLE [dbo].[TaskFileRelation] CHECK CONSTRAINT [FK_TaskInfo_FileInfo]
GO

ALTER TABLE [dbo].[TaskInfo]  WITH CHECK ADD  CONSTRAINT [FK_TaskInfo_Project] FOREIGN KEY([ProjectID])
REFERENCES [dbo].[Project] ([ProjectID])
GO

ALTER TABLE [dbo].[TaskInfo] CHECK CONSTRAINT [FK_TaskInfo_Project]
GO

ALTER TABLE [dbo].[Transmitter]  WITH CHECK ADD  CONSTRAINT [FK_Transmitter_Antenna] FOREIGN KEY([AntennaName])
REFERENCES [dbo].[Antenna] ([Name])
GO

ALTER TABLE [dbo].[Transmitter] CHECK CONSTRAINT [FK_Transmitter_Antenna]
GO

