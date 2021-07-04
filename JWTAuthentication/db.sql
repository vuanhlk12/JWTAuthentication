-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- db_a743ce_vuanhlk15.dbo.AspNetRoles definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetRoles;

CREATE TABLE AspNetRoles (
	Id nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	NormalizedName nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ConcurrencyStamp nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_AspNetRoles PRIMARY KEY (Id)
);
 CREATE  UNIQUE NONCLUSTERED INDEX RoleNameIndex ON dbo.AspNetRoles (  NormalizedName ASC  )  
	 WHERE  ([NormalizedName] IS NOT NULL)
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a743ce_vuanhlk15.dbo.AspNetUsers definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetUsers;

CREATE TABLE AspNetUsers (
	Id nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserName nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	NormalizedUserName nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Email nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	NormalizedEmail nvarchar(256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	EmailConfirmed bit NOT NULL,
	PasswordHash nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	SecurityStamp nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ConcurrencyStamp nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PhoneNumber nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PhoneNumberConfirmed bit NOT NULL,
	TwoFactorEnabled bit NOT NULL,
	LockoutEnd datetimeoffset NULL,
	LockoutEnabled bit NOT NULL,
	AccessFailedCount int NOT NULL,
	Gender nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ProfilePhoto nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	FirstName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LastName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	DateOfBirth datetime2(7) DEFAULT '0001-01-01T00:00:00.0000000' NOT NULL,
	CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id)
);
 CREATE NONCLUSTERED INDEX EmailIndex ON dbo.AspNetUsers (  NormalizedEmail ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE  UNIQUE NONCLUSTERED INDEX UserNameIndex ON dbo.AspNetUsers (  NormalizedUserName ASC  )  
	 WHERE  ([NormalizedUserName] IS NOT NULL)
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a743ce_vuanhlk15.dbo.City definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.City;

CREATE TABLE City (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CityName nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LastModify datetime2(7) NULL,
	CONSTRAINT City_PK PRIMARY KEY (ID)
);


-- db_a743ce_vuanhlk15.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.[__EFMigrationsHistory];

CREATE TABLE [__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- db_a743ce_vuanhlk15.dbo.AspNetRoleClaims definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetRoleClaims;

CREATE TABLE AspNetRoleClaims (
	Id int IDENTITY(1,1) NOT NULL,
	RoleId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ClaimType nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ClaimValue nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
	CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_AspNetRoleClaims_RoleId ON dbo.AspNetRoleClaims (  RoleId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a743ce_vuanhlk15.dbo.AspNetUserClaims definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetUserClaims;

CREATE TABLE AspNetUserClaims (
	Id int IDENTITY(1,1) NOT NULL,
	UserId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ClaimType nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ClaimValue nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_AspNetUserClaims PRIMARY KEY (Id),
	CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_AspNetUserClaims_UserId ON dbo.AspNetUserClaims (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a743ce_vuanhlk15.dbo.AspNetUserLogins definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetUserLogins;

CREATE TABLE AspNetUserLogins (
	LoginProvider nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProviderKey nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProviderDisplayName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	UserId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider,ProviderKey),
	CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_AspNetUserLogins_UserId ON dbo.AspNetUserLogins (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a743ce_vuanhlk15.dbo.AspNetUserRoles definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetUserRoles;

CREATE TABLE AspNetUserRoles (
	UserId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	RoleId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId,RoleId),
	CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE,
	CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_AspNetUserRoles_RoleId ON dbo.AspNetUserRoles (  RoleId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- db_a743ce_vuanhlk15.dbo.AspNetUserTokens definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.AspNetUserTokens;

CREATE TABLE AspNetUserTokens (
	UserId nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LoginProvider nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Value nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId,LoginProvider,Name),
	CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);


-- db_a743ce_vuanhlk15.dbo.Category definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Category;

CREATE TABLE Category (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ParentID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Priority int NULL,
	[Image] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT Category_PK PRIMARY KEY (ID),
	CONSTRAINT Category_FK FOREIGN KEY (ParentID) REFERENCES Category(ID)
);


-- db_a743ce_vuanhlk15.dbo.District definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.District;

CREATE TABLE District (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DistrictName nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CityID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	LastModify datetime2(7) NULL,
	CONSTRAINT District_PK PRIMARY KEY (ID),
	CONSTRAINT District_FK FOREIGN KEY (CityID) REFERENCES City(ID)
);


-- db_a743ce_vuanhlk15.dbo.Payment definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Payment;

CREATE TABLE Payment (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Type] nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Detail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	UserID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CardNumber nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	WalletID nvarchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT Payment_PK PRIMARY KEY (ID),
	CONSTRAINT Payment_FK FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id)
);


-- db_a743ce_vuanhlk15.dbo.Store definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Store;

CREATE TABLE Store (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Detail nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CreateTime datetime2(7) NULL,
	OwnerID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Approved int NULL,
	RatingsCount int NULL,
	FollowerCount int NULL,
	Star float NULL,
	CONSTRAINT Store_PK PRIMARY KEY (ID),
	CONSTRAINT Store_FK FOREIGN KEY (OwnerID) REFERENCES AspNetUsers(Id)
);


-- db_a743ce_vuanhlk15.dbo.Address definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Address;

CREATE TABLE Address (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Address nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Phone varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	UserID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	DistrictID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	IsDefault int NULL,
	CONSTRAINT Address_PK PRIMARY KEY (ID),
	CONSTRAINT Address_FK FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT Address_FK_1 FOREIGN KEY (DistrictID) REFERENCES District(ID)
);


-- db_a743ce_vuanhlk15.dbo.Bill definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Bill;

CREATE TABLE Bill (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BuyerID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ListItem nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Total int NOT NULL,
	OrderTime datetime2(7) NULL,
	ShipTime datetime2(7) NULL,
	Status int NOT NULL,
	AddressID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	PaymentID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ShippedProductID nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT Bill_PK PRIMARY KEY (ID),
	CONSTRAINT Bill_FK FOREIGN KEY (BuyerID) REFERENCES AspNetUsers(Id),
	CONSTRAINT Bill_FK_1 FOREIGN KEY (PaymentID) REFERENCES Payment(ID),
	CONSTRAINT Bill_FK_2 FOREIGN KEY (AddressID) REFERENCES Address(ID)
);


-- db_a743ce_vuanhlk15.dbo.[Following] definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.[Following];

CREATE TABLE [Following] (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	StoreID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	FollowTime datetime2(7) NULL,
	CONSTRAINT Following_PK PRIMARY KEY (ID),
	CONSTRAINT Following_FK FOREIGN KEY (StoreID) REFERENCES Store(ID),
	CONSTRAINT Following_FK_1 FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id)
);


-- db_a743ce_vuanhlk15.dbo.Product definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Product;

CREATE TABLE Product (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Price int NULL,
	Color nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Size] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Detail nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CategoryID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Discount int NULL,
	Quanlity int NULL,
	[Image] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	AddedTime datetime2(7) NOT NULL,
	LastModify datetime2(7) NOT NULL,
	StoreID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	SoldQuanlity int NULL,
	Star float NULL,
	RatingsCount int NULL,
	CONSTRAINT Product_PK PRIMARY KEY (ID),
	CONSTRAINT Product_FK FOREIGN KEY (StoreID) REFERENCES Store(ID),
	CONSTRAINT Product_FK_1 FOREIGN KEY (CategoryID) REFERENCES Category(ID) ON DELETE CASCADE ON UPDATE CASCADE
);


-- db_a743ce_vuanhlk15.dbo.Rating definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Rating;

CREATE TABLE Rating (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Comment nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Star int NULL,
	[Image] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Time] datetime2(7) NULL,
	ProductID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	UserID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Like] int NULL,
	CONSTRAINT Rating_PK PRIMARY KEY (ID),
	CONSTRAINT Rating_FK FOREIGN KEY (ProductID) REFERENCES Product(ID),
	CONSTRAINT Rating_FK_user FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id)
);


-- db_a743ce_vuanhlk15.dbo.UsefulRating definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.UsefulRating;

CREATE TABLE UsefulRating (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	RatingID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT UserfulRating_PK PRIMARY KEY (ID),
	CONSTRAINT UserfulRating_FK FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id),
	CONSTRAINT UserfulRating_FK_1 FOREIGN KEY (RatingID) REFERENCES Rating(ID)
);


-- db_a743ce_vuanhlk15.dbo.BillProduct definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.BillProduct;

CREATE TABLE BillProduct (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BillID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ProductID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	ProductQuantity int NULL,
	TransactionTime datetime2(7) NULL,
	StoreID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT BillProduct_PK PRIMARY KEY (ID),
	CONSTRAINT BillProduct_FK FOREIGN KEY (ProductID) REFERENCES Product(ID),
	CONSTRAINT BillProduct_FK_1 FOREIGN KEY (BillID) REFERENCES Bill(ID) ON DELETE CASCADE ON UPDATE CASCADE
);


-- db_a743ce_vuanhlk15.dbo.Cart definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Cart;

CREATE TABLE Cart (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	BuyerID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	AddedTime datetime2(7) NULL,
	Quantity int NULL,
	CONSTRAINT Cart_PK PRIMARY KEY (ID),
	CONSTRAINT Cart_FK FOREIGN KEY (BuyerID) REFERENCES AspNetUsers(Id),
	CONSTRAINT Cart_FK_1 FOREIGN KEY (ProductID) REFERENCES Product(ID) ON DELETE CASCADE ON UPDATE CASCADE
);


-- db_a743ce_vuanhlk15.dbo.Notifications definition

-- Drop table

-- DROP TABLE db_a743ce_vuanhlk15.dbo.Notifications;

CREATE TABLE Notifications (
	ID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Notification nvarchar(1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Time] datetime2(7) NOT NULL,
	Status varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CartID nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT Notifications_PK PRIMARY KEY (ID),
	CONSTRAINT Notifications_FK FOREIGN KEY (UserID) REFERENCES AspNetUsers(Id) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT Notifications_FK_1 FOREIGN KEY (CartID) REFERENCES Cart(ID)
);
