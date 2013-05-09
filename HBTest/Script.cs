using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBTest
{
    public static class Script
    {
        public static string script = @"USE [master]
GO
/****** Object:  Database [HomeBank]    Script Date: 5/9/2013 9:59:51 AM ******/
CREATE DATABASE [HomeBank] 
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HomeBank].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HomeBank] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [HomeBank] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [HomeBank] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [HomeBank] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [HomeBank] SET ARITHABORT OFF 
GO
ALTER DATABASE [HomeBank] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [HomeBank] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [HomeBank] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [HomeBank] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [HomeBank] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [HomeBank] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [HomeBank] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [HomeBank] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [HomeBank] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [HomeBank] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [HomeBank] SET  DISABLE_BROKER 
GO
ALTER DATABASE [HomeBank] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [HomeBank] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [HomeBank] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [HomeBank] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [HomeBank] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [HomeBank] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [HomeBank] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [HomeBank] SET RECOVERY FULL 
GO
ALTER DATABASE [HomeBank] SET  MULTI_USER 
GO
ALTER DATABASE [HomeBank] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [HomeBank] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'HomeBank', N'ON'
GO
USE [HomeBank]
GO
/****** Object:  StoredProcedure [dbo].[AddNewAccount]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewAccount] @Name nvarchar(50),@Balance smallmoney, @CreditInstID int, @date datetime2 = null
AS
BEGIN
	IF (@date IS NULL)
		SET @date = GETDATE()
	IF (@Name != '') 
		INSERT INTO Accounts(Name,Balance, CreditInst, CreateDate )
		VALUES (@Name, @Balance, @CreditInstID, @date)
	ELSE RETURN -1
END
GO
/****** Object:  StoredProcedure [dbo].[AddNewBank]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewBank] @UserID int, @Name nvarchar(12)
AS
INSERT INTO Banks( UserID, Name) VALUES (@UserID, @Name)

SELECT CAST(IDENT_CURRENT( 'Banks' ) AS int)
GO
/****** Object:  StoredProcedure [dbo].[AddNewCategory]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewCategory] @Name nvarchar(50),@UserID int, @type bit, @BelongsTo int = null
AS
	IF (@BelongsTo IS NULL)
		INSERT INTO Categories( Name, Type, UserID)
			VALUES (@Name, @type, @UserID)
	ELSE 
		IF EXISTS(SELECT *FROM Categories WHERE ID = @BelongsTo) AND
			@type = (SELECT type FROM Categories WHERE id = @BelongsTo)
			INSERT INTO Categories( Name, Type, UserID, BelongsTo)
				VALUES (@Name, @type, @UserID, @BelongsTo)
		ELSE
			RETURN -2
	SELECT CAST(IDENT_CURRENT( 'Categories' ) AS int)
GO
/****** Object:  StoredProcedure [dbo].[AddNewExpence]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewExpence] @AccoutnID int, @Name nvarchar(50), @Amount smallmoney,
							 @CategoryID int, @Date datetime2 = null
AS
	DECLARE @type bit
	IF (@Amount > 0)
		SET @Amount *= -1
	SELECT @type = type FROM Categories WHERE ID = @CategoryID
	--if amount does not match categorie's type return-1
	IF ( @Amount > 0 )
		BEGIN
			PRINT ('Expence amount cannot be positive')
			RETURN -1
		END
	IF ( @type = 1)
	BEGIN
			PRINT ('This category is not for expence')
			RETURN -2
		END
	IF (@Date IS NULL) SET @Date = GETDATE()
	INSERT INTO Transactions(AccountID,Amount, Name, CategoryID,Date)
		VALUES( @AccoutnID,@Amount,@Name,@CategoryID,@Date)
GO
/****** Object:  StoredProcedure [dbo].[AddNewIncome]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewIncome] @AccoutnID int, @Name nvarchar(50), @Amount smallmoney,
							 @CategoryID int, @Date datetime2 = null
AS
	DECLARE @type bit
	SELECT @type = type FROM Categories WHERE ID = @CategoryID
	--if amount does not match categorie's type return-1
	IF ( @Amount < 0 )
		BEGIN
			PRINT ('Amount cannot be less than zero')
			RETURN -1
		END
	IF ( @type = 0)
	BEGIN
			PRINT ('This category is not for income')
			RETURN -2
		END
	IF (@Date IS NULL) SET @Date = GETDATE()
	INSERT INTO Transactions(AccountID,Amount, Name, CategoryID,Date)
		VALUES( @AccoutnID,@Amount,@Name,@CategoryID,@Date)
GO
/****** Object:  StoredProcedure [dbo].[AddNewTrans]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewTrans] @AccoutnID int, @Name nvarchar(50), @Amount smallmoney,
							 @CategoryID int, @Date datetime2 = null
AS	
	DECLARE @type bit
	SELECT @type = type FROM Categories WHERE ID = @CategoryID
	--if amount does not match categorie's type return-1
	IF ((@type = 1 AND @Amount < 0) OR (@type = 0 AND @Amount > 0))
		RETURN -1
	IF (@Date IS NULL) SET @Date = GETDATE()
	INSERT INTO Transactions(AccountID,Amount, Name, CategoryID,Date)
		VALUES( @AccoutnID,@Amount,@Name,@CategoryID,@Date)
GO
/****** Object:  StoredProcedure [dbo].[AddNewUser]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[AddNewUser] 
	@Login nvarchar(10),
	@Password nvarchar(50),
	@FirstName nvarchar(12)=null,
	@LastName nvarchar(12)= null
AS
BEGIN

	if (not exists(select * from Users where UserName = @login))
		INSERT INTO Users(UserName,Password,FirstName,LastName)
		VALUES (@Login,@Password,@FirstName, @LastName)
	else 
		return -1
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetAccountByID]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAccountByID] @ID int
AS
	SELECT * FROM Accounts WHERE ID = @ID
	IF (@@ROWCOUNT < 1) RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[GetAllAccounts]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllAccounts] @UserID int
AS
	SELECT a.ID,a.CreateDate, a.LastModDate, b.Name + ': ' + a.Name AS 'Name', CreditInst, a.Balance
		FROM Accounts a INNER JOIN Banks b
		ON a.CreditInst = b.ID
		INNER JOIN Users u ON u.ID = b.UserID
		WHERE u.ID = @UserID
	
GO
/****** Object:  StoredProcedure [dbo].[GetAllBanks]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllBanks] @UserID int
AS
SELECT ID, Name 
	FROM Banks
	WHERE UserID = @UserID
GO
/****** Object:  StoredProcedure [dbo].[GetAllCategories]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllCategories] 
AS
	SELECT * FROM Categories

GO
/****** Object:  StoredProcedure [dbo].[GetallExpenceCats]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetallExpenceCats]

AS	
	SELECT * FROM Categories WHERE type = 0 AND ID >= 0
GO
/****** Object:  StoredProcedure [dbo].[GetAllIncomeCats]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllIncomeCats]
AS	
	SELECT * FROM Categories WHERE type = 1 AND ID >=0
GO
/****** Object:  StoredProcedure [dbo].[GetAllMainCategories]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllMainCategories] @UserID int
AS	
	SELECT * FROM Categories 
		WHERE BelongsTo IS NULL AND UserID = @UserID AND ID >=0
GO
/****** Object:  StoredProcedure [dbo].[GetAllTransByCategory]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllTransByCategory] @ID int
AS 
	-- selects all transactions by category and sub category
	SELECT * FROM Transactions 
		WHERE CategoryID = @ID OR CategoryID IN 
									(SELECT ID FROM Categories 
										WHERE BelongsTo = @ID)
GO
/****** Object:  StoredProcedure [dbo].[GetAllTransForAccountAndDate]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetAllTransForAccountAndDate] @AccountID int, @StartDate datetime2, @EndDate datetime2 = null
AS
BEGIN
IF (@EndDate IS NULL)
	SET @EndDate = GETDATE()

IF (NOT EXISTS(SELECT * FROM Accounts WHERE ID = @AccountID))
	BEGIN
		PRINT('Account with id of '+CAST(@AccountID AS VARCHAR(7)) +' does not exists.')
		RETURN -1
	END
	SELECT t.ID, t.Date,t.Amount,c.Name ,t.Name AS 'Category'
		FROM Transactions t
		INNER JOIN Categories c 
		ON t.CategoryID = c.ID
		WHERE AccountID = @AccountID AND Date BETWEEN @StartDate AND @EndDate
END
GO
/****** Object:  StoredProcedure [dbo].[GetSubCategoriesByID]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetSubCategoriesByID] @ID int 
AS
	SELECT * FROM Categories WHERE BelongsTo = @ID

GO
/****** Object:  StoredProcedure [dbo].[GetUser]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetUser] @UserName nvarchar(12), @hashedPassword nvarchar(50)
AS
BEGIN
	SET ROWCOUNT 1
	SELECT * FROM Users WHERE UserName = @UserName AND Password = @hashedPassword 
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserIDByAccountID]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[GetUserIDByAccountID] @AccountID int
AS
SELECT b.UserID  
	FROM Accounts a INNER JOIN Banks b
	ON a.CreditInst = b.ID
	WHERE a.ID = @AccountID
IF (@@ROWCOUNT =0) SELECT -1
GO
/****** Object:  StoredProcedure [dbo].[RemoveAccountByID]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[RemoveAccountByID] @ID int
AS
	ALTER TABLE Transactions NOCHECK CONSTRAINT FK_Transactions_Accounts
	DELETE Accounts WHERE ID = @ID
	ALTER TABLE Transactions CHECK CONSTRAINT FK_Transactions_Accounts
	IF (@@ROWCOUNT < 1) RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[RemoveCategoryByID]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[RemoveCategoryByID] @ID int 
AS	
	-- remove all childs first
	DELETE Categories WHERE BelongsTo = @ID 
	-- remove category
	DELETE Categories WHERE ID = @ID
GO
/****** Object:  StoredProcedure [dbo].[RemoveTransByID]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[RemoveTransByID] @ID int
AS 
BEGIN
	DECLARE @CatID int

	SELECT @CatID = CategoryID FROM Transactions
		WHERE ID = @ID

	IF (@CatID = -200)
		DELETE Transactions WHERE ID = (@ID + 1)
	IF (@CatID = -201)
		DELETE Transactions WHERE ID = (@ID -1)

	DELETE Transactions WHERE ID = @ID
END
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountByName]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[SearchAccountByName] @Name nvarchar(50)
AS
	SELECT * FROM Accounts WHERE Name like '%'+@Name+'%'
	IF (@@ROWCOUNT < 1) RETURN -1
GO
/****** Object:  StoredProcedure [dbo].[Transfer]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[Transfer] @AccountFrom int, @AccountTo int,@Name varchar(50), @Amount smallmoney, @Date datetime2
AS
BEGIN TRAN
	exec AddNewExpence @AccountFrom, @Name, @Amount,'-200',@Date
	exec AddNewIncome @AccountTo, @Name,@Amount,'-201',@Date
	IF @@ERROR > 0
		ROLLBACK 
	ELSE 
		COMMIT TRAN
	
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[ID] [int] IDENTITY(1000,1) NOT NULL,
	[CreditInst] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Balance] [smallmoney] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[LastModDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Banks]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Banks](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Name] [nvarchar](12) NOT NULL,
 CONSTRAINT [PK_CreditInst] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [bit] NOT NULL,
	[BelongsTo] [int] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Amount] [smallmoney] NOT NULL,
	[CategoryID] [int] NULL,
	[Date] [datetime2](7) NOT NULL,
	[Description] [nvarchar](100) NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transfers]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transfers](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[AccountFrom] [int] NOT NULL,
	[AccountTo] [int] NOT NULL,
	[Amount] [smallmoney] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Transfers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](10) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](12) NULL,
	[LastName] [nvarchar](12) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Accounts] ADD  CONSTRAINT [DF_Accounts_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Transactions] ADD  CONSTRAINT [DF_Transactions_Date]  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_CreditInst] FOREIGN KEY([CreditInst])
REFERENCES [dbo].[Banks] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_CreditInst]
GO
ALTER TABLE [dbo].[Banks]  WITH CHECK ADD  CONSTRAINT [FK_CreditInst_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Banks] CHECK CONSTRAINT [FK_CreditInst_Users]
GO
ALTER TABLE [dbo].[Categories]  WITH CHECK ADD  CONSTRAINT [FK_Categories_Categories] FOREIGN KEY([BelongsTo])
REFERENCES [dbo].[Categories] ([ID])
GO
ALTER TABLE [dbo].[Categories] CHECK CONSTRAINT [FK_Categories_Categories]
GO
ALTER TABLE [dbo].[Transactions]  WITH NOCHECK ADD  CONSTRAINT [FK_Transactions_Accounts] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Accounts]
GO
ALTER TABLE [dbo].[Transactions]  WITH CHECK ADD  CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([ID])
GO
ALTER TABLE [dbo].[Transactions] CHECK CONSTRAINT [FK_Transactions_Categories]
GO
ALTER TABLE [dbo].[Transfers]  WITH CHECK ADD  CONSTRAINT [FK_Transfers_Accounts] FOREIGN KEY([AccountFrom])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Transfers] CHECK CONSTRAINT [FK_Transfers_Accounts]
GO
ALTER TABLE [dbo].[Transfers]  WITH CHECK ADD  CONSTRAINT [FK_Transfers_Accounts1] FOREIGN KEY([AccountTo])
REFERENCES [dbo].[Accounts] ([ID])
GO
ALTER TABLE [dbo].[Transfers] CHECK CONSTRAINT [FK_Transfers_Accounts1]
GO
/****** Object:  Trigger [dbo].[on_acc_delete]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[on_acc_delete] ON [dbo].[Accounts]
FOR delete AS
DECLARE @AccID int
SELECT @AccID = ID FROM deleted
DELETE Transactions WHERE AccountID = @AccID
GO
/****** Object:  Trigger [dbo].[update_date]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [dbo].[update_date]
ON [dbo].[Accounts] FOR update As
	Update Accounts SET LastModDate = GETDATE() 
		WHERE id = (SELECT id FROM deleted)

GO
/****** Object:  Trigger [dbo].[on_delete_set_trans_to_null]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[on_delete_set_trans_to_null]
ON [dbo].[Categories] FOR delete AS
BEGIN 
	DECLARE @CatID int
	SELECT @CatID = ID FROM deleted
	UPDATE Transactions SET CategoryID = null WHERE CategoryID = @CatID
END

GO
/****** Object:  Trigger [dbo].[update_account]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[update_account]
ON [dbo].[Transactions] for insert
as
BEGIN

	DECLARE @Amount smallmoney, @AccountID int

	SELECT @Amount = Amount, @AccountID = AccountID FROM inserted

	UPDATE Accounts SET Balance += @Amount WHERE ID = @AccountID

END
GO
/****** Object:  Trigger [dbo].[update_account_del]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[update_account_del]

ON [dbo].[Transactions] FOR delete

AS

BEGIN

	DECLARE @Amount smallmoney, @AccountID int

	SELECT @Amount = Amount, @AccountID = AccountID FROM deleted

	UPDATE Accounts SET Balance -= @Amount WHERE ID = @AccountID

END
GO
/****** Object:  Trigger [dbo].[new_transfer]    Script Date: 5/9/2013 9:59:51 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[new_transfer] ON [dbo].[Transfers]
FOR INSERT
AS
BEGIN
	DECLARE @AccountFrom int, @AccountTo int,@Name nvarchar(12),
		@Amount smallmoney, @Date datetime2

	SELECT @AccountFrom= AccountFrom, @AccountTo = AccountTo,
		@Amount = Amount, @Date = Date, @Name = Name
		FROM inserted


	BEGIN TRAN
		exec AddNewExpence @AccountFrom, @Name, @Amount,'-200',@Date
		exec AddNewIncome @AccountTo, @Name,@Amount,'-201',@Date
	IF @@ERROR > 0
		BEGIN
			ROLLBACK
			RETURN
		END 
	ELSE 
		COMMIT TRAN
END
GO
USE [master]
GO
ALTER DATABASE [HomeBank] SET  READ_WRITE 
GO
";
    }
}
