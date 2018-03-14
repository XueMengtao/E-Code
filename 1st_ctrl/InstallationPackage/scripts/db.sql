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

USE [ParallelTask]
GO

/****** Object:  StoredProcedure [dbo].[addAntenna]    Script Date: 05/27/2014 12:46:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addAntenna]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50),
	@Type nvarchar(50),
	@MaxGain float,
	@Polarization nvarchar(10),
	@ReceiverThreshold float,
	@TransmisionLoss float,
	@VSWR float,
	@Temperature float,
	@Radius float=null,
	@BlockageRadius float=null,
	@ApetureDistribution nvarchar(50)=null,
	@EdgeTeper float=null,
	@Length float=null,
	@Pitch float=null
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Antenna
	VALUES(@Name,@Type,@MaxGain,@Polarization,@ReceiverThreshold,@TransmisionLoss,@VSWR,@Temperature,@Radius,@BlockageRadius,@ApetureDistribution,@EdgeTeper,@Length,@Pitch)
END

GO

/****** Object:  StoredProcedure [dbo].[addLZMaterial]    Script Date: 05/27/2014 12:46:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addLZMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50),
	@LayerNum int,
	@Conductivity float,
	@Permittivity float,
	@Roughness float,
	@Thickness float,
	@Red int,
	@Green int,
	@Blue int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    insert into LZ_Material(MaterialName,LayerNum,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue) 
	values(@MaterialName,@LayerNum,@Conductivity,@Permittivity,@Roughness,@Thickness,@Red,@Green,@Blue);
END

GO

/****** Object:  StoredProcedure [dbo].[addMaterial]    Script Date: 05/27/2014 12:46:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialSort nvarchar(50),
	@MaterialName nvarchar(50),
	@Red int,
	@Green int,
	@Blue int,
	@Conductivity float,
	@Permittivity float,
	@Roughness float,
	@Thickness float,
	@LayerOrder int=null,
	@LayerName nvarchar(50)=null,
	@UpdateMaterial int=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into Material(MaterialSort,MaterialName,Red,Green,Blue,Conductivity,Permittivity,Roughness,Thickness,LayerOrder,LayerName,UpdateMaterial) 
	values(@MaterialSort,@MaterialName,@Red,@Green,@Blue,@Conductivity,@Permittivity,@Roughness,@Thickness,@LayerOrder,@LayerName,@UpdateMaterial);
	
END

GO

/****** Object:  StoredProcedure [dbo].[addTerInfo]    Script Date: 05/27/2014 12:46:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[addTerInfo]
	@Name nvarchar(50),
	@OriginX float,
	@OriginY float,
	@Path nvarchar(50),
	@Zmin float,
	@Zmax float,
	@Vertex1X float,
	@Vertex1Y float,
	@Vertex1Z float,
	@Vertex2X float,
	@Vertex2Y float,
	@Vertex2Z float,
	@Vertex3X float,
	@Vertex3Y float,
	@Vertex3Z float,
	@Vertex4X float,
	@Vertex4Y float,
	@Vertex4Z float
	
AS
	/* SET NOCOUNT ON */
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO TerInfo
	VALUES(@name,@originX,@originY,@path,@Zmin,@Zmax,@Vertex1X,@Vertex1Y,@Vertex1Z,@Vertex2X,@Vertex2Y,@Vertex2Z,@Vertex3X,@Vertex3Y,@Vertex3Z,@Vertex4X,@Vertex4Y,@Vertex4Z)
END
GO

/****** Object:  StoredProcedure [dbo].[addTMaterial]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addTMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50),
	@Conductivity float,
	@Permittivity float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into T_Material(MaterialName,Conductivity,Permittivity) 
	values(@MaterialName,@Conductivity,@Permittivity);
END

GO

/****** Object:  StoredProcedure [dbo].[addTransmitter]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addTransmitter] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50),
	@RotateX float,
	@RotateY float,
	@RotateZ float,
	@Power float,
	@AntennaName nvarchar(50)
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Transmitter
	VALUES(@Name,@RotateX,@RotateY,@RotateZ,@Power,@AntennaName)
END

GO

/****** Object:  StoredProcedure [dbo].[addWaveForm]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addWaveForm] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50) ,
	@Type varchar(50),
	@Frequency float,
	@BandWidth float,
	@Phase float=null,
	@StartFrequency float=null,
	@EndFrequency float=null,
	@RollOffFactor float=null,
	@FreChangeRate varchar(50)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO WaveForm
	VALUES(@Name,@Type,@Frequency,@BandWidth,@Phase,@StartFrequency,@EndFrequency,@RollOffFactor,@FreChangeRate)
END

GO

/****** Object:  StoredProcedure [dbo].[addZDYMaterial]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addZDYMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50),
	@LayerNum int,
	@TMaterialName nvarchar(50),
	@Conductivity float,
	@Permittivity float,
	@Roughness float,
	@Thickness float,
	@Red int,
	@Green int,
	@Blue int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	insert into ZDYMaterial(MaterialName,LayerNum,TMaterialName,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue) 
	values(@MaterialName,@LayerNum,@TMaterialName,@Conductivity,@Permittivity,@Roughness,@Thickness,@Red,@Green,@Blue);
END

GO

/****** Object:  StoredProcedure [dbo].[addZMaterial]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[addZMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50),
	@Conductivity float,
	@Permittivity float,
	@Roughness float,
	@Thickness float,
	@Red int,
	@Green int,
	@Blue int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    insert into Z_Material(MaterialName,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue) 
	values(@MaterialName,@Conductivity,@Permittivity,@Roughness,@Thickness,@Red,@Green,@Blue);
	
END

GO

/****** Object:  StoredProcedure [dbo].[createFileInfo]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[createFileInfo]
	@FileName nvarchar(50),
	@FilePath nvarchar(50),
	@ProjectName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
    
    declare 
    @ProjectID int,
    @FileInfoID int,
    @TaskInfoID int,
    @true int
	set @true=1
    set @ProjectID=-1
    set @TaskInfoID=-1
    
    select @FileInfoID=COUNT(*) 
	from FileInfo
	set @FileInfoID=@FileInfoID+1

	while @true=1
	begin
		if exists (
		select FileInfoID 
		from FileInfo
		where FileInfoID=@FileInfoID)
		begin
			set @FileInfoID=@FileInfoID+1
			continue
		end
		else
		begin
			insert into FileInfo(FileInfoID,FileName,FilePath)
			values(@FileInfoID,@FileName,@FilePath)
			break
		end
	end
	
	select @ProjectID=ProjectID
    from Project
    where Name=@ProjectName
    
	if @ProjectID<>-1
	begin
		select @TaskInfoID=TaskInfoID  
		from TaskInfo
		where @ProjectID=ProjectID
		
		if @TaskInfoID<>-1
		begin
		insert into TaskFileRelation
		values(@TaskInfoID,@FileInfoID)
		end
	end    
	
END

GO

/****** Object:  StoredProcedure [dbo].[createProject]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[createProject]
	@Name nvarchar(50),
	@Directory nvarchar(50),
	@ResultDirectory nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @ProjectID int,
	@true int
	set @true=1
	
	select @ProjectID=COUNT(*) 
	from Project
	set @ProjectID=@ProjectID+1

	while @true=1
	begin
		if exists (
		select ProjectID 
		from Project
		where ProjectID=@ProjectID)
		begin
			set @ProjectID=@ProjectID+1
			continue
		end
		else
		begin
			insert into Project(ProjectID,Name,ProjectState,Directory,ResultDirectory,StartTime)
			values(@ProjectID,@Name,0,@Directory,@ResultDirectory,GETDATE())
			break
		end
	end
		
END

GO

/****** Object:  StoredProcedure [dbo].[createTask]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[createTask]
	@ProName nvarchar(50),
	@FileNames nvarchar(1000)
AS
BEGIN
	
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	declare @ProjectID int,
	@FileInfoID int,
	@TaskInfoID int,
	@true int,
	@count int,
	@FileName nvarchar(50)
	set @count=0
	set @true=1
	set @FileInfoID=-1
	
	select * into #temp
	from dbo.f_split(@FileNames,',')
	
	select @count=COUNT(*)
	from #temp
	
	select @ProjectID=ProjectID 
	from Project 
	where Name=@ProName
	
	select @TaskInfoID=COUNT(*) 
	from TaskInfo
	
	set @TaskInfoID=@TaskInfoID+1

	while @true=1
	begin
	if exists (
	select TaskInfoID 
	from TaskInfo
	where TaskInfoID=@TaskInfoID)
		begin
			set @TaskInfoID=@TaskInfoID+1
			continue
			
		end
	else
		begin
			insert into TaskInfo(TaskInfoID,TaskState,ProjectID,StartTime) 
			values (@TaskInfoID,0,@ProjectID,GETDATE())
			
			while @count>0
			begin
				select @FileInfoID=FileInfoID
				from FileInfo
				where FileName=(select top 1 * from #temp)

				delete top(1) from #temp
				

				if @FileInfoID<>-1
				begin
				
					insert into TaskFileRelation(TaskInfoID,FileInfoID)
					values (@TaskInfoID,@FileInfoID)
					
				end
				set @FileInfoID=-1
				set @count=@count-1
				
			end
			
			break
			
		end
	end
	
	
END

GO

/****** Object:  StoredProcedure [dbo].[deleteAntenna]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deleteAntenna]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM Antenna
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[deleteMaterial]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deleteMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE From Material 
	where MaterialName=@MaterialName
END

GO

/****** Object:  StoredProcedure [dbo].[deleteProjectF]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[deleteProjectF]
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--select FileInfoID into #FileInfoIDtemp
	--from TaskFileRelation
	--where
	--	TaskFileRelation.TaskInfoID in (
	--	select TaskInfoID 
	--	from TaskInfo
	--	where TaskInfo.ProjectID in(
	--			select ProjectID
	--			from Project 
	--			where Project.Name=@Name
	--		)
	--	)

	
	--delete from FileInfo
	--	where FileInfo.FileInfoID in (
	--		select FileInfoID 
	--		from TaskFileRelation
	--		where
	--			TaskFileRelation.TaskInfoID in (
	--			select TaskInfoID 
	--			from TaskInfo
	--			where TaskInfo.ProjectID in(
	--				select ProjectID
	--				from Project 
	--				where Project.Name=@Name
	--			)
	--		)
	--	)

	--select TaskInfoID into #TaskInfoIDtemp
	--from TaskInfo
	--where TaskInfo.ProjectID in(
	--		select ProjectID
	--		from Project 
	--		where Project.Name=@Name
	--	)
	
	delete from TaskFileRelation
	where
		TaskFileRelation.TaskInfoID in (
		select TaskInfoID 
		from TaskInfo
		where TaskInfo.ProjectID in(
				select ProjectID
				from Project 
				where Project.Name=@Name
			)
		)
	delete from FileInfo
	where FileInfo.FileInfoID not in(
	select FileInfoID from TaskFileRelation
	)

	delete TaskInfo
	from Project,TaskInfo
	where
		TaskInfo.ProjectID=Project.ProjectID AND
		Project.Name=@Name
		
	--delete from TaskFileRelation
	--where
	--	TaskFileRelation.TaskInfoID in (
	--	select TaskInfoID 
	--	from #TaskInfoIDtemp
	--	)
	--drop table #TaskInfoIDtemp
	
	--delete from FileInfo
	--where FileInfo.FileInfoID in(
	--select FileInfoID from #FileInfoIDtemp
	--)
	--drop table #FileInfoIDtemp
	
	delete Project
	from Project
	where Project.Name=@Name
	
END

GO

/****** Object:  StoredProcedure [dbo].[deleteTerInfo]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deleteTerInfo]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM TerInfo
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[deleteTransmitter]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deleteTransmitter]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM Transmitter
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[deleteWaveForm]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[deleteWaveForm]
	-- Add the parameters for the stored procedure here
	@WaveFormName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM WaveForm
	WHERE Name=@WaveFormName
END

GO

/****** Object:  StoredProcedure [dbo].[getAllLZMaterial]    Script Date: 05/27/2014 12:46:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllLZMaterial] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,LayerNum,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue FROM LZ_Material;
END

GO

/****** Object:  StoredProcedure [dbo].[getAllMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllMaterial] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialSort,MaterialName,Red,Green,Blue,Conductivity,Permittivity,Roughness,Thickness,LayerOrder,LayerName,UpdateMaterial FROM Material;
END

GO

/****** Object:  StoredProcedure [dbo].[getAllTerNames]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllTerNames] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT name FROM TerInfo
END

GO

/****** Object:  StoredProcedure [dbo].[getAllTMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllTMaterial] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,Conductivity,Permittivity FROM T_Material 
END

GO

/****** Object:  StoredProcedure [dbo].[getAllTransmitters]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllTransmitters] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Name FROM Transmitter
END

GO

/****** Object:  StoredProcedure [dbo].[getAllWaveForms]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllWaveForms]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Name FROM WaveForm
END

GO

/****** Object:  StoredProcedure [dbo].[getAllZDYMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllZDYMaterial] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,LayerNum,TMaterialName,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue FROM ZDYMaterial 
END

GO

/****** Object:  StoredProcedure [dbo].[getAllZMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAllZMaterial] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue FROM Z_Material 
END

GO

/****** Object:  StoredProcedure [dbo].[getAntenna]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAntenna] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Antenna
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[getAntennaByType]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getAntennaByType] 
	-- Add the parameters for the stored procedure here
	@Type nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Antenna WHERE Type=@Type
END

GO

/****** Object:  StoredProcedure [dbo].[getFileNames]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getFileNames]
	-- Add the parameters for the stored procedure here
	@TaskInfoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT FileInfo.FileName FROM 
	(TaskInfo join TaskFileRelation 
	  on TaskInfo.TaskInfoID=TaskFileRelation.TaskInfoID
	  AND TaskInfo.TaskInfoID=@TaskInfoID) 
	  join FileInfo on TaskFileRelation.FileInfoID=FileInfo.FileInfoID
	
	
END

GO

/****** Object:  StoredProcedure [dbo].[getFilePath]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getFilePath] 
	-- Add the parameters for the stored procedure here
	@FileName nvarchar(50),
	@FilePath nvarchar(50) output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SET @FilePath=
	(SELECT FilePath FROM FileInfo WHERE FileName=@FileName)
END

GO

/****** Object:  StoredProcedure [dbo].[getLZMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getLZMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    SELECT MaterialName,LayerNum,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue FROM LZ_Material where MaterialName=@MaterialName
END

GO

/****** Object:  StoredProcedure [dbo].[getMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getMaterial] 
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialSort,MaterialName,Red,Green,Blue,Conductivity,Permittivity,Roughness,Thickness,LayerOrder,LayerName,UpdateMaterial FROM Material where MaterialName=@MaterialName
END

GO

/****** Object:  StoredProcedure [dbo].[getMinTaskInfo]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getMinTaskInfo]
	-- Add the parameters for the stored procedure here
	-- @TaskInfoID int output	
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @taskid int
    -- Insert statements for procedure here
	set @taskid=(select MIN(TaskInfoID) from TaskInfo where TaskState=0)
	select TaskInfo.TaskInfoID,Project.Name,FileInfo.FileName,FileInfo.FilePath 
	from TaskInfo,Project,FileInfo,TaskFileRelation
	where TaskInfo.TaskInfoID=@taskid and TaskInfo.ProjectID=Project.ProjectID
	and TaskFileRelation.TaskInfoID=@taskid and TaskFileRelation.FileInfoID=FileInfo.FileInfoID
	UPDATE TaskInfo
	SET TaskState=1
	where TaskInfoID=@taskid;
END

GO

/****** Object:  StoredProcedure [dbo].[getProjectInfo]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getProjectInfo] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    Create Table #Finished
    (ProjectID int,
     FinishedNo int
     )
     Create Table #All
     (ProjectID int,
     AllNo int
     )
     
     Insert Into #Finished( ProjectID,FinishedNo)
     SELECT Project.ProjectID,count(TaskInfoID)FROM Project join TaskInfo
     on Project.ProjectID=TaskInfo.ProjectID AND TaskInfo.TaskState=2
     GROUP BY Project.ProjectID
     
     Insert Into #All( ProjectID,AllNo)
     SELECT Project.ProjectID,count(TaskInfoID)FROM Project join TaskInfo
     on Project.ProjectID=TaskInfo.ProjectID 
     GROUP BY Project.ProjectID
     
     
     
	SELECT Name,Directory,ResultDirectory,ProjectState,Cast(#Finished.FinishedNo/#All.AllNo as varchar(50)) as 'percent',Project.StartTime,Project.EndTime
	FROM Project left outer join #Finished on Project.ProjectID=#Finished.ProjectID,#All
	WHERE Project.ProjectID=#All.ProjectID 
	ORDER BY Project.StartTime
	DESC
	
	
END

GO

/****** Object:  StoredProcedure [dbo].[getProjects]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getProjects]
	-- Add the parameters for the stored procedure here
	@ProjectState smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Name,ResultDirectory FROM Project
	WHERE ProjectState=@ProjectState
END

GO

/****** Object:  StoredProcedure [dbo].[getResultDirectory]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[getResultDirectory]
	@TaskInfoID int,
	@ResultDirectory nvarchar(50) output
AS
BEGIN
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    select @ResultDirectory=ResultDirectory
    from Project
    where ProjectID=(
	select ProjectID
	from TaskInfo
	where @TaskInfoID=TaskInfoID)
	
END

GO

/****** Object:  StoredProcedure [dbo].[getTasks]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getTasks]
	-- Add the parameters for the stored procedure here
	@TaskState smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM TaskInfo
	WHERE TaskState=@TaskState
END

GO

/****** Object:  StoredProcedure [dbo].[getTerInfo]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getTerInfo] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM TerInfo 
	WHERE @Name=name
END

GO

/****** Object:  StoredProcedure [dbo].[getTMaterial]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getTMaterial] 
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,Conductivity,Permittivity FROM T_Material where MaterialName=@MaterialName
END

GO

/****** Object:  StoredProcedure [dbo].[getTransmitter]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getTransmitter]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM Transmitter
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[getWaveForm]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getWaveForm]
	-- Add the parameters for the stored procedure here
	@WaveFormName nvarchar(50)
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM WaveForm
	WHERE Name=@WaveFormName
END

GO

/****** Object:  StoredProcedure [dbo].[getWaveFormByType]    Script Date: 05/27/2014 12:46:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[getWaveFormByType]
@Type nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM WaveForm WHERE Type=@Type
END

GO

/****** Object:  StoredProcedure [dbo].[getZDYMaterial]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getZDYMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,LayerNum,TMaterialName,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue FROM ZDYMaterial where MaterialName=@MaterialName
END

GO

/****** Object:  StoredProcedure [dbo].[getZMaterial]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[getZMaterial] 
	-- Add the parameters for the stored procedure here
	@MaterialName nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT MaterialName,Conductivity,Permittivity,Roughness,Thickness,Red,Green,Blue FROM Z_Material where MaterialName=@MaterialName
END

GO

/****** Object:  StoredProcedure [dbo].[setProjectFinished]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[setProjectFinished] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Update Project
	SET ProjectState=1
	WHERE ProjectID NOT in(SELECT ProjectID FROM TaskInfo WHERE TaskState=0 or TaskState=1 Group By ProjectID ) 
	AND 
	ProjectID in(SELECT ProjectID FROM Project WHERE ProjectState=0)
	--AND (SELECT COUNT(*) FROM TaskInfo)>0 
	AND ProjectID in (select ProjectID from TaskInfo)

END

GO

/****** Object:  StoredProcedure [dbo].[setProjectState]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[setProjectState] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50),
	@ProState int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    if @ProState=2
    BEGIN
	UPDATE Project
	SET ProjectState=@ProState,EndTime=GETDATE(),[Percent]=100
	WHERE Name=@Name
	END
	else
	BEGIN
	UPDATE Project
	SET ProjectState=@ProState
	WHERE Name=@Name
	END
END

GO

/****** Object:  StoredProcedure [dbo].[setTaskState]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[setTaskState]
	-- Add the parameters for the stored procedure here
	 @TaskInfoID int,
	 @TaskState smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE TaskInfo
	SET TaskState=@TaskState
	WHERE TaskInfoID=@TaskInfoID
END

GO

/****** Object:  StoredProcedure [dbo].[sp_confirmRelation]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[sp_confirmRelation]
@file varchar(20)=null
as 
select top 4 
	 Project.Name,Project.Directory as Projectpath,Project.ResultDirectory,
	 FileInfo.FileName as Fileinfoname,FileInfo.FilePath as Fileinfopath,
	 Project.StartTime
from FileInfo,Project,TaskInfo,TaskFileRelation
where Project.ProjectID=TaskInfo.ProjectID 
and   TaskInfo.TaskInfoID=TaskFileRelation.TaskInfoID
and   TaskFileRelation.FileInfoID=FileInfo.FileInfoID
and	  Project.Name=@file
order by Project.StartTime desc

GO

/****** Object:  StoredProcedure [dbo].[updateAntenna]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[updateAntenna]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50),
	@Type nvarchar(50),
	@MaxGain float,
	@Polarization nvarchar(50),
	@ReceiverThreshold float,
	@TransmisionLoss float,
	@VSWR float,
	@Temperature float,
	@Radius float,
	@BlockageRadius float,
	@ApetureDistribution nvarchar(50),
	@EdgeTeper float,
	@Length float,
	@Pitch float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Antenna
	SET Type=@Type,MaxGain=@MaxGain,Polarization=@Polarization,ReceiverThreshold=@ReceiverThreshold,
	TransmissionLoss=@TransmisionLoss,VSWR=@VSWR,Temperature=@Temperature,Radius=@Radius,BlockageRadius=@BlockageRadius,ApertureDistribution=@ApetureDistribution,
	EdgeTeper=@EdgeTeper,Length=@Length,Pitch=@Pitch
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[updateMaterial]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[updateMaterial]
	-- Add the parameters for the stored procedure here
	@MaterialSort nvarchar(50),
	@MaterialName nvarchar(50),
	@Red int,
	@Green int,
	@Blue int,
	@Conductivity float,
	@Permittivity float,
	@Roughness float,
	@Thickness float,
	@LayerOrder int=null,
	@LayerName nvarchar(50)=null,
	@UpdateMaterial int=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@LayerName is NULL)
	UPDATE Material
	SET MaterialSort=@MaterialSort,Red=@Red,Green=@Green,Blue=@Blue,Conductivity=@Conductivity,Permittivity=@Permittivity,
	Roughness=@Roughness,Thickness=@Thickness,LayerOrder=@LayerOrder,UpdateMaterial=@UpdateMaterial
	WHERE MaterialName=@MaterialName 
	ELSE
	UPDATE Material
	SET MaterialSort=@MaterialSort,Red=@Red,Green=@Green,Blue=@Blue,Conductivity=@Conductivity,Permittivity=@Permittivity,
	Roughness=@Roughness,Thickness=@Thickness,LayerOrder=@LayerOrder,UpdateMaterial=@UpdateMaterial
	WHERE MaterialName=@MaterialName AND LayerName=@LayerName
END

GO

/****** Object:  StoredProcedure [dbo].[updateTransmitter]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[updateTransmitter] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50),
	@RotateX float,
	@RotateY float,
	@RotateZ float,
	@Power float,
	@AntennaName nvarchar(50)
	
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Transmitter
	SET RotateX=@RotateX,RotateY=@RotateY,RotateZ=@RotateZ,Power=@Power,AntennaName=@AntennaName
	WHERE Name=@Name
END

GO

/****** Object:  StoredProcedure [dbo].[updateWaveForm]    Script Date: 05/27/2014 12:46:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[updateWaveForm]
	-- Add the parameters for the stored procedure here
	@Name nvarchar(50) ,
	@Type varchar(50),
	@Frequency float,
	@BandWidth float,
	@Phase float=null,
	@StartFrequency float=null,
	@EndFrequency float=null,
	@RollOffFactor float=null,
	@FreChangeRate varchar(50)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE WaveForm
	SET Type=@Type,Frequency=@Frequency,BandWidth=@BandWidth,Phase=@Phase,StartFrequency=@StartFrequency,EndFrequency=@EndFrequency,RollOffFactor=@RollOffFactor,FreChangeRate=@FreChangeRate
	WHERE Name=@Name
END

GO

