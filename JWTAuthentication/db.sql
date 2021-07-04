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

INSERT INTO db_a743ce_vuanhlk15.dbo.AspNetRoles (Id,Name,NormalizedName,ConcurrencyStamp) VALUES
	 (N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6',N'User',N'USER',N'83990c6c-4c33-4175-b7ba-535b67966a10'),
	 (N'a20604e2-ecac-4630-9e27-f7c56ec48865',N'Seller',N'SELLER',N'b3ad79c2-84d6-421b-a275-bb79cd1400c5'),
	 (N'cc419013-5d33-4f08-adf5-ff7c7bcaec72',N'Manager',N'MANAGER',N'871257e2-9131-472b-b8ab-ebc1171ee0d1'),
	 (N'd536667c-c645-4fac-9ed6-ae19f60eea11',N'Admin',N'ADMIN',N'01ed7d61-c543-43df-9ab9-8807d4d106e1');

--------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,Gender,ProfilePhoto,FirstName,LastName,DateOfBirth) VALUES
	 (N'15b816a9-1b73-45dd-8c0b-d20870fa5ec0',N'vuanhmng111',N'VUANHMNG111',N'phongcoibg99@gmail.com',N'PHONGCOIBG99@GMAIL.COM',0,N'AQAAAAEAACcQAAAAENKJCH92A0pInNx/TNbScIyT+/C06FLclmAqaSsOQTdJNio4VYNQ3gBqWKD8WupHvw==',N'YIIK66QDFEHQWITBNUDTZ2XKH3CPDE4Z',N'355382ab-7ce5-4ede-99ee-91854fe34270',N'096525621',0,0,NULL,1,0,N'M',NULL,N'Nguyễn',N'Phong','2021-06-17 00:00:00.000'),
	 (N'2e2d4362-daaa-4540-9385-5074fae26a28',N'datanh123',N'DATANH123',N'datanh235@gmail.com',N'DATANH235@GMAIL.COM',0,N'AQAAAAEAACcQAAAAECWGIH4a28a93P/IImTHbkQkTpFdx91s44Cs5kzZNqF8zZHK68IZ1u21YipPsnrASQ==',N'XAZGBONDSX5KUDODDJ3UHZF54YTKEWKB',N'02cfcbe5-8f3f-4dca-9212-e90d8330a4b2',N'0962396877',0,0,NULL,1,0,N'M',NULL,N'Dat',N'Anh 123','2021-10-07 00:00:00.000'),
	 (N'4cbb8244-b86c-4cb3-a1b0-0b56bdc9f9f7',N'datanh11',N'DATANH11',N'datanh141@gmail.com',N'DATANH141@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEJn+SgrE1Pu7VqpDmL2uqJMiFQweXxjLiAcES/q/SXm1gW1EWdEXXcusws9NB+mXKQ==',N'NGLIH7F4B3QSZGYA5RSTLE2EMF6YHMLE',N'16a022a2-8e2e-4689-a3d5-0a6879867982',N'09876543221',0,0,NULL,1,0,N'',NULL,N'Nguyễn ',N'Đạt','1999-02-03 00:00:00.000'),
	 (N'534abc92-8669-41c8-97e4-1e9a9ed98735',N'vuanhlk13',N'VUANHLK13',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEN2Jw+JjGGlyfHlZrNtiHHK5maGMYifPhgGnRw2377dk9NMZFwbQrA4+9SRpfQyhpA==',N'OVG2KA3ZET4V5JIQLPQMHIYI343LKULP',N'ac8b7f55-3fc9-4c2a-9daf-30c3f7913033',N'0394668699',0,0,NULL,1,0,N'M',NULL,N'Vũ Anh',N'Bùi','1999-10-29 00:00:00.000'),
	 (N'5bafa296-7c23-42f9-86c6-dbef6b095e92',N'vuanhtest',N'VUANHTEST',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEN20dF6S+dBzC62jcZA/Mk2d1hPpYg6VqJGoMXaoWL1Z6PkPY4b4WmBZTw/tPEJCgg==',N'DYX4VINFBKKLTOSUIIZEICIBV5U2L6PN',N'2707f48b-eeec-4588-a77c-cabb0760e23e',N'0394668699',0,0,NULL,1,0,N'M',NULL,N'Vũ Anh',N'Bùi','1999-10-29 00:00:00.000'),
	 (N'63d876b6-5ce3-4e32-80d9-2edb706aa14d',N'phongcoibg99',N'PHONGCOIBG99',N'phongcoi99@gmail.com',N'PHONGCOI99@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEIoYjwUVL/nEqh6UbvKxu2xml9kfCiHbePZ0op7ztosNn89pRPjtxqsMGfLMO0wkYg==',N'KOGSS4UQFELBQZNW76VTVDSGZLJ5M5QV',N'42487897-37a9-41ad-9dc9-fb2b28938691',N'0364431866',0,0,NULL,1,0,N'',NULL,N'Nguyễn',N'Phong','1999-09-11 00:00:00.000'),
	 (N'87660958-cf24-4b94-9eca-2cec66a711d3',N'vuanhmng',N'VUANHMNG',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEPg8XKprGVwWwwQXd0zr6J5Fvr5GPDrva3uCULBW1A8/sz36sIPicENJtnkHkjxjXQ==',N'HUGMQVBY4BSWJPAKSYVUBWLUINNAYGKT',N'de0a2dc5-d3ca-4d12-a4fe-09e7428164fd',N'0394668699',0,0,NULL,1,0,N'M',N'https://www.facebook.com/',N'Vũ Anh',N'Bùi','1999-10-29 00:00:00.000'),
	 (N'9c97a43e-a55b-4b19-a69f-e58e2153c99b',N'vuanhlk12',N'VUANHLK12',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEGRw/vXzyHjn3fm2fM3OQ+3vCG9r0fdpGg+8VX1EDSJU/sYacwZvpxUj4WMSBmewNA==',N'R3MSEHEVWPNY6ARYKR2VLKJTC6PNNBUK',N'92ed19e8-df83-42b4-a8bb-e86a1febde5f',N'2132123123',0,0,NULL,1,0,N'M',NULL,N'Anh',N'Vũ','2021-06-17 00:00:00.000'),
	 (N'9e1fb721-c2c5-4d6e-9860-f76a2ee1a22d',N'datanh230599',N'DATANH230599',N'anhndn.uet@gmail.com',N'ANHNDN.UET@GMAIL.COM',0,N'AQAAAAEAACcQAAAAELbVXILK0wCKU6KzgWmH/Rg0LUB3WueLWa6/S/qk81w6fZRgV56+NF2HcO6//jNGjA==',N'LXSRIMA2QD2ZVKERPIX4DQ2UNM4VUTCI',N'b7505fe7-2411-49c4-a635-32b61a0528f8',N'09623967877',0,0,NULL,1,0,N'',NULL,N'Nguyễn',N'Anh','1999-06-23 00:00:00.000'),
	 (N'a0a12e57-31d4-4532-8906-ec7768d4fcc3',N'vuanhtiko',N'VUANHTIKO',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEDNjOWoqgGil/OLV/rv5gOwOYNJBsjsJIfJKV6KKeCjmQ+aQWx7uC49fExemH1wmrQ==',N'CXPNU5DGT64SZXPQZOPG5VH6DZOGYQPL',N'75f37b28-1de4-45c1-b97d-2db77c423cb5',N'0394444444',0,0,NULL,1,0,N'',NULL,N'ti',N'ko','2021-06-17 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,Gender,ProfilePhoto,FirstName,LastName,DateOfBirth) VALUES
	 (N'aca06373-1efe-4da9-82eb-b66fe7185754',N'vuanhad',N'VUANHAD',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEGa2Y0OEyzfMwzOrX8c8IwCXXgTcwXlfG4Ve8wWYFxq/9Q96i0QveVo+2e5i3ZECWA==',N'7FEW4NIXGN6DNYO6S7E7TWUPE7VNRK7N',N'c2c7485f-0b15-4a2b-a789-38e6686e5411',N'0394668699',0,0,NULL,1,0,N'M',N'https://www.facebook.com/',N'Vũ Anh',N'Bùi','1999-10-29 00:00:00.000'),
	 (N'b0d517c7-e2de-4132-87a6-d1f22d4033bf',N'datanh14',N'DATANH14',N'datanh14@gmail.com',N'DATANH14@GMAIL.COM',0,N'AQAAAAEAACcQAAAAECbTuU4aPjoNvPD3uPoD2tuHgCZiM25IOSEfchj9afH2T9JiuraiyiKbOFpkJksLqg==',N'7L3NFTRDHFPTYJGXTWIHCH44ZB3IC24V',N'14e0bd59-67b5-44fe-b670-ae81c70f6c30',N'0962396877',0,0,NULL,1,0,N'M',NULL,N'Đạt Anh',N'Nguyễn','1999-05-23 00:00:00.000'),
	 (N'b371d81c-54fc-4e39-b3f3-5732997066a9',N'vuanhlk122',N'VUANHLK122',N'vuanhlk212@gmail.com',N'VUANHLK212@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEEOh7fOF+IHCTfqlRjPYo+ZCD/Y1BCjvlq62km3Pt5W/sb8bN1hmhLUuAX9a5HMFXQ==',N'KD6PKZHYC7UA4BZYCPMNE5J7DMN7YNYZ',N'8048ef4b-5e53-45a5-b8a5-2acc583eaa7e',N'153212313',0,0,NULL,1,0,N'F',NULL,N'Anh',N'Ánh','2021-06-24 00:00:00.000'),
	 (N'c74ff8d5-7862-4c67-b95c-856faf94d105',N'vuanhtikoo',N'VUANHTIKOO',N'vuanhlk12@gmail.com',N'VUANHLK12@GMAIL.COM',0,N'AQAAAAEAACcQAAAAECZD0crA9Utjb7NU3u544h8ZeH/XtRjppd3W3YsRPdPqPRoT/HLpos5wxbmZRWFTJw==',N'JMOTXYBG6UXTZLHTKHK3KFCMBXSSGCRW',N'f6050719-4d2b-4f9f-a822-3024c1756d2c',N'039444444',0,0,NULL,1,0,N'M',NULL,N'ti',N'ko','2021-06-25 00:00:00.000'),
	 (N'e8472aab-ca79-4287-b985-9e50426fd39a',N'vuanhmng112',N'VUANHMNG112',N'vuanhlk19@gmail.com',N'VUANHLK19@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEOleP7BhhkCIQHS+UcqXZgV4VShX4CT8ikPBpQX7fo0DwqmjT1FGN9iqCRmbN+tsYg==',N'34J5ALR3TFP5ZDKCJNQRGBM654QJSEVX',N'e1ecf167-707c-4751-8060-dc5813585d3a',N'039556825',0,0,NULL,1,0,N'',NULL,N'Vu',N'Anh','2021-06-23 00:00:00.000'),
	 (N'eda2a409-1694-4663-af0a-e76c03e70cd7',N'kakaka',N'KAKAKA',N'asdasd@gmail.com',N'ASDASD@GMAIL.COM',0,N'AQAAAAEAACcQAAAAEDGV3+oZXz74cj1qNsQIp4CdEdxNlVWV9bkM4G1i7FcLZGWACldecU0N9AwmMQHXNg==',N'2XWEIVF4VNUOVH2VDCTQGZFW2EH3IMUJ',N'8a83b4b8-67ef-4a25-836d-b5b454a6ac4b',N'adsda',0,0,NULL,1,0,N'F',NULL,N'dasd',N'asd','2021-06-08 00:00:00.000');
	 
-----------------------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.City (ID,CityName,LastModify) VALUES
	 (N'06cabf66-0a17-4309-8f48-9671aadb5d9c',N'Tỉnh Điện Biên','2020-04-01 00:00:00.000'),
	 (N'1aa38467-d970-403b-b084-3cabb273f166',N'Tỉnh Lai Châu','2020-04-01 00:00:00.000'),
	 (N'1c584863-69c3-440b-be15-be87dd08ccbc',N'Tỉnh Hà Giang','2020-04-01 00:00:00.000'),
	 (N'250b932c-4b12-41ce-bf8f-1f30c74359ee',N'Tỉnh Bắc Kạn','2020-04-01 00:00:00.000'),
	 (N'2c115596-d147-421f-ae7c-528aceb94375',N'Tỉnh Thái Nguyên','2020-04-01 00:00:00.000'),
	 (N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc',N'Tỉnh Tuyên Quang','2020-04-01 00:00:00.000'),
	 (N'4c2a97de-2cf4-4b7d-b22a-e63b1894f73c',N'Tỉnh Hoà Bình','2020-04-01 00:00:00.000'),
	 (N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1',N'Tỉnh Lào Cai','2020-04-01 00:00:00.000'),
	 (N'8c39dfdf-e8a4-4132-bb55-1aadaf8e531b',N'Tỉnh Yên Bái','2020-04-01 00:00:00.000'),
	 (N'930b3538-3e14-4690-8522-19080a57ebae',N'Thành phố Hà Nội','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.City (ID,CityName,LastModify) VALUES
	 (N'94ac63f5-e233-4357-953b-dd38180d5580',N'Tỉnh Sơn La','2020-04-01 00:00:00.000'),
	 (N'9ba47634-23a9-46c6-8172-d11e2727fea6',N'Tỉnh Cao Bằng','2020-04-01 00:00:00.000'),
	 (N'd7ab7895-6e56-4599-9ac2-16f59867f350',N'Tỉnh Lạng Sơn','2020-04-01 00:00:00.000');
---------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.AspNetUserRoles (UserId,RoleId) VALUES
	 (N'15b816a9-1b73-45dd-8c0b-d20870fa5ec0',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'2e2d4362-daaa-4540-9385-5074fae26a28',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'4cbb8244-b86c-4cb3-a1b0-0b56bdc9f9f7',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'534abc92-8669-41c8-97e4-1e9a9ed98735',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'5bafa296-7c23-42f9-86c6-dbef6b095e92',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'63d876b6-5ce3-4e32-80d9-2edb706aa14d',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'9c97a43e-a55b-4b19-a69f-e58e2153c99b',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'9e1fb721-c2c5-4d6e-9860-f76a2ee1a22d',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'a0a12e57-31d4-4532-8906-ec7768d4fcc3',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'b0d517c7-e2de-4132-87a6-d1f22d4033bf',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6');
INSERT INTO db_a743ce_vuanhlk15.dbo.AspNetUserRoles (UserId,RoleId) VALUES
	 (N'b371d81c-54fc-4e39-b3f3-5732997066a9',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'c74ff8d5-7862-4c67-b95c-856faf94d105',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'e8472aab-ca79-4287-b985-9e50426fd39a',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'eda2a409-1694-4663-af0a-e76c03e70cd7',N'7f99c69c-fcb7-46f0-b6ef-d30042b376e6'),
	 (N'4cbb8244-b86c-4cb3-a1b0-0b56bdc9f9f7',N'a20604e2-ecac-4630-9e27-f7c56ec48865'),
	 (N'534abc92-8669-41c8-97e4-1e9a9ed98735',N'a20604e2-ecac-4630-9e27-f7c56ec48865'),
	 (N'87660958-cf24-4b94-9eca-2cec66a711d3',N'a20604e2-ecac-4630-9e27-f7c56ec48865'),
	 (N'9e1fb721-c2c5-4d6e-9860-f76a2ee1a22d',N'a20604e2-ecac-4630-9e27-f7c56ec48865'),
	 (N'a0a12e57-31d4-4532-8906-ec7768d4fcc3',N'a20604e2-ecac-4630-9e27-f7c56ec48865'),
	 (N'87660958-cf24-4b94-9eca-2cec66a711d3',N'cc419013-5d33-4f08-adf5-ff7c7bcaec72');
INSERT INTO db_a743ce_vuanhlk15.dbo.AspNetUserRoles (UserId,RoleId) VALUES
	 (N'aca06373-1efe-4da9-82eb-b66fe7185754',N'd536667c-c645-4fac-9ed6-ae19f60eea11');
---------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.Category (ID,ParentID,Name,Priority,[Image]) VALUES
	 (N'00319ee8-4785-434e-8845-d7a7f1033c37',N'5293cbf6-8259-4c1b-97f8-dc92ddda3001',N'Tai nghe chụp tai',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Funnamed.jpg?alt=media&token=80cf1b22-d1c6-4ce9-9ed3-9aec99068af3'),
	 (N'0823c828-165a-4f6d-8d24-92fc3982f2d7',N'3870e5f9-61f1-48c9-ab98-c8aeb34464f7',N'Thiết bị nhà thông minh',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fthiet-bi-dien-thong-minh-lumi.jpg?alt=media&token=08c63835-a306-4e36-b14c-e28a98f32e07'),
	 (N'103e3ddc-5170-425e-a9b2-7ef184016a93',N'3fba6643-fac5-47f6-9093-3898aac0d2fb',N'Tủ lạnh',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fpanasonic-nr-bl340gavn-2-org.jpg?alt=media&token=63108916-8f17-44ed-b54e-6f00a69da25f'),
	 (N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'6d7922b1-6a78-44b9-8d08-332f4d429238',N'Thiết bị game',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2F4ee8cca2064707a3631699daf1244348.jpg?alt=media&token=95fd9ea7-02d2-487c-8362-01cc766a7154'),
	 (N'24e01437-677a-418f-a48b-ec705edf2406',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Đồng hồ gaming',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2F40843_forerunner_745_black_ha1.jpg?alt=media&token=9fd117ce-cf2c-4aac-9025-6bb6fb8eb606'),
	 (N'2b4717d6-592f-4269-b975-933343e86615',N'5293cbf6-8259-4c1b-97f8-dc92ddda3001',N'Tai nghe nhét tai',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Ftai-nghe-nhet-tai-samsung-galaxy-s4-co-microphone_40721_5.jpg?alt=media&token=ece75ec1-1e11-4a30-b5b6-2ca9baa7b923'),
	 (N'2cf3e7ad-dce1-4582-b68b-8c48fc9d9174',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Lót chuột game',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Frazer.jpg?alt=media&token=7f689c2d-c4fd-4f75-b185-8423ddf9a284'),
	 (N'36a02659-4d4b-433d-af64-1bb9fb602b66',N'3fba6643-fac5-47f6-9093-3898aac0d2fb',N'Máy giặt',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fdownload.jpg?alt=media&token=70d86d59-c27f-427b-af40-47d94cda5810'),
	 (N'3834f537-601b-4894-bea6-8f24e89c169c',N'3870e5f9-61f1-48c9-ab98-c8aeb34464f7',N'Đồng hồ thông minh',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fdong-ho-thong-minh.jpg?alt=media&token=5fb9ddbd-9a47-4b38-977d-16a1fe2349af'),
	 (N'3870e5f9-61f1-48c9-ab98-c8aeb34464f7',N'6d7922b1-6a78-44b9-8d08-332f4d429238',N'Thiết bị đeo thông minh',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fbansao01galaxywatchactive2cfit2cbuds_onuj.jpg?alt=media&token=497ddf70-990d-4e86-87c3-102f0f2ca1be');
INSERT INTO db_a743ce_vuanhlk15.dbo.Category (ID,ParentID,Name,Priority,[Image]) VALUES
	 (N'39ba1a02-4a92-459d-8be1-e304a1fae073',N'b82de6ac-6f76-4dcf-8dc7-1940bfce3339',N'Dell',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Ffc78304d937382dd87e5e1385c665172.jpg?alt=media&token=658e4454-70a6-479b-8a3b-0e9b034ec871'),
	 (N'3bfefaae-f37c-42ac-922d-dfa7ae0f6cc5',N'3870e5f9-61f1-48c9-ab98-c8aeb34464f7',N'Kính thực tế ảo',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2FUntitled-1_o344-hx.jpg?alt=media&token=444c1f18-e000-48c5-839c-7dd764224aed'),
	 (N'3da05cb0-c746-438a-9061-22881a5cb7f9',N'6706b9be-562d-457f-b472-e15f93392a0e',N'Loa Bluetooth',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fbluetoth-jbl-charge-4-den-1-org.jpg?alt=media&token=e7712b56-bf7b-4ed5-a848-adb184039670'),
	 (N'3fba6643-fac5-47f6-9093-3898aac0d2fb',NULL,N'Điện tử - Điện lạnh',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fsua-chua-dien-tu-hai-phong-11.jpg?alt=media&token=f9b6c4da-b4db-4270-bed7-d5e6d9418e54'),
	 (N'412393c1-1bf8-4e68-aa52-73c34b80f9eb',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Tay bấm game',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2F57027_tay_cam_choi_game_sony_ps5_dualsense_0003_4.jpg?alt=media&token=f4d2d93e-8556-421c-97f7-5525b81b9a7c'),
	 (N'41fd2f63-91b3-4f40-be7d-dfd5e106cc70',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Đĩa game',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Ff2f5bf53df74f16e07de0e4eca7e444a_tn.jpg?alt=media&token=b9b8fdf7-9aed-4ef4-abfa-f6aa5f280ec1'),
	 (N'47a6113d-c937-4f75-a804-40beecdb0ab5',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Sim số, 3G/4G',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fdownload.png?alt=media&token=2f2ab447-e2b9-467a-88d9-bd70545a3e59'),
	 (N'4d309a66-5958-455f-94c2-3223cc12c615',N'e044a2a8-3c3f-4c02-9ed6-ae7a4aac7a89',N'Smartphone',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fsmartphone-o-viet-nam_800x450.jpg?alt=media&token=2d8c2159-38c5-4393-af12-d4764efa9d4d'),
	 (N'4e166f05-133c-4301-b86a-2e66cb8aec70',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Tai nghe gaming',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Fdownload%20(1).jpg?alt=media&token=478e87d7-0681-4a36-8615-2b663e03b9d5'),
	 (N'5293cbf6-8259-4c1b-97f8-dc92ddda3001',N'62ec913c-d06a-422c-b631-c6dc29cbc852',N'Tai nghe',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Ftai-nghe-bluetooth-true-wireless-qcy-t7__10_.jpg?alt=media&token=3959f87e-e7a2-4bb9-9df9-db9a338be8e4');
INSERT INTO db_a743ce_vuanhlk15.dbo.Category (ID,ParentID,Name,Priority,[Image]) VALUES
	 (N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'6d7922b1-6a78-44b9-8d08-332f4d429238',N'Phụ kiện điện thoại',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2F2(9).png?alt=media&token=c099cfd5-992e-4f1b-9b07-5deaf50ab869'),
	 (N'62ec913c-d06a-422c-b631-c6dc29cbc852',N'6d7922b1-6a78-44b9-8d08-332f4d429238',N'Thiết bị âm thanh',1,N''),
	 (N'64ffb2b0-bd91-4a14-9eae-5322d1b827f1',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Pin sạc dự phòng',2,N''),
	 (N'6706b9be-562d-457f-b472-e15f93392a0e',N'62ec913c-d06a-422c-b631-c6dc29cbc852',N'Loa',2,N''),
	 (N'678f9b53-3164-46cc-8985-d6d7bdf5fab9',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Gậy chụp hình',2,N''),
	 (N'6c7bbb06-6794-42ea-b1a8-64c2816fc6ce',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Miếng dán màn hình',2,N''),
	 (N'6d7922b1-6a78-44b9-8d08-332f4d429238',NULL,N'Phụ kiện - thiết bị số',0,N''),
	 (N'6d92fc71-dfe7-4909-9c05-a5e3f0a32a94',N'3fba6643-fac5-47f6-9093-3898aac0d2fb',N'Điều hòa',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2Ff82a7ce86c8b49bfe9797bfc0adf85cc.jpg?alt=media&token=43398b6c-c2db-49ff-a47e-ecbb605eba9c'),
	 (N'71b43f31-30ce-40f8-9d09-7cfb8276c7eb',N'e044a2a8-3c3f-4c02-9ed6-ae7a4aac7a89',N'Máy tính bảng',1,N''),
	 (N'763d432e-efcb-4baf-a43b-c4b2d536ebb2',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Ghế gaming',2,N'');
INSERT INTO db_a743ce_vuanhlk15.dbo.Category (ID,ParentID,Name,Priority,[Image]) VALUES
	 (N'7bbd7a18-fd7e-44b2-bcdc-fc65f77c5ac5',N'e044a2a8-3c3f-4c02-9ed6-ae7a4aac7a89',N'Phụ kiện điện thoại',1,N''),
	 (N'7d7ced46-3250-4d8b-bb37-f91425f26f53',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Màn hình gaming',2,N''),
	 (N'7f42eb0c-f1f5-47f7-9b2c-9e042a0f971a',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Bàn phím gaming',2,N''),
	 (N'81ea822f-d024-4c6d-b9f6-58e7f5948aa0',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Bao da- Ốp lưng',2,N''),
	 (N'82608b78-d62e-4384-92df-5fcaa43c963f',N'6706b9be-562d-457f-b472-e15f93392a0e',N'Phụ kiện âm thanh',3,N''),
	 (N'8e61d119-aaf9-4188-94d5-2bf3b8563c31',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Adapter - Củ sạc',2,N''),
	 (N'8f39fb00-c302-4ef8-8f8c-b0f0c9b74027',N'e044a2a8-3c3f-4c02-9ed6-ae7a4aac7a89',N'Máy đọc sách',1,N''),
	 (N'8fccfad4-4483-4c7e-9374-ac92280f9a20',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Chuột gaming',2,N''),
	 (N'b82de6ac-6f76-4dcf-8dc7-1940bfce3339',NULL,N'Laptop - Thiết bị IT',0,N''),
	 (N'c41aefa0-853c-4ad5-98ef-2e6cba9e918f',N'6706b9be-562d-457f-b472-e15f93392a0e',N'Máy nghe nhạc - Máy ghi âm',3,N'');
INSERT INTO db_a743ce_vuanhlk15.dbo.Category (ID,ParentID,Name,Priority,[Image]) VALUES
	 (N'ce32e17c-d2ad-4c46-b2d3-bd44bf4a0288',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Đế sạc không dây',2,N''),
	 (N'd77cfd81-f45c-456f-befc-6365826cd732',N'6706b9be-562d-457f-b472-e15f93392a0e',N'Loa Vi Tính',3,N''),
	 (N'e044a2a8-3c3f-4c02-9ed6-ae7a4aac7a89',NULL,N'Điện thoại - Máy tính bảng',0,N''),
	 (N'e2a5751a-396b-4b43-90ae-90891d07c75d',N'3fba6643-fac5-47f6-9093-3898aac0d2fb',N'Tivi',0,N'https://firebasestorage.googleapis.com/v0/b/duanimage-50853.appspot.com/o/images%2F9-4.jpg?alt=media&token=d0f85a36-6dcf-4623-8f1f-dddb08a15087'),
	 (N'e73a7dc3-6bb3-40d6-a8e1-37bb9811096b',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Dây cáp sạc',2,N''),
	 (N'eb4eca7b-53e6-4bf8-992b-81ff18860ea4',N'553be93a-fecd-44a6-99ea-cc4b0a14a1ad',N'Thẻ nhớ điện thoại',2,N''),
	 (N'fd3062e6-fb50-4c74-bae6-e4f086fcaa71',N'5293cbf6-8259-4c1b-97f8-dc92ddda3001',N'Tai nghe Bluetooth',3,N''),
	 (N'fe62e309-3f7f-49b1-a804-656f4d27fa25',N'1a8ac6cf-7637-4850-8447-06fcd3fd697f',N'Máy chơi game',2,N'');
--------------------------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'034c7615-4c8c-4546-a972-9c1279906919',N'Huyện Bảo Lâm',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'07b5169f-aff5-48db-9899-421fbab00227',N'Thị Xã Mường Lay',N'06cabf66-0a17-4309-8f48-9671aadb5d9c','2020-04-01 00:00:00.000'),
	 (N'07c6c45f-a1cf-4c5e-b9d6-e229ae5d2438',N'Huyện Sìn Hồ',N'1aa38467-d970-403b-b084-3cabb273f166','2020-04-01 00:00:00.000'),
	 (N'08553de6-0849-41b9-9862-e33728942dee',N'Huyện Văn Quan',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'08cc6437-5b30-49f2-94e1-c5169741f53d',N'Huyện Đà Bắc',N'4c2a97de-2cf4-4b7d-b22a-e63b1894f73c','2020-04-01 00:00:00.000'),
	 (N'0a16de60-299e-4a79-9180-3d253dc9bbb0',N'Huyện Ba Vì',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'0ef7f035-0b70-4e9c-b8c8-27392a530e5c',N'Thành phố Sông Công',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'1094419d-814e-4d8e-b4d3-0b1e675226c1',N'Thành phố Lạng Sơn',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'13ea126e-289a-4d89-b87f-9bc47d4ed066',N'Huyện Chi Lăng',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'15abf600-731e-4ae6-99ea-1478d0b8ce95',N'Huyện Phú Xuyên',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'18aedc51-314d-45f0-9739-e0ea8c43ac3b',N'Huyện Kim Bôi',N'4c2a97de-2cf4-4b7d-b22a-e63b1894f73c','2020-04-01 00:00:00.000'),
	 (N'1aa43f1f-9cb5-45d1-b7e2-9449025ff794',N'Huyện Hà Quảng',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'1cbc129e-de07-4923-b7b3-009b51094af9',N'Huyện Ba Bể',N'250b932c-4b12-41ce-bf8f-1f30c74359ee','2020-04-01 00:00:00.000'),
	 (N'1ce23982-dddb-4ae8-8852-0f268fa6316c',N'Huyện Ngân Sơn',N'250b932c-4b12-41ce-bf8f-1f30c74359ee','2020-04-01 00:00:00.000'),
	 (N'1ecf4f89-0b21-475d-b450-5202c983a155',N'Huyện Bình Gia',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'22e770eb-02d9-4d86-bef7-a66d18982411',N'Huyện Bảo Thắng',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'25bf98b1-24af-4076-81ce-8c44e3763fa0',N'Huyện Lâm Bình',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'25c43d95-a991-48df-82b1-01cc218b4232',N'Huyện Thạch An',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'28b6dbf2-f3a2-4e6c-8886-450701f092ba',N'Quận Hoàn Kiếm',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'28f297fb-6abd-4841-aa0b-81da0481575f',N'Huyện Mù Căng Chải',N'8c39dfdf-e8a4-4132-bb55-1aadaf8e531b','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'2a9c5d22-ca75-45c0-8de5-824ff8e14850',N'Huyện Phong Thổ',N'1aa38467-d970-403b-b084-3cabb273f166','2020-04-01 00:00:00.000'),
	 (N'2cc0876d-f001-4cd9-9a08-8d4507592746',N'Huyện Yên Minh',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'2ef131d8-1034-4c84-98b2-77761929d193',N'Huyện Phú Bình',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'2fd0afbe-7704-41ea-8dd7-b485a2beec0b',N'Thành phố Lai Châu',N'1aa38467-d970-403b-b084-3cabb273f166','2020-04-01 00:00:00.000'),
	 (N'324b09fd-f963-419b-ad9c-7d546e62ab92',N'Huyện Yên Sơn',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'35a2a016-4f2e-4e0e-bb47-d546b9504b59',N'Huyện Chương Mỹ',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'3993bb59-7b61-46dd-b7d2-17979bf616e9',N'Thành Phố Bắc Kạn',N'250b932c-4b12-41ce-bf8f-1f30c74359ee','2020-04-01 00:00:00.000'),
	 (N'39956f7c-4b13-4cb5-8d41-115b85fadfc7',N'Huyện Mường La',N'94ac63f5-e233-4357-953b-dd38180d5580','2020-04-01 00:00:00.000'),
	 (N'3abfc601-65b9-4302-844f-857491f5b6aa',N'Huyện Sóc Sơn',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'3bd0ada9-7e42-4ec9-9000-83e6edf8259d',N'Huyện Quảng Uyên',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'3d18dfca-3605-4c54-b4e8-2d05a023fdc9',N'Huyện Bát Xát',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'3d8b016b-ba29-40e9-b0f1-34081450d1b6',N'Huyện Lương Sơn',N'4c2a97de-2cf4-4b7d-b22a-e63b1894f73c','2020-04-01 00:00:00.000'),
	 (N'40aa5247-64c7-49a2-9e0e-dfa58db86098',N'Huyện Nà Hang',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'4242382f-78a9-4da9-9e01-126d060d947f',N'Huyện Bắc Yên',N'94ac63f5-e233-4357-953b-dd38180d5580','2020-04-01 00:00:00.000'),
	 (N'44d62dda-a0c2-437d-ad16-98087e0d5dd1',N'Huyện Đông Anh',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'44faae9b-743e-4939-ba5e-1474706b647f',N'Huyện Nguyên Bình',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'4506fcec-0291-4f6b-9bcc-eeb639bec4ab',N'Huyện Văn Bàn',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'45813d86-45c2-4975-8a6e-ff2eae75d6c5',N'Huyện Văn Lãng',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'48ad077a-4be6-4a82-a7c4-9608e4b15dd5',N'Huyện Trà Lĩnh',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'48bf4620-5700-4641-9525-00227bee9ae8',N'Huyện Đồng Văn',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'49ee0d42-f9d0-41af-ac9d-5e2d739743ac',N'Huyện Bắc Hà',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'4a6e98d3-c0a7-4d5c-9208-c827dc0e09eb',N'Huyện Bảo Yên',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'4ab1c7ae-3807-4bb5-beee-00e630c141f5',N'Huyện Si Ma Cai',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'4b1e24e9-8b47-4079-9b2f-da0fa8517b28',N'Huyện Sơn Dương',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'4d1d1e16-8209-42f3-9478-14850fdefcc1',N'Huyện Mèo Vạc',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'4dbc516b-34a0-4c1f-a422-84707ff5accc',N'Thành phố Hòa Bình',N'4c2a97de-2cf4-4b7d-b22a-e63b1894f73c','2020-04-01 00:00:00.000'),
	 (N'514c72b5-0eaf-4d28-8b7e-622bee7da000',N'Huyện Bắc Sơn',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'52e68d84-b4dc-4a0a-b64d-3c37f251aaa6',N'Thành phố Lào Cai',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'55f450c6-1a54-42b5-b33c-cd7f75a83f9c',N'Huyện Thuận Châu',N'94ac63f5-e233-4357-953b-dd38180d5580','2020-04-01 00:00:00.000'),
	 (N'5621bd4e-077a-44a1-a541-920e1228bb25',N'Huyện Phúc Thọ',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'58547dbc-a2dd-49f7-a8a4-b73d6179f0c7',N'Huyện Mù Căng Chải',N'8c39dfdf-e8a4-4132-bb55-1aadaf8e531b','2020-04-01 00:00:00.000'),
	 (N'5a448513-9ca4-4696-ab9a-9e729d15c4b5',N'Thị xã Sơn Tây',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'5e805016-f33f-49ed-aa60-1b91fa89cbf9',N'Huyện Mường Khương',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000'),
	 (N'5f256ed9-ed3e-4428-8d1e-0d1a3a6dd3ee',N'Quận Hà Đông',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'616989a0-7a32-40b1-a73c-9b535d82ac84',N'Huyện Tam Đường',N'1aa38467-d970-403b-b084-3cabb273f166','2020-04-01 00:00:00.000'),
	 (N'619d6fa9-931b-4026-a6ab-a3c361c86066',N'Quận Hai Bà Trưng',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'62ee6e1b-c300-4275-ba6c-9c78d2d02b41',N'Huyện Đại Từ',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'68358a5b-9a46-4f8c-9caf-5e151a21f660',N'Thành phố Hà Giang',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'688c269e-f789-420e-a39b-6b23d0f7b120',N'Thành phố Thái Nguyên',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'6943fbe4-59bd-4365-9216-aacfc9e35f2e',N'Quận Hoàng Mai',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'6a2ef814-66f1-4b47-b989-f2e57657abe7',N'Huyện Chiêm Hóa',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'6ba89604-0129-4e5a-b679-d1cf36e2974e',N'Quận Ba Đình',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'74d2aaa6-a7e1-4930-881c-2905a71903a8',N'Huyện Lục Yên',N'8c39dfdf-e8a4-4132-bb55-1aadaf8e531b','2020-04-01 00:00:00.000'),
	 (N'78dfb396-313f-4068-a5e1-faaa5aa4be65',N'Huyện Mỹ Đức',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'7a07d8dd-6534-42f9-912d-d3251e88be0c',N'Huyện Đồng Hỷ',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'7e033c3c-db3a-4560-b43c-39e520ca9f17',N'Huyện Mường Tè',N'1aa38467-d970-403b-b084-3cabb273f166','2020-04-01 00:00:00.000'),
	 (N'7e94666e-0154-4d98-8191-dc86476649f8',N'Huyện Quốc Oai',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'86af3cbc-a343-4de6-acb1-4a708c138f4b',N'Huyện Mê Linh',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'897c36fe-da90-4ef0-bd25-6070221ea523',N'Huyện Hàm Yên',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'899ea91f-aaba-45fe-adbe-7b0068be1e6d',N'Huyện Văn Bàn',N'6d4fa66a-0218-479c-b7d8-b3c4970b21e1','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'8a7127d1-4825-4e2b-b29f-69433e3a55dd',N'Quận Bắc Từ Liêm',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'8d74a79b-9a47-4dfd-b374-412063becdce',N'Quận Nam Từ Liêm',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'8df46526-3801-4082-a34d-4950404fa000',N'Thành phố Tuyên Quang',N'2ce9ef0f-6718-4922-bf2e-b025b6b9b4fc','2020-04-01 00:00:00.000'),
	 (N'9030267b-af6e-44a0-ab74-b9714deee94e',N'Huyện Võ Nhai',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'9a919cf0-395c-4848-8aed-5e5af98caff6',N'Thành phố Yên Bái',N'8c39dfdf-e8a4-4132-bb55-1aadaf8e531b','2020-04-01 00:00:00.000'),
	 (N'9b4024e4-546c-4eae-bc11-fcb3f4917e0a',N'Huyện Hạ Lang',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'a2dde9d1-84d0-43c1-a6d3-7dfdba532332',N'Huyện Bạch Thông',N'250b932c-4b12-41ce-bf8f-1f30c74359ee','2020-04-01 00:00:00.000'),
	 (N'a668500b-2bdc-4914-a100-e5657db0858a',N'Huyện Trùng Khánh',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'a77f3d30-0e2f-4ef3-b5ec-4302e3ab210d',N'Huyện Quang Bình',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'aa82fde1-9ee7-4e04-8b7f-2384e5310a86',N'Huyện Thanh Trì',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'ab244c62-d87f-423b-b1e6-b1983026ece5',N'Huyện Thanh Oai',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'ad7438b7-7593-46d4-9c7b-99f32734fba2',N'Huyện Đan Phượng',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'ada312ee-77df-4eec-831f-0f297cae56da',N'Huyện Quỳnh Nhai',N'94ac63f5-e233-4357-953b-dd38180d5580','2020-04-01 00:00:00.000'),
	 (N'b14ccd35-5d7c-436b-ad59-b1f847c7c485',N'Quận Thanh Xuân',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'b43dcbad-11e5-4b90-a324-2cdc1859e5e3',N'Huyện Tủa Chùa',N'06cabf66-0a17-4309-8f48-9671aadb5d9c','2020-04-01 00:00:00.000'),
	 (N'b75d51eb-16d3-4745-9521-848b984df88f',N'Huyện Thông Nông',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'b9303818-c47e-4a83-a241-6495887bf07f',N'Huyện Phú Lương',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'ba1a41a7-3311-4b4e-91ed-e6bdbdc42a73',N'Huyện Lộc Bình',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'bab18d4a-ecbc-445c-aeee-ed90ce9d8ef0',N'Huyện Gia Lâm',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'bad1dd22-a847-4674-b706-09fe63558ba7',N'Huyện Kỳ Sơn',N'4c2a97de-2cf4-4b7d-b22a-e63b1894f73c','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'bc3e4823-6d02-4849-8025-f9a46d4db0ff',N'Thành phố Cao Bằng',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'bd103512-0489-47f2-bacb-57cab0812e94',N'Huyện Nguyên Bình',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'bf84af8a-0a80-47a2-bf8d-f164a9c6cca2',N'Huyện Bảo Lạc',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'c050e195-165a-477e-a712-18db6daa3763',N'Quận Đống Đa',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'c2bb790b-a45b-4600-9f48-2622f21b88ba',N'Huyện Hoàng Su Phì',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'c9701b0d-a70d-4d32-af0e-40ef76ab3201',N'Thành phố Sơn La',N'94ac63f5-e233-4357-953b-dd38180d5580','2020-04-01 00:00:00.000'),
	 (N'cab3df97-e46a-47d8-b296-b60247206536',N'Huyện Hoài Đức',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'ce4282c4-1432-4eb8-8560-6abdfe1358ba',N'Huyện Pác Nặm',N'250b932c-4b12-41ce-bf8f-1f30c74359ee','2020-04-01 00:00:00.000'),
	 (N'd103efd7-efe6-4ca5-adc3-2ec75d5a55be',N'Huyện Hữu Lũng',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'd2f7d804-fd60-426b-a495-000eaf700c36',N'Huyện Vị Xuyên',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'd3aca136-b7df-4f20-9413-813aef1a7250',N'Huyện Tràng Định',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'd48cca43-7e48-4377-b399-c6b40a17766b',N'Thị xã Nghĩa Lộ',N'8c39dfdf-e8a4-4132-bb55-1aadaf8e531b','2020-04-01 00:00:00.000'),
	 (N'd6da73a5-fed9-4a09-b3a2-654aeeb2b673',N'Thành phố Điện Biên Phủ',N'06cabf66-0a17-4309-8f48-9671aadb5d9c','2020-04-01 00:00:00.000'),
	 (N'd92e782f-bf13-4f55-b49a-512eb6faf390',N'Huyện Đình Lập',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'ddbe87df-0fbb-4c27-af0d-d5a8d35f4bb4',N'Quận Tây Hồ',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'ded826b4-e1e6-4bbd-98f0-a8b2f3faa9d7',N'Huyện Thạch Thất',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'e001dd74-696f-4a2f-bb05-cb63cd2620ad',N'Huyện Bắc Mê',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'e0ace982-312b-4a45-b73e-839eafbddf3c',N'Huyện Mường Nhé',N'06cabf66-0a17-4309-8f48-9671aadb5d9c','2020-04-01 00:00:00.000'),
	 (N'e1882a12-0fbd-4c7e-ae81-3ccae3af7605',N'Huyện Bắc Quang',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'e5b3a439-cf8e-44ca-8d66-ae0d9890c8bc',N'Huyện Xín Mần',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000');
INSERT INTO db_a743ce_vuanhlk15.dbo.District (ID,DistrictName,CityID,LastModify) VALUES
	 (N'e62a68d3-d625-4e02-98cf-7c1448939f14',N'Quận Long Biên',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'e802c132-2c4f-49bb-acf2-b1361411fc93',N'Huyện Quản Bạ',N'1c584863-69c3-440b-be15-be87dd08ccbc','2020-04-01 00:00:00.000'),
	 (N'edd36ad1-5c2c-4248-91d4-3eacad7a5d46',N'Huyện Ứng Hòa',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'ee371fcd-2727-45e9-a445-f4e0e18b8039',N'Huyện Phục Hoà',N'9ba47634-23a9-46c6-8172-d11e2727fea6','2020-04-01 00:00:00.000'),
	 (N'ef309775-6b65-44ba-ae45-3e57b58671d2',N'Thị xã Phổ Yên',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'ef5d9d64-7de8-4eee-8b57-865ffc98050f',N'Huyện Cao Lộc',N'd7ab7895-6e56-4599-9ac2-16f59867f350','2020-04-01 00:00:00.000'),
	 (N'f2953df8-4df7-467f-ae17-ec2a60c51ae0',N'Quận Cầu Giấy',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000'),
	 (N'f36cf328-4695-48f1-99b2-ae2cba24c0f3',N'Huyện Tủa Chùa',N'06cabf66-0a17-4309-8f48-9671aadb5d9c','2020-04-01 00:00:00.000'),
	 (N'fc05eae1-79f8-4a45-805d-17a68eef302d',N'Huyện Định Hóa',N'2c115596-d147-421f-ae7c-528aceb94375','2020-04-01 00:00:00.000'),
	 (N'ff2cec0b-7e6a-499c-a137-887506133f1c',N'Huyện Thường Tín',N'930b3538-3e14-4690-8522-19080a57ebae','2020-04-01 00:00:00.000');
------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.Payment (ID,[Type],Detail,UserID,CardNumber,WalletID) VALUES
	 (N'88888888-8888-8888-8888-888888888888',N'Thanh toán COD',N'Thanh toán khi nhận hàng',N'aca06373-1efe-4da9-82eb-b66fe7185754',NULL,NULL),
	 (N'888dac37-66d1-4df9-a01f-4ed0c7803192',N'Wallet',N'Ví MoMo',N'2e2d4362-daaa-4540-9385-5074fae26a28',N' ',N'123123123'),
	 (N'97f9e531-e81c-4737-a17d-7809b78a181e',N'Banking',N'Ngan hang BIDV',N'9c97a43e-a55b-4b19-a69f-e58e2153c99b',N'5435354235',N''),
	 (N'da227ac5-95e6-416f-844a-97f42399922c',N'Wallet',N'Ví MoMo',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'',N'243413123342343'),
	 (N'da227ac5-95e6-416f-844a-97f42399952c',N'Wallet',N'Ví MoMo',N'9c97a43e-a55b-4b19-a69f-e58e2153c99b',N'',N'24342342343');
------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.Store (ID,Name,Detail,CreateTime,OwnerID,Approved,RatingsCount,FollowerCount,Star) VALUES
	 (N'336be9a9-b9b2-4511-a942-08b20d244e79',N'VA Store',N'Cửa hàng của Vũ Anh 5 sao ok','2021-04-06 00:00:00.000',N'87660958-cf24-4b94-9eca-2cec66a711d3',1,15,3,4.41),
	 (N'336be9a9-b9b2-4811-a942-08b209244e79',N'QB Store',N'Binh''s Store','2021-04-06 00:00:00.000',N'b371d81c-54fc-4e39-b3f3-5732997066a9',1,0,2,0.0),
	 (N'72e8c348-ed4a-4024-8b79-158299c5c650',N' Mai Chi',N'Vợ Đạt Anh','2021-06-08 10:04:23.000',N'9e1fb721-c2c5-4d6e-9860-f76a2ee1a22d',1,0,0,0.0),
	 (N'756beya9-79b2-4001-a942-08b209244e79',N'Admin Store',N'Cửa hàng của admin','2021-04-06 00:00:00.000',N'aca06373-1efe-4da9-82eb-b66fe7185754',1,1,4,5.0),
	 (N'9ff715ef-ce43-4054-a371-41b427ee5e94',N'VA Store',N'Cửa hàng của Vũ Anh lk13','2021-05-31 14:39:15.000',N'534abc92-8669-41c8-97e4-1e9a9ed98735',1,0,0,0.0),
	 (N'a1ac9793-c766-40f6-b0ca-b6b31a94a9f4',N' Mai Chi1',N'Vợ Đạt Anh','2021-06-08 10:08:01.000',N'4cbb8244-b86c-4cb3-a1b0-0b56bdc9f9f7',1,0,0,0.0),
	 (N'e1f8f1ed-42c4-43ea-bc76-73fbcad26896',N'VA Store test',N'Cửa hàng của Vũ Anh lk13','2021-06-08 11:05:50.000',N'5bafa296-7c23-42f9-86c6-dbef6b095e92',2,0,0,0.0),
	 (N'e4a32ddd-35fd-4a93-a477-0e9cf515febd',N'Vũ Anh tiki test',N'123','2021-06-11 09:40:04.000',N'a0a12e57-31d4-4532-8906-ec7768d4fcc3',1,0,0,0.0);
----------------------------
INSERT INTO db_a743ce_vuanhlk15.dbo.Address (ID,Address,Phone,UserID,DistrictID,IsDefault) VALUES
	 (N'040e521a-2d69-4271-81f3-074e42f4fc54',N'sđfsfd',N'123123',N'e8472aab-ca79-4287-b985-9e50426fd39a',N'2a9c5d22-ca75-45c0-8de5-824ff8e14850',2),
	 (N'05e1f726-2f94-4403-b1a7-16b36c2aa237',N'Hồ Tây',N'123456789',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'35a2a016-4f2e-4e0e-bb47-d546b9504b59',0),
	 (N'0ef0cc2b-ec3d-4037-a03d-8375d4f493e9',N'Đinh Tiên Hoàng',N'0927120254',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'ef309775-6b65-44ba-ae45-3e57b58671d2',0),
	 (N'1b4466e8-40b1-4de7-b548-831662708fa5',N'Đinh Bộ Lĩnh',N'0962396877',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'15abf600-731e-4ae6-99ea-1478d0b8ce95',0),
	 (N'2480a09a-d968-41b2-80a3-ec1535d051d5',N'Nguyễn Huệ',N'0394123123',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'28b6dbf2-f3a2-4e6c-8886-450701f092ba',0),
	 (N'349be331-9816-4d00-ad73-efbd35d47414',N'26 Doãn Kế Thiện, Mai dịch, Hà Nội',N'0364431866',N'63d876b6-5ce3-4e32-80d9-2edb706aa14d',N'f2953df8-4df7-467f-ae17-ec2a60c51ae0',1),
	 (N'3ac62180-79b1-41b4-a4b8-0cba6364e57e',N'Bát Ðàn',N'0978524784',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'ab244c62-d87f-423b-b1e6-b1983026ece5',1),
	 (N'4f376f02-7955-4d1b-9e70-0796602a7185',N'Hồ Hoàn Kiếm',N'0394668699',N'2e2d4362-daaa-4540-9385-5074fae26a28',N'28b6dbf2-f3a2-4e6c-8886-450701f092ba',1),
	 (N'9229f0c1-d54a-4638-a677-be5d10d9db24',N'Xã Ðàn',N'0394668699',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'6ba89604-0129-4e5a-b679-d1cf36e2974e',0),
	 (N'af1139e0-4364-4fe1-89e9-e5cf8fa5971b',N'ửerwerwer',N'3234',N'e8472aab-ca79-4287-b985-9e50426fd39a',N'48bf4620-5700-4641-9525-00227bee9ae8',2);
INSERT INTO db_a743ce_vuanhlk15.dbo.Address (ID,Address,Phone,UserID,DistrictID,IsDefault) VALUES
	 (N'bb5e520f-f717-48ae-909a-46af1ded054d',N'Nguyễn Trãi',N'0394123123',N'2e2d4362-daaa-4540-9385-5074fae26a28',N'f2953df8-4df7-467f-ae17-ec2a60c51ae0',0),
	 (N'be161729-7553-410c-a595-fe3d1cea264f',N'Nguyễn Huệ',N'0394444444',N'a0a12e57-31d4-4532-8906-ec7768d4fcc3',N'2a9c5d22-ca75-45c0-8de5-824ff8e14850',1),
	 (N'c0954f77-0972-4c2d-8d40-acbc0bf360eb',N'Xã Ðàn',N'0394668699',N'aca06373-1efe-4da9-82eb-b66fe7185754',N'6ba89604-0129-4e5a-b679-d1cf36e2974e',1),
	 (N'c2ea500f-f2f3-4e64-92dd-ef7acc98ccb8',N'Quang Trung',N'0962396877',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'15abf600-731e-4ae6-99ea-1478d0b8ce95',0),
	 (N'd6a619d5-1834-4fce-8028-93dceb64d6f0',N'Số 12, Phong Thổ, Lai Châu',N'0364456122',N'e8472aab-ca79-4287-b985-9e50426fd39a',N'2a9c5d22-ca75-45c0-8de5-824ff8e14850',1),
	 (N'd9963588-a0ad-4cfc-ba17-1aed82348598',N'Liên An',N'035721697',N'87660958-cf24-4b94-9eca-2cec66a711d3',N'15abf600-731e-4ae6-99ea-1478d0b8ce95',0),
	 (N'da08be4b-98f6-4f5e-a381-3400d88d2fce',N'Xã Ðàn',N'0394123123',N'9c97a43e-a55b-4b19-a69f-e58e2153c99b',N'6ba89604-0129-4e5a-b679-d1cf36e2974e',1),
	 (N'db9cfc89-92d7-4ba2-b7ee-3b492328e54f',N'Xã Ðàn',N'0394668699',N'b371d81c-54fc-4e39-b3f3-5732997066a9',N'6ba89604-0129-4e5a-b679-d1cf36e2974e',1),
	 (N'e5ef530d-1cb0-4c4c-85a7-d45f624f3316',N'Ngô Quyền',N'0394111111',N'eda2a409-1694-4663-af0a-e76c03e70cd7',N'35a2a016-4f2e-4e0e-bb47-d546b9504b59',1);