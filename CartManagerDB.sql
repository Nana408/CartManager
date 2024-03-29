USE [master]
GO
/****** Object:  Database [CartManagementSystem]    Script Date: 2/28/2024 10:19:49 PM ******/
CREATE DATABASE [CartManagementSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CartManagementSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.NANA\MSSQL\DATA\CartManagementSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CartManagementSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.NANA\MSSQL\DATA\CartManagementSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CartManagementSystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CartManagementSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CartManagementSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CartManagementSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CartManagementSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CartManagementSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CartManagementSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [CartManagementSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CartManagementSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CartManagementSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CartManagementSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CartManagementSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CartManagementSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CartManagementSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CartManagementSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CartManagementSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CartManagementSystem] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CartManagementSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CartManagementSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CartManagementSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CartManagementSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CartManagementSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CartManagementSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CartManagementSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CartManagementSystem] SET RECOVERY FULL 
GO
ALTER DATABASE [CartManagementSystem] SET  MULTI_USER 
GO
ALTER DATABASE [CartManagementSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CartManagementSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CartManagementSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CartManagementSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CartManagementSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CartManagementSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CartManagementSystem', N'ON'
GO
ALTER DATABASE [CartManagementSystem] SET QUERY_STORE = OFF
GO
USE [CartManagementSystem]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 2/28/2024 10:19:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[SourceKey] [nvarchar](256) NULL,
	[SourceToken] [nvarchar](256) NULL,
	[Status] [bit] NULL,
	[DateCreated] [datetime] NULL,
	[CreatedBy] [nvarchar](256) NULL,
	[DateModified] [datetime] NULL,
	[ModifiedBy] [nvarchar](256) NULL,
 CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 2/28/2024 10:19:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreationBy] [nvarchar](100) NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartItems]    Script Date: 2/28/2024 10:19:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartItems](
	[CartItemID] [int] IDENTITY(1,1) NOT NULL,
	[CartID] [int] NULL,
	[ProductID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](10, 2) NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreationBy] [nvarchar](100) NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[CartItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 2/28/2024 10:19:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreationBy] [nvarchar](100) NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserLog]    Script Date: 2/28/2024 10:19:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserAgent] [nvarchar](255) NULL,
	[UserFunction] [nvarchar](255) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[TransStatus] [int] NULL,
	[SourceID] [int] NULL,
	[SourceName] [nvarchar](255) NULL,
	[ResponseBody] [nvarchar](max) NULL,
	[RequestBody] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2/28/2024 10:19:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Phonenumber] [nvarchar](100) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreationBy] [nvarchar](100) NOT NULL,
	[LastUpdatedDate] [datetime] NULL,
	[LastUpdatedBy] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Application] ON 

INSERT [dbo].[Application] ([Id], [Name], [SourceKey], [SourceToken], [Status], [DateCreated], [CreatedBy], [DateModified], [ModifiedBy]) VALUES (1, N'Hubtel', N'98B4423E-B6B4-4649-B014-9100D85BA9AE', N'AC3CCB3D-3AA3-41FD-8367-6563E70BCADE', 1, CAST(N'2024-02-28T19:45:54.170' AS DateTime), N'App Admin', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Application] OFF
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([CartID], [UserID], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (2, 1, CAST(N'2024-02-24T10:03:57.630' AS DateTime), N'1', CAST(N'2024-02-24T10:05:53.743' AS DateTime), N'1')
INSERT [dbo].[Cart] ([CartID], [UserID], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (3, 1, CAST(N'2024-02-24T10:05:13.507' AS DateTime), N'1', NULL, NULL)
INSERT [dbo].[Cart] ([CartID], [UserID], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (4, 1, CAST(N'2024-02-24T10:06:31.460' AS DateTime), N'1', CAST(N'2024-02-28T22:10:02.780' AS DateTime), N'1')
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[CartItems] ON 

INSERT [dbo].[CartItems] ([CartItemID], [CartID], [ProductID], [Quantity], [Price], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (1, 2, 1, 1, CAST(0.00 AS Decimal(10, 2)), CAST(N'2024-02-24T10:04:15.877' AS DateTime), N'1', NULL, NULL)
INSERT [dbo].[CartItems] ([CartItemID], [CartID], [ProductID], [Quantity], [Price], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (2, 3, 2, 1, CAST(0.00 AS Decimal(10, 2)), CAST(N'2024-02-24T10:05:13.910' AS DateTime), N'1', NULL, NULL)
INSERT [dbo].[CartItems] ([CartItemID], [CartID], [ProductID], [Quantity], [Price], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (3, 2, 2, 6, CAST(0.00 AS Decimal(10, 2)), CAST(N'2024-02-24T10:05:36.780' AS DateTime), N'1', NULL, NULL)
INSERT [dbo].[CartItems] ([CartItemID], [CartID], [ProductID], [Quantity], [Price], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (4, 4, 2, 7, CAST(143.43 AS Decimal(10, 2)), CAST(N'2024-02-24T10:06:31.480' AS DateTime), N'1', NULL, NULL)
INSERT [dbo].[CartItems] ([CartItemID], [CartID], [ProductID], [Quantity], [Price], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (1003, 4, 8, 514, CAST(9637.50 AS Decimal(10, 2)), CAST(N'2024-02-28T21:38:53.777' AS DateTime), N'1', NULL, NULL)
SET IDENTITY_INSERT [dbo].[CartItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (1, N'Product 1', N'Description for Product 1', CAST(10.99 AS Decimal(10, 2)), 100, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (2, N'Product 2', N'Description for Product 2', CAST(20.49 AS Decimal(10, 2)), 50, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (3, N'Product 3', N'Description for Product 3', CAST(5.75 AS Decimal(10, 2)), 200, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (4, N'Product 4', N'Description for Product 4', CAST(15.25 AS Decimal(10, 2)), 75, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (5, N'Product 5', N'Description for Product 5', CAST(8.99 AS Decimal(10, 2)), 150, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (6, N'Product 6', N'Description for Product 6', CAST(30.00 AS Decimal(10, 2)), 25, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (7, N'Product 7', N'Description for Product 7', CAST(12.50 AS Decimal(10, 2)), 80, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (8, N'Product 8', N'Description for Product 8', CAST(18.75 AS Decimal(10, 2)), 120, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (9, N'Product 9', N'Description for Product 9', CAST(25.99 AS Decimal(10, 2)), 60, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Products] ([ProductID], [ProductName], [Description], [Price], [Quantity], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (10, N'Product 10', N'Description for Product 10', CAST(7.49 AS Decimal(10, 2)), 90, CAST(N'2024-02-24T08:18:59.037' AS DateTime), N'admin', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[UserLog] ON 

INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (1, N'::1/PostmanRuntime/7.36.3', N'FilterCartItem', CAST(N'2024-02-28T21:31:27.220' AS DateTime), CAST(N'2024-02-28T21:31:29.517' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'{"From":"2024-02-01","To":"2024-02-29","PhoneNumber":"123-456-7890"}')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (2, N'::1/PostmanRuntime/7.36.3', N'AddToCart', CAST(N'2024-02-28T21:33:04.503' AS DateTime), CAST(N'2024-02-28T21:33:05.247' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'{"CartId":4,"UserId":1,"ProductId":8,"Quantity":512}')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (3, N'::1/PostmanRuntime/7.36.3', N'RemoveFromCart', CAST(N'2024-02-28T21:33:17.713' AS DateTime), CAST(N'2024-02-28T21:33:18.337' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'{"CartId":4,"UserId":1,"ProductId":8}')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (4, N'::1/PostmanRuntime/7.36.3', N'GetCartItem', CAST(N'2024-02-28T21:33:32.327' AS DateTime), CAST(N'2024-02-28T21:33:32.470' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'2')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (5, N'::1/PostmanRuntime/7.36.3', N'GetProducts', CAST(N'2024-02-28T21:34:19.893' AS DateTime), CAST(N'2024-02-28T21:34:19.967' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'""')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (6, N'::1/PostmanRuntime/7.36.3', N'GetCartItem', CAST(N'2024-02-28T21:35:14.333' AS DateTime), CAST(N'2024-02-28T21:35:14.377' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'""')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (7, N'::1/PostmanRuntime/7.36.3', N'AddToCart', CAST(N'2024-02-28T21:38:53.407' AS DateTime), CAST(N'2024-02-28T21:38:53.910' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'{"CartId":4,"UserId":1,"ProductId":8,"Quantity":512}')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (8, N'::1/PostmanRuntime/7.36.3', N'GetCartItem', CAST(N'2024-02-28T21:41:57.280' AS DateTime), CAST(N'2024-02-28T21:41:58.753' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'2')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (9, N'::1/PostmanRuntime/7.36.3', N'GetCartItem', CAST(N'2024-02-28T21:42:02.903' AS DateTime), CAST(N'2024-02-28T21:42:03.087' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'2')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (10, N'::1/PostmanRuntime/7.36.3', N'GetCartItem', CAST(N'2024-02-28T21:42:20.553' AS DateTime), CAST(N'2024-02-28T21:42:20.577' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'2')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (11, N'::1/PostmanRuntime/7.36.3', N'GetProducts', CAST(N'2024-02-28T21:45:57.977' AS DateTime), CAST(N'2024-02-28T21:45:58.013' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'""')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (12, N'::1/PostmanRuntime/7.36.3', N'GetCartItem', CAST(N'2024-02-28T21:46:33.050' AS DateTime), CAST(N'2024-02-28T21:46:33.120' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'""')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (13, N'::1/PostmanRuntime/7.36.3', N'AddToCart', CAST(N'2024-02-28T22:09:17.687' AS DateTime), CAST(N'2024-02-28T22:09:22.880' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'{"CartId":4,"UserId":1,"ProductId":8,"Quantity":2}')
INSERT [dbo].[UserLog] ([Id], [UserAgent], [UserFunction], [StartDate], [EndDate], [TransStatus], [SourceID], [SourceName], [ResponseBody], [RequestBody]) VALUES (14, N'::1/PostmanRuntime/7.36.3', N'AddToCart', CAST(N'2024-02-28T22:10:02.747' AS DateTime), CAST(N'2024-02-28T22:10:02.840' AS DateTime), 1, 1, N'Hubtel', N'Request Successful', N'{"CartId":4,"UserId":1,"ProductId":2,"Quantity":2}')
SET IDENTITY_INSERT [dbo].[UserLog] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (1, N'john_doe', N'john.doe@example.com', N'password123', N'123-456-7890', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (2, N'jane_smith', N'jane.smith@example.com', N'securepass', N'987-654-3210', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (3, N'bob_jackson', N'bob.jackson@example.com', N'bobspassword', N'555-555-5555', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (4, N'alice_walker', N'alice.walker@example.com', N'letmein', N'111-222-3333', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (5, N'david_brown', N'david.brown@example.com', N'davidpass', N'444-444-4444', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (6, N'sarah_miller', N'sarah.miller@example.com', N'sarahpass', N'666-666-6666', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (7, N'michael_clark', N'michael.clark@example.com', N'mikepass', N'777-777-7777', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (8, N'emily_taylor', N'emily.taylor@example.com', N'emilypass', N'888-888-8888', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (9, N'samuel_wilson', N'samuel.wilson@example.com', N'sampass', N'999-999-9999', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
INSERT [dbo].[Users] ([UserID], [Username], [Email], [Password], [Phonenumber], [CreationDate], [CreationBy], [LastUpdatedDate], [LastUpdatedBy]) VALUES (10, N'laura_jones', N'laura.jones@example.com', N'laurapass', N'000-000-0000', CAST(N'2024-02-24T08:17:38.243' AS DateTime), N'admin', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD FOREIGN KEY([CartID])
REFERENCES [dbo].[Cart] ([CartID])
GO
ALTER TABLE [dbo].[CartItems]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
USE [master]
GO
ALTER DATABASE [CartManagementSystem] SET  READ_WRITE 
GO
