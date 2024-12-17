--upitno
--create table FolderType (
--IDFolderType int identity primary key, 
--Type nvarchar(50) not null,
--)
--create table Folder (
--IDFolder int identity primary key,
--Path nvarchar(150) not null,
--FolderTypeID int foreign key references FolderType(IDFolderType) 
--)

create database STUDYMATERIALORGANISER
go
use STUDYMATERIALORGANISER
go
create table Material (
IDMaterial int identity constraint PK_Material primary key, 
Name nvarchar(100) not null, 
Description nvarchar(max), 
Link nvarchar(max) not null, 
FilePath nvarchar(150) not null, 
FolderTypeID int not null
)

create table Tag (
IDTag int identity constraint PK_Tag primary key, 
TagName nvarchar(150) not null
)

create table MaterialTag (
IDMaterialTag int identity constraint PK_MaterialTag primary key, 
MaterialID int constraint FK_Material foreign key references Material(IDMaterial) ,
TagID int constraint FK_Tag foreign key  references Tag(IDTag)
)
declare @i int; 
exec createMaterial @i, 'test', 'tsset', 'tsdf', 'asd', 2
--Material CRUD
create or alter procedure createMaterial 
@IDMaterial int output, 
@Name nvarchar(100), 
@Description nvarchar(max), 
@Link nvarchar(max), 
@FilePath nvarchar(150), 
@FolderTypeID int 
as
begin  
        begin transaction;
        if exists (select 1 from Material where Name = @Name)
        begin
            rollback transaction;
            throw 51000, 'A material with the given name already exists.', 1;
        end
        insert into Material (Name, Description, Link, FilePath, FolderTypeID)
        values (@Name, @Description, @Link, @FilePath, @FolderTypeID);

        set @IDMaterial = SCOPE_IDENTITY();

        commit transaction;

end

GO
create or alter procedure updateMaterial
@IDMaterial int , 
@Name nvarchar(100) , 
@Description nvarchar(max), 
@Link nvarchar(max) , 
@FilePath nvarchar(150) , 
@FolderTypeID int
as
begin
update Material set
Name = @Name ,
Description =@Description, 
Link = @Link , 
FilePath = @FilePath, 
FolderTypeID = @FolderTypeID
where 
IDMaterial = @IDMaterial 
end 
GO

create or alter procedure deleteMaterial
@IDMaterial int
as
begin
delete from Material where IDMaterial = @IDMaterial
end
GO

create or alter procedure selectMaterial
@IDMaterial int
as
begin
select Name, Description, Link, FilePath, FolderTypeID from Material where IDMaterial = @IDMaterial
end
GO
create or alter procedure selectMaterials
as
begin
select Name, Description, Link, FilePath, FolderTypeID from Material
end

--MaterialTag CRUD
create or alter procedure createMaterialTag
@IDMaterialTag int , 
@MaterialID int,
@TagID int
as
begin
insert into MaterialTag values (@MaterialID, @TagID)
set @IDMaterialTag = SCOPE_IDENTITY()
end

create or alter procedure updateMaterialTag
@IDMaterialTag int , 
@MaterialID int,
@TagID int
as
begin
update MaterialTag set
MaterialID = @MaterialID,
TagID =  @TagID
where IDMaterialTag =  @IDMaterialTag
end

create or alter procedure deleteMaterialTag
@IDMaterialTag int 
as
begin
delete from MaterialTag where IDMaterialTag = @IDMaterialTag
end
GO

create or alter procedure selectMaterialByTag
@TagID int 
as
begin
select t.TagName, m.Name from MaterialTag as mt
left outer join Tag as t on t.IDTag = mt.TagID
left outer join Material as m on m.IDMaterial = mt.MaterialID
where TagID = @TagID
end
GO
create or alter procedure selectTagByMaterial
@MaterialID int
as
begin
select t.TagName, m.Name from MaterialTag as mt
left outer join Tag as t on t.IDTag = mt.TagID
left outer join Material as m on m.IDMaterial = mt.MaterialID
where MaterialID = @MaterialID
end



CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL DEFAULT (getutcdate()),
	[DeletedAt] [datetime2](7) NULL,
	[Username] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PwdHash] [nvarchar](256) NOT NULL,
	[PwdSalt] [nvarchar](256) NOT NULL,
	[Phone] [nvarchar](256) NULL,
	[Role] [int] NOT NULL,
	[SecurityToken] [nvarchar](256) NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT(0),
	CONSTRAINT [UQ_User_Username] UNIQUE ([Username]),
    CONSTRAINT [UQ_User_Email] UNIQUE ([Email]),
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)

CREATE TABLE [dbo].[Log](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [Level] NVARCHAR(50) NOT NULL,
    [Timestamp] DATETIME NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
    (
        [Id] ASC
    )
);
/*
CREATE TABLE [dbo].[Tag](
[Id]INT IDENTITY(1,1) NOT NULL,
[Name] NVARCHAR(MAX) NOT NULL,
CONSTRAINT [UQ_Tag_Name] UNIQUE ([Name]),
CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED
(
[ID] ASC
)
);
*/

CREATE TABLE [dbo].[Group](
[Id] INT IDENTITY(1,1) NOT NULL,
[Name] NVARCHAR(50) NOT NULL,
[TagId] [int] NOT NULL,
CONSTRAINT [UQ_Group_Name] UNIQUE ([Name]),
CONSTRAINT [FK_Group_Tag] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tag]([IDTag]) ON DELETE CASCADE,
CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED
(
[Id] ASC
)
);

CREATE TABLE [dbo].[UserGroup](
[Id]INT IDENTITY(1,1) NOT NULL,
[UserId] [int] NOT NULL,
[GroupId][int]NOT NULL,
CONSTRAINT [FK_UserGroup_User] FOREIGN KEY ([UserID]) REFERENCES [dbo].[User]([Id]) ON DELETE CASCADE,
CONSTRAINT [FK_UserGroup_Group] FOREIGN KEY ([GroupID]) REFERENCES [dbo].[Group]([Id]) ON DELETE CASCADE,
CONSTRAINT [PK_UserGroup] PRIMARY KEY CLUSTERED
(
[Id] ASC
)
);