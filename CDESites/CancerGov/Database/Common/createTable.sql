--Create database PercCancerGov
GO

USE [PercCancerGov]
GO
/****** Object:  User [PercussionUser]    Script Date: 09/23/2010 17:15:27 ******/
GO
CREATE USER [PercussionUser] FOR LOGIN [PercussionUser] WITH DEFAULT_SCHEMA=[dbo]
GO
exec sp_addrolemember 'db_owner', 'percussionUser'
GO
--drop table cgvPageMetadata
go
create table cgvPageMetadata
(CONTENTID int primary key,
LONG_TITLE	nvarchar(255),
SHORT_TITLE	nvarchar(100),
LONG_DESCRIPTION	ntext,
SHORT_DESCRIPTION	nvarchar(255),
META_KEYWORDS	nvarchar(255),
PRETTYURL	nvarchar(300),
[DATE_FIRST_PUBLISHED] [datetime] NULL,
[DATE_LAST_MODIFIED] [datetime] NULL,
[DATE_LAST_REVIEWED] [datetime] NULL,
[DATE_NEXT_REVIEW] [datetime] NULL,
[DATE_DISPLAY_MODE] INT NULL,
LEGACY_SEARCH_FILTER nvarchar(max),
[LANGUAGE] nchar(2),
VIDEOURL nvarchar(max),
AUDIOURL nvarchar(max),
IMAGEURL nvarchar(max),
News_QandA_URL nvarchar(max)
)

GO
--drop table cgvStagingPageMetadata
go

create table cgvStagingPageMetadata
(CONTENTID int primary key,
LONG_TITLE	nvarchar(255),
SHORT_TITLE	nvarchar(100),
LONG_DESCRIPTION	ntext,
SHORT_DESCRIPTION	nvarchar(255),
META_KEYWORDS	nvarchar(255),
PRETTYURL	nvarchar(300),
[DATE_FIRST_PUBLISHED] [datetime] NULL,
[DATE_LAST_MODIFIED] [datetime] NULL,
[DATE_LAST_REVIEWED] [datetime] NULL,
[DATE_NEXT_REVIEW] [datetime] NULL,
[DATE_DISPLAY_MODE] INT NULL,
LEGACY_SEARCH_FILTER nvarchar(max),
[LANGUAGE] nchar(2),
VIDEOURL nvarchar(max),
AUDIOURL nvarchar(max),
IMAGEURL nvarchar(max),
News_QandA_URL nvarchar(max)

)

GO





/*
--drop table cgvPageSearchMetadata
go
create table cgvPageSearchMetadata
(CONTENTID int NOT NULL,
SEQ INT   primary key,
SEARCH_FILTER [nvarchar](50) NULL
)
GO

alter table cgvPageSearchMetadata add constraint FK_cgvPageSearchMetadata  foreign key (CONTENTID) references 
cgvPageMetadata (CONTENTID)
GO
*/





