USE [master]
GO
/****** Object:  Database [AddressBook]    Script Date: 6/28/2018 1:59:15 AM ******/
CREATE DATABASE [AddressBook]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AddressBook', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\AddressBook.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'AddressBook_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\AddressBook_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [AddressBook] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AddressBook].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AddressBook] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AddressBook] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AddressBook] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AddressBook] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AddressBook] SET ARITHABORT OFF 
GO
ALTER DATABASE [AddressBook] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AddressBook] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AddressBook] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AddressBook] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AddressBook] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AddressBook] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AddressBook] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AddressBook] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AddressBook] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AddressBook] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AddressBook] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AddressBook] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AddressBook] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AddressBook] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AddressBook] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AddressBook] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AddressBook] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AddressBook] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AddressBook] SET  MULTI_USER 
GO
ALTER DATABASE [AddressBook] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AddressBook] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AddressBook] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AddressBook] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [AddressBook]
GO
/****** Object:  Schema [address]    Script Date: 6/28/2018 1:59:15 AM ******/
CREATE SCHEMA [address]
GO
/****** Object:  Table [address].[Contact]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [address].[Contact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NULL,
	[company] [nvarchar](500) NULL,
	[email] [nvarchar](100) NULL,
	[skype] [nvarchar](50) NULL,
	[phone] [nvarchar](50) NULL,
	[linkedIn] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [address].[contact_tag]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [address].[contact_tag](
	[contactId] [int] NOT NULL,
	[tagId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [address].[Tag]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [address].[Tag](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [address].[assign_contact]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [address].[assign_contact]
	@ContactId int,
	@TagId int,
	@IDout int out,
	@processMessage varchar(200) out
as
	if exists (select 'x' from [address].[contact_tag] where contactId = @ContactId and tagId=@TagId)
	begin
		set @processMessage = 'Failed: existing relationship';
		set @IDout = 0;
		return;
	end
	insert into [address].[contact_tag] values (@contactId, @TagId);
	set @IDout = scope_identity();
	set @processMessage = 'Success: assigned tag successfully';
GO
/****** Object:  StoredProcedure [address].[change_tag_assign]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [address].[change_tag_assign]
	@ContactId int,
	@TagId int,
	@IDout int out,
	@processMessage varchar(200) out
as
	if exists (select 'x' from [address].[contact_tag] where contactId = @ContactId and tagId=@TagId)
	begin
		delete from [address].[contact_tag] where contactId = @ContactId and tagId = @TagId;
		set @processMessage = 'Success: removed tag';
		set @IDout = 0;
		return;
	end
	insert into [address].[contact_tag] values (@ContactId, @TagId);
	set @IDout = 1;
	set @processMessage = 'Success: assigned tag successfully';
GO
/****** Object:  StoredProcedure [address].[delete_tag]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [address].[delete_tag]
	@TagID int = 0,
	@IDOut int output,
	@ProcessMessage varchar(200) output 
as 
	
	if (@TagID = 0)
	begin
		set @ProcessMessage='Failed: no record deleted';
		set @IDOut = 0;
		return;
	end 
	else if exists (select 'x' from [address].contact_tag where tagId = @TagID) 
	begin
		set @ProcessMessage = 'Failed: tag assigned to contact';
		set @IDOut = 0;
		return;
	end
	else
	begin
		delete from [address].Tag where ID = @TagID;
		set @IDOut = @TagID;
		set @ProcessMessage = 'Success: deleted record';
	end
	
GO
/****** Object:  StoredProcedure [address].[get_contact]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [address].[get_contact]
	@ContactId int
as
	select * from [address].Contact where ID = @ContactId;
GO
/****** Object:  StoredProcedure [address].[merge_tag]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [address].[merge_tag]
	@TagName varchar(200),
	@TagID int = 0,
	@IDOut int output,
	@ProcessMessage varchar(200) output 
as
	if (@TagName = '') 
	begin 
		set @ProcessMessage = 'Failed: Invalid TagName!'  
		return;
	end
	if (@TagID = 0) 
	begin
		insert into [address].Tag values (@TagName);
		set @IDOut = scope_identity();
		set @ProcessMessage = 'Success: Inserted record';
	end
	else
	begin
		update [address].Tag set name = @TagName where ID = @TagID;
		set @ProcessMessage='Success: Updated record';
	end
GO
/****** Object:  StoredProcedure [address].[remove_contact]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [address].[remove_contact]
	@ContactId int,
	@TagId int,
	@IDout int out,
	@processMessage varchar(200) out
as
	if exists (select 'x' from [address].[contact_tag] where contactId = @ContactId and tagId=@TagId)
	begin
		delete from [address].[contact_tag] where contactId = @ContactId and tagId = @TagId;
		set @processMessage = 'Success: removed tag';
		set @IDout = 0;
	end
	
GO
/****** Object:  StoredProcedure [address].[select_address_by_name_or_company]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [address].[select_address_by_name_or_company]
	@Keyword varchar(200)
as
	select * from [address].Contact c 
	where c.name like '%'+@Keyword+'%' or c.company like '%'+@Keyword+'%';
GO
/****** Object:  StoredProcedure [address].[select_contact]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [address].[select_contact]
	@Keyword varchar(200) = '',
	@TagId int = 0
as
	create table #tmp(contactId int, tagIds varchar(500));
	if (@TagId = 0 and @Keyword <> '')
	begin
		select distinct c.*,
		stuff((select ','+ convert(varchar(100),tagId) from [address].contact_tag ct where c.ID=ct.contactId for xml path('')),1,1,'') [tags]
		from [address].Contact c 
		where c.name like '%'+@Keyword+'%' or c.company like '%'+@Keyword+'%';

	end
	else if (@Keyword <> '')
	begin
		select distinct c.*,
		stuff((select ','+ convert(varchar(100),tagId) from [address].contact_tag ct where c.ID=ct.contactId for xml path('')),1,1,'') [tags]
		from [address].Contact c;
	end
	else
	begin
		select distinct c.*,
		stuff((select ','+ convert(varchar(100),tagId) from [address].contact_tag ct where c.ID=ct.contactId for xml path('')),1,1,'') [tags]
		from [address].Contact c 
		left join [address].contact_tag ct 
		on c.ID = ct.contactId
		where ct.tagId = @TagId
	end
GO
/****** Object:  StoredProcedure [address].[select_tag]    Script Date: 6/28/2018 1:59:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [address].[select_tag]
as
	select * from [address].Tag;
GO
USE [master]
GO
ALTER DATABASE [AddressBook] SET  READ_WRITE 
GO
