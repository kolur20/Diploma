USE [master]
GO

/****** Object:  Database [Parking]    Script Date: 01.03.2018 10:36:48 ******/
CREATE DATABASE [Parking]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Parking', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.RESTO\MSSQL\DATA\Parking.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Parking_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.RESTO\MSSQL\DATA\Parking_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [Parking] SET COMPATIBILITY_LEVEL = 120
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Parking].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Parking] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Parking] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Parking] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Parking] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Parking] SET ARITHABORT OFF 
GO

ALTER DATABASE [Parking] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Parking] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Parking] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Parking] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Parking] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Parking] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Parking] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Parking] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Parking] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Parking] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Parking] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Parking] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Parking] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Parking] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Parking] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Parking] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Parking] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Parking] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [Parking] SET  MULTI_USER 
GO

ALTER DATABASE [Parking] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Parking] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Parking] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Parking] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [Parking] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Parking] SET  READ_WRITE 
GO



USE [Parking]
GO

/****** Object:  Table [dbo].[Book]    Script Date: 01.03.2018 9:58:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Book](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [nchar](15) NOT NULL,
	[Tariff] [int] NULL,
	[DataIn] [datetime] NOT NULL,
	[DataOut] [datetime] NULL,
	[DataPay] [datetime] NULL,
	[Time] [float] NULL,
	[Price] [int] NULL,
	[Number] [nchar](15) NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[Guests](
	[Uid] [nchar](15) NOT NULL,
	[Discount] [float] NOT NULL,
	[Name] [nchar](30) NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[Tariffs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrgNode] [hierarchyid] NULL,
	[Use] [bit] NULL,
	[Day] [nchar](15) NULL,
	[idTariff] [int] NULL,
	[TimeS] [time](7) NULL,
	[TimeF] [time](7) NULL,
	[Tariff] [int] NULL
) ON [PRIMARY]

INSERT INTO [dbo].[Tariffs]
           ([OrgNode]
           ,[Use]
           ,[Day]
           ,[TimeS]
           ,[TimeF]
           ,[Tariff])
     VALUES
           (hierarchyid::GetRoot()
           ,1
           ,'Все'
           ,'00:00:00'
           ,'23:59:59'
           ,100)


CREATE PROC AddDayWeek(@use bit, @day varchar(15))   
AS   
BEGIN  
   DECLARE @dOrgNode hierarchyid, @lc hierarchyid  
   SELECT @dOrgNode = OrgNode   
   FROM Tariffs   
   WHERE Day = 'Все'  
   SET TRANSACTION ISOLATION LEVEL SERIALIZABLE  
   BEGIN TRANSACTION  
      SELECT @lc = max(OrgNode)   
      FROM Tariffs   
      WHERE OrgNode.GetAncestor(1) = @dOrgNode ;  

      INSERT Tariffs (OrgNode, [Use], Day)  
      VALUES(@dOrgNode.GetDescendant(@lc, NULL), @use, @day)  
   COMMIT  
END ;

CREATE PROC AddTariff(@day varchar(15), @idtariff int, @times time(7), @timef time(7), @cost int)   
AS   
BEGIN  
   DECLARE @dOrgNode hierarchyid, @lc hierarchyid  
   SELECT @dOrgNode = OrgNode   
   FROM Tariffs   
   WHERE Day = @day  
   SET TRANSACTION ISOLATION LEVEL SERIALIZABLE  
   BEGIN TRANSACTION  
      SELECT @lc = max(OrgNode)   
      FROM Tariffs   
      WHERE OrgNode.GetAncestor(1) = @dOrgNode ;  

      INSERT Tariffs (OrgNode, idTariff, TimeS, TimeF, Tariff)  
      VALUES(@dOrgNode.GetDescendant(@lc, NULL), @idtariff, @times, @timef, @cost)  
   COMMIT  
END ; 


CREATE FUNCTION GetTariffs(@dayweek varchar(15))
	RETURNS TABLE
 RETURN SELECT *  
	FROM Tariffs  
	WHERE OrgNode.GetAncestor(1) = (SELECT hierarchyid = OrgNode  
		FROM Tariffs  
		WHERE Day = @dayweek) ;
		
		
CREATE FUNCTION GetRoot()
	RETURNS TABLE
 RETURN	SELECT  *  
	FROM Tariffs 
	WHERE OrgNode = hierarchyid::GetRoot() ;

GO