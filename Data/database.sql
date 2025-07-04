
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'HallManagementSchema')
BEGIN
    EXEC('CREATE SCHEMA [HallManagementSchema]')  
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[DSW]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[DSW] (
    [Email]        NVARCHAR (255)  NOT NULL,
    [PasswordHash] VARBINARY (MAX) NOT NULL,
    [PasswordSalt] VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Email] ASC)
);
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[StudentPendingRequest]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[StudentPendingRequest] (
    [Email] NVARCHAR (50) NOT NULL,
    [Code]  NVARCHAR (6)  NOT NULL,
    [Sent]  DATETIME      NOT NULL,
    CONSTRAINT [PK_StudentPendingRequest] PRIMARY KEY CLUSTERED ([Email] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[StudentAuthentication]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[StudentAuthentication] (
    [Email]        NVARCHAR (50)   NOT NULL,
    [PasswordHash] VARBINARY (MAX) NOT NULL,
    [PasswordSalt] VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_StudentAuthentication] PRIMARY KEY CLUSTERED ([Email] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[HallDetails]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[HallDetails] (
    [HallId]         INT            IDENTITY (1, 1) NOT NULL,
    [HallName]       NVARCHAR (100) NOT NULL,
    [Institution]    NVARCHAR (100) NOT NULL,
    [TotalSeats]     INT            NOT NULL,
    [OccupiedSeats]  INT            NOT NULL,
    [AvailableSeats] INT            NOT NULL,
    [HallType]       NVARCHAR (50)  NOT NULL,
    [ImageData]      NVARCHAR (MAX) NULL,
    [Established]    DATE           NULL,
    PRIMARY KEY CLUSTERED ([HallId] ASC),
    CONSTRAINT [UC_HallName_Institution] UNIQUE NONCLUSTERED ([HallName] ASC, [Institution] ASC)
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[HallAdminAuthentications]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[HallAdminAuthentications] (
    [Email]        NVARCHAR (50)   NOT NULL,
    [PasswordHash] VARBINARY (MAX) NULL,
    [PasswordSalt] VARBINARY (MAX) NULL,
    CONSTRAINT [PK_HallAdminAuthentication] PRIMARY KEY CLUSTERED ([Email] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);
END




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[PendingRoomRequests]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[PendingRoomRequests] (
    [RoomNo]      NVARCHAR (50) NULL,
    [StudentId]   INT           NOT NULL,
    [HallId]      INT           NOT NULL,
    [RequestedAt] DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([HallId] ASC, [StudentId] ASC)
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[AssignedDinningFee]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[AssignedDinningFee] (
    [Year]        INT           NOT NULL,
    [Month]       VARCHAR (255) NOT NULL,
    [TotalAmount] INT           NULL,
    [Date]        DATETIME      NULL,
    [HallId]      INT           NULL,
    PRIMARY KEY CLUSTERED ([Year] ASC, [Month] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END




IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[AssignedHallFee]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[AssignedHallFee] (
    [HallId]       INT           NULL,
    [Batch]        INT           NOT NULL,
    [LevelAndTerm] VARCHAR (255) NOT NULL,
    [TotalAmount]  INT           NULL,
    [Date]         DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Batch] ASC, [LevelAndTerm] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[HallAdmin]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[HallAdmin] (
    [HallAdminId] INT           IDENTITY (1, 1) NOT NULL,
    [HallId]      INT           NULL,
    [Email]       NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([HallAdminId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[Images]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[Images] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [Title]     NVARCHAR (50)   NULL,
    [ImageData] VARBINARY (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[Notice]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[Notice] (
    [NoticeId]    INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (255) NOT NULL,
    [NoticeType]  NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Date]        DATETIME       NOT NULL,
    [HallId]      INT            NULL,
    [Priority]    BIT            NOT NULL,
    [IsRead]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([NoticeId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[Room]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[Room] (
    [RoomNo]        NVARCHAR (50) NOT NULL,
    [RoomType]      NVARCHAR (50) NOT NULL,
    [RoomStatus]    NVARCHAR (50) NOT NULL,
    [RoomCondition] NVARCHAR (50) NOT NULL,
    [HasSeats]      INT           NOT NULL,
    [OccupiedSeats] INT           NOT NULL,
    [HallId]        INT           NOT NULL,
    CONSTRAINT [PK_Room] PRIMARY KEY CLUSTERED ([HallId] ASC, [RoomNo] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[Students]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[Students] (
    [Id]         INT            NOT NULL,
    [Name]       NVARCHAR (100) NOT NULL,
    [Email]      NVARCHAR (100) NOT NULL,
    [Department] NVARCHAR (100) NOT NULL,
    [Role]       NVARCHAR (100) NOT NULL,
    [ImageData]  NVARCHAR (MAX) NOT NULL,
    [RoomNo]     NVARCHAR (50)  NULL,
    [HallId]     INT            NULL,
    [Batch]      INT            NULL,
    [IsActive]   BIT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Students_Hall] FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId]),
    CONSTRAINT [FK_Students_Room] FOREIGN KEY ([HallId], [RoomNo]) REFERENCES [HallManagementSchema].[Room] ([HallId], [RoomNo]),
    UNIQUE NONCLUSTERED ([Email] ASC)
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[IsRead]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[IsRead] (
    [StudentId] INT NOT NULL,
    [NoticeId]  INT NOT NULL,
    FOREIGN KEY ([NoticeId]) REFERENCES [HallManagementSchema].[Notice] ([NoticeId]),
    FOREIGN KEY ([StudentId]) REFERENCES [HallManagementSchema].[Students] ([Id])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[NoticePriority]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[NoticePriority] (
    [StudentId] INT NOT NULL,
    [NoticeId]  INT NOT NULL,
    FOREIGN KEY ([NoticeId]) REFERENCES [HallManagementSchema].[Notice] ([NoticeId]),
    FOREIGN KEY ([StudentId]) REFERENCES [HallManagementSchema].[Students] ([Id])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[HallReview]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[HallReview] (
    [ReviewId]   INT            IDENTITY (1, 1) NOT NULL,
    [HallId]     INT            NULL,
    [Review]     NVARCHAR (MAX) NULL,
    [Rating]     INT            NULL,
    [Reviewer]   INT            NULL,
    [ReviewDate] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ReviewId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId]),
    FOREIGN KEY ([Reviewer]) REFERENCES [HallManagementSchema].[Students] ([Id])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[DinningFeePayments]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[DinningFeePayments] (
    [DinningFeePaymentId] INT           IDENTITY (1, 1) NOT NULL,
    [StudentId]           INT           NULL,
    [HallId]              INT           NULL,
    [PaymentAmount]       INT           NULL,
    [PaymentDate]         DATETIME      NULL,
    [PaymentMethod]       NVARCHAR (50) NULL,
    [PaymentStatus]       NVARCHAR (50) NULL,
    [Month]               NVARCHAR (50) NULL,
    [Year]                INT           NULL,
    PRIMARY KEY CLUSTERED ([DinningFeePaymentId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId]),
    FOREIGN KEY ([StudentId]) REFERENCES [HallManagementSchema].[Students] ([Id])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[HallFeePayments]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[HallFeePayments] (
    [HallFeePaymentId] INT           IDENTITY (1, 1) NOT NULL,
    [StudentId]        INT           NULL,
    [HallId]           INT           NULL,
    [PaymentAmount]    INT           NULL,
    [PaymentDate]      DATETIME      NULL,
    [PaymentMethod]    NVARCHAR (50) NULL,
    [PaymentStatus]    NVARCHAR (50) NULL,
    [LevelAndTerm]     NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([HallFeePaymentId] ASC),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId]),
    FOREIGN KEY ([StudentId]) REFERENCES [HallManagementSchema].[Students] ([Id])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[Complaint]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[Complaint] (
    [ComplaintId]   INT            IDENTITY (1, 1) NOT NULL,
    [Catagory]      NVARCHAR (100) DEFAULT ('') NOT NULL,
    [Priority]      NVARCHAR (50)  DEFAULT ('') NOT NULL,
    [Status]        NVARCHAR (50)  DEFAULT ('') NOT NULL,
    [Description]   NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [StudentId]     INT            NOT NULL,
    [Location]      NVARCHAR (100) DEFAULT ('') NOT NULL,
    [HallId]        INT            NOT NULL,
    [ImageData]     NVARCHAR (MAX) NULL,
    [FileData]      NVARCHAR (MAX) NULL,
    [ComplaintDate] DATETIME       NOT NULL,
    [Title]         NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([ComplaintId] ASC),
    FOREIGN KEY ([StudentId]) REFERENCES [HallManagementSchema].[Students] ([Id]),
    CONSTRAINT [FK_Complaint_HallDetails] FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[StudentsMessage]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[StudentsMessage] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Message] NVARCHAR (MAX) DEFAULT (' ') NOT NULL,
    [Sender]  INT            NOT NULL,
    [Date]    DATETIME       DEFAULT (getdate()) NOT NULL,
    [HallId]  INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Sender]) REFERENCES [HallManagementSchema].[Students] ([Id]),
    CONSTRAINT [FK_StudentsMessage_HallDetails] FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId])
);
END



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HallManagementSchema].[Comment]') AND type = 'U')
BEGIN
CREATE TABLE [HallManagementSchema].[Comment] (
    [CommentId]   INT            IDENTITY (1, 1) NOT NULL,
    [CommentText] NVARCHAR (MAX) DEFAULT ('') NOT NULL,
    [ComplaintId] INT            NULL,
    [StudentId]   INT            NULL,
    [HallAdminId] INT            NULL,
    [HallId]      INT            NULL,
    [CommentedBy] NVARCHAR (100) DEFAULT ('') NOT NULL,
    [CommentedAt] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentId] ASC),
    FOREIGN KEY ([ComplaintId]) REFERENCES [HallManagementSchema].[Complaint] ([ComplaintId]),
    FOREIGN KEY ([HallAdminId]) REFERENCES [HallManagementSchema].[HallAdmin] ([HallAdminId]),
    FOREIGN KEY ([HallId]) REFERENCES [HallManagementSchema].[HallDetails] ([HallId]),
    FOREIGN KEY ([StudentId]) REFERENCES [HallManagementSchema].[Students] ([Id])
);
END