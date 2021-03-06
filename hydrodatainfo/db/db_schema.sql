USE [db1856]
GO
/****** Object:  User [db1856]    Script Date: 12/10/2014 10:15:34 AM ******/
CREATE USER [db1856] FOR LOGIN [db1856] WITH DEFAULT_SCHEMA=[plaveninycz]
GO
/****** Object:  User [plaveninycz]    Script Date: 12/10/2014 10:15:35 AM ******/
CREATE USER [plaveninycz] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[plaveninycz]
GO
/****** Object:  DatabaseRole [aspnet_WebEvent_FullAccess]    Script Date: 12/10/2014 10:15:35 AM ******/
CREATE ROLE [aspnet_WebEvent_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_ReportingAccess]    Script Date: 12/10/2014 10:15:36 AM ******/
CREATE ROLE [aspnet_Roles_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_FullAccess]    Script Date: 12/10/2014 10:15:36 AM ******/
CREATE ROLE [aspnet_Roles_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Roles_BasicAccess]    Script Date: 12/10/2014 10:15:36 AM ******/
CREATE ROLE [aspnet_Roles_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_ReportingAccess]    Script Date: 12/10/2014 10:15:37 AM ******/
CREATE ROLE [aspnet_Profile_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_FullAccess]    Script Date: 12/10/2014 10:15:37 AM ******/
CREATE ROLE [aspnet_Profile_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Profile_BasicAccess]    Script Date: 12/10/2014 10:15:38 AM ******/
CREATE ROLE [aspnet_Profile_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_ReportingAccess]    Script Date: 12/10/2014 10:15:38 AM ******/
CREATE ROLE [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_FullAccess]    Script Date: 12/10/2014 10:15:38 AM ******/
CREATE ROLE [aspnet_Personalization_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Personalization_BasicAccess]    Script Date: 12/10/2014 10:15:39 AM ******/
CREATE ROLE [aspnet_Personalization_BasicAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_ReportingAccess]    Script Date: 12/10/2014 10:15:39 AM ******/
CREATE ROLE [aspnet_Membership_ReportingAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_FullAccess]    Script Date: 12/10/2014 10:15:39 AM ******/
CREATE ROLE [aspnet_Membership_FullAccess]
GO
/****** Object:  DatabaseRole [aspnet_Membership_BasicAccess]    Script Date: 12/10/2014 10:15:40 AM ******/
CREATE ROLE [aspnet_Membership_BasicAccess]
GO
ALTER ROLE [db_owner] ADD MEMBER [db1856]
GO
ALTER ROLE [db_securityadmin] ADD MEMBER [plaveninycz]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [plaveninycz]
GO
ALTER ROLE [db_datareader] ADD MEMBER [plaveninycz]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [plaveninycz]
GO
ALTER ROLE [aspnet_Roles_BasicAccess] ADD MEMBER [aspnet_Roles_FullAccess]
GO
ALTER ROLE [aspnet_Roles_ReportingAccess] ADD MEMBER [aspnet_Roles_FullAccess]
GO
ALTER ROLE [aspnet_Profile_BasicAccess] ADD MEMBER [aspnet_Profile_FullAccess]
GO
ALTER ROLE [aspnet_Profile_ReportingAccess] ADD MEMBER [aspnet_Profile_FullAccess]
GO
ALTER ROLE [aspnet_Personalization_BasicAccess] ADD MEMBER [aspnet_Personalization_FullAccess]
GO
ALTER ROLE [aspnet_Personalization_ReportingAccess] ADD MEMBER [aspnet_Personalization_FullAccess]
GO
ALTER ROLE [aspnet_Membership_BasicAccess] ADD MEMBER [aspnet_Membership_FullAccess]
GO
ALTER ROLE [aspnet_Membership_ReportingAccess] ADD MEMBER [aspnet_Membership_FullAccess]
GO
/****** Object:  Schema [aspnet_Membership_BasicAccess]    Script Date: 12/10/2014 10:15:43 AM ******/
CREATE SCHEMA [aspnet_Membership_BasicAccess]
GO
/****** Object:  Schema [aspnet_Membership_FullAccess]    Script Date: 12/10/2014 10:15:44 AM ******/
CREATE SCHEMA [aspnet_Membership_FullAccess]
GO
/****** Object:  Schema [aspnet_Membership_ReportingAccess]    Script Date: 12/10/2014 10:15:44 AM ******/
CREATE SCHEMA [aspnet_Membership_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Personalization_BasicAccess]    Script Date: 12/10/2014 10:15:44 AM ******/
CREATE SCHEMA [aspnet_Personalization_BasicAccess]
GO
/****** Object:  Schema [aspnet_Personalization_FullAccess]    Script Date: 12/10/2014 10:15:45 AM ******/
CREATE SCHEMA [aspnet_Personalization_FullAccess]
GO
/****** Object:  Schema [aspnet_Personalization_ReportingAccess]    Script Date: 12/10/2014 10:15:45 AM ******/
CREATE SCHEMA [aspnet_Personalization_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Profile_BasicAccess]    Script Date: 12/10/2014 10:15:45 AM ******/
CREATE SCHEMA [aspnet_Profile_BasicAccess]
GO
/****** Object:  Schema [aspnet_Profile_FullAccess]    Script Date: 12/10/2014 10:15:46 AM ******/
CREATE SCHEMA [aspnet_Profile_FullAccess]
GO
/****** Object:  Schema [aspnet_Profile_ReportingAccess]    Script Date: 12/10/2014 10:15:46 AM ******/
CREATE SCHEMA [aspnet_Profile_ReportingAccess]
GO
/****** Object:  Schema [aspnet_Roles_BasicAccess]    Script Date: 12/10/2014 10:15:47 AM ******/
CREATE SCHEMA [aspnet_Roles_BasicAccess]
GO
/****** Object:  Schema [aspnet_Roles_FullAccess]    Script Date: 12/10/2014 10:15:47 AM ******/
CREATE SCHEMA [aspnet_Roles_FullAccess]
GO
/****** Object:  Schema [aspnet_Roles_ReportingAccess]    Script Date: 12/10/2014 10:15:47 AM ******/
CREATE SCHEMA [aspnet_Roles_ReportingAccess]
GO
/****** Object:  Schema [aspnet_WebEvent_FullAccess]    Script Date: 12/10/2014 10:15:48 AM ******/
CREATE SCHEMA [aspnet_WebEvent_FullAccess]
GO
/****** Object:  Schema [plaveninycz]    Script Date: 12/10/2014 10:15:48 AM ******/
CREATE SCHEMA [plaveninycz]
GO
/****** Object:  StoredProcedure [dbo].[aspnet_AnyDataInTables]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_AnyDataInTables]
    @TablesToCheck int
AS
BEGIN
    -- Check Membership table if (@TablesToCheck & 1) is set
    IF ((@TablesToCheck & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Membership))
        BEGIN
            SELECT N'aspnet_Membership'
            RETURN
        END
    END

    -- Check aspnet_Roles table if (@TablesToCheck & 2) is set
    IF ((@TablesToCheck & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Roles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 RoleId FROM dbo.aspnet_Roles))
        BEGIN
            SELECT N'aspnet_Roles'
            RETURN
        END
    END

    -- Check aspnet_Profile table if (@TablesToCheck & 4) is set
    IF ((@TablesToCheck & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Profile))
        BEGIN
            SELECT N'aspnet_Profile'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 8) is set
    IF ((@TablesToCheck & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_PersonalizationPerUser))
        BEGIN
            SELECT N'aspnet_PersonalizationPerUser'
            RETURN
        END
    END

    -- Check aspnet_PersonalizationPerUser table if (@TablesToCheck & 16) is set
    IF ((@TablesToCheck & 16) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'aspnet_WebEvent_LogEvent') AND (type = 'P'))) )
    BEGIN
        IF (EXISTS(SELECT TOP 1 * FROM dbo.aspnet_WebEvent_Events))
        BEGIN
            SELECT N'aspnet_WebEvent_Events'
            RETURN
        END
    END

    -- Check aspnet_Users table if (@TablesToCheck & 1,2,4 & 8) are all set
    IF ((@TablesToCheck & 1) <> 0 AND
        (@TablesToCheck & 2) <> 0 AND
        (@TablesToCheck & 4) <> 0 AND
        (@TablesToCheck & 8) <> 0 AND
        (@TablesToCheck & 32) <> 0 AND
        (@TablesToCheck & 128) <> 0 AND
        (@TablesToCheck & 256) <> 0 AND
        (@TablesToCheck & 512) <> 0 AND
        (@TablesToCheck & 1024) <> 0)
    BEGIN
        IF (EXISTS(SELECT TOP 1 UserId FROM dbo.aspnet_Users))
        BEGIN
            SELECT N'aspnet_Users'
            RETURN
        END
        IF (EXISTS(SELECT TOP 1 ApplicationId FROM dbo.aspnet_Applications))
        BEGIN
            SELECT N'aspnet_Applications'
            RETURN
        END
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Applications_CreateApplication]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Applications_CreateApplication]
    @ApplicationName      nvarchar(256),
    @ApplicationId        uniqueidentifier OUTPUT
AS
BEGIN
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName

    IF(@ApplicationId IS NULL)
    BEGIN
        DECLARE @TranStarted   bit
        SET @TranStarted = 0

        IF( @@TRANCOUNT = 0 )
        BEGIN
	        BEGIN TRANSACTION
	        SET @TranStarted = 1
        END
        ELSE
    	    SET @TranStarted = 0

        SELECT  @ApplicationId = ApplicationId
        FROM dbo.aspnet_Applications WITH (UPDLOCK, HOLDLOCK)
        WHERE LOWER(@ApplicationName) = LoweredApplicationName

        IF(@ApplicationId IS NULL)
        BEGIN
            SELECT  @ApplicationId = NEWID()
            INSERT  dbo.aspnet_Applications (ApplicationId, ApplicationName, LoweredApplicationName)
            VALUES  (@ApplicationId, @ApplicationName, LOWER(@ApplicationName))
        END


        IF( @TranStarted = 1 )
        BEGIN
            IF(@@ERROR = 0)
            BEGIN
	        SET @TranStarted = 0
	        COMMIT TRANSACTION
            END
            ELSE
            BEGIN
                SET @TranStarted = 0
                ROLLBACK TRANSACTION
            END
        END
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_CheckSchemaVersion]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_CheckSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    IF (EXISTS( SELECT  *
                FROM    dbo.aspnet_SchemaVersions
                WHERE   Feature = LOWER( @Feature ) AND
                        CompatibleSchemaVersion = @CompatibleSchemaVersion ))
        RETURN 0

    RETURN 1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_CreateUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_CreateUser]
    @ApplicationName                        nvarchar(256),
    @UserName                               nvarchar(256),
    @Password                               nvarchar(128),
    @PasswordSalt                           nvarchar(128),
    @Email                                  nvarchar(256),
    @PasswordQuestion                       nvarchar(256),
    @PasswordAnswer                         nvarchar(128),
    @IsApproved                             bit,
    @CurrentTimeUtc                         datetime,
    @CreateDate                             datetime = NULL,
    @UniqueEmail                            int      = 0,
    @PasswordFormat                         int      = 0,
    @UserId                                 uniqueidentifier OUTPUT
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @NewUserId uniqueidentifier
    SELECT @NewUserId = NULL

    DECLARE @IsLockedOut bit
    SET @IsLockedOut = 0

    DECLARE @LastLockoutDate  datetime
    SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAttemptCount int
    SET @FailedPasswordAttemptCount = 0

    DECLARE @FailedPasswordAttemptWindowStart  datetime
    SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @FailedPasswordAnswerAttemptCount int
    SET @FailedPasswordAnswerAttemptCount = 0

    DECLARE @FailedPasswordAnswerAttemptWindowStart  datetime
    SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )

    DECLARE @NewUserCreated bit
    DECLARE @ReturnValue   int
    SET @ReturnValue = 0

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    SET @CreateDate = @CurrentTimeUtc

    SELECT  @NewUserId = UserId FROM dbo.aspnet_Users WHERE LOWER(@UserName) = LoweredUserName AND @ApplicationId = ApplicationId
    IF ( @NewUserId IS NULL )
    BEGIN
        SET @NewUserId = @UserId
        EXEC @ReturnValue = dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CreateDate, @NewUserId OUTPUT
        SET @NewUserCreated = 1
    END
    ELSE
    BEGIN
        SET @NewUserCreated = 0
        IF( @NewUserId <> @UserId AND @UserId IS NOT NULL )
        BEGIN
            SET @ErrorCode = 6
            GOTO Cleanup
        END
    END

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @ReturnValue = -1 )
    BEGIN
        SET @ErrorCode = 10
        GOTO Cleanup
    END

    IF ( EXISTS ( SELECT UserId
                  FROM   dbo.aspnet_Membership
                  WHERE  @NewUserId = UserId ) )
    BEGIN
        SET @ErrorCode = 6
        GOTO Cleanup
    END

    SET @UserId = @NewUserId

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership m WITH ( UPDLOCK, HOLDLOCK )
                    WHERE ApplicationId = @ApplicationId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            SET @ErrorCode = 7
            GOTO Cleanup
        END
    END

    IF (@NewUserCreated = 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate = @CreateDate
        WHERE  @UserId = UserId
        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    INSERT INTO dbo.aspnet_Membership
                ( ApplicationId,
                  UserId,
                  Password,
                  PasswordSalt,
                  Email,
                  LoweredEmail,
                  PasswordQuestion,
                  PasswordAnswer,
                  PasswordFormat,
                  IsApproved,
                  IsLockedOut,
                  CreateDate,
                  LastLoginDate,
                  LastPasswordChangedDate,
                  LastLockoutDate,
                  FailedPasswordAttemptCount,
                  FailedPasswordAttemptWindowStart,
                  FailedPasswordAnswerAttemptCount,
                  FailedPasswordAnswerAttemptWindowStart )
         VALUES ( @ApplicationId,
                  @UserId,
                  @Password,
                  @PasswordSalt,
                  @Email,
                  LOWER(@Email),
                  @PasswordQuestion,
                  @PasswordAnswer,
                  @PasswordFormat,
                  @IsApproved,
                  @IsLockedOut,
                  @CreateDate,
                  @CreateDate,
                  @CreateDate,
                  @LastLockoutDate,
                  @FailedPasswordAttemptCount,
                  @FailedPasswordAttemptWindowStart,
                  @FailedPasswordAnswerAttemptCount,
                  @FailedPasswordAnswerAttemptWindowStart )

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByEmail]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByEmail]
    @ApplicationName       nvarchar(256),
    @EmailToMatch          nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    IF( @EmailToMatch IS NULL )
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.Email IS NULL
            ORDER BY m.LoweredEmail
    ELSE
        INSERT INTO #PageIndexForUsers (UserId)
            SELECT u.UserId
            FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
            WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND m.LoweredEmail LIKE LOWER(@EmailToMatch)
            ORDER BY m.LoweredEmail

    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY m.LoweredEmail

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_FindUsersByName]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_FindUsersByName]
    @ApplicationName       nvarchar(256),
    @UserNameToMatch       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT u.UserId
        FROM   dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  u.ApplicationId = @ApplicationId AND m.UserId = u.UserId AND u.LoweredUserName LIKE LOWER(@UserNameToMatch)
        ORDER BY u.UserName


    SELECT  u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName

    SELECT  @TotalRecords = COUNT(*)
    FROM    #PageIndexForUsers
    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetAllUsers]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetAllUsers]
    @ApplicationName       nvarchar(256),
    @PageIndex             int,
    @PageSize              int
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN 0


    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
    SELECT u.UserId
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u
    WHERE  u.ApplicationId = @ApplicationId AND u.UserId = m.UserId
    ORDER BY u.UserName

    SELECT @TotalRecords = @@ROWCOUNT

    SELECT u.UserName, m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate,
            m.LastLoginDate,
            u.LastActivityDate,
            m.LastPasswordChangedDate,
            u.UserId, m.IsLockedOut,
            m.LastLockoutDate
    FROM   dbo.aspnet_Membership m, dbo.aspnet_Users u, #PageIndexForUsers p
    WHERE  u.UserId = p.UserId AND u.UserId = m.UserId AND
           p.IndexId >= @PageLowerBound AND p.IndexId <= @PageUpperBound
    ORDER BY u.UserName
    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetNumberOfUsersOnline]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetNumberOfUsersOnline]
    @ApplicationName            nvarchar(256),
    @MinutesSinceLastInActive   int,
    @CurrentTimeUtc             datetime
AS
BEGIN
    DECLARE @DateActive datetime
    SELECT  @DateActive = DATEADD(minute,  -(@MinutesSinceLastInActive), @CurrentTimeUtc)

    DECLARE @NumOnline int
    SELECT  @NumOnline = COUNT(*)
    FROM    dbo.aspnet_Users u(NOLOCK),
            dbo.aspnet_Applications a(NOLOCK),
            dbo.aspnet_Membership m(NOLOCK)
    WHERE   u.ApplicationId = a.ApplicationId                  AND
            LastActivityDate > @DateActive                     AND
            a.LoweredApplicationName = LOWER(@ApplicationName) AND
            u.UserId = m.UserId
    RETURN(@NumOnline)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPassword]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPassword]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @PasswordAnswer                 nvarchar(128) = NULL
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @PasswordFormat                         int
    DECLARE @Password                               nvarchar(128)
    DECLARE @passAns                                nvarchar(128)
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @Password = m.Password,
            @passAns = m.PasswordAnswer,
            @PasswordFormat = m.PasswordFormat,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    IF ( NOT( @PasswordAnswer IS NULL ) )
    BEGIN
        IF( ( @passAns IS NULL ) OR ( LOWER( @passAns ) <> LOWER( @PasswordAnswer ) ) )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
        ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    IF( @ErrorCode = 0 )
        SELECT @Password, @PasswordFormat

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetPasswordWithFormat]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetPasswordWithFormat]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @UpdateLastLoginActivityDate    bit,
    @CurrentTimeUtc                 datetime
AS
BEGIN
    DECLARE @IsLockedOut                        bit
    DECLARE @UserId                             uniqueidentifier
    DECLARE @Password                           nvarchar(128)
    DECLARE @PasswordSalt                       nvarchar(128)
    DECLARE @PasswordFormat                     int
    DECLARE @FailedPasswordAttemptCount         int
    DECLARE @FailedPasswordAnswerAttemptCount   int
    DECLARE @IsApproved                         bit
    DECLARE @LastActivityDate                   datetime
    DECLARE @LastLoginDate                      datetime

    SELECT  @UserId          = NULL

    SELECT  @UserId = u.UserId, @IsLockedOut = m.IsLockedOut, @Password=Password, @PasswordFormat=PasswordFormat,
            @PasswordSalt=PasswordSalt, @FailedPasswordAttemptCount=FailedPasswordAttemptCount,
		    @FailedPasswordAnswerAttemptCount=FailedPasswordAnswerAttemptCount, @IsApproved=IsApproved,
            @LastActivityDate = LastActivityDate, @LastLoginDate = LastLoginDate
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF (@UserId IS NULL)
        RETURN 1

    IF (@IsLockedOut = 1)
        RETURN 99

    SELECT   @Password, @PasswordFormat, @PasswordSalt, @FailedPasswordAttemptCount,
             @FailedPasswordAnswerAttemptCount, @IsApproved, @LastLoginDate, @LastActivityDate

    IF (@UpdateLastLoginActivityDate = 1 AND @IsApproved = 1)
    BEGIN
        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @CurrentTimeUtc
        WHERE   UserId = @UserId

        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @CurrentTimeUtc
        WHERE   @UserId = UserId
    END


    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByEmail]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByEmail]
    @ApplicationName  nvarchar(256),
    @Email            nvarchar(256)
AS
BEGIN
    IF( @Email IS NULL )
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.ApplicationId = a.ApplicationId AND
                m.LoweredEmail IS NULL
    ELSE
        SELECT  u.UserName
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                u.UserId = m.UserId AND
                m.ApplicationId = a.ApplicationId AND
                LOWER(@Email) = m.LoweredEmail

    IF (@@rowcount = 0)
        RETURN(1)
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByName]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByName]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier

    IF (@UpdateLastActivity = 1)
    BEGIN
        -- select user ID from aspnet_users table
        SELECT TOP 1 @UserId = u.UserId
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1

        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        WHERE    @UserId = UserId

        SELECT m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut, m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE  @UserId = u.UserId AND u.UserId = m.UserId 
    END
    ELSE
    BEGIN
        SELECT TOP 1 m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
                m.CreateDate, m.LastLoginDate, u.LastActivityDate, m.LastPasswordChangedDate,
                u.UserId, m.IsLockedOut,m.LastLockoutDate
        FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m
        WHERE    LOWER(@ApplicationName) = a.LoweredApplicationName AND
                u.ApplicationId = a.ApplicationId    AND
                LOWER(@UserName) = u.LoweredUserName AND u.UserId = m.UserId

        IF (@@ROWCOUNT = 0) -- Username not found
            RETURN -1
    END

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_GetUserByUserId]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_GetUserByUserId]
    @UserId               uniqueidentifier,
    @CurrentTimeUtc       datetime,
    @UpdateLastActivity   bit = 0
AS
BEGIN
    IF ( @UpdateLastActivity = 1 )
    BEGIN
        UPDATE   dbo.aspnet_Users
        SET      LastActivityDate = @CurrentTimeUtc
        FROM     dbo.aspnet_Users
        WHERE    @UserId = UserId

        IF ( @@ROWCOUNT = 0 ) -- User ID not found
            RETURN -1
    END

    SELECT  m.Email, m.PasswordQuestion, m.Comment, m.IsApproved,
            m.CreateDate, m.LastLoginDate, u.LastActivityDate,
            m.LastPasswordChangedDate, u.UserName, m.IsLockedOut,
            m.LastLockoutDate
    FROM    dbo.aspnet_Users u, dbo.aspnet_Membership m
    WHERE   @UserId = u.UserId AND u.UserId = m.UserId

    IF ( @@ROWCOUNT = 0 ) -- User ID not found
       RETURN -1

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ChangePasswordQuestionAndAnswer]
    @ApplicationName       nvarchar(256),
    @UserName              nvarchar(256),
    @NewPasswordQuestion   nvarchar(256),
    @NewPasswordAnswer     nvarchar(128)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Membership m, dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId
    IF (@UserId IS NULL)
    BEGIN
        RETURN(1)
    END

    UPDATE dbo.aspnet_Membership
    SET    PasswordQuestion = @NewPasswordQuestion, PasswordAnswer = @NewPasswordAnswer
    WHERE  UserId=@UserId
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_ResetPassword]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_ResetPassword]
    @ApplicationName             nvarchar(256),
    @UserName                    nvarchar(256),
    @NewPassword                 nvarchar(128),
    @MaxInvalidPasswordAttempts  int,
    @PasswordAttemptWindow       int,
    @PasswordSalt                nvarchar(128),
    @CurrentTimeUtc              datetime,
    @PasswordFormat              int = 0,
    @PasswordAnswer              nvarchar(128) = NULL
AS
BEGIN
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @UserId                                 uniqueidentifier
    SET     @UserId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    SELECT @IsLockedOut = IsLockedOut,
           @LastLockoutDate = LastLockoutDate,
           @FailedPasswordAttemptCount = FailedPasswordAttemptCount,
           @FailedPasswordAttemptWindowStart = FailedPasswordAttemptWindowStart,
           @FailedPasswordAnswerAttemptCount = FailedPasswordAnswerAttemptCount,
           @FailedPasswordAnswerAttemptWindowStart = FailedPasswordAnswerAttemptWindowStart
    FROM dbo.aspnet_Membership WITH ( UPDLOCK )
    WHERE @UserId = UserId

    IF( @IsLockedOut = 1 )
    BEGIN
        SET @ErrorCode = 99
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Membership
    SET    Password = @NewPassword,
           LastPasswordChangedDate = @CurrentTimeUtc,
           PasswordFormat = @PasswordFormat,
           PasswordSalt = @PasswordSalt
    WHERE  @UserId = UserId AND
           ( ( @PasswordAnswer IS NULL ) OR ( LOWER( PasswordAnswer ) = LOWER( @PasswordAnswer ) ) )

    IF ( @@ROWCOUNT = 0 )
        BEGIN
            IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAnswerAttemptWindowStart ) )
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = 1
            END
            ELSE
            BEGIN
                SET @FailedPasswordAnswerAttemptWindowStart = @CurrentTimeUtc
                SET @FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount + 1
            END

            BEGIN
                IF( @FailedPasswordAnswerAttemptCount >= @MaxInvalidPasswordAttempts )
                BEGIN
                    SET @IsLockedOut = 1
                    SET @LastLockoutDate = @CurrentTimeUtc
                END
            END

            SET @ErrorCode = 3
        END
    ELSE
        BEGIN
            IF( @FailedPasswordAnswerAttemptCount > 0 )
            BEGIN
                SET @FailedPasswordAnswerAttemptCount = 0
                SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            END
        END

    IF( NOT ( @PasswordAnswer IS NULL ) )
    BEGIN
        UPDATE dbo.aspnet_Membership
        SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
            FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
            FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
            FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
            FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
        WHERE @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_SetPassword]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_SetPassword]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @NewPassword      nvarchar(128),
    @PasswordSalt     nvarchar(128),
    @CurrentTimeUtc   datetime,
    @PasswordFormat   int = 0
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    UPDATE dbo.aspnet_Membership
    SET Password = @NewPassword, PasswordFormat = @PasswordFormat, PasswordSalt = @PasswordSalt,
        LastPasswordChangedDate = @CurrentTimeUtc
    WHERE @UserId = UserId
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UnlockUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UnlockUser]
    @ApplicationName                         nvarchar(256),
    @UserName                                nvarchar(256)
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF ( @UserId IS NULL )
        RETURN 1

    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = 0,
        FailedPasswordAttemptCount = 0,
        FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        FailedPasswordAnswerAttemptCount = 0,
        FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 ),
        LastLockoutDate = CONVERT( datetime, '17540101', 112 )
    WHERE @UserId = UserId

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUser]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @Email                nvarchar(256),
    @Comment              ntext,
    @IsApproved           bit,
    @LastLoginDate        datetime,
    @LastActivityDate     datetime,
    @UniqueEmail          int,
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @UserId uniqueidentifier
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @UserId = NULL
    SELECT  @UserId = u.UserId, @ApplicationId = a.ApplicationId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a, dbo.aspnet_Membership m
    WHERE   LoweredUserName = LOWER(@UserName) AND
            u.ApplicationId = a.ApplicationId  AND
            LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.UserId = m.UserId

    IF (@UserId IS NULL)
        RETURN(1)

    IF (@UniqueEmail = 1)
    BEGIN
        IF (EXISTS (SELECT *
                    FROM  dbo.aspnet_Membership WITH (UPDLOCK, HOLDLOCK)
                    WHERE ApplicationId = @ApplicationId  AND @UserId <> UserId AND LoweredEmail = LOWER(@Email)))
        BEGIN
            RETURN(7)
        END
    END

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    UPDATE dbo.aspnet_Users WITH (ROWLOCK)
    SET
         LastActivityDate = @LastActivityDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.aspnet_Membership WITH (ROWLOCK)
    SET
         Email            = @Email,
         LoweredEmail     = LOWER(@Email),
         Comment          = @Comment,
         IsApproved       = @IsApproved,
         LastLoginDate    = @LastLoginDate
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Membership_UpdateUserInfo]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Membership_UpdateUserInfo]
    @ApplicationName                nvarchar(256),
    @UserName                       nvarchar(256),
    @IsPasswordCorrect              bit,
    @UpdateLastLoginActivityDate    bit,
    @MaxInvalidPasswordAttempts     int,
    @PasswordAttemptWindow          int,
    @CurrentTimeUtc                 datetime,
    @LastLoginDate                  datetime,
    @LastActivityDate               datetime
AS
BEGIN
    DECLARE @UserId                                 uniqueidentifier
    DECLARE @IsApproved                             bit
    DECLARE @IsLockedOut                            bit
    DECLARE @LastLockoutDate                        datetime
    DECLARE @FailedPasswordAttemptCount             int
    DECLARE @FailedPasswordAttemptWindowStart       datetime
    DECLARE @FailedPasswordAnswerAttemptCount       int
    DECLARE @FailedPasswordAnswerAttemptWindowStart datetime

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    SELECT  @UserId = u.UserId,
            @IsApproved = m.IsApproved,
            @IsLockedOut = m.IsLockedOut,
            @LastLockoutDate = m.LastLockoutDate,
            @FailedPasswordAttemptCount = m.FailedPasswordAttemptCount,
            @FailedPasswordAttemptWindowStart = m.FailedPasswordAttemptWindowStart,
            @FailedPasswordAnswerAttemptCount = m.FailedPasswordAnswerAttemptCount,
            @FailedPasswordAnswerAttemptWindowStart = m.FailedPasswordAnswerAttemptWindowStart
    FROM    dbo.aspnet_Applications a, dbo.aspnet_Users u, dbo.aspnet_Membership m WITH ( UPDLOCK )
    WHERE   LOWER(@ApplicationName) = a.LoweredApplicationName AND
            u.ApplicationId = a.ApplicationId    AND
            u.UserId = m.UserId AND
            LOWER(@UserName) = u.LoweredUserName

    IF ( @@rowcount = 0 )
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    IF( @IsLockedOut = 1 )
    BEGIN
        GOTO Cleanup
    END

    IF( @IsPasswordCorrect = 0 )
    BEGIN
        IF( @CurrentTimeUtc > DATEADD( minute, @PasswordAttemptWindow, @FailedPasswordAttemptWindowStart ) )
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = 1
        END
        ELSE
        BEGIN
            SET @FailedPasswordAttemptWindowStart = @CurrentTimeUtc
            SET @FailedPasswordAttemptCount = @FailedPasswordAttemptCount + 1
        END

        BEGIN
            IF( @FailedPasswordAttemptCount >= @MaxInvalidPasswordAttempts )
            BEGIN
                SET @IsLockedOut = 1
                SET @LastLockoutDate = @CurrentTimeUtc
            END
        END
    END
    ELSE
    BEGIN
        IF( @FailedPasswordAttemptCount > 0 OR @FailedPasswordAnswerAttemptCount > 0 )
        BEGIN
            SET @FailedPasswordAttemptCount = 0
            SET @FailedPasswordAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @FailedPasswordAnswerAttemptCount = 0
            SET @FailedPasswordAnswerAttemptWindowStart = CONVERT( datetime, '17540101', 112 )
            SET @LastLockoutDate = CONVERT( datetime, '17540101', 112 )
        END
    END

    IF( @UpdateLastLoginActivityDate = 1 )
    BEGIN
        UPDATE  dbo.aspnet_Users
        SET     LastActivityDate = @LastActivityDate
        WHERE   @UserId = UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END

        UPDATE  dbo.aspnet_Membership
        SET     LastLoginDate = @LastLoginDate
        WHERE   UserId = @UserId

        IF( @@ERROR <> 0 )
        BEGIN
            SET @ErrorCode = -1
            GOTO Cleanup
        END
    END


    UPDATE dbo.aspnet_Membership
    SET IsLockedOut = @IsLockedOut, LastLockoutDate = @LastLockoutDate,
        FailedPasswordAttemptCount = @FailedPasswordAttemptCount,
        FailedPasswordAttemptWindowStart = @FailedPasswordAttemptWindowStart,
        FailedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount,
        FailedPasswordAnswerAttemptWindowStart = @FailedPasswordAnswerAttemptWindowStart
    WHERE @UserId = UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
	SET @TranStarted = 0
	COMMIT TRANSACTION
    END

    RETURN @ErrorCode

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Paths_CreatePath]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Paths_CreatePath]
    @ApplicationId UNIQUEIDENTIFIER,
    @Path           NVARCHAR(256),
    @PathId         UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
    BEGIN TRANSACTION
    IF (NOT EXISTS(SELECT * FROM dbo.aspnet_Paths WHERE LoweredPath = LOWER(@Path) AND ApplicationId = @ApplicationId))
    BEGIN
        INSERT dbo.aspnet_Paths (ApplicationId, Path, LoweredPath) VALUES (@ApplicationId, @Path, LOWER(@Path))
    END
    COMMIT TRANSACTION
    SELECT @PathId = PathId FROM dbo.aspnet_Paths WHERE LOWER(@Path) = LoweredPath AND ApplicationId = @ApplicationId
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Personalization_GetApplicationId]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Personalization_GetApplicationId] (
    @ApplicationName NVARCHAR(256),
    @ApplicationId UNIQUEIDENTIFIER OUT)
AS
BEGIN
    SELECT @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_DeleteAllState]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_DeleteAllState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Count int OUT)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        IF (@AllUsersScope = 1)
            DELETE FROM aspnet_PersonalizationAllUsers
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)
        ELSE
            DELETE FROM aspnet_PersonalizationPerUser
            WHERE PathId IN
               (SELECT Paths.PathId
                FROM dbo.aspnet_Paths Paths
                WHERE Paths.ApplicationId = @ApplicationId)

        SELECT @Count = @@ROWCOUNT
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_FindState]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_FindState] (
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @PageIndex              INT,
    @PageSize               INT,
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound INT
    DECLARE @PageUpperBound INT
    DECLARE @TotalRecords   INT
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table to store the selected results
    CREATE TABLE #PageIndex (
        IndexId int IDENTITY (0, 1) NOT NULL,
        ItemId UNIQUEIDENTIFIER
    )

    IF (@AllUsersScope = 1)
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT Paths.PathId
        FROM dbo.aspnet_Paths Paths,
             ((SELECT Paths.PathId
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND AllUsers.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT DISTINCT Paths.PathId
               FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Paths Paths
               WHERE Paths.ApplicationId = @ApplicationId
                      AND PerUser.PathId = Paths.PathId
                      AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path,
               SharedDataPerPath.LastUpdatedDate,
               SharedDataPerPath.SharedDataLength,
               UserDataPerPath.UserDataLength,
               UserDataPerPath.UserCount
        FROM dbo.aspnet_Paths Paths,
             ((SELECT PageIndex.ItemId AS PathId,
                      AllUsers.LastUpdatedDate AS LastUpdatedDate,
                      DATALENGTH(AllUsers.PageSettings) AS SharedDataLength
               FROM dbo.aspnet_PersonalizationAllUsers AllUsers, #PageIndex PageIndex
               WHERE AllUsers.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
              ) AS SharedDataPerPath
              FULL OUTER JOIN
              (SELECT PageIndex.ItemId AS PathId,
                      SUM(DATALENGTH(PerUser.PageSettings)) AS UserDataLength,
                      COUNT(*) AS UserCount
               FROM aspnet_PersonalizationPerUser PerUser, #PageIndex PageIndex
               WHERE PerUser.PathId = PageIndex.ItemId
                     AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
               GROUP BY PageIndex.ItemId
              ) AS UserDataPerPath
              ON SharedDataPerPath.PathId = UserDataPerPath.PathId
             )
        WHERE Paths.PathId = SharedDataPerPath.PathId OR Paths.PathId = UserDataPerPath.PathId
        ORDER BY Paths.Path ASC
    END
    ELSE
    BEGIN
        -- Insert into our temp table
        INSERT INTO #PageIndex (ItemId)
        SELECT PerUser.Id
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
        WHERE Paths.ApplicationId = @ApplicationId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
              AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
              AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
        ORDER BY Paths.Path ASC, Users.UserName ASC

        SELECT @TotalRecords = @@ROWCOUNT

        SELECT Paths.Path, PerUser.LastUpdatedDate, DATALENGTH(PerUser.PageSettings), Users.UserName, Users.LastActivityDate
        FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths, #PageIndex PageIndex
        WHERE PerUser.Id = PageIndex.ItemId
              AND PerUser.UserId = Users.UserId
              AND PerUser.PathId = Paths.PathId
              AND PageIndex.IndexId >= @PageLowerBound AND PageIndex.IndexId <= @PageUpperBound
        ORDER BY Paths.Path ASC, Users.UserName ASC
    END

    RETURN @TotalRecords
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_GetCountOfState]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_GetCountOfState] (
    @Count int OUT,
    @AllUsersScope bit,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256) = NULL,
    @UserName NVARCHAR(256) = NULL,
    @InactiveSinceDate DATETIME = NULL)
AS
BEGIN

    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
        IF (@AllUsersScope = 1)
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND AllUsers.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
        ELSE
            SELECT @Count = COUNT(*)
            FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
            WHERE Paths.ApplicationId = @ApplicationId
                  AND PerUser.UserId = Users.UserId
                  AND PerUser.PathId = Paths.PathId
                  AND (@Path IS NULL OR Paths.LoweredPath LIKE LOWER(@Path))
                  AND (@UserName IS NULL OR Users.LoweredUserName LIKE LOWER(@UserName))
                  AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetSharedState]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetSharedState] (
    @Count int OUT,
    @ApplicationName NVARCHAR(256),
    @Path NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationAllUsers
        WHERE PathId IN
            (SELECT AllUsers.PathId
             FROM dbo.aspnet_PersonalizationAllUsers AllUsers, dbo.aspnet_Paths Paths
             WHERE Paths.ApplicationId = @ApplicationId
                   AND AllUsers.PathId = Paths.PathId
                   AND Paths.LoweredPath = LOWER(@Path))

        SELECT @Count = @@ROWCOUNT
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAdministration_ResetUserState]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAdministration_ResetUserState] (
    @Count                  int                 OUT,
    @ApplicationName        NVARCHAR(256),
    @InactiveSinceDate      DATETIME            = NULL,
    @UserName               NVARCHAR(256)       = NULL,
    @Path                   NVARCHAR(256)       = NULL)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
        SELECT @Count = 0
    ELSE
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser
        WHERE Id IN (SELECT PerUser.Id
                     FROM dbo.aspnet_PersonalizationPerUser PerUser, dbo.aspnet_Users Users, dbo.aspnet_Paths Paths
                     WHERE Paths.ApplicationId = @ApplicationId
                           AND PerUser.UserId = Users.UserId
                           AND PerUser.PathId = Paths.PathId
                           AND (@InactiveSinceDate IS NULL OR Users.LastActivityDate <= @InactiveSinceDate)
                           AND (@UserName IS NULL OR Users.LoweredUserName = LOWER(@UserName))
                           AND (@Path IS NULL OR Paths.LoweredPath = LOWER(@Path)))

        SELECT @Count = @@ROWCOUNT
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationAllUsers p WHERE p.PathId = @PathId
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path              NVARCHAR(256))
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    DELETE FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationAllUsers_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationAllUsers WHERE PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationAllUsers SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationAllUsers(PathId, PageSettings, LastUpdatedDate) VALUES (@PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_GetPageSettings]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_GetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    SELECT p.PageSettings FROM dbo.aspnet_PersonalizationPerUser p WHERE p.PathId = @PathId AND p.UserId = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_ResetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Personalization_GetApplicationId @ApplicationName, @ApplicationId OUTPUT
    IF (@ApplicationId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        RETURN
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        RETURN
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE PathId = @PathId AND UserId = @UserId
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_PersonalizationPerUser_SetPageSettings]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_PersonalizationPerUser_SetPageSettings] (
    @ApplicationName  NVARCHAR(256),
    @UserName         NVARCHAR(256),
    @Path             NVARCHAR(256),
    @PageSettings     IMAGE,
    @CurrentTimeUtc   DATETIME)
AS
BEGIN
    DECLARE @ApplicationId UNIQUEIDENTIFIER
    DECLARE @PathId UNIQUEIDENTIFIER
    DECLARE @UserId UNIQUEIDENTIFIER

    SELECT @ApplicationId = NULL
    SELECT @PathId = NULL
    SELECT @UserId = NULL

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    SELECT @PathId = u.PathId FROM dbo.aspnet_Paths u WHERE u.ApplicationId = @ApplicationId AND u.LoweredPath = LOWER(@Path)
    IF (@PathId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Paths_CreatePath @ApplicationId, @Path, @PathId OUTPUT
    END

    SELECT @UserId = u.UserId FROM dbo.aspnet_Users u WHERE u.ApplicationId = @ApplicationId AND u.LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
    BEGIN
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, 0, @CurrentTimeUtc, @UserId OUTPUT
    END

    UPDATE   dbo.aspnet_Users WITH (ROWLOCK)
    SET      LastActivityDate = @CurrentTimeUtc
    WHERE    UserId = @UserId
    IF (@@ROWCOUNT = 0) -- Username not found
        RETURN

    IF (EXISTS(SELECT PathId FROM dbo.aspnet_PersonalizationPerUser WHERE UserId = @UserId AND PathId = @PathId))
        UPDATE dbo.aspnet_PersonalizationPerUser SET PageSettings = @PageSettings, LastUpdatedDate = @CurrentTimeUtc WHERE UserId = @UserId AND PathId = @PathId
    ELSE
        INSERT INTO dbo.aspnet_PersonalizationPerUser(UserId, PathId, PageSettings, LastUpdatedDate) VALUES (@UserId, @PathId, @PageSettings, @CurrentTimeUtc)
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteInactiveProfiles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT  0
        RETURN
    END

    DELETE
    FROM    dbo.aspnet_Profile
    WHERE   UserId IN
            (   SELECT  UserId
                FROM    dbo.aspnet_Users u
                WHERE   ApplicationId = @ApplicationId
                        AND (LastActivityDate <= @InactiveSinceDate)
                        AND (
                                (@ProfileAuthOptions = 2)
                             OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                             OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                            )
            )

    SELECT  @@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_DeleteProfiles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_DeleteProfiles]
    @ApplicationName        nvarchar(256),
    @UserNames              nvarchar(4000)
AS
BEGIN
    DECLARE @UserName     nvarchar(256)
    DECLARE @CurrentPos   int
    DECLARE @NextPos      int
    DECLARE @NumDeleted   int
    DECLARE @DeletedUser  int
    DECLARE @TranStarted  bit
    DECLARE @ErrorCode    int

    SET @ErrorCode = 0
    SET @CurrentPos = 1
    SET @NumDeleted = 0
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    WHILE (@CurrentPos <= LEN(@UserNames))
    BEGIN
        SELECT @NextPos = CHARINDEX(N',', @UserNames,  @CurrentPos)
        IF (@NextPos = 0 OR @NextPos IS NULL)
            SELECT @NextPos = LEN(@UserNames) + 1

        SELECT @UserName = SUBSTRING(@UserNames, @CurrentPos, @NextPos - @CurrentPos)
        SELECT @CurrentPos = @NextPos+1

        IF (LEN(@UserName) > 0)
        BEGIN
            SELECT @DeletedUser = 0
            EXEC dbo.aspnet_Users_DeleteUser @ApplicationName, @UserName, 4, @DeletedUser OUTPUT
            IF( @@ERROR <> 0 )
            BEGIN
                SET @ErrorCode = -1
                GOTO Cleanup
            END
            IF (@DeletedUser <> 0)
                SELECT @NumDeleted = @NumDeleted + 1
        END
    END
    SELECT @NumDeleted
    IF (@TranStarted = 1)
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END
    SET @TranStarted = 0

    RETURN 0

Cleanup:
    IF (@TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END
    RETURN @ErrorCode
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetNumberOfInactiveProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @InactiveSinceDate      datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
    BEGIN
        SELECT 0
        RETURN
    END

    SELECT  COUNT(*)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
    WHERE   ApplicationId = @ApplicationId
        AND u.UserId = p.UserId
        AND (LastActivityDate <= @InactiveSinceDate)
        AND (
                (@ProfileAuthOptions = 2)
                OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
            )
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProfiles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetProfiles]
    @ApplicationName        nvarchar(256),
    @ProfileAuthOptions     int,
    @PageIndex              int,
    @PageSize               int,
    @UserNameToMatch        nvarchar(256) = NULL,
    @InactiveSinceDate      datetime      = NULL
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    -- Set the page bounds
    DECLARE @PageLowerBound int
    DECLARE @PageUpperBound int
    DECLARE @TotalRecords   int
    SET @PageLowerBound = @PageSize * @PageIndex
    SET @PageUpperBound = @PageSize - 1 + @PageLowerBound

    -- Create a temp table TO store the select results
    CREATE TABLE #PageIndexForUsers
    (
        IndexId int IDENTITY (0, 1) NOT NULL,
        UserId uniqueidentifier
    )

    -- Insert into our temp table
    INSERT INTO #PageIndexForUsers (UserId)
        SELECT  u.UserId
        FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p
        WHERE   ApplicationId = @ApplicationId
            AND u.UserId = p.UserId
            AND (@InactiveSinceDate IS NULL OR LastActivityDate <= @InactiveSinceDate)
            AND (     (@ProfileAuthOptions = 2)
                   OR (@ProfileAuthOptions = 0 AND IsAnonymous = 1)
                   OR (@ProfileAuthOptions = 1 AND IsAnonymous = 0)
                 )
            AND (@UserNameToMatch IS NULL OR LoweredUserName LIKE LOWER(@UserNameToMatch))
        ORDER BY UserName

    SELECT  u.UserName, u.IsAnonymous, u.LastActivityDate, p.LastUpdatedDate,
            DATALENGTH(p.PropertyNames) + DATALENGTH(p.PropertyValuesString) + DATALENGTH(p.PropertyValuesBinary)
    FROM    dbo.aspnet_Users u, dbo.aspnet_Profile p, #PageIndexForUsers i
    WHERE   u.UserId = p.UserId AND p.UserId = i.UserId AND i.IndexId >= @PageLowerBound AND i.IndexId <= @PageUpperBound

    SELECT COUNT(*)
    FROM   #PageIndexForUsers

    DROP TABLE #PageIndexForUsers
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_GetProperties]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_GetProperties]
    @ApplicationName      nvarchar(256),
    @UserName             nvarchar(256),
    @CurrentTimeUtc       datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM dbo.aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN

    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)

    IF (@UserId IS NULL)
        RETURN
    SELECT TOP 1 PropertyNames, PropertyValuesString, PropertyValuesBinary
    FROM         dbo.aspnet_Profile
    WHERE        UserId = @UserId

    IF (@@ROWCOUNT > 0)
    BEGIN
        UPDATE dbo.aspnet_Users
        SET    LastActivityDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    END
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Profile_SetProperties]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Profile_SetProperties]
    @ApplicationName        nvarchar(256),
    @PropertyNames          ntext,
    @PropertyValuesString   ntext,
    @PropertyValuesBinary   image,
    @UserName               nvarchar(256),
    @IsUserAnonymous        bit,
    @CurrentTimeUtc         datetime
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
       BEGIN TRANSACTION
       SET @TranStarted = 1
    END
    ELSE
    	SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DECLARE @UserId uniqueidentifier
    DECLARE @LastActivityDate datetime
    SELECT  @UserId = NULL
    SELECT  @LastActivityDate = @CurrentTimeUtc

    SELECT @UserId = UserId
    FROM   dbo.aspnet_Users
    WHERE  ApplicationId = @ApplicationId AND LoweredUserName = LOWER(@UserName)
    IF (@UserId IS NULL)
        EXEC dbo.aspnet_Users_CreateUser @ApplicationId, @UserName, @IsUserAnonymous, @LastActivityDate, @UserId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    UPDATE dbo.aspnet_Users
    SET    LastActivityDate=@CurrentTimeUtc
    WHERE  UserId = @UserId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS( SELECT *
               FROM   dbo.aspnet_Profile
               WHERE  UserId = @UserId))
        UPDATE dbo.aspnet_Profile
        SET    PropertyNames=@PropertyNames, PropertyValuesString = @PropertyValuesString,
               PropertyValuesBinary = @PropertyValuesBinary, LastUpdatedDate=@CurrentTimeUtc
        WHERE  UserId = @UserId
    ELSE
        INSERT INTO dbo.aspnet_Profile(UserId, PropertyNames, PropertyValuesString, PropertyValuesBinary, LastUpdatedDate)
             VALUES (@UserId, @PropertyNames, @PropertyValuesString, @PropertyValuesBinary, @CurrentTimeUtc)

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
    	SET @TranStarted = 0
    	COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_RegisterSchemaVersion]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_RegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128),
    @IsCurrentVersion          bit,
    @RemoveIncompatibleSchema  bit
AS
BEGIN
    IF( @RemoveIncompatibleSchema = 1 )
    BEGIN
        DELETE FROM dbo.aspnet_SchemaVersions WHERE Feature = LOWER( @Feature )
    END
    ELSE
    BEGIN
        IF( @IsCurrentVersion = 1 )
        BEGIN
            UPDATE dbo.aspnet_SchemaVersions
            SET IsCurrentVersion = 0
            WHERE Feature = LOWER( @Feature )
        END
    END

    INSERT  dbo.aspnet_SchemaVersions( Feature, CompatibleSchemaVersion, IsCurrentVersion )
    VALUES( LOWER( @Feature ), @CompatibleSchemaVersion, @IsCurrentVersion )
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_CreateRole]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Roles_CreateRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    EXEC dbo.aspnet_Applications_CreateApplication @ApplicationName, @ApplicationId OUTPUT

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF (EXISTS(SELECT RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId))
    BEGIN
        SET @ErrorCode = 1
        GOTO Cleanup
    END

    INSERT INTO dbo.aspnet_Roles
                (ApplicationId, RoleName, LoweredRoleName)
         VALUES (@ApplicationId, @RoleName, LOWER(@RoleName))

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_DeleteRole]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_DeleteRole]
    @ApplicationName            nvarchar(256),
    @RoleName                   nvarchar(256),
    @DeleteOnlyIfRoleIsEmpty    bit
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)

    DECLARE @ErrorCode     int
    SET @ErrorCode = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
        BEGIN TRANSACTION
        SET @TranStarted = 1
    END
    ELSE
        SET @TranStarted = 0

    DECLARE @RoleId   uniqueidentifier
    SELECT  @RoleId = NULL
    SELECT  @RoleId = RoleId FROM dbo.aspnet_Roles WHERE LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
    BEGIN
        SELECT @ErrorCode = 1
        GOTO Cleanup
    END
    IF (@DeleteOnlyIfRoleIsEmpty <> 0)
    BEGIN
        IF (EXISTS (SELECT RoleId FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId))
        BEGIN
            SELECT @ErrorCode = 2
            GOTO Cleanup
        END
    END


    DELETE FROM dbo.aspnet_UsersInRoles  WHERE @RoleId = RoleId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    DELETE FROM dbo.aspnet_Roles WHERE @RoleId = RoleId  AND ApplicationId = @ApplicationId

    IF( @@ERROR <> 0 )
    BEGIN
        SET @ErrorCode = -1
        GOTO Cleanup
    END

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        COMMIT TRANSACTION
    END

    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_GetAllRoles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_GetAllRoles] (
    @ApplicationName           nvarchar(256))
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN
    SELECT RoleName
    FROM   dbo.aspnet_Roles WHERE ApplicationId = @ApplicationId
    ORDER BY RoleName
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Roles_RoleExists]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Roles_RoleExists]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(0)
    IF (EXISTS (SELECT RoleName FROM dbo.aspnet_Roles WHERE LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId ))
        RETURN(1)
    ELSE
        RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RemoveAllRoleMembers]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Setup_RemoveAllRoleMembers]
    @name   sysname
AS
BEGIN
    CREATE TABLE #aspnet_RoleMembers
    (
        Group_name      sysname,
        Group_id        smallint,
        Users_in_group  sysname,
        User_id         smallint
    )

    INSERT INTO #aspnet_RoleMembers
    EXEC sp_helpuser @name

    DECLARE @user_id smallint
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT User_id FROM #aspnet_RoleMembers

    OPEN c1

    FETCH c1 INTO @user_id
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = 'EXEC sp_droprolemember ' + '''' + @name + ''', ''' + USER_NAME(@user_id) + ''''
        EXEC (@cmd)
        FETCH c1 INTO @user_id
    END

    CLOSE c1
    DEALLOCATE c1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Setup_RestorePermissions]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Setup_RestorePermissions]
    @name   sysname
AS
BEGIN
    DECLARE @object sysname
    DECLARE @protectType char(10)
    DECLARE @action varchar(60)
    DECLARE @grantee sysname
    DECLARE @cmd nvarchar(500)
    DECLARE c1 cursor FORWARD_ONLY FOR
        SELECT Object, ProtectType, [Action], Grantee FROM #aspnet_Permissions where Object = @name

    OPEN c1

    FETCH c1 INTO @object, @protectType, @action, @grantee
    WHILE (@@fetch_status = 0)
    BEGIN
        SET @cmd = @protectType + ' ' + @action + ' on ' + @object + ' TO [' + @grantee + ']'
        EXEC (@cmd)
        FETCH c1 INTO @object, @protectType, @action, @grantee
    END

    CLOSE c1
    DEALLOCATE c1
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UnRegisterSchemaVersion]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UnRegisterSchemaVersion]
    @Feature                   nvarchar(128),
    @CompatibleSchemaVersion   nvarchar(128)
AS
BEGIN
    DELETE FROM dbo.aspnet_SchemaVersions
        WHERE   Feature = LOWER(@Feature) AND @CompatibleSchemaVersion = CompatibleSchemaVersion
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Users_CreateUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_Users_CreateUser]
    @ApplicationId    uniqueidentifier,
    @UserName         nvarchar(256),
    @IsUserAnonymous  bit,
    @LastActivityDate DATETIME,
    @UserId           uniqueidentifier OUTPUT
AS
BEGIN
    IF( @UserId IS NULL )
        SELECT @UserId = NEWID()
    ELSE
    BEGIN
        IF( EXISTS( SELECT UserId FROM dbo.aspnet_Users
                    WHERE @UserId = UserId ) )
            RETURN -1
    END

    INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
    VALUES (@ApplicationId, @UserId, @UserName, LOWER(@UserName), @IsUserAnonymous, @LastActivityDate)

    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_Users_DeleteUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_Users_DeleteUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @TablesToDeleteFrom int,
    @NumTablesDeletedFrom int OUTPUT
AS
BEGIN
    DECLARE @UserId               uniqueidentifier
    SELECT  @UserId               = NULL
    SELECT  @NumTablesDeletedFrom = 0

    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0

    DECLARE @ErrorCode   int
    DECLARE @RowCount    int

    SET @ErrorCode = 0
    SET @RowCount  = 0

    SELECT  @UserId = u.UserId
    FROM    dbo.aspnet_Users u, dbo.aspnet_Applications a
    WHERE   u.LoweredUserName       = LOWER(@UserName)
        AND u.ApplicationId         = a.ApplicationId
        AND LOWER(@ApplicationName) = a.LoweredApplicationName

    IF (@UserId IS NULL)
    BEGIN
        GOTO Cleanup
    END

    -- Delete from Membership table if (@TablesToDeleteFrom & 1) is set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_MembershipUsers') AND (type = 'V'))))
    BEGIN
        DELETE FROM dbo.aspnet_Membership WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
               @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_UsersInRoles table if (@TablesToDeleteFrom & 2) is set
    IF ((@TablesToDeleteFrom & 2) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_UsersInRoles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_UsersInRoles WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Profile table if (@TablesToDeleteFrom & 4) is set
    IF ((@TablesToDeleteFrom & 4) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_Profiles') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_Profile WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_PersonalizationPerUser table if (@TablesToDeleteFrom & 8) is set
    IF ((@TablesToDeleteFrom & 8) <> 0  AND
        (EXISTS (SELECT name FROM sysobjects WHERE (name = N'vw_aspnet_WebPartState_User') AND (type = 'V'))) )
    BEGIN
        DELETE FROM dbo.aspnet_PersonalizationPerUser WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    -- Delete from aspnet_Users table if (@TablesToDeleteFrom & 1,2,4 & 8) are all set
    IF ((@TablesToDeleteFrom & 1) <> 0 AND
        (@TablesToDeleteFrom & 2) <> 0 AND
        (@TablesToDeleteFrom & 4) <> 0 AND
        (@TablesToDeleteFrom & 8) <> 0 AND
        (EXISTS (SELECT UserId FROM dbo.aspnet_Users WHERE @UserId = UserId)))
    BEGIN
        DELETE FROM dbo.aspnet_Users WHERE @UserId = UserId

        SELECT @ErrorCode = @@ERROR,
                @RowCount = @@ROWCOUNT

        IF( @ErrorCode <> 0 )
            GOTO Cleanup

        IF (@RowCount <> 0)
            SELECT  @NumTablesDeletedFrom = @NumTablesDeletedFrom + 1
    END

    IF( @TranStarted = 1 )
    BEGIN
	    SET @TranStarted = 0
	    COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:
    SET @NumTablesDeletedFrom = 0

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
	    ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_AddUsersToRoles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_AddUsersToRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000),
	@CurrentTimeUtc   datetime
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)
	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames	table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles	table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers	table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num		int
	DECLARE @Pos		int
	DECLARE @NextPos	int
	DECLARE @Name		nvarchar(256)

	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		SELECT TOP 1 Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END

	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1

	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	IF (@@ROWCOUNT <> @Num)
	BEGIN
		DELETE FROM @tbNames
		WHERE LOWER(Name) IN (SELECT LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE au.UserId = u.UserId)

		INSERT dbo.aspnet_Users (ApplicationId, UserId, UserName, LoweredUserName, IsAnonymous, LastActivityDate)
		  SELECT @AppId, NEWID(), Name, LOWER(Name), 0, @CurrentTimeUtc
		  FROM   @tbNames

		INSERT INTO @tbUsers
		  SELECT  UserId
		  FROM	dbo.aspnet_Users au, @tbNames t
		  WHERE   LOWER(t.Name) = au.LoweredUserName AND au.ApplicationId = @AppId
	END

	IF (EXISTS (SELECT * FROM dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr WHERE tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId))
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 dbo.aspnet_UsersInRoles ur, @tbUsers tu, @tbRoles tr, aspnet_Users u, aspnet_Roles r
		WHERE		u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND tu.UserId = ur.UserId AND tr.RoleId = ur.RoleId

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	INSERT INTO dbo.aspnet_UsersInRoles (UserId, RoleId)
	SELECT UserId, RoleId
	FROM @tbUsers, @tbRoles

	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_FindUsersInRole]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_FindUsersInRole]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256),
    @UserNameToMatch  nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId AND LoweredUserName LIKE LOWER(@UserNameToMatch)
    ORDER BY u.UserName
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetRolesForUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetRolesForUser]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(1)

    SELECT r.RoleName
    FROM   dbo.aspnet_Roles r, dbo.aspnet_UsersInRoles ur
    WHERE  r.RoleId = ur.RoleId AND r.ApplicationId = @ApplicationId AND ur.UserId = @UserId
    ORDER BY r.RoleName
    RETURN (0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_GetUsersInRoles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_GetUsersInRoles]
    @ApplicationName  nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(1)
     DECLARE @RoleId uniqueidentifier
     SELECT  @RoleId = NULL

     SELECT  @RoleId = RoleId
     FROM    dbo.aspnet_Roles
     WHERE   LOWER(@RoleName) = LoweredRoleName AND ApplicationId = @ApplicationId

     IF (@RoleId IS NULL)
         RETURN(1)

    SELECT u.UserName
    FROM   dbo.aspnet_Users u, dbo.aspnet_UsersInRoles ur
    WHERE  u.UserId = ur.UserId AND @RoleId = ur.RoleId AND u.ApplicationId = @ApplicationId
    ORDER BY u.UserName
    RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_IsUserInRole]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_IsUserInRole]
    @ApplicationName  nvarchar(256),
    @UserName         nvarchar(256),
    @RoleName         nvarchar(256)
AS
BEGIN
    DECLARE @ApplicationId uniqueidentifier
    SELECT  @ApplicationId = NULL
    SELECT  @ApplicationId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
    IF (@ApplicationId IS NULL)
        RETURN(2)
    DECLARE @UserId uniqueidentifier
    SELECT  @UserId = NULL
    DECLARE @RoleId uniqueidentifier
    SELECT  @RoleId = NULL

    SELECT  @UserId = UserId
    FROM    dbo.aspnet_Users
    WHERE   LoweredUserName = LOWER(@UserName) AND ApplicationId = @ApplicationId

    IF (@UserId IS NULL)
        RETURN(2)

    SELECT  @RoleId = RoleId
    FROM    dbo.aspnet_Roles
    WHERE   LoweredRoleName = LOWER(@RoleName) AND ApplicationId = @ApplicationId

    IF (@RoleId IS NULL)
        RETURN(3)

    IF (EXISTS( SELECT * FROM dbo.aspnet_UsersInRoles WHERE  UserId = @UserId AND RoleId = @RoleId))
        RETURN(1)
    ELSE
        RETURN(0)
END
GO
/****** Object:  StoredProcedure [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[aspnet_UsersInRoles_RemoveUsersFromRoles]
	@ApplicationName  nvarchar(256),
	@UserNames		  nvarchar(4000),
	@RoleNames		  nvarchar(4000)
AS
BEGIN
	DECLARE @AppId uniqueidentifier
	SELECT  @AppId = NULL
	SELECT  @AppId = ApplicationId FROM aspnet_Applications WHERE LOWER(@ApplicationName) = LoweredApplicationName
	IF (@AppId IS NULL)
		RETURN(2)


	DECLARE @TranStarted   bit
	SET @TranStarted = 0

	IF( @@TRANCOUNT = 0 )
	BEGIN
		BEGIN TRANSACTION
		SET @TranStarted = 1
	END

	DECLARE @tbNames  table(Name nvarchar(256) NOT NULL PRIMARY KEY)
	DECLARE @tbRoles  table(RoleId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @tbUsers  table(UserId uniqueidentifier NOT NULL PRIMARY KEY)
	DECLARE @Num	  int
	DECLARE @Pos	  int
	DECLARE @NextPos  int
	DECLARE @Name	  nvarchar(256)
	DECLARE @CountAll int
	DECLARE @CountU	  int
	DECLARE @CountR	  int


	SET @Num = 0
	SET @Pos = 1
	WHILE(@Pos <= LEN(@RoleNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @RoleNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@RoleNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@RoleNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbRoles
	  SELECT RoleId
	  FROM   dbo.aspnet_Roles ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredRoleName AND ar.ApplicationId = @AppId
	SELECT @CountR = @@ROWCOUNT

	IF (@CountR <> @Num)
	BEGIN
		SELECT TOP 1 N'', Name
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT ar.LoweredRoleName FROM dbo.aspnet_Roles ar,  @tbRoles r WHERE r.RoleId = ar.RoleId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(2)
	END


	DELETE FROM @tbNames WHERE 1=1
	SET @Num = 0
	SET @Pos = 1


	WHILE(@Pos <= LEN(@UserNames))
	BEGIN
		SELECT @NextPos = CHARINDEX(N',', @UserNames,  @Pos)
		IF (@NextPos = 0 OR @NextPos IS NULL)
			SELECT @NextPos = LEN(@UserNames) + 1
		SELECT @Name = RTRIM(LTRIM(SUBSTRING(@UserNames, @Pos, @NextPos - @Pos)))
		SELECT @Pos = @NextPos+1

		INSERT INTO @tbNames VALUES (@Name)
		SET @Num = @Num + 1
	END

	INSERT INTO @tbUsers
	  SELECT UserId
	  FROM   dbo.aspnet_Users ar, @tbNames t
	  WHERE  LOWER(t.Name) = ar.LoweredUserName AND ar.ApplicationId = @AppId

	SELECT @CountU = @@ROWCOUNT
	IF (@CountU <> @Num)
	BEGIN
		SELECT TOP 1 Name, N''
		FROM   @tbNames
		WHERE  LOWER(Name) NOT IN (SELECT au.LoweredUserName FROM dbo.aspnet_Users au,  @tbUsers u WHERE u.UserId = au.UserId)

		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(1)
	END

	SELECT  @CountAll = COUNT(*)
	FROM	dbo.aspnet_UsersInRoles ur, @tbUsers u, @tbRoles r
	WHERE   ur.UserId = u.UserId AND ur.RoleId = r.RoleId

	IF (@CountAll <> @CountU * @CountR)
	BEGIN
		SELECT TOP 1 UserName, RoleName
		FROM		 @tbUsers tu, @tbRoles tr, dbo.aspnet_Users u, dbo.aspnet_Roles r
		WHERE		 u.UserId = tu.UserId AND r.RoleId = tr.RoleId AND
					 tu.UserId NOT IN (SELECT ur.UserId FROM dbo.aspnet_UsersInRoles ur WHERE ur.RoleId = tr.RoleId) AND
					 tr.RoleId NOT IN (SELECT ur.RoleId FROM dbo.aspnet_UsersInRoles ur WHERE ur.UserId = tu.UserId)
		IF( @TranStarted = 1 )
			ROLLBACK TRANSACTION
		RETURN(3)
	END

	DELETE FROM dbo.aspnet_UsersInRoles
	WHERE UserId IN (SELECT UserId FROM @tbUsers)
	  AND RoleId IN (SELECT RoleId FROM @tbRoles)
	IF( @TranStarted = 1 )
		COMMIT TRANSACTION
	RETURN(0)
END
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
GO
/****** Object:  StoredProcedure [dbo].[aspnet_WebEvent_LogEvent]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[aspnet_WebEvent_LogEvent]
        @EventId         char(32),
        @EventTimeUtc    datetime,
        @EventTime       datetime,
        @EventType       nvarchar(256),
        @EventSequence   decimal(19,0),
        @EventOccurrence decimal(19,0),
        @EventCode       int,
        @EventDetailCode int,
        @Message         nvarchar(1024),
        @ApplicationPath nvarchar(256),
        @ApplicationVirtualPath nvarchar(256),
        @MachineName    nvarchar(256),
        @RequestUrl      nvarchar(1024),
        @ExceptionType   nvarchar(256),
        @Details         ntext
AS
BEGIN
    INSERT
        dbo.aspnet_WebEvent_Events
        (
            EventId,
            EventTimeUtc,
            EventTime,
            EventType,
            EventSequence,
            EventOccurrence,
            EventCode,
            EventDetailCode,
            Message,
            ApplicationPath,
            ApplicationVirtualPath,
            MachineName,
            RequestUrl,
            ExceptionType,
            Details
        )
    VALUES
    (
        @EventId,
        @EventTimeUtc,
        @EventTime,
        @EventType,
        @EventSequence,
        @EventOccurrence,
        @EventCode,
        @EventDetailCode,
        @Message,
        @ApplicationPath,
        @ApplicationVirtualPath,
        @MachineName,
        @RequestUrl,
        @ExceptionType,
        @Details
    )
END
GO
/****** Object:  StoredProcedure [plaveninycz].[maintain_database]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[maintain_database]
AS
BEGIN
  DBCC DBREINDEX(periods, PK_periods, 85)
  DBCC DBREINDEX(observations2, PK_observations2, 85)
  DBCC DBREINDEX(hydrodata, PK_hydrodata, 85)
END


GO
/****** Object:  StoredProcedure [plaveninycz].[menu_query_sitemap]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[menu_query_sitemap]
@language varchar(12)
AS
BEGIN

  SELECT sm.id, sm.parent_id,
  smd.url, smd.description, smd.title
  FROM languages lng
  INNER JOIN sitemap_details smd
  ON lng.lang_id = smd.lang_id
  INNER JOIN sitemap sm
  ON smd.sitemapnode_id = sm.id
  WHERE lng.lang_abrev like @language + '%'

END

GO
/****** Object:  StoredProcedure [plaveninycz].[menu_query_sitemapbyid]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[menu_query_sitemapbyid]
@language varchar(2),
@id smallint
AS
BEGIN

  SELECT sm.id, sm.parent_id,
  smd.url, 'desc' as 'description', smd.title
  FROM languages lng
  INNER JOIN sitemap_details smd
  ON lng.lang_id = smd.lang_id
  INNER JOIN sitemap sm
  ON smd.sitemapnode_id = sm.id
  WHERE sm.id = @id
  AND lng.lang_abrev like @language + '%'
END

GO
/****** Object:  StoredProcedure [plaveninycz].[new_query_observations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_query_observations] 
	-- Add the parameters for the stored procedure here
	@st_id smallint, 
	@var_id tinyint,
	@start_time smalldatetime,
	@end_time smalldatetime,
	@time_step varchar(10) = 'hour', /* this can be HOUR or DAY */
	@group_function varchar(10) = 'sum' /* this can be AVG, MAX, MIN, SUM, COUNT */
AS
BEGIN
    -- STAGE --
	IF @var_id = 4
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT time_utc as 'obs_time',
					stage_mm * 0.1 as 'obs_value'
					FROM stage
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					ORDER BY time_utc
				END
			ELSE
				BEGIN
					SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
					AVG(stage_mm) * 0.1 as 'obs_value'
					FROM stage
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					GROUP BY
					plaveninycz.DateOnly(time_utc)
					ORDER BY plaveninycz.DateOnly(time_utc)
				END
		END

	-- DISCHARGE --
	ELSE IF @var_id = 5
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT time_utc as 'obs_time',
					discharge_m3s as 'obs_value'
					FROM discharge
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					ORDER BY time_utc
				END
			ELSE
				BEGIN
					SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
					MAX(discharge_m3s) as 'obs_value'
					FROM discharge
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					GROUP BY
					plaveninycz.DateOnly(time_utc)
					ORDER BY plaveninycz.DateOnly(time_utc)
				END
		END
		
	-- TEMPERATURE --
	ELSE IF @var_id = 16
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT time_utc as 'obs_time',
					temperature as 'obs_value'
					FROM temperature
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					ORDER BY time_utc
				END
			ELSE
				BEGIN
					SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
					AVG(temperature) as 'obs_value'
					FROM temperature
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					GROUP BY
					plaveninycz.DateOnly(time_utc)
					ORDER BY plaveninycz.DateOnly(time_utc)
				END
		END
		
	-- TEMPERATURE-MIN --
	ELSE IF @var_id = 17
		BEGIN
		    SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
			MIN(temperature) as 'obs_value'
			FROM temperature
			WHERE station_id = @st_id
			AND (time_utc BETWEEN @start_time AND @end_time)
			GROUP BY
			plaveninycz.DateOnly(time_utc)
			ORDER BY plaveninycz.DateOnly(time_utc)
		END
		
	-- TEMPERATURE-MAX --
	ELSE IF @var_id = 18
		BEGIN
		    SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
			MAX(temperature) as 'obs_value'
			FROM temperature
			WHERE station_id = @st_id
			AND (time_utc BETWEEN @start_time AND @end_time)
			GROUP BY
			plaveninycz.DateOnly(time_utc)
			ORDER BY plaveninycz.DateOnly(time_utc)
		END
		
	-- SNOW --
	ELSE IF @var_id = 8
		BEGIN
		    SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
			snow_cm as 'obs_value'
			FROM snow
			WHERE station_id = @st_id
			AND (time_utc BETWEEN @start_time AND @end_time)
			ORDER BY plaveninycz.DateOnly(time_utc)
		END

	-- HOURLY PCP --
	ELSE
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT rh.time_utc as 'obs_time', rh.rain_mm_10 as 'obs_value'
					FROM rain_hourly rh 
					WHERE rh.station_id = @st_id 
					AND (rh.time_utc BETWEEN @start_time AND @end_time)
					ORDER BY rh.time_utc
				END
			ELSE
				BEGIN
					IF @group_function = 'sum'
						BEGIN
							SELECT plaveninycz.DateOnly(rh.time_utc) as 'obs_time',
							SUM(rh.rain_mm_10) as 'obs_value'
							FROM rain_hourly rh
							WHERE rh.station_id = @st_id
							AND (rh.time_utc BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(rh.time_utc)
							ORDER BY plaveninycz.DateOnly(rh.time_utc)
						END
					ELSE IF @group_function = 'avg'
						BEGIN
							SELECT plaveninycz.DateOnly(rh.time_utc) as 'obs_time',
							AVG(rh.rain_mm_10) as 'obs_value'
							FROM rain_hourly rh
							WHERE rh.station_id = @st_id
							AND (rh.time_utc BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(rh.time_utc)
							ORDER BY plaveninycz.DateOnly(rh.time_utc)
						END
					ELSE IF @group_function = 'max'
						BEGIN
							SELECT plaveninycz.DateOnly(rh.time_utc) as 'obs_time',
							MAX(rh.rain_mm_10) as 'obs_value'
							FROM rain_hourly rh
							WHERE rh.station_id = @st_id
							AND (rh.time_utc BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(rh.time_utc)
							ORDER BY plaveninycz.DateOnly(rh.time_utc)
						END
					ELSE IF @group_function = 'min'
						BEGIN
							SELECT plaveninycz.DateOnly(rh.time_utc) as 'obs_time',
							MIN(rh.rain_mm_10) as 'obs_value'
							FROM rain_hourly rh
							WHERE rh.station_id = @st_id
							AND (rh.time_utc BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(rh.time_utc)
							ORDER BY plaveninycz.DateOnly(rh.time_utc)
						END
				END
		END
END
GO
/****** Object:  StoredProcedure [plaveninycz].[new_query_precipday]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_query_precipday] 
	-- Add the parameters for the stored procedure here
	@station_id smallint = 0, 
	@start_time smalldatetime,
	@end_time smalldatetime
	--@hour_period_start smalldatetime output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @operator smallint
	SELECT @operator = operator_id from stations where st_id = @station_id

	IF @operator BETWEEN 2 AND 6
	-- stations of POVODI --
	BEGIN
		DECLARE @no_hour_start smalldatetime
		SET @no_hour_start = '2008-04-17'
		DECLARE @no_hour_end smalldatetime
		SET @no_hour_end = '2008-08-12'

		DECLARE @hour_period_start smalldatetime
		SELECT @hour_period_start = MIN(start_time)
		FROM periods p INNER JOIN stations st
		ON p.station_id = st.st_id
		WHERE p.station_id = @station_id AND p.variable_id = 1

		-- POVODI: HOURLY data NOT AVAILABLE for the period --
		IF @hour_period_start >= @end_time
		BEGIN
			SELECT rd.time_utc, rd.rain_mm_10
			FROM rain_daily rd
			WHERE rd.station_id = @station_id
			AND rd.time_utc >= @start_time AND rd.time_utc <= @end_time
			ORDER BY rd.time_utc
		END

		-- POVODI: HOURLY data available for WHOLE PERIOD --
		ELSE IF @hour_period_start <= @start_time
		BEGIN
			SELECT plaveninycz.dateonly66(rh.time_utc) as 'obs_time', 
			SUM(rh.rain_mm_10) AS 'obs_value'
			FROM rain_hourly rh
			WHERE rh.station_id = @station_id
			AND rh.rain_mm_10 > 0
			AND (rh.time_utc BETWEEN @start_time AND @end_time)
			AND (rh.time_utc < @no_hour_start OR rh.time_utc > @no_hour_end)
			GROUP BY plaveninycz.dateonly66(rh.time_utc)

			UNION

			SELECT rd.time_utc AS 'obs_time', 
			rd.rain_mm_10 as 'obs_value' 
			FROM rain_daily rd
			WHERE rd.station_id = @station_id
			AND (rd.time_utc BETWEEN @start_time AND @end_time)
			AND (rd.time_utc BETWEEN @no_hour_start AND @no_hour_end)
			ORDER BY 'obs_time'
		END

		-- POVODI: HOURLY data available for PART OF PERIOD --
		ELSE
		BEGIN
			SELECT rd.time_utc AS 'obs_time', rd.rain_mm_10 as 'obs_value' 
			FROM rain_daily rd
			WHERE rd.station_id = @station_id
			AND rd.time_utc >= @start_time AND rd.time_utc < @hour_period_start

			UNION
			
			SELECT plaveninycz.dateonly66(rh.time_utc) as 'obs_time', 
			SUM(rh.rain_mm_10) AS 'obs_value'
			FROM rain_hourly rh
			WHERE rh.station_id = @station_id
			AND rh.rain_mm_10 > 0
			AND rh.time_utc >= @hour_period_start AND rh.time_utc <= @end_time
			AND (rh.time_utc < @no_hour_start OR rh.time_utc > @no_hour_end)
			GROUP BY plaveninycz.dateonly66(rh.time_utc)

			UNION

			SELECT rd.time_utc AS 'obs_time', 
			rd.rain_mm_10 as 'obs_value' 
			FROM rain_daily rd
			WHERE rd.station_id = @station_id
			AND (rd.time_utc BETWEEN @start_time AND @end_time)
			AND (rd.time_utc BETWEEN @no_hour_start AND @no_hour_end)

			ORDER BY 'obs_time'
		END
	END

	ELSE
	BEGIN
		-- CHMI: daily data available for the whole period --
		IF EXISTS (
			SELECT stv.var_id
			FROM stationsvariables stv
			WHERE stv.st_id = @station_id AND stv.var_id = 2 )
		BEGIN
			SELECT rd.time_utc as 'obs_time',
			rd.rain_mm_10 AS 'obs_value'
			FROM rain_daily rd
			WHERE rd.station_id = @station_id
			AND rd.time_utc >= @start_time AND rd.time_utc <= @end_time
			ORDER BY rd.time_utc
		END

		-- CHMI: hourly data available for the whole period --
		ELSE
		BEGIN
			SELECT plaveninycz.dateonly66(rh.time_utc) as 'obs_time2', 
			SUM(rh.rain_mm_10) AS 'obs_value'
			FROM rain_hourly rh
			WHERE rh.station_id = @station_id
			AND rh.rain_mm_10 > 0
			AND rh.time_utc BETWEEN @start_time AND @end_time
			GROUP BY plaveninycz.dateonly66(rh.time_utc)
			ORDER BY 'obs_time2'
		END
	END

	-- STATIONS OF CHMI (todo...) --
		--DAILY DATA AVAILABLE (synop stations)
		--ONLY hourly data (hpps stations)

END






GO
/****** Object:  StoredProcedure [plaveninycz].[new_query_precipstationtable]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* creates a table of CHMI or POVODI hourly precipitation stations */
/* if only_chmu is more than zero, only the chmu stations are selected */
/* otherwise, just the POVODI stations are selected */

CREATE PROCEDURE [plaveninycz].[new_query_precipstationtable]
@only_chmu tinyint
AS

  BEGIN
  IF @only_chmu > 0
    BEGIN
    SELECT st.st_id, st.st_seq, st.st_name, st.operator_id, 
    MAX (time_utc) AS 'max_obs_time'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    left join rain_hourly p
    on st.st_id = p.station_id
    where st.operator_id = 1 AND stvar.var_id=1 
    group by st.st_id, st.st_seq, st.st_name, st.operator_id 
    ORDER BY st.st_id
    END
  ELSE
    BEGIN
    SELECT st.st_id, st.st_seq, st.st_name,st.operator_id,
    MIN(stvar.var_id) AS 'min_variable_id',
    MAX (time_utc) AS 'max_obs_time'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
	and stvar.var_id = 1
    left join rain_hourly p
    on st.st_id = p.station_id
    where
    st.operator_id between 2 and 6
    group by st.st_id, st.st_seq, st.st_name, st.operator_id
    ORDER BY st.st_id
    END
  END



GO
/****** Object:  StoredProcedure [plaveninycz].[new_query_stationshydro_chmi]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_query_stationshydro_chmi]
AS
BEGIN

  SELECT st.st_id, st.st_name, st.st_name2, st.st_seq,st.riv_id,
  Max(h.time_utc) AS latest_obs
  FROM stations st
    LEFT JOIN stage h
    ON st.st_id = h.station_id
  GROUP BY st.st_id, st.st_name, st.st_name2, st.st_seq, st.riv_id
  HAVING ((st.st_name2 is null or st.st_name2 not like '%_%') AND st.riv_id IS NOT NULL)

END
GO
/****** Object:  StoredProcedure [plaveninycz].[new_query_stationshydro_povodi]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_query_stationshydro_povodi]
AS
SELECT st.st_id, st.st_name, st.st_name2,st.riv_id, st.division_name,
Max(h.time_utc) AS latest_obs
FROM stations st
  LEFT JOIN stage h
  ON st.st_id = h.station_id
GROUP BY st.st_id, st.st_name, st.st_name2, st.riv_id, st.division_name
HAVING (st.st_name2 like '%_%' AND st.riv_id IS NOT NULL)

GO
/****** Object:  StoredProcedure [plaveninycz].[new_query_temperaturestations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_query_temperaturestations]
AS

select operator_id, stations.st_id, st_seq, st_name, st_name2, division_name, meteo_code, var_id, max(time_utc) as 'max_obs_time' from stations
left join stationsvariables on stations.st_id = stationsvariables.st_id
left join temperature on temperature.station_id = stations.st_id
where var_id = 16
group by operator_id, stations.st_id, st_seq, st_name, division_name, st_name2, meteo_code, var_id 
order by operator_id
GO
/****** Object:  StoredProcedure [plaveninycz].[new_update_discharge]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_update_discharge]
@station_id smallint = 0,
@obs_time smalldatetime,
@discharge real = null,
@status tinyint output
AS

SET @status = 0

/* add or update the observation     */
IF EXISTS
(
SELECT st_id
FROM [stations]
WHERE st_id = @station_id
)

BEGIN
  IF NOT EXISTS
  (
  SELECT station_id, time_utc
  FROM [discharge]
  WHERE time_utc = @obs_time AND station_id = @station_id
  )
  BEGIN
    INSERT INTO [discharge]
    (station_id, time_utc, discharge_m3s, qualifier_id)
    VALUES(@station_id,@obs_time,@discharge, 1)
    SET @status = 2
  END
  ELSE
  BEGIN
    UPDATE [discharge] SET
    discharge_m3s = @discharge
    WHERE time_utc = @obs_time AND station_id = @station_id
    SET @status = 1
  END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[new_update_log]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_update_log]
@log_time smalldatetime,
@log_text text = null
AS

BEGIN  
  INSERT INTO logging(time_utc, log_text)
  VALUES (@log_time, @log_text)
END
GO
/****** Object:  StoredProcedure [plaveninycz].[new_update_rain_daily]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_update_rain_daily]
@station_id smallint = 0,
@obs_time smalldatetime,
@rain smallint = null,
@status tinyint output
AS

SET @status = 0

/* add or update the observation     */
IF EXISTS
(
SELECT st_id
FROM [stations]
WHERE st_id = @station_id
)

BEGIN
  IF NOT EXISTS
  (
  SELECT station_id, time_utc
  FROM [rain_daily]
  WHERE time_utc = @obs_time AND station_id = @station_id
  )
  BEGIN
    INSERT INTO [rain_daily]
    (station_id, time_utc, rain_mm_10, qualifier_id)
    VALUES(@station_id,@obs_time,@rain, 1)
    SET @status = 2
  END
  ELSE
  BEGIN
    UPDATE [rain_daily] SET
    rain_mm_10 = @rain
    WHERE time_utc = @obs_time AND station_id = @station_id
    SET @status = 1
  END
END
GO
/****** Object:  StoredProcedure [plaveninycz].[new_update_rain_hourly]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_update_rain_hourly]
@observations varchar(800),
@station_id smallint = 0,
@start_time smalldatetime,
@added_rows smallint output
AS
BEGIN
  /* declare helper variables */
  /*@observations example: '111100220000444455550000777788889999'*/

  DECLARE @cur_time smalldatetime
  DECLARE @cur_val smallint
  DECLARE @cur_pos smallint
  DECLARE @end_pos smallint
  
  DECLARE @end_time smalldatetime
  SET @added_rows = 0
  
  SET @end_time = DATEADD(HOUR, (LEN(@observations) / 4), @start_time)
  
  SET @cur_pos = 1
  SET @end_pos = LEN(@observations)
  SET @cur_time = @start_time
  
  WHILE @cur_pos <= @end_pos
  BEGIN
    set @cur_val  = CAST(SUBSTRING(@observations,@cur_pos,4) AS SMALLINT)

    IF @cur_val BETWEEN -9989 AND 9989
    BEGIN
      INSERT INTO rain_hourly(station_id, time_utc, rain_mm_10, qualifier_id)
      values(@station_id, @cur_time, @cur_val, 1)
      SET @added_rows = @added_rows + 1
    END
    
    set @cur_pos =  @cur_pos + 4
    set @cur_time = DATEADD(HOUR,1,@cur_time)
  END
  
  SELECT @cur_val

END
GO
/****** Object:  StoredProcedure [plaveninycz].[new_update_snow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_update_snow]
@station_id smallint = 0,
@obs_time smalldatetime,
@snow smallint = null,
@status tinyint output
AS

SET @status = 0

/* add or update the observation     */
IF EXISTS
(
SELECT st_id
FROM [stations]
WHERE st_id = @station_id
)

BEGIN
  IF NOT EXISTS
  (
  SELECT station_id, time_utc
  FROM [snow]
  WHERE time_utc = @obs_time AND station_id = @station_id
  )
  BEGIN
    INSERT INTO [snow]
    (station_id, time_utc, snow_cm, value_accuracy, qualifier_id)
    VALUES(@station_id,@obs_time,@snow, 0, 1)
    SET @status = 2
  END
  ELSE
  BEGIN
    UPDATE [snow] SET
    snow_cm = @snow
    WHERE time_utc = @obs_time AND station_id = @station_id
    SET @status = 1
  END
END
GO
/****** Object:  StoredProcedure [plaveninycz].[new_update_stage]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[new_update_stage]
@station_id smallint = 0,
@obs_time smalldatetime,
@stage smallint = null,
@status tinyint output
AS

SET @status = 0

/* add or update the observation     */
IF EXISTS
(
SELECT st_id
FROM [stations]
WHERE st_id = @station_id
)

BEGIN
  IF NOT EXISTS
  (
  SELECT station_id, time_utc
  FROM [stage]
  WHERE time_utc = @obs_time AND station_id = @station_id
  )
  BEGIN
    INSERT INTO [stage]
    (station_id, time_utc, stage_mm, qualifier_id)
    VALUES(@station_id,@obs_time,@stage, 1)
    SET @status = 2
  END
  ELSE
  BEGIN
    UPDATE [stage] SET
    stage_mm = @stage
    WHERE time_utc = @obs_time AND station_id = @station_id
    SET @status = 1
  END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[qry_data_hydro]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[qry_data_hydro]
@ivar tinyint = 1,
@idt tinyint = 1,
@st_id smallint,
@t1 smalldatetime,
@t2 smalldatetime
AS

BEGIN
  IF ( @idt = 1 )
    begin
    IF ( @ivar = 1 )
      begin
        SELECT plaveninycz.DateOnly(obs_time) as 'date',
        SUM(klog2_discharge_cms) as 'value'
        FROM hydrodata
        WHERE station_id = @st_id
        AND (obs_time BETWEEN @t1 AND @t2)
        GROUP BY
        plaveninycz.DateOnly(obs_time)
        ORDER BY plaveninycz.DateOnly(obs_time)
      end
    ELSE
      begin
        SELECT plaveninycz.DateOnly(obs_time) as 'date',
        AVG(stage_mm) as 'value'
        FROM hydrodata
        WHERE station_id = @st_id
        AND (obs_time BETWEEN @t1 AND @t2)
        GROUP BY
        plaveninycz.DateOnly(obs_time)
        ORDER BY plaveninycz.DateOnly(obs_time)
      end
    end
  ELSE
    begin
    IF ( @ivar = 1 )
      begin
        SELECT obs_time as 'date',
        klog2_discharge_cms as 'value'
        FROM hydrodata
        WHERE station_id = @st_id
        AND (obs_time BETWEEN @t1 AND @t2)
        ORDER BY obs_time
      end
    ELSE
      begin
        SELECT obs_time as 'date',
        stage_mm as 'value'
        FROM hydrodata
        WHERE station_id = @st_id
        AND (obs_time BETWEEN @t1 AND @t2)
        ORDER BY obs_time
      end
    end
END

GO
/****** Object:  StoredProcedure [plaveninycz].[qry_observations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jiri Kadlec
-- Create date: 2008-03-08
-- Description:	retrieve observation results from a given station and variable for a specified time period
-- =============================================
CREATE PROCEDURE [plaveninycz].[qry_observations] 
	-- Add the parameters for the stored procedure here
	@st_id smallint, 
	@var_id tinyint,
	@start_time smalldatetime,
	@end_time smalldatetime,
	@time_step varchar(10) = 'hour', /* this can be HOUR or DAY */
	@group_function varchar(10) = 'sum' /* this can be AVG, MAX, MIN, SUM, COUNT */
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- DAILY PRECIPITATION --
	IF @var_id = 2
		BEGIN
			EXEC qry_precipday @st_id, @start_time, @end_time
		END	

    -- STAGE --
	ELSE IF @var_id = 4
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT obs_time as 'obs_time',
					stage_mm as 'obs_value'
					FROM hydrodata
					WHERE station_id = @st_id
					AND (obs_time BETWEEN @start_time AND @end_time)
					ORDER BY obs_time
				END
			ELSE
				BEGIN
					SELECT plaveninycz.DateOnly(obs_time) as 'obs_time',
					AVG(stage_mm) as 'obs_value'
					FROM hydrodata
					WHERE station_id = @st_id
					AND (obs_time BETWEEN @start_time AND @end_time)
					GROUP BY
					plaveninycz.DateOnly(obs_time)
					ORDER BY plaveninycz.DateOnly(obs_time)
				END
		END

	-- DISCHARGE --
	ELSE IF @var_id = 5
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT obs_time as 'obs_time',
					klog2_discharge_cms as 'obs_value'
					FROM hydrodata
					WHERE station_id = @st_id
					AND (obs_time BETWEEN @start_time AND @end_time)
					ORDER BY obs_time
				END
			ELSE
				BEGIN
					SELECT plaveninycz.DateOnly(obs_time) as 'obs_time',
					MAX(klog2_discharge_cms) as 'obs_value'
					FROM hydrodata
					WHERE station_id = @st_id
					AND (obs_time BETWEEN @start_time AND @end_time)
					GROUP BY
					plaveninycz.DateOnly(obs_time)
					ORDER BY plaveninycz.DateOnly(obs_time)
				END
		END
		
	-- TEMPERATURE --
	ELSE IF @var_id = 16
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT time_utc as 'obs_time',
					temperature as 'obs_value'
					FROM temperature
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					ORDER BY time_utc
				END
			ELSE
				BEGIN
					SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
					AVG(temperature) as 'obs_value'
					FROM temperature
					WHERE station_id = @st_id
					AND (time_utc BETWEEN @start_time AND @end_time)
					GROUP BY
					plaveninycz.DateOnly(time_utc)
					ORDER BY plaveninycz.DateOnly(time_utc)
				END
		END
		
	-- TEMPERATURE-MIN --
	ELSE IF @var_id = 17
		BEGIN
		    SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
			MIN(temperature) as 'obs_value'
			FROM temperature
			WHERE station_id = @st_id
			AND (time_utc BETWEEN @start_time AND @end_time)
			GROUP BY
			plaveninycz.DateOnly(time_utc)
			ORDER BY plaveninycz.DateOnly(time_utc)
		END
		
	-- TEMPERATURE-MAX --
	ELSE IF @var_id = 18
		BEGIN
		    SELECT plaveninycz.DateOnly(time_utc) as 'obs_time',
			MAX(temperature) as 'obs_value'
			FROM temperature
			WHERE station_id = @st_id
			AND (time_utc BETWEEN @start_time AND @end_time)
			GROUP BY
			plaveninycz.DateOnly(time_utc)
			ORDER BY plaveninycz.DateOnly(time_utc)
		END

	-- OTHER VARIABLES (SNOW, EVAP, SOIL WATER, HOURLY PCP) --
	ELSE
		BEGIN
			IF @time_step = 'hour'
				BEGIN
					SELECT o2.obs_time, o2.obs_value
					FROM periods p
					INNER JOIN observations2 o2
					ON p.period_id = o2.period_id
					WHERE p.station_id = @st_id AND p.variable_id = @var_id
					AND (o2.obs_time BETWEEN @start_time AND @end_time)
					ORDER BY o2.obs_time
				END
			ELSE
				BEGIN
					IF @group_function = 'sum'
						BEGIN
							SELECT plaveninycz.DateOnly(o2.obs_time) as 'obs_time',
							SUM(o2.obs_value) as 'obs_value'
							FROM periods p
							INNER JOIN observations2 o2
							ON p.period_id = o2.period_id
							WHERE p.station_id = @st_id AND p.variable_id = @var_id
							AND (o2.obs_time BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(o2.obs_time)
							ORDER BY plaveninycz.DateOnly(o2.obs_time)
						END
					ELSE IF @group_function = 'avg'
						BEGIN
							SELECT plaveninycz.DateOnly(o2.obs_time) as 'obs_time',
							AVG(o2.obs_value) as 'obs_value'
							FROM periods p
							INNER JOIN observations2 o2
							ON p.period_id = o2.period_id
							WHERE p.station_id = @st_id AND p.variable_id = @var_id
							AND (o2.obs_time BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(o2.obs_time)
							ORDER BY plaveninycz.DateOnly(o2.obs_time)
						END
					ELSE IF @group_function = 'max'
						BEGIN
							SELECT plaveninycz.DateOnly(o2.obs_time) as 'obs_time',
							MAX(o2.obs_value) as 'obs_value'
							FROM periods p
							INNER JOIN observations2 o2
							ON p.period_id = o2.period_id
							WHERE p.station_id = @st_id AND p.variable_id = @var_id
							AND (o2.obs_time BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(o2.obs_time)
							ORDER BY plaveninycz.DateOnly(o2.obs_time)
						END
					ELSE IF @group_function = 'min'
						BEGIN
							SELECT plaveninycz.DateOnly(o2.obs_time) as 'obs_time',
							MIN(o2.obs_value) as 'obs_value'
							FROM periods p
							INNER JOIN observations2 o2
							ON p.period_id = o2.period_id
							WHERE p.station_id = @st_id AND p.variable_id = @var_id
							AND (o2.obs_time BETWEEN @start_time AND @end_time)
							GROUP BY plaveninycz.DateOnly(o2.obs_time)
							ORDER BY plaveninycz.DateOnly(o2.obs_time)
						END
				END
		END
END









GO
/****** Object:  StoredProcedure [plaveninycz].[qry_periods]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		J. Kadlec
-- Create date: 
-- Description:	select periods in given time frame
-- =============================================
CREATE PROCEDURE [plaveninycz].[qry_periods] 
	-- Add the parameters for the stored procedure here
	@st_id smallint = 0, 
	@var_id tinyint = 0,
	@ch_id smallint = 0,
	@start_time smalldatetime,
	@end_time smalldatetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN
		SELECT p.start_time, p.end_time FROM periods p
		WHERE p.station_id = @st_id AND p.variable_id = @var_id
		AND NOT (p.start_time > @end_time OR p.end_time < @start_time)
	END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[qry_periods_all]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jiri Kadlec
-- Create date: 
-- Description:	Select all periods for a given station / variable
-- =============================================
CREATE PROCEDURE [plaveninycz].[qry_periods_all] 
	-- Add the parameters for the stored procedure here
	@st_id smallint = 0, 
	@var_id tinyint = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- special case is precipitation - daily // hourly
	IF @var_id = 2 AND NOT EXISTS
	(SELECT var_id FROM stationsvariables 
		WHERE var_id=@var_id AND st_id = @st_id)
	BEGIN
		SET @var_id = 1
	END
	

	-- stage or discharge
	IF @var_id = 4 OR @var_id = 5
	BEGIN
		SELECT MIN(obs_time) as 'start_time', MAX(obs_time) as 'end_time'
		FROM hydrodata
		WHERE station_id = @st_id
	END
	-- snow, precipitation, soil water..
	ELSE
	BEGIN 
		SELECT start_time, end_time FROM periods
		WHERE station_id = @st_id AND variable_id = @var_id
	END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[qry_periods2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		J. Kadlec
-- Create date: 
-- Description:	select periods in given time frame
-- =============================================
CREATE PROCEDURE [plaveninycz].[qry_periods2] 
	-- Add the parameters for the stored procedure here
	@st_id smallint, 
	@var_id tinyint,
	@start_time smalldatetime,
	@end_time smalldatetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- special case is precipitation - daily // hourly
	IF @var_id = 2 AND NOT EXISTS
	(SELECT var_id FROM stationsvariables 
		WHERE var_id=@var_id AND st_id = @st_id)
	BEGIN
		SET @var_id = 1
	END

	SELECT start_time, end_time FROM periods
	WHERE station_id = @st_id AND variable_id = @var_id
	AND NOT (start_time > @end_time OR end_time < @start_time)
END


GO
/****** Object:  StoredProcedure [plaveninycz].[qry_precipday]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jiri Kadlec
-- Create date: 2008-03-27
-- Modified date: 2008-07-22
-- Description:	load DAILY precipitation data
-- =============================================
CREATE PROCEDURE [plaveninycz].[qry_precipday] 
	-- Add the parameters for the stored procedure here
	@station_id smallint = 0, 
	@start_time smalldatetime,
	@end_time smalldatetime
	--@hour_period_start smalldatetime output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @operator smallint
	SELECT @operator = operator_id from stations where st_id = @station_id

	IF @operator BETWEEN 2 AND 6
	-- stations of POVODI --
	BEGIN
		DECLARE @no_hour_start smalldatetime
		SET @no_hour_start = '2008-04-17'
		DECLARE @no_hour_end smalldatetime
		SET @no_hour_end = '2008-08-12'

		DECLARE @hour_period_start smalldatetime
		SELECT @hour_period_start = MIN(start_time)
		FROM periods p INNER JOIN stations st
		ON p.station_id = st.st_id
		WHERE p.station_id = @station_id AND p.variable_id = 1

		-- POVODI: HOURLY data NOT AVAILABLE for the period --
		IF @hour_period_start >= @end_time
		BEGIN
			SELECT o2.obs_time, o2.obs_value 
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 2
			AND o2.obs_time >= @start_time AND o2.obs_time <= @end_time
			ORDER BY o2.obs_time
		END

		-- POVODI: HOURLY data available for WHOLE PERIOD --
		ELSE IF @hour_period_start <= @start_time
		BEGIN
			SELECT plaveninycz.dateonly66(o2.obs_time) as 'obs_time', 
			SUM(o2.obs_value) AS 'obs_value'
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id
			AND p.variable_id = 1
			AND o2.obs_value > 0
			AND (o2.obs_time BETWEEN @start_time AND @end_time)
			AND (o2.obs_time < @no_hour_start OR o2.obs_time > @no_hour_end)
			GROUP BY plaveninycz.dateonly66(o2.obs_time)

			UNION

			SELECT o2.obs_time AS 'obs_time', 
			o2.obs_value as 'obs_value' 
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 2
			AND (o2.obs_time BETWEEN @start_time AND @end_time)
			AND (o2.obs_time BETWEEN @no_hour_start AND @no_hour_end)
			ORDER BY 'obs_time'
		END

		-- POVODI: HOURLY data available for PART OF PERIOD --
		ELSE
		BEGIN
			SELECT o2.obs_time AS 'obs_time', o2.obs_value as 'obs_value' 
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 2
			AND o2.obs_time >= @start_time AND o2.obs_time < @hour_period_start

			UNION
			
			SELECT plaveninycz.dateonly66(o2.obs_time) as 'obs_time', 
			SUM(o2.obs_value) AS 'obs_value'
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 1
			AND o2.obs_value > 0
			AND o2.obs_time >= @hour_period_start AND o2.obs_time <= @end_time
			AND (o2.obs_time < @no_hour_start OR o2.obs_time > @no_hour_end)
			GROUP BY plaveninycz.dateonly66(o2.obs_time)

			UNION

			SELECT o2.obs_time AS 'obs_time', 
			o2.obs_value as 'obs_value' 
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 2
			AND (o2.obs_time BETWEEN @start_time AND @end_time)
			AND (o2.obs_time BETWEEN @no_hour_start AND @no_hour_end)

			ORDER BY 'obs_time'
		END
	END

	ELSE
	BEGIN
		-- CHMI: daily data available for the whole period --
		IF EXISTS (
			SELECT p.period_id
			FROM periods p 
			WHERE p.station_id = @station_id AND p.variable_id = 2 )
		BEGIN
			SELECT o2.obs_time as 'obs_time',
			o2.obs_value AS 'obs_value'
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 2
			AND o2.obs_time >= @start_time AND o2.obs_time <= @end_time
			ORDER BY o2.obs_time
		END

		-- CHMI: hourly data available for the whole period --
		ELSE
		BEGIN
			SELECT plaveninycz.dateonly66(o2.obs_time) as 'obs_time2', 
			SUM(o2.obs_value) AS 'obs_value'
			FROM observations2 o2
			INNER JOIN periods p
			ON o2.period_id = p.period_id
			WHERE p.station_id = @station_id AND p.variable_id = 1
			AND o2.obs_value > 0
			AND o2.obs_time BETWEEN @start_time AND @end_time
			GROUP BY plaveninycz.dateonly66(o2.obs_time)
			ORDER BY 'obs_time2'
		END
	END

	-- STATIONS OF CHMI (todo...) --
		--DAILY DATA AVAILABLE (synop stations)
		--ONLY hourly data (hpps stations)

END






GO
/****** Object:  StoredProcedure [plaveninycz].[query_day_avg]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* iavg = 1...returns daily average          */
/* iavg <>1...returns values for the given time */

CREATE PROCEDURE [plaveninycz].[query_day_avg]
@st_id smallint,
@var_id tinyint,
@date datetime,
@iavg tinyint
AS
BEGIN
  IF (@iavg = 1)
  BEGIN
    SELECT
    avg(100 * o2.obs_value) as 'value'
    from stations st
    inner join periods p
    on p.station_id = st.st_id
    inner join observations2 o2
    on p.period_id = o2.period_id
    where (st.st_id = @st_id)
    and p.variable_id = @var_id
    and plaveninycz.DateOnly(obs_time) = plaveninycz.DateOnly(@date)
    group by
    plaveninycz.DateOnly(obs_time)
  END
  ELSE
  BEGIN
  SELECT
    o2.obs_value as 'value'
    from stations st
    inner join periods p
    on p.station_id = st.st_id
    inner join observations2 o2
    on p.period_id = o2.period_id
    where (st.st_id = @st_id)
    and p.variable_id = @var_id
    and o2.obs_time = @date
  END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_hydrodata]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_hydrodata]
@st_name varchar(25),
@st_id smallint
AS
BEGIN
  select st.st_name, h.obs_time,
  h.stage_mm,
  h.klog2_discharge_cms as 'discharge_cms'
  from hydrodata h
  inner join stations st
  on h.station_id = st.st_id
  where st.st_name like @st_name
  or st.st_id like @st_id
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_latestradarfile]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_latestradarfile]
@radarnet_name varchar(32) = '',
@radarnet_id tinyint = 0
AS
IF EXISTS
  (
  SELECT radarnet_id
  FROM [radarnetworks]
  WHERE radarnet_id = @radarnet_id OR radarnet_name = @radarnet_name
  )

  BEGIN

    IF @radarnet_id = 0
      BEGIN
        SELECT @radarnet_id = radarnet_id
        FROM [radarnetworks]
        WHERE radarnet_name = @radarnet_name
      END

    SELECT TOP 1 obs_time FROM [radarfiles]
    WHERE radnet_id = @radarnet_id
    ORDER BY obs_time DESC
  END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_longrecordbystation]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [plaveninycz].[query_longrecordbystation]
@longrectype_name varchar(32),
@station_id smallint
AS
BEGIN
  SELECT lr.longrec_value from stations st
  inner join longrecords lr
  on st.st_id = lr.station_id
  inner join longrectypes lrt
  on lr.longrectype_id = lrt.longrectype_id
  where st.st_id = @station_id
  and lrt.longrectype_name = @longrectype_name
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_longrectypes]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_longrectypes]
AS
BEGIN
  SELECT [longrectypes].longrectype_id,[longrectypes].longrectype_name,
        [variables].var_name
        FROM [variables] INNER JOIN [longrectypes]
        ON [variables].var_id = [longrectypes].longrecvar_id
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_maxprecipobs2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_maxprecipobs2]
@obs_time_from smalldatetime,
@variable_id tinyint
AS

/* here, we select only daily precipitation at 6 UT */
IF @variable_id = 2
BEGIN
  SELECT o2.obs_time, MAX(o2.obs_value) AS max_precip
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.variable_id = @variable_id AND o2.obs_time > @obs_time_from
  AND datepart(HOUR,o2.obs_time) = 6 AND p.station_id < 259
  GROUP BY o2.obs_time
  HAVING MAX(o2.obs_value) > 1
  ORDER BY o2.obs_time
END
/* here, we select every hour with precipitation */
ELSE
BEGIN
  SELECT o2.obs_time, MAX(o2.obs_value) AS max_precip
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.variable_id = @variable_id AND o2.obs_time > @obs_time_from
  GROUP BY o2.obs_time
  HAVING MAX(o2.obs_value) > 1
  ORDER BY o2.obs_time
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_menuitemname]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_menuitemname]
@basename varchar(32),
@lang_abrev varchar(2)
AS
BEGIN
  SELECT menu_details.item_name
  FROM   menu_items
           INNER JOIN (languages
             INNER JOIN menu_details
             ON languages.lang_id = menu_details.lang_id)
           ON menu_items.ID = menu_details.menuitem_id
  WHERE (((languages.lang_abrev)=@lang_abrev)
  AND ((menu_items.item_basename)=@basename));
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_menuitems]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_menuitems]
@category_name varchar(20),
@lang_abrev varchar(2)
AS
BEGIN

  SELECT languages.lang_abrev,
         menu_categories.category_name,
         menu_items.item_basename AS 'basename',
         menu_details.item_name,
         menu_items.uristring + '-' + languages.lang_abrev + '.aspx'
         AS 'uristring2',
         'leftmenu' AS 'td_class'
  FROM menu_categories
    INNER JOIN (menu_items
      INNER JOIN (languages
        INNER JOIN menu_details
        ON languages.lang_id = menu_details.lang_id)
      ON menu_items.ID = menu_details.menuitem_id)
    ON menu_categories.id = menu_items.category_id
  WHERE (((languages.lang_abrev)=@lang_abrev)
  AND ((menu_categories.category_name)=@category_name));

END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_menuitemuristring]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_menuitemuristring]
@item_name varchar(32)
AS
BEGIN
  SELECT TOP 1 uristring
  FROM menu_items
  WHERE item_basename = @item_name
  ORDER BY id
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_obsbystation_old]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [plaveninycz].[query_obsbystation_old]
@st_id smallint,
@var_id tinyint,
@date1 smalldatetime,
@date2 smalldatetime
AS

IF @var_id = 2
BEGIN

SELECT plaveninycz.DateOnly66(o2.obs_time) as 'date',
  sum(o2.obs_value) as 'value'
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.station_id=@st_id
  AND p.variable_id IN (SELECT TOP 1 var_id FROM stationsvariables where
  st_id = @st_id and var_id <= 2 order by var_id desc)
  AND plaveninycz.DateOnly66(o2.obs_time) BETWEEN @date1 AND DATEADD(DAY,1,@date2)
  AND o2.obs_value > 0
  GROUP BY plaveninycz.DateOnly66(o2.obs_time)
  ORDER BY plaveninycz.DateOnly66(o2.obs_time)
END

ELSE
BEGIN
  SELECT plaveninycz.DateOnly66(o2.obs_time) as 'date',o2.obs_value as 'value'
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.station_id=@st_id
  AND p.variable_id=@var_id
  AND o2.obs_time BETWEEN @date1 AND DATEADD(DAY,1,@date2)
  AND o2.obs_value > 0
  ORDER BY plaveninycz.DateOnly66(o2.obs_time)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_obsbystation2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [plaveninycz].[query_obsbystation2]

@st_id smallint,
@var_id tinyint,
@date1 smalldatetime,
@date2 smalldatetime
AS

/* daily precipitation, stored as hourly data or 24-h-data */
IF @var_id = 2
BEGIN
SELECT @var_id = MAX( var_id ) FROM stationsvariables where
  st_id = @st_id and var_id <= 2
END

IF @var_id = 1
BEGIN
  SELECT plaveninycz.DateOnly66(o2.obs_time) as 'date',
  SUM(o2.obs_value) as 'value'
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.station_id=@st_id
  AND p.variable_id=@var_id
  AND plaveninycz.DateOnly66(o2.obs_time) BETWEEN @date1 AND DATEADD(DAY,1,@date2)
  AND o2.obs_value > 0
  GROUP BY plaveninycz.DateOnly66(o2.obs_time)
  ORDER BY plaveninycz.DateOnly66(o2.obs_time)
END

ELSE

IF @var_id = 2
BEGIN
  SELECT plaveninycz.DateOnly66(o2.obs_time) as 'date',
  AVG(o2.obs_value) as 'value'
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.station_id=@st_id
  AND p.variable_id=@var_id
  AND plaveninycz.DateOnly66(o2.obs_time) BETWEEN @date1 AND DATEADD(DAY,1,@date2)
  AND o2.obs_value > 0
  GROUP BY plaveninycz.DateOnly66(o2.obs_time)
  ORDER BY plaveninycz.DateOnly66(o2.obs_time)
END

ELSE
BEGIN
  SELECT plaveninycz.DateOnly(o2.obs_time) as 'date',o2.obs_value as 'value'
  FROM observations2 o2
  INNER JOIN periods p
  ON o2.period_id = p.period_id
  WHERE p.station_id=@st_id
  AND p.variable_id=@var_id
  AND o2.obs_time BETWEEN @date1 AND DATEADD(DAY,1,@date2)
  AND o2.obs_value > 0
  ORDER BY plaveninycz.DateOnly(o2.obs_time)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_observationbytime]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_observationbytime]
@station_id  smallint,
@variable_id tinyint,
@obs_time smalldatetime,
@obs_value smallint output
AS
SELECT @obs_value = obs_value FROM [observations]
WHERE station_id = @station_id AND variable_id = @variable_id
AND obs_time = @obs_time
ORDER BY obs_time DESC

GO
/****** Object:  StoredProcedure [plaveninycz].[query_observations_sql]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_observations_sql]
@st_id smallint,
@var_id tinyint
AS
BEGIN
select 
o2.obs_time, o2.obs_value
from stations st
inner join periods p
on p.station_id = st.st_id
inner join observations2 o2
on p.period_id = o2.period_id
where (st.st_id = @st_id)
and p.variable_id = @var_id
order by st.st_id, p.variable_id, o2.obs_time DESC
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_observations2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_observations2]
@st_name varchar(20),
@st_id smallint,
@var_id tinyint
AS
BEGIN
select st.st_name,
p.period_id,p.variable_id,p.start_time,p.end_time,
o2.obs_time, o2.obs_value
from stations st
inner join periods p
on p.station_id = st.st_id
inner join observations2 o2
on p.period_id = o2.period_id
where (st.st_name LIKE @st_name or st.st_id = @st_id)
and p.variable_id = @var_id
order by st.st_name, p.variable_id, o2.obs_time DESC

END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_periods]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* find all periods, that overlap a given interval */
/* @start_time,@end_time means whole day for snow  */
CREATE PROCEDURE [plaveninycz].[query_periods]
@variable_id smallint,
@station_id smallint,
@start_time smalldatetime,
@end_time smalldatetime
AS
BEGIN
  DECLARE @period_start_time smalldatetime
  DECLARE @period_end_time smalldatetime
  DECLARE @interval_h smallint
  SELECT @interval_h = interval_h FROM variables
  WHERE var_id = @variable_id

  IF @interval_h = 24
  BEGIN
    SET @period_start_time = convert(SMALLDATETIME,
    FLOOR(CONVERT(FLOAT, @start_time)))
    SET @period_end_time = convert(SMALLDATETIME,
    FLOOR(CONVERT(FLOAT, @end_time)))
    SET @period_end_time = DATEADD(DAY,1,@period_end_time)
  END
  IF @interval_h = 1
  BEGIN
    SET @period_start_time = DATEADD(HOUR,-1,@period_start_time)
    SET @period_end_time = @end_time
  END

  SELECT start_time, end_time
  FROM periods
  WHERE variable_id = @variable_id
  AND station_id = @station_id
  AND
  (
  (start_time >= @period_start_time AND end_time < @period_end_time)
  OR (start_time <= @period_start_time AND end_time > @period_start_time)
  OR (start_time < @period_end_time AND end_time >= @period_end_time)
  )
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_precipstationtable]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* creates a table of CHMI or POVODI hourly precipitation stations */
/* if only_chmu is more than zero, only the chmu stations are selected */
/* otherwise, just the POVODI stations are selected */

CREATE PROCEDURE [plaveninycz].[query_precipstationtable]
@only_chmu tinyint
AS

  BEGIN
  IF @only_chmu > 0
    BEGIN
    SELECT st.st_id, st.st_seq, st.st_name,
    MAX (p.end_time) AS 'max_obs_time'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    left join periods p
    on st.st_id = p.station_id
    where
    st.operator_id = 1
    and stvar.var_id = 1
    and (p.variable_id = 1 or p.variable_id is null)
    group by st.st_id, st.st_seq, st.st_name
    ORDER BY st.st_id
    END
  ELSE
    BEGIN
    SELECT st.st_id, st.st_seq, st.st_name,
    MIN(p.variable_id) AS 'min_variable_id',
    MAX (p.end_time) AS 'max_obs_time'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
	and stvar.var_id = 1
    left join periods p
    on st.st_id = p.station_id
    where
    st.operator_id between 2 and 6
    and (p.variable_id = 1 or p.variable_id is null)
    group by st.st_id, st.st_seq, st.st_name
    ORDER BY st.st_id
    END
  END



GO
/****** Object:  StoredProcedure [plaveninycz].[query_snow_coord]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_snow_coord]
AS
BEGIN
  SELECT stations.st_id, stations.st_seq, stations.st_name,
    longrecords.longrec_value, longrectypes.longrectype_name
    FROM stations
    INNER JOIN (longrectypes
    INNER JOIN longrecords ON
    longrectypes.longrectype_id = longrecords.longrectype_id)
    ON stations.st_id = longrecords.station_id
  WHERE ((longrectypes.longrectype_name = 'st_coord_topleft'))
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_snowbydate2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_snowbydate2]
@date smalldatetime,
@lang char(2),
@order char(3)
AS
BEGIN
IF @order = 'sta'
BEGIN
  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, o2.obs_value as snow_value,
  plaveninycz.snow_str(o2.obs_value,@lang) as snow_string,
  plaveninycz.snow_tdclass(isnull(o2.obs_value,0)) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  inner join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  inner join observations2 o2
  on p.period_id = o2.period_id
  where o2.obs_time between @date and dateadd(hour,23,@date)

  union

  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, -3 as snow_value, '0' as snow_string,
  plaveninycz.snow_tdclass(0) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  inner join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  where p.period_id not in
  (select period_id from observations2 where
  variable_id = 8 and
  obs_time between @date and dateadd(hour,23,@date))

  union

  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, -9 as snow_value, plaveninycz.nodata_str(@lang) as snow_string,
  plaveninycz.snow_tdclass(-9) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  left join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  where isnull(p.period_id,0) = 0
  order by st.st_name
END

IF @order = 'snw'
BEGIN
  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, o2.obs_value as snow_value,
  plaveninycz.snow_str(o2.obs_value,@lang) as snow_string,
  plaveninycz.snow_tdclass(isnull(o2.obs_value,0)) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  inner join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  inner join observations2 o2
  on p.period_id = o2.period_id
  where o2.obs_time between @date and dateadd(hour,23,@date)

  union

  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, -3 as snow_value, '0' as snow_string,
  plaveninycz.snow_tdclass(0) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  inner join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  where p.period_id not in
  (select period_id from observations2 where
  variable_id = 8 and
  obs_time between @date and dateadd(hour,23,@date))

  union

  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, -9 as snow_value, plaveninycz.nodata_str(@lang) as snow_string,
  plaveninycz.snow_tdclass(-9) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  left join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  where isnull(p.period_id,0) = 0
  order by snow_value desc
END

IF @order = 'alt'
BEGIN
  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, o2.obs_value as snow_value,
  plaveninycz.snow_str(o2.obs_value,@lang) as snow_string,
  plaveninycz.snow_tdclass(isnull(o2.obs_value,0)) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  inner join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  inner join observations2 o2
  on p.period_id = o2.period_id
  where o2.obs_time between @date and dateadd(hour,23,@date)

  union

  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, -3 as snow_value, '0' as snow_string,
  plaveninycz.snow_tdclass(0) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  inner join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  where p.period_id not in
  (select period_id from observations2 where
  variable_id = 8 and
  obs_time between @date and dateadd(hour,23,@date))

  union

  select st.st_name, st.altitude,
  '/snih/' + st.st_uri + '/graf-' + @lang + '.aspx' as uristring,
  p.period_id, -9 as snow_value, plaveninycz.nodata_str(@lang) as snow_string,
  plaveninycz.snow_tdclass(-9) as td_class
  from stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  and stvar.var_id = 8
  left join periods p
  on st.st_id = p.station_id
  and p.variable_id = 8
  and p.start_time <= @date
  and p.end_time >= dateadd(day,1,@date)
  where isnull(p.period_id,0) = 0
  order by st.altitude desc
END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_snowbystation]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_snowbystation]
@st_id smallint,
@date1 smalldatetime,
@date2 smalldatetime
AS
BEGIN
  SELECT obs_time,obs_value
  FROM observations
  WHERE station_id=@st_id
  AND variable_id=8
  AND obs_time BETWEEN @date1 AND DATEADD(DAY,1,@date2)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_station]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* get information about the station! */
CREATE PROCEDURE [plaveninycz].[query_station]
@id smallint = 0,
@var_id tinyint = 0,
@name varchar(32) = '',
@uri varchar(32) = ''
AS
BEGIN
  /* we know station id */
  IF (@id > 0)
    BEGIN
    select st.st_id, st.st_name, st.st_uri from stations st
    WHERE st.st_id = @id
    END
  ELSE
    BEGIN
    IF (@var_id > 0)
      BEGIN
      IF (@name <> '')
        BEGIN
        SELECT st.st_id, st.st_name, st.st_uri from stations st
        INNER join stationsvariables stvar
        ON st.st_id = stvar.st_id
        WHERE st.st_name LIKE '%'+@name+'%'
        END
      ELSE
        BEGIN
        SELECT st.st_id, st.st_name, st.st_uri FROM stations st
        INNER join stationsvariables stvar
        ON st.st_id = stvar.st_id
        WHERE st.st_uri = @uri
        END
      END
    ELSE
      BEGIN
      IF (@name <> '')
        BEGIN
        SELECT st.st_id, st.st_name, st.st_uri from stations st
        WHERE st.st_name LIKE '%'+@name+'%'
        END
      ELSE
        BEGIN
        SELECT st.st_id, st.st_name, st.st_uri FROM stations st
        WHERE st.st_uri = @uri
        END
      END
   END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_station_attrributes]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* select the station atrributes! */

CREATE PROCEDURE [plaveninycz].[query_station_attrributes]
@var_id tinyint = 0,
@st_name varchar(64) = '',
@st_uri varchar(32) = ''

AS
IF @var_id > 2
BEGIN
  SELECT st.st_id, st.st_name, st.tok, st.st_uri
  FROM stations st
  INNER JOIN stationsvariables stvar
  ON st.st_id = stvar.st_id
  WHERE
  ( stvar.var_id = @var_id )
END
ELSE
BEGIN
  SELECT st.st_id, st.st_name, st.tok, st.st_uri
  FROM stations st
  INNER JOIN stationsvariables stvar
  ON st.st_id = stvar.st_id
  WHERE ( st.st_name LIKE '%'+@st_name+'%' or st.st_uri = @st_uri ) AND
  ( stvar.var_id <= @var_id )
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationbycoord]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* select the station id with a given              */
/* st_coord_topleft attribute                      */
/* @coord_type determines the type of coordinates  */
/* (corresponds to longrectype_id in longrectypes) */

CREATE PROCEDURE [plaveninycz].[query_stationbycoord]
@st_coord_topleft int = 0,
@coord_type tinyint = 0
AS

declare @st_coord_topleft2 int
set @st_coord_topleft2 = @st_coord_topleft - 50000

BEGIN

IF ( @coord_type = 25 )
BEGIN
  SELECT st.st_id
  FROM stations st
  INNER JOIN longrecords lr
  ON st.st_id = lr.station_id
  WHERE
  ( lr.longrectype_id = @coord_type ) and
  ( lr.longrec_value
  BETWEEN @st_coord_topleft2 AND @st_coord_topleft2 + 10)
END

ELSE 
BEGIN
  SELECT st.st_id
  FROM stations st
  INNER JOIN longrecords lr
  ON st.st_id = lr.station_id
  WHERE
  ( lr.longrectype_id = @coord_type ) and
  ( lr.longrec_value = @st_coord_topleft )
END

END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationhydro2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_stationhydro2]
@st_seq int
AS
BEGIN

SELECT st.st_id, st.st_seq, st.st_name,
lrt.longrectype_name,
lr.longrec_value AS disch_stage_avg,
Max(h.obs_time) AS latest_obs_time
FROM stations st
  INNER JOIN longrecords lr
  ON st.st_id = lr.station_id
  INNER JOIN longrectypes lrt
  ON lr.longrectype_id = lrt.longrectype_id
  LEFT JOIN hydrodata h
  ON st.st_id = h.station_id
  WHERE (lrt.longrectype_name = 'discharge_avg'
  OR lrt.longrectype_name = 'stage_avg')
  AND st.st_seq = @st_seq
  AND operator_id = 1
  GROUP BY st.st_id, st.st_seq, st.st_name,
  lrt.longrectype_name, lr.longrec_value
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationid]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* select the station id with a given */
/* attribute of [stations] table      */
CREATE PROCEDURE [plaveninycz].[query_stationid]
@st_name2 varchar(64) = '',
@st_name varchar(64) = '',
@st_uri varchar(32) = '',
@st_seq int = 0,
@location_id int = 0,
@st_ind smallint = 0
AS
BEGIN
IF @st_name2 <> ''
  select st_id from stations where st_name2 = @st_name2
IF @st_name <> ''
  select st_id from stations where st_name = @st_name
IF @st_uri <> ''
  select st_id from stations where st_uri = @st_uri
IF @st_seq > 0
  select st_id from stations where st_seq = @st_seq
IF @location_id > 0
  select st_id from stations where location_id = @location_id
IF @st_ind > 0
  select st_id from stations where st_ind = @st_ind
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationid_snow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* select the station id with a given */
/* attribute of [stations] table      */
CREATE PROCEDURE [plaveninycz].[query_stationid_snow]
@st_name varchar(64) = ''
AS
BEGIN
  select st_id from stations where st_name = @st_name AND riv_id IS NULL
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stations_snow_coord]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_stations_snow_coord]
AS
BEGIN
  SELECT stations.st_id, stations.st_seq, stations.st_name,
    longrecords.longrec_value AS st_coord_topleft, longrectypes.longrectype_name
    FROM stations
    INNER JOIN (longrectypes
    INNER JOIN longrecords ON
    longrectypes.longrectype_id = longrecords.longrectype_id)
    ON stations.st_id = longrecords.station_id
  WHERE ((longrectypes.longrectype_name = 'st_coord_topleft'))
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationsbyvariable]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* this procedure selects all station that measure the given variable */
CREATE PROCEDURE [plaveninycz].[query_stationsbyvariable]
@var_id tinyint,
@order tinyint
AS
BEGIN

/* snow or precipitation */
IF ( @var_id = 2 OR @var_id = 8 ) 
BEGIN
  IF ( @order = 1 ) /* order by st. name */
    BEGIN
    SELECT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id = @var_id and stvar.var_id=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    ORDER BY st.st_name
    END
  ELSE
    IF ( @order = 2 ) /* order by location */
    BEGIN
    SELECT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id = @var_id and stvar.var_id=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    ORDER BY st.st_name
    END
  ELSE
    IF ( @order = 3 ) /* order by elevation */
    BEGIN
    SELECT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id = @var_id and stvar.var_id=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    ORDER BY st.altitude DESC
    END
  ELSE
    IF ( @order = 4 ) /* order by length of record */
    BEGIN
    SELECT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id = @var_id and stvar.var_id=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    ORDER BY MIN (p.start_time)
    END
    
END
/* stage or discharge */
ELSE
IF ( @var_id = 4 OR @var_id = 5 )
BEGIN
IF ( @order = 1 ) /* order by st.name */
  BEGIN
  SELECT st.st_name AS 'station', st.tok AS 'location',
  st.altitude AS 'elevation', st.st_uri AS 'url',
  MIN (hd.obs_time) AS 'first_date', MAX (hd.obs_time) AS 'last_date'
  FROM stationsvariables stvar
  INNER JOIN stations st
  ON stvar.st_id = st.st_id
  INNER JOIN hydrodata hd
  ON stvar.st_id = hd.station_id
  WHERE st.st_uri is not null
  group by st.st_name, st.tok, st.altitude, st.st_uri
  ORDER BY st.st_name, st.tok
  END
ELSE
IF ( @order = 2 ) /* order by tok */
  BEGIN
  SELECT st.st_name AS 'station', st.tok AS 'location',
  st.altitude AS 'elevation', st.st_uri AS 'url',
  MIN (hd.obs_time) AS 'first_date', MAX (hd.obs_time) AS 'last_date'
  FROM stationsvariables stvar
  INNER JOIN stations st
  ON stvar.st_id = st.st_id
  INNER JOIN hydrodata hd
  ON stvar.st_id = hd.station_id
  WHERE st.st_uri is not null
  group by st.st_name, st.tok, st.altitude, st.st_uri
  ORDER BY st.tok, st.altitude DESC, st.st_name
  END
ELSE
IF ( @order = 3 ) /* order by elevation */
  BEGIN
  SELECT st.st_name AS 'station', st.tok AS 'location',
  st.altitude AS 'elevation', st.st_uri AS 'url',
  MIN (hd.obs_time) AS 'first_date', MAX (hd.obs_time) AS 'last_date'
  FROM stationsvariables stvar
  INNER JOIN stations st
  ON stvar.st_id = st.st_id
  INNER JOIN hydrodata hd
  ON stvar.st_id = hd.station_id
  WHERE st.st_uri is not null
  group by st.st_name, st.tok, st.altitude, st.st_uri
  ORDER BY st.altitude DESC, st.st_name, st.tok
  END
ELSE
IF ( @order = 4 ) /* order by length of record */
  BEGIN
  SELECT st.st_name AS 'station', st.tok AS 'location',
  st.altitude AS 'elevation', st.st_uri AS 'url',
  MIN (hd.obs_time) AS 'first_date', MAX (hd.obs_time) AS 'last_date'
  FROM stationsvariables stvar
  INNER JOIN stations st
  ON stvar.st_id = st.st_id
  INNER JOIN hydrodata hd
  ON stvar.st_id = hd.station_id
  WHERE st.st_uri is not null
  group by st.st_name, st.tok, st.altitude, st.st_uri
  ORDER BY MIN (hd.obs_time), st.st_name, st.tok
  END
END

/* hourly precipitation */
ELSE
IF ( @var_id = 1 )
BEGIN
  BEGIN
  SELECT st.st_id, st.st_seq,
  MAX (p.end_time) AS 'last_date'
  FROM stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  left join periods p
  on st.st_id = p.station_id
  where p.variable_id = @var_id and stvar.var_id=@var_id
  group by st.st_id, st.st_seq
  UNION
  SELECT st.st_id, st.st_seq,
  plaveninycz.DateOnly(DATEADD(DAY,-5,GETDATE())) AS 'last_date'
  FROM stationsvariables stvar
  inner join stations st
  on stvar.st_id = st.st_id
  where stvar.var_id = @var_id
  and st.st_id NOT IN (
  SELECT station_id FROM periods WHERE variable_id = 1)
  END
  
END

END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationshydro_povodi]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_stationshydro_povodi]
AS
SELECT st.st_id, st.st_name, st.st_name2,st.tok,
lr.longrec_value AS h_avg,
Max(h.obs_time) AS latest_obs
FROM stations st
  INNER JOIN longrecords lr
  ON st.st_id = lr.station_id
  LEFT JOIN hydrodata h
  ON st.st_id = h.station_id
GROUP BY st.st_id, st.st_name, st.st_name2, st.tok,
  lr.longrec_value,lr.longrectype_id
HAVING (lr.longrectype_id=1
AND st.st_name2 like '%_%')

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationsprecip]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* this procedure selects all stations with hourly or daily precip. records! */
/* (daily records are preferred) */
CREATE PROCEDURE [plaveninycz].[query_stationsprecip]
@var_id tinyint,
@order tinyint
AS
SET @var_id = 2
DECLARE @date_to_display smalldatetime
SET @date_to_display = dateadd(dd,-14,getdate())
/* hourly or daily precipitation */
BEGIN
  IF ( @order = 1 ) /* order by st. name */
    BEGIN
    SELECT DISTINCT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id <=@var_id and stvar.var_id<=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    having MAX(p.end_time) > @date_to_display
    ORDER BY st.st_name
    END
  ELSE
    IF ( @order = 2 ) /* order by location */
    BEGIN
    SELECT DISTINCT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id <= @var_id and stvar.var_id<=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    having MAX(p.end_time) > @date_to_display
    ORDER BY st.st_name
    END
  ELSE
    IF ( @order = 3 ) /* order by elevation */
    BEGIN
    SELECT DISTINCT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id <= @var_id and stvar.var_id<=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    having MAX(p.end_time) > @date_to_display
    ORDER BY st.altitude DESC
    END
  ELSE
    IF ( @order = 4 ) /* order by length of record */
    BEGIN
    SELECT DISTINCT st.st_name AS 'station', st.tok AS 'location',
    st.altitude AS 'elevation', st.st_uri AS 'url',
    MIN (p.start_time) AS 'first_date', MAX (p.end_time) AS 'last_date'
    FROM stationsvariables stvar
    inner join stations st
    on stvar.st_id = st.st_id
    inner join periods p
    on st.st_id = p.station_id
    where p.variable_id <= @var_id and stvar.var_id<=@var_id and st.st_uri is not null
    group by st.st_name, st.tok, st.altitude, st.st_uri
    having MAX (p.end_time) > @date_to_display
    ORDER BY MIN (p.start_time)
    END

END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_stationssnow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_stationssnow]
AS
BEGIN
  SELECT stations.st_id, stations.st_name
  FROM stations INNER JOIN stationsvariables
  ON stations.st_id = stationsvariables.st_id
  WHERE stationsvariables.var_id = 8
  ORDER BY stations.st_name
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_statistics]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_statistics]
@start_time datetime,
@end_time datetime,
@st_id smallint,
@var_id tinyint
AS
BEGIN
  select
sum(o2.obs_value) as 'sum',
max(o2.obs_value) as 'max',
count(o2.obs_value) as 'count'
from stations st
inner join periods p
on p.station_id = st.st_id
inner join observations2 o2
on p.period_id = o2.period_id
where st.st_id = @st_id
and p.variable_id = @var_id
and o2.obs_value > 0
and o2.obs_time between @start_time and @end_time

END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_temperaturestations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[query_temperaturestations]
AS

select operator_id, stations.st_id, st_seq, st_name, division_name, meteo_code, var_id, max(time_utc) as 'max_obs_time' from stations
left join stationsvariables on stations.st_id = stationsvariables.st_id
left join temperature on temperature.station_id = stations.st_id
where var_id = 16
group by operator_id, stations.st_id, st_seq, st_name, division_name, meteo_code, var_id 
order by operator_id
GO
/****** Object:  StoredProcedure [plaveninycz].[query_variables]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* get information about all observation variables      */
/* (variable name, units, url, short name) in one table */
CREATE PROCEDURE [plaveninycz].[query_variables]

AS

BEGIN
  SELECT v.var_id, lang.lang_abrev,vd.unit_name as 'units',v.shortname,v.enum,
    v.scalefactor,vd.name,vd.url, v.basevar_id
  FROM variable_details vd
  INNER JOIN variables v
  ON vd.var_id = v.var_id
  INNER JOIN languages lang
  ON vd.lang_id = lang.lang_id
END



GO
/****** Object:  StoredProcedure [plaveninycz].[query_zero_snow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* selects all synoptic stations with */
/* no records of a given variable     */
/* (snow or precip) for a given date  */

CREATE PROCEDURE [plaveninycz].[query_zero_snow]
@date smalldatetime,
@var_id tinyint
AS
BEGIN

  select st.st_id, st.st_name
from stationsvariables stvar
inner join stations st
on stvar.st_id = st.st_id
inner join longrecords lr
on st.st_id = lr.station_id
inner join longrectypes lrt
on lr.longrectype_id = lrt.longrectype_id
where lrt.longrectype_name = 'st_coord_topleft'
and st.st_id not in
(select station_id from periods where
variable_id = @var_id and start_time <= @date and
end_time > @date)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_zero_synop_rain]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* selects all synoptic stations with */
/* no records of a given variable     */
/* (snow or precip) for a given date  */

CREATE PROCEDURE [plaveninycz].[query_zero_synop_rain]
@date smalldatetime,
@var_id tinyint
AS
BEGIN

  select st.st_id, st.st_name
from stationsvariables stvar
inner join stations st
on stvar.st_id = st.st_id
inner join longrecords lr
on st.st_id = lr.station_id
inner join longrectypes lrt
on lr.longrectype_id = lrt.longrectype_id
where lrt.longrectype_name = 'st_coord_topleft'
and st.st_id not in
(select station_id from rain_daily where
time_utc <= @date and
time_utc > @date)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[query_zero_synop_snow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* selects all synoptic stations with */
/* no records of a given variable     */
/* (snow or precip) for a given date  */

CREATE PROCEDURE [plaveninycz].[query_zero_synop_snow]
@date smalldatetime,
@var_id tinyint
AS
BEGIN

  select st.st_id, st.st_name
from stationsvariables stvar
inner join stations st
on stvar.st_id = st.st_id
inner join longrecords lr
on st.st_id = lr.station_id
inner join longrectypes lrt
on lr.longrectype_id = lrt.longrectype_id
where lrt.longrectype_name = 'st_coord_topleft'
and st.st_id not in
(select station_id from snow where
time_utc <= @date and
time_utc > @date)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_configuration]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_configuration]
@config_name varchar(64),
@config_value varchar(128)
AS

DECLARE @config_id smallint
SET @config_id = 0

IF NOT EXISTS
(
SELECT config_name
FROM [configuration]
WHERE config_name = @config_name
)
BEGIN
  SELECT TOP 1 @config_id = config_id
  FROM [configuration]
  ORDER by config_id DESC
  SET @config_id = @config_id + 1
  INSERT INTO [configuration](config_id, config_name,
  config_value)
  VALUES
  (@config_id,@config_name,@config_value)
END

ELSE
BEGIN
  UPDATE [configuration] SET
  config_value = @config_value
  WHERE
  config_name = @config_name
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_hydrodata]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_hydrodata]
@station_id smallint = 0,
@obs_time smalldatetime,
@stage smallint = null,
@discharge smallint = null,
@status tinyint output
AS

SET @status = 0

/* add or update the observation     */
IF EXISTS
(
SELECT st_id
FROM [stations]
WHERE st_id = @station_id
)

BEGIN
  IF NOT EXISTS
  (
  SELECT station_id, obs_time
  FROM [hydrodata]
  WHERE obs_time = @obs_time AND station_id = @station_id
  )
  BEGIN
    INSERT INTO [hydrodata]
    (station_id, obs_time, stage_mm, klog2_discharge_cms)
    VALUES(@station_id,@obs_time,@stage, @discharge)
    SET @status = 2
  END
  ELSE
  BEGIN
    UPDATE [hydrodata] SET
    stage_mm = @stage,
    klog2_discharge_cms = @discharge
    WHERE obs_time = @obs_time AND station_id = @station_id
    SET @status = 1
  END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_longrecord]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_longrecord]
@station_id smallint,
@longrectype_name varchar(64),
@longrec_value smallint
AS
DECLARE @longrec_id smallint
SET @longrec_id = 0

DECLARE @longrectype_id tinyint
SELECT @longrectype_id = longrectype_id
FROM [longrectypes]
WHERE longrectype_name = @longrectype_name

IF NOT EXISTS
(
SELECT longrec_id
FROM [longrecords]
WHERE longrectype_id = @longrectype_id AND
station_id = @station_id
)
BEGIN
  SELECT TOP 1 @longrec_id = longrec_id
  FROM [longrecords]
  ORDER by longrec_id DESC
  SET @longrec_id = @longrec_id + 1

  INSERT INTO [longrecords]
  (longrec_id,station_id,longrectype_id,longrec_value)
  VALUES
  (@longrec_id,@station_id,@longrectype_id,@longrec_value)
END

ELSE
BEGIN
  UPDATE [longrecords] SET
  station_id = @station_id,
  longrectype_id = @longrectype_id,
  longrec_value = @longrec_value
  WHERE longrectype_id = @longrectype_id AND
  station_id = @station_id
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_longrectype]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_longrectype]
@longrecvar_id tinyint,
@longrectype_name varchar(64)

AS

DECLARE @longrectype_id smallint
SET @longrectype_id = 0

IF NOT EXISTS
(
SELECT longrectype_name
FROM [longrectypes]
WHERE longrectype_name = @longrectype_name
)
BEGIN
  SELECT TOP 1 @longrectype_id = longrectype_id
  FROM [longrectypes]
  ORDER by longrectype_id DESC
  SET @longrectype_id = @longrectype_id + 1

  INSERT INTO [longrectypes]
  (longrectype_id,longrecvar_id,longrectype_name)
  VALUES
  (@longrectype_id,@longrecvar_id,@longrectype_name)
END

ELSE
BEGIN
  UPDATE [longrectypes] SET
  longrecvar_id = @longrecvar_id,
  longrectype_name = @longrectype_name
  WHERE
  longrectype_name = @longrectype_name
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_observation]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_observation]
@station_id smallint = 0,
@station_seq int = 0,
@variable_name varchar(64),
@obs_time smalldatetime,
@obs_value smallint,
@nodata_value smallint = -9999
AS
BEGIN
DECLARE @variable_id smallint

IF EXISTS
  (
  SELECT var_id
  FROM [variables]
  WHERE var_name = @variable_name
  )
  AND EXISTS
  (
  SELECT st_id
  FROM stations
  WHERE st_id = @station_id OR st_seq = @station_seq
  )

  BEGIN

    SELECT @variable_id = var_id
    FROM [variables]
    WHERE var_name = @variable_name

    IF @station_id = 0
      BEGIN
        SELECT @station_id = st_id
        FROM [stations]
        WHERE st_seq = @station_seq
      END

    IF NOT EXISTS
      (
      SELECT station_id,obs_time
      FROM [observations]
      WHERE obs_time = @obs_time AND station_id = @station_id
      AND variable_id = @variable_id
      )

      BEGIN
        INSERT INTO [observations]
        (station_id,variable_id,obs_time,obs_value)
        VALUES(@station_id,@variable_id,@obs_time,@obs_value)
      END

    ELSE
      BEGIN
        UPDATE [observations] SET
        obs_value = @obs_value
        WHERE obs_time = @obs_time AND station_id = @station_id
          AND variable_id = @variable_id
          AND (obs_value IS NULL OR obs_value = @nodata_value)
      END

  END
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_observation2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* status: returns 0 if nothing changed */
/*         returns 1 if row was updated */
/*         returns 2 if row was added   */
CREATE PROCEDURE [plaveninycz].[update_observation2]

@station_id smallint = 0,
@variable_name varchar(64),
@obs_time smalldatetime,
@obs_value int,
@status tinyint output
AS

DECLARE @variable_id smallint
DECLARE @interval_h smallint
DECLARE @min_period_end smalldatetime
DECLARE @period_id int
SET @status = 0


IF EXISTS
(
SELECT var_id
FROM [variables]
WHERE var_name = @variable_name
)

BEGIN
  SELECT @variable_id = var_id,
  @interval_h = interval_h
  FROM [variables]
  WHERE var_name = @variable_name

  SET @min_period_end = convert(SMALLDATETIME,
  FLOOR(CONVERT(FLOAT, @obs_time)))

  IF @interval_h = 1
  BEGIN
    SET @min_period_end =
    DATEADD(HOUR, DATEPART(HOUR, @obs_time)-1, @min_period_end)
  END

/* Update end_time of existing period, if necessary */
  IF EXISTS
  (
  SELECT period_id
  FROM periods
  WHERE station_id = @station_id
  AND variable_id = @variable_id
  AND end_time BETWEEN
  DATEADD(MINUTE, -5, @min_period_end)
  AND DATEADD(MINUTE, 5, @min_period_end)
  )

  BEGIN
    UPDATE periods SET
    end_time = DATEADD(HOUR, @interval_h, @min_period_end)
    WHERE station_id = @station_id
    AND variable_id = @variable_id
    AND end_time BETWEEN
    DATEADD(MINUTE, -5, @min_period_end)
    AND DATEADD(MINUTE, 5, @min_period_end)
  END

/* Add new period, if necessary   */
/* This is when there exists no   */
/* period with smaller start_time */
/* and greater end_time and same  */
/* variable and station as the    */
/* current observation            */
/* if period already exists, only */
/* select correct period_id       */
  IF NOT EXISTS
  (
  SELECT period_id
  FROM periods
  WHERE station_id = @station_id
  AND variable_id = @variable_id
  AND start_time < @obs_time
  AND end_time >= @obs_time
  )
  BEGIN
    SELECT TOP 1
    @period_id = period_id
    FROM periods
    ORDER BY period_id DESC

    SET @period_id = @period_id + 1
    INSERT INTO periods(period_id,station_id,variable_id,
    start_time,end_time)
    VALUES(@period_id,@station_id, @variable_id,
    @min_period_end,
    DATEADD(HOUR, @interval_h, @min_period_end))
  END
  ELSE
  BEGIN
    SELECT @period_id = period_id
    FROM periods
    WHERE station_id = @station_id
    AND variable_id = @variable_id
    AND start_time < @obs_time
    AND end_time >= @obs_time
  END

  /* add or update the observation     */
  /* do this only when obs_value <> 0  */
  /* negative obs_value for snow means */
  /* 'nes' or 'pop'                    */
  IF NOT EXISTS
  (
  SELECT period_id, obs_time
  FROM [observations2]
  WHERE obs_time = @obs_time AND period_id = @period_id
  )
  BEGIN
    IF @obs_value <> 0
    BEGIN
      INSERT INTO [observations2]
      (period_id, obs_time, obs_value)
      VALUES(@period_id,@obs_time,@obs_value)
      SET @status = 2
    END
  END
  ELSE
  BEGIN
    IF @obs_value <> 0
    BEGIN
      UPDATE [observations2] SET
      obs_value = @obs_value
      WHERE obs_time = @obs_time AND period_id = @period_id
      SET @status = 1
    END
  END

END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_precip_hour]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_precip_hour]
@observations varchar(800),
@station_id smallint,
@start_time smalldatetime,
@added_rows smallint output
AS
BEGIN
  /* declare helper variables */
  /*SET @observations = '111100220000444455550000777788889999'*/

  DECLARE @cur_time smalldatetime
  DECLARE @cur_val smallint
  DECLARE @cur_pos smallint
  DECLARE @end_pos smallint
  
  DECLARE @end_time smalldatetime
  DECLARE @period_id int
  SET @added_rows = 0
  
  /* update  a period */
  SET @end_time = DATEADD(HOUR, (LEN(@observations) / 4), @start_time)
  
  IF EXISTS
  (
  SELECT period_id
  FROM periods
  WHERE station_id = @station_id
  AND variable_id = 1
  AND end_time BETWEEN
  DATEADD(MINUTE, -65, @start_time)
  AND DATEADD(MINUTE, 5, @end_time)
  )

  BEGIN
    UPDATE periods SET
    end_time = @end_time
    WHERE station_id = @station_id
    AND variable_id = 1
    AND end_time BETWEEN
    DATEADD(MINUTE, -65, @start_time)
    AND DATEADD(MINUTE, 5, @end_time)
  END
  
  /* ... or ADD a new period!! **/
  /* Add new period, if necessary   */
/* This is when there exists no   */
/* period with smaller start_time */
/* and greater end_time and same  */
/* variable and station as the    */
/* current observation            */
/* if period already exists, only */
/* select correct period_id       */
  IF NOT EXISTS
  (
  SELECT period_id
  FROM periods
  WHERE station_id = @station_id
  AND variable_id = 1
  AND start_time < DATEADD(MINUTE, 5, @start_time)
  AND end_time > DATEADD(MINUTE, -5, @end_time)
  )
  BEGIN
    SELECT TOP 1
    @period_id = period_id
    FROM periods
    ORDER BY period_id DESC

    SET @period_id = @period_id + 1
    INSERT INTO periods(period_id,station_id,variable_id,
    start_time,end_time)
    VALUES(@period_id,@station_id, 1,
    @start_time, @end_time)
  END
  ELSE
  BEGIN
    SELECT @period_id = period_id
    FROM periods
    WHERE station_id = @station_id
    AND variable_id = 1
    AND start_time < DATEADD(MINUTE, 5, @start_time)
    AND end_time > DATEADD(MINUTE, -5, @end_time)
  END
  
  
  
  SET @cur_pos = 1
  SET @end_pos = LEN(@observations)
  SET @cur_time = @start_time
  
  WHILE @cur_pos <= @end_pos
  BEGIN
    set @cur_val  = CAST(SUBSTRING(@observations,@cur_pos,4) AS SMALLINT)

    IF @cur_val BETWEEN 1 AND 9998
    BEGIN
      INSERT INTO observations2(period_id, obs_time, obs_value)
      values(@period_id, @cur_time, @cur_val)
      SET @added_rows = @added_rows + 1
    END
    
    IF @cur_val = 9999
    BEGIN
      INSERT INTO observations2(period_id, obs_time, obs_value)
      values(@period_id, @cur_time, -1)
    END
    
    set @cur_pos =  @cur_pos + 4
    set @cur_time = DATEADD(HOUR,1,@cur_time)
  END
  
  SELECT @cur_val

END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_radarfile]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_radarfile]
@obs_time smalldatetime,
@radarnet_name varchar(32) = '',
@radarnet_id tinyint = 0
AS
IF EXISTS
  (
  SELECT radarnet_id
  FROM [radarnetworks]
  WHERE radarnet_id = @radarnet_id OR radarnet_name = @radarnet_name
  )

  BEGIN

    IF @radarnet_id = 0
      BEGIN
        SELECT @radarnet_id = radarnet_id
        FROM [radarnetworks]
        WHERE radarnet_name = @radarnet_name
      END

    IF NOT EXISTS
      (
      SELECT radnet_id,obs_time
      FROM [radarfiles]
      WHERE obs_time = @obs_time AND radnet_id = @radarnet_id
      )

      BEGIN
        INSERT INTO [radarfiles]
        (obs_time,radnet_id)
        VALUES(@obs_time,@radarnet_id)
      END

  END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_radarnetwork]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_radarnetwork]
@radarnet_name varchar(32)
AS
DECLARE @radarnet_id smallint
SET @radarnet_id = 0

IF NOT EXISTS
(
SELECT radarnet_name
FROM [radarnetworks]
WHERE radarnet_name = @radarnet_name
)
BEGIN
  SELECT TOP 1 @radarnet_id = radarnet_id
  FROM [radarnetworks]
  ORDER by radarnet_id DESC
  SET @radarnet_id = @radarnet_id + 1

  INSERT INTO [radarnetworks]
  (radarnet_id,radarnet_name)
  VALUES
  (@radarnet_id,@radarnet_name)
END

GO
/****** Object:  StoredProcedure [plaveninycz].[update_temperature]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [plaveninycz].[update_temperature]
@observations varchar(800),
@station_id smallint = 0,
@start_time smalldatetime,
@added_rows smallint output
AS
BEGIN
  /* declare helper variables */
  /*@observations example: '111100220000444455550000777788889999'*/

  DECLARE @cur_time smalldatetime
  DECLARE @cur_val smallint
  DECLARE @cur_pos smallint
  DECLARE @end_pos smallint
  
  DECLARE @end_time smalldatetime
  SET @added_rows = 0
  
  SET @end_time = DATEADD(HOUR, (LEN(@observations) / 4), @start_time)
  
  SET @cur_pos = 1
  SET @end_pos = LEN(@observations)
  SET @cur_time = @start_time
  
  WHILE @cur_pos <= @end_pos
  BEGIN
    set @cur_val  = CAST(SUBSTRING(@observations,@cur_pos,4) AS SMALLINT)

    IF @cur_val BETWEEN -9989 AND 9989
    BEGIN
      INSERT INTO temperature(station_id, time_utc, temperature)
      values(@station_id, @cur_time, @cur_val)
      SET @added_rows = @added_rows + 1
    END
    
    set @cur_pos =  @cur_pos + 4
    set @cur_time = DATEADD(HOUR,1,@cur_time)
  END
  
  SELECT @cur_val

END
GO
/****** Object:  UserDefinedFunction [plaveninycz].[DateOnly]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  function [plaveninycz].[DateOnly](@DateTime DateTime)
-- Strips out the time portion of any dateTime value.
returns datetime
as
    begin
    return dateadd(dd,0, datediff(dd,0,@DateTime))
    end

GO
/****** Object:  UserDefinedFunction [plaveninycz].[DateOnly66]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  function [plaveninycz].[DateOnly66](@DateTime DateTime)
-- returns date, computed from 6am to 6am
returns datetime
as
    begin
    return dateadd(dd,0, datediff(dd,0,dateadd(hh,-7,@DateTime)))
    end

GO
/****** Object:  UserDefinedFunction [plaveninycz].[DateOnlyNoon]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create  function [plaveninycz].[DateOnlyNoon](@DateTime DateTime)
-- Strips out the time portion of any dateTime value.
returns datetime
as
    begin
    return dateadd(hh,12, dateadd(dd,0, datediff(dd,0,@DateTime)))
    end

GO
/****** Object:  UserDefinedFunction [plaveninycz].[fnc_test]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [plaveninycz].[fnc_test] (
)
RETURNS varchar(7)
AS
BEGIN
  /* Function body */
  declare @return varchar(7)
  set @return = 'ty vole'
  return @return
END

GO
/****** Object:  UserDefinedFunction [plaveninycz].[fnc_testpow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [plaveninycz].[fnc_testpow] (@value smallint
)
RETURNS bigint
AS
BEGIN
  /* Function body */
  return cast(POWER( 2, @value ) as bigint)
END

GO
/****** Object:  UserDefinedFunction [plaveninycz].[nodata_str]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* generates a 'no data' string  */
/* in a specified language       */
CREATE FUNCTION [plaveninycz].[nodata_str] (
@lang char(2))
RETURNS varchar(11)
AS
BEGIN
  declare @result varchar(11)
  IF @lang = 'cz' set @result = ('chybí data')
  IF @lang = 'en' set @result = ('no data')
  IF @lang = 'de' set @result = ('keine Daten')
  RETURN @result
END

GO
/****** Object:  UserDefinedFunction [plaveninycz].[snow_str]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* function used in station list */
/* converts snow value column to */
/* string based on the value     */
/* and language                  */

CREATE FUNCTION [plaveninycz].[snow_str] (
@snow_value smallint,
@lang char(2))
RETURNS varchar(11)
AS
BEGIN
  declare @result varchar(11)
    set @result = convert(varchar(10),0)
    IF @snow_value > 0
      set @result = convert(varchar(10),@snow_value)
    IF @snow_value = -1
    BEGIN
      if @lang = 'cz' set @result = ('nes.')
      if @lang = 'en' set @result = ('residues')
      if @lang = 'de' set @result = ('Schneereste')
    END
    IF @snow_value = -2
    BEGIN
      if @lang = 'cz' set @result = ('pop.')
      if @lang = 'en' set @result = ('< 1')
      if @lang = 'de' set @result = ('< 1')
    END
  RETURN @result
END

GO
/****** Object:  UserDefinedFunction [plaveninycz].[snow_tdclass]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* function used in station list */
/* converts snow value column to */
/* string based on the value and */
/* period_id and language        */

CREATE FUNCTION [plaveninycz].[snow_tdclass] (@snow_value smallint)
RETURNS varchar(10)
AS
BEGIN
  declare @result varchar(10)
  IF @snow_value = -9 set @result = 'nodata'
  IF @snow_value between -8 and 0 set @result = 'tbl'
  IF @snow_value > 0 set @result = 'snw'
  RETURN @result
END

GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](255) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[aspnet_Applications]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Applications](
	[ApplicationName] [nvarchar](256) NOT NULL,
	[LoweredApplicationName] [nvarchar](256) NOT NULL,
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[ApplicationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Membership]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Membership](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[MobilePIN] [nvarchar](16) NULL,
	[Email] [nvarchar](256) NULL,
	[LoweredEmail] [nvarchar](256) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastLoginDate] [datetime] NOT NULL,
	[LastPasswordChangedDate] [datetime] NOT NULL,
	[LastLockoutDate] [datetime] NOT NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NOT NULL,
	[Comment] [ntext] NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Paths]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Paths](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[LoweredPath] [nvarchar](256) NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_PersonalizationAllUsers]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationAllUsers](
	[PathId] [uniqueidentifier] NOT NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_PersonalizationPerUser]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_PersonalizationPerUser](
	[Id] [uniqueidentifier] NOT NULL,
	[PathId] [uniqueidentifier] NULL,
	[UserId] [uniqueidentifier] NULL,
	[PageSettings] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Profile]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Profile](
	[UserId] [uniqueidentifier] NOT NULL,
	[PropertyNames] [ntext] NOT NULL,
	[PropertyValuesString] [ntext] NOT NULL,
	[PropertyValuesBinary] [image] NOT NULL,
	[LastUpdatedDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Roles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Roles](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[LoweredRoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_SchemaVersions]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_SchemaVersions](
	[Feature] [nvarchar](128) NOT NULL,
	[CompatibleSchemaVersion] [nvarchar](128) NOT NULL,
	[IsCurrentVersion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Feature] ASC,
	[CompatibleSchemaVersion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_Users]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_Users](
	[ApplicationId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[LoweredUserName] [nvarchar](256) NOT NULL,
	[MobileAlias] [nvarchar](16) NULL,
	[IsAnonymous] [bit] NOT NULL,
	[LastActivityDate] [datetime] NOT NULL,
PRIMARY KEY NONCLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_UsersInRoles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[aspnet_UsersInRoles](
	[UserId] [uniqueidentifier] NOT NULL,
	[RoleId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[aspnet_WebEvent_Events]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[aspnet_WebEvent_Events](
	[EventId] [char](32) NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [nvarchar](256) NOT NULL,
	[EventSequence] [decimal](19, 0) NOT NULL,
	[EventOccurrence] [decimal](19, 0) NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[Message] [nvarchar](1024) NULL,
	[ApplicationPath] [nvarchar](256) NULL,
	[ApplicationVirtualPath] [nvarchar](256) NULL,
	[MachineName] [nvarchar](256) NOT NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[ExceptionType] [nvarchar](256) NULL,
	[Details] [ntext] NULL,
PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[configuration]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[configuration](
	[config_id] [smallint] NOT NULL,
	[config_name] [varchar](64) NULL,
	[config_value] [varchar](128) NULL,
 CONSTRAINT [PK_parameters] PRIMARY KEY CLUSTERED 
(
	[config_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[discharge]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[discharge](
	[station_id] [smallint] NOT NULL,
	[time_utc] [smalldatetime] NOT NULL,
	[discharge_m3s] [real] NOT NULL,
	[qualifier_id] [tinyint] NULL,
 CONSTRAINT [PK_discharge] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[hydrodata]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[hydrodata](
	[station_id] [smallint] NOT NULL,
	[obs_time] [smalldatetime] NOT NULL,
	[stage_mm] [smallint] NULL,
	[klog2_discharge_cms] [smallint] NULL,
 CONSTRAINT [PK_hydrodata] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[obs_time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[languages]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[languages](
	[lang_id] [tinyint] NOT NULL,
	[lang_abrev] [varchar](2) NOT NULL,
	[lang_name] [varchar](12) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[locations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[locations](
	[loc_id] [smallint] NOT NULL,
	[loc_name] [varchar](32) NULL,
	[lat] [decimal](10, 7) NULL,
	[lon] [decimal](10, 7) NULL,
	[elevation] [smallint] NULL,
	[fg_region_id] [smallint] NULL,
	[country_id] [smallint] NULL,
 CONSTRAINT [PK_locations] PRIMARY KEY CLUSTERED 
(
	[loc_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[logging]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[logging](
	[time_utc] [datetime] NOT NULL,
	[log_text] [text] NOT NULL,
 CONSTRAINT [PK_logging] PRIMARY KEY CLUSTERED 
(
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[longrecords]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[longrecords](
	[longrec_id] [smallint] NOT NULL,
	[station_id] [smallint] NOT NULL,
	[longrectype_id] [tinyint] NOT NULL,
	[longrec_value] [int] NULL,
 CONSTRAINT [PK_longrecords] PRIMARY KEY CLUSTERED 
(
	[longrec_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[longrectypes]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[longrectypes](
	[longrectype_id] [tinyint] NOT NULL,
	[longrecvar_id] [tinyint] NOT NULL,
	[longrectype_name] [varchar](32) NOT NULL,
 CONSTRAINT [PK_longrectypes] PRIMARY KEY CLUSTERED 
(
	[longrectype_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[longrectype_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[menu_categories]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[menu_categories](
	[id] [smallint] NOT NULL,
	[category_name] [varchar](20) NOT NULL,
 CONSTRAINT [PK_menu_categories] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[menu_details]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[menu_details](
	[menuitem_id] [smallint] NOT NULL,
	[lang_id] [tinyint] NOT NULL,
	[item_name] [varchar](24) NOT NULL,
 CONSTRAINT [PK_menu_details] PRIMARY KEY CLUSTERED 
(
	[menuitem_id] ASC,
	[lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[menu_items]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[menu_items](
	[id] [smallint] NOT NULL,
	[category_id] [smallint] NOT NULL,
	[item_basename] [varchar](32) NOT NULL,
	[uristring] [varchar](128) NOT NULL,
 CONSTRAINT [PK_menuitems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[observations2]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[observations2](
	[period_id] [int] NOT NULL,
	[obs_time] [smalldatetime] NOT NULL,
	[obs_value] [int] NOT NULL,
 CONSTRAINT [PK_observations2] PRIMARY KEY CLUSTERED 
(
	[period_id] ASC,
	[obs_time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[observstationdates]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[observstationdates](
	[stid] [int] NOT NULL,
	[varid] [int] NOT NULL,
	[last_date] [datetime] NULL,
	[start_date] [datetime] NULL,
 CONSTRAINT [PK_obs_lastdates] PRIMARY KEY CLUSTERED 
(
	[stid] ASC,
	[varid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[operator]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[operator](
	[id] [smallint] NOT NULL,
	[name] [varchar](24) NULL,
	[name2] [varchar](24) NULL,
	[url] [varchar](32) NULL,
 CONSTRAINT [PK_operator] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[periods]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[periods](
	[period_id] [int] NOT NULL,
	[variable_id] [tinyint] NOT NULL,
	[station_id] [smallint] NOT NULL,
	[start_time] [smalldatetime] NOT NULL,
	[end_time] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_periods] PRIMARY KEY CLUSTERED 
(
	[period_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 85) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[qualifiers]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[qualifiers](
	[qualifier_id] [tinyint] NOT NULL,
	[qualifier_name] [varchar](16) NOT NULL,
 CONSTRAINT [PK_qualifiers] PRIMARY KEY CLUSTERED 
(
	[qualifier_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[radarfiles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[radarfiles](
	[obs_time] [smalldatetime] NOT NULL,
	[radnet_id] [tinyint] NOT NULL,
 CONSTRAINT [PK_radarfiles] PRIMARY KEY CLUSTERED 
(
	[obs_time] ASC,
	[radnet_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[radarnetworks]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[radarnetworks](
	[radarnet_id] [tinyint] NOT NULL,
	[radarnet_name] [varchar](32) NULL,
 CONSTRAINT [PK_radarnetworks] PRIMARY KEY CLUSTERED 
(
	[radarnet_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[rain_daily]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[rain_daily](
	[station_id] [smallint] NOT NULL,
	[time_utc] [smalldatetime] NOT NULL,
	[rain_mm_10] [smallint] NOT NULL,
	[qualifier_id] [tinyint] NOT NULL,
 CONSTRAINT [PK_rain_daily] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[rain_hourly]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[rain_hourly](
	[station_id] [smallint] NOT NULL,
	[time_utc] [smalldatetime] NOT NULL,
	[rain_mm_10] [smallint] NOT NULL,
	[qualifier_id] [tinyint] NULL,
 CONSTRAINT [PK_rain_hourly] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[river]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[river](
	[riv_id] [bigint] NOT NULL,
	[recip_id] [bigint] NULL,
	[riv_name] [varchar](64) NULL,
	[riv_name2] [varchar](64) NULL,
	[riv_url] [varchar](24) NULL,
	[start_elev] [smallint] NULL,
	[end_elev] [smallint] NULL,
	[riv_len_m] [int] NULL,
	[recip_name] [varchar](64) NULL,
 CONSTRAINT [PK_rivers] PRIMARY KEY CLUSTERED 
(
	[riv_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[river_old]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[river_old](
	[riv_id] [bigint] NOT NULL,
	[recip_id] [bigint] NULL,
	[riv_name] [varchar](64) NULL,
	[riv_name2] [varchar](64) NULL,
	[riv_url] [varchar](24) NULL,
	[start_elev] [smallint] NULL,
	[end_elev] [smallint] NULL,
	[riv_len_m] [int] NULL,
	[recip_name] [varchar](64) NULL,
 CONSTRAINT [PK_river] PRIMARY KEY CLUSTERED 
(
	[riv_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[SiteBigTexts]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[SiteBigTexts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TextId] [nvarchar](200) NOT NULL,
	[Text0] [nvarchar](max) NULL,
	[Text1] [nvarchar](max) NULL,
	[ModifiedDate] [datetime] NULL,
	[ChangedCount] [int] NULL,
 CONSTRAINT [PK_SiteBigTexts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[sitemap]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[sitemap](
	[id] [smallint] NOT NULL,
	[keytext] [varchar](64) NULL,
	[parent_id] [smallint] NULL,
	[sort_order] [smallint] NULL,
 CONSTRAINT [PK_sitemap] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[sitemap_details]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[sitemap_details](
	[sitemapnode_id] [smallint] NOT NULL,
	[lang_id] [tinyint] NOT NULL,
	[url] [varchar](32) NOT NULL,
	[title] [varchar](32) NOT NULL,
	[description] [varchar](128) NULL,
 CONSTRAINT [PK_sitemap_details] PRIMARY KEY CLUSTERED 
(
	[sitemapnode_id] ASC,
	[lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[SiteTexts]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[SiteTexts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TextId] [nvarchar](200) NOT NULL,
	[Text0] [nvarchar](500) NULL,
	[Text1] [nvarchar](500) NULL,
	[ModifiedDate] [datetime] NULL,
	[ChangedCount] [int] NULL,
 CONSTRAINT [PK_SiteTexts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[snow]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[snow](
	[station_id] [smallint] NOT NULL,
	[time_utc] [smalldatetime] NOT NULL,
	[snow_cm] [smallint] NOT NULL,
	[value_accuracy] [smallint] NOT NULL,
	[qualifier_id] [tinyint] NOT NULL,
 CONSTRAINT [PK_snow] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[stage]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[stage](
	[station_id] [smallint] NOT NULL,
	[time_utc] [smalldatetime] NOT NULL,
	[stage_mm] [smallint] NOT NULL,
	[qualifier_id] [tinyint] NULL,
 CONSTRAINT [PK_stage] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[stations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[stations](
	[st_id] [smallint] NOT NULL,
	[st_seq] [int] NOT NULL,
	[st_name] [varchar](64) NULL,
	[altitude] [smallint] NULL,
	[tok] [varchar](64) NULL,
	[st_name2] [varchar](64) NULL,
	[location_id] [int] NULL,
	[st_ind] [smallint] NULL,
	[st_uri] [varchar](32) NULL,
	[riv_id] [bigint] NULL,
	[operator_id] [smallint] NULL,
	[division_name] [varchar](32) NULL,
	[meteo_code] [varchar](16) NULL,
	[lat] [decimal](10, 7) NULL,
	[lon] [decimal](10, 7) NULL,
 CONSTRAINT [PK_stations] PRIMARY KEY CLUSTERED 
(
	[st_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[stationsvariables]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[stationsvariables](
	[st_id] [smallint] NOT NULL,
	[var_id] [tinyint] NOT NULL,
	[ch_id] [smallint] NULL,
	[is_public] [bit] NOT NULL,
 CONSTRAINT [PK_stationsvariables] PRIMARY KEY CLUSTERED 
(
	[st_id] ASC,
	[var_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[temperature]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [plaveninycz].[temperature](
	[station_id] [smallint] NOT NULL,
	[time_utc] [smalldatetime] NOT NULL,
	[temperature] [smallint] NOT NULL,
	[qualifier_id] [tinyint] NULL,
 CONSTRAINT [PK_temperature] PRIMARY KEY CLUSTERED 
(
	[station_id] ASC,
	[time_utc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [plaveninycz].[variable_details]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[variable_details](
	[var_id] [tinyint] NOT NULL,
	[lang_id] [tinyint] NOT NULL,
	[name] [varchar](24) NOT NULL,
	[url] [varchar](16) NOT NULL,
	[description] [varchar](64) NULL,
	[unit_name] [varchar](24) NULL,
 CONSTRAINT [PK_variable_details] PRIMARY KEY CLUSTERED 
(
	[var_id] ASC,
	[lang_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [plaveninycz].[variables]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [plaveninycz].[variables](
	[var_id] [tinyint] NOT NULL,
	[var_name] [varchar](64) NOT NULL,
	[var_units] [varchar](32) NOT NULL,
	[scalefactor] [float] NOT NULL,
	[interval_h] [smallint] NULL,
	[shortname] [char](3) NULL,
	[enum] [varchar](16) NULL,
	[basevar_id] [tinyint] NULL,
 CONSTRAINT [PK_variables] PRIMARY KEY CLUSTERED 
(
	[var_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[var_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[vw_aspnet_Applications]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Applications]
  AS SELECT [dbo].[aspnet_Applications].[ApplicationName], [dbo].[aspnet_Applications].[LoweredApplicationName], [dbo].[aspnet_Applications].[ApplicationId], [dbo].[aspnet_Applications].[Description]
  FROM [dbo].[aspnet_Applications]
  
GO
/****** Object:  View [dbo].[vw_aspnet_MembershipUsers]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_MembershipUsers]
  AS SELECT [dbo].[aspnet_Membership].[UserId],
            [dbo].[aspnet_Membership].[PasswordFormat],
            [dbo].[aspnet_Membership].[MobilePIN],
            [dbo].[aspnet_Membership].[Email],
            [dbo].[aspnet_Membership].[LoweredEmail],
            [dbo].[aspnet_Membership].[PasswordQuestion],
            [dbo].[aspnet_Membership].[PasswordAnswer],
            [dbo].[aspnet_Membership].[IsApproved],
            [dbo].[aspnet_Membership].[IsLockedOut],
            [dbo].[aspnet_Membership].[CreateDate],
            [dbo].[aspnet_Membership].[LastLoginDate],
            [dbo].[aspnet_Membership].[LastPasswordChangedDate],
            [dbo].[aspnet_Membership].[LastLockoutDate],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAttemptWindowStart],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptCount],
            [dbo].[aspnet_Membership].[FailedPasswordAnswerAttemptWindowStart],
            [dbo].[aspnet_Membership].[Comment],
            [dbo].[aspnet_Users].[ApplicationId],
            [dbo].[aspnet_Users].[UserName],
            [dbo].[aspnet_Users].[MobileAlias],
            [dbo].[aspnet_Users].[IsAnonymous],
            [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Membership] INNER JOIN [dbo].[aspnet_Users]
      ON [dbo].[aspnet_Membership].[UserId] = [dbo].[aspnet_Users].[UserId]
  
GO
/****** Object:  View [dbo].[vw_aspnet_Profiles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Profiles]
  AS SELECT [dbo].[aspnet_Profile].[UserId], [dbo].[aspnet_Profile].[LastUpdatedDate],
      [DataSize]=  DATALENGTH([dbo].[aspnet_Profile].[PropertyNames])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesString])
                 + DATALENGTH([dbo].[aspnet_Profile].[PropertyValuesBinary])
  FROM [dbo].[aspnet_Profile]
  
GO
/****** Object:  View [dbo].[vw_aspnet_Roles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[aspnet_Roles].[ApplicationId], [dbo].[aspnet_Roles].[RoleId], [dbo].[aspnet_Roles].[RoleName], [dbo].[aspnet_Roles].[LoweredRoleName], [dbo].[aspnet_Roles].[Description]
  FROM [dbo].[aspnet_Roles]
  
GO
/****** Object:  View [dbo].[vw_aspnet_Users]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_Users]
  AS SELECT [dbo].[aspnet_Users].[ApplicationId], [dbo].[aspnet_Users].[UserId], [dbo].[aspnet_Users].[UserName], [dbo].[aspnet_Users].[LoweredUserName], [dbo].[aspnet_Users].[MobileAlias], [dbo].[aspnet_Users].[IsAnonymous], [dbo].[aspnet_Users].[LastActivityDate]
  FROM [dbo].[aspnet_Users]
  
GO
/****** Object:  View [dbo].[vw_aspnet_UsersInRoles]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[aspnet_UsersInRoles].[UserId], [dbo].[aspnet_UsersInRoles].[RoleId]
  FROM [dbo].[aspnet_UsersInRoles]
  
GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_Paths]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Paths]
  AS SELECT [dbo].[aspnet_Paths].[ApplicationId], [dbo].[aspnet_Paths].[PathId], [dbo].[aspnet_Paths].[Path], [dbo].[aspnet_Paths].[LoweredPath]
  FROM [dbo].[aspnet_Paths]
  
GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_Shared]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_Shared]
  AS SELECT [dbo].[aspnet_PersonalizationAllUsers].[PathId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationAllUsers].[PageSettings]), [dbo].[aspnet_PersonalizationAllUsers].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationAllUsers]
  
GO
/****** Object:  View [dbo].[vw_aspnet_WebPartState_User]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

  CREATE VIEW [dbo].[vw_aspnet_WebPartState_User]
  AS SELECT [dbo].[aspnet_PersonalizationPerUser].[PathId], [dbo].[aspnet_PersonalizationPerUser].[UserId], [DataSize]=DATALENGTH([dbo].[aspnet_PersonalizationPerUser].[PageSettings]), [dbo].[aspnet_PersonalizationPerUser].[LastUpdatedDate]
  FROM [dbo].[aspnet_PersonalizationPerUser]
  
GO
/****** Object:  View [plaveninycz].[view_precipstations]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [plaveninycz].[view_precipstations]
AS
SELECT     plaveninycz.operator.name AS operator, plaveninycz.stations.st_name AS station, plaveninycz.stations.st_uri AS url, 
                      plaveninycz.variables.var_name AS variable, plaveninycz.stations.altitude
FROM         plaveninycz.stations LEFT OUTER JOIN
                      plaveninycz.stationsvariables ON plaveninycz.stations.st_id = plaveninycz.stationsvariables.st_id INNER JOIN
                      plaveninycz.operator ON plaveninycz.operator.id = plaveninycz.stations.operator_id INNER JOIN
                      plaveninycz.variables ON plaveninycz.stationsvariables.var_id = plaveninycz.variables.var_id
WHERE     (plaveninycz.variables.var_name <> 'precip_hour') AND (plaveninycz.variables.var_name = 'precip_day') AND 
                      (plaveninycz.operator.name = 'ČHMÚ') OR
                      (plaveninycz.variables.var_name = 'precip_hour' AND plaveninycz.variables.var_name <> 'precip_day') AND (plaveninycz.operator.name = 'ČHMÚ')


GO
/****** Object:  View [plaveninycz].[view_prefered_variables]    Script Date: 12/10/2014 10:15:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [plaveninycz].[view_prefered_variables]
AS
SELECT     st.st_name, st.st_id, MAX(stv.var_id) AS Prefered_variable, st.operator_id
FROM         plaveninycz.stations AS st INNER JOIN
                      plaveninycz.stationsvariables AS stv ON st.st_id = stv.st_id
WHERE     (stv.var_id = 2 OR
                      stv.var_id = 1) AND (st.operator_id = 1)
GROUP BY st.st_name, st.st_id, st.operator_id
UNION
SELECT     st.st_name, st.st_id, stv.var_id AS Prefered_variable, st.operator_id
FROM         plaveninycz.stations AS st INNER JOIN
                      plaveninycz.stationsvariables AS stv ON st.st_id = stv.st_id
WHERE     (stv.var_id = 1) AND (st.operator_id > 1)


GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Applications_Index]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE CLUSTERED INDEX [aspnet_Applications_Index] ON [dbo].[aspnet_Applications]
(
	[LoweredApplicationName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Membership_index]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE CLUSTERED INDEX [aspnet_Membership_index] ON [dbo].[aspnet_Membership]
(
	[ApplicationId] ASC,
	[LoweredEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Paths_index]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Paths_index] ON [dbo].[aspnet_Paths]
(
	[ApplicationId] ASC,
	[LoweredPath] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_PersonalizationPerUser_index1]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_PersonalizationPerUser_index1] ON [dbo].[aspnet_PersonalizationPerUser]
(
	[PathId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Roles_index1]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Roles_index1] ON [dbo].[aspnet_Roles]
(
	[ApplicationId] ASC,
	[LoweredRoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [aspnet_Users_Index]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE UNIQUE CLUSTERED INDEX [aspnet_Users_Index] ON [dbo].[aspnet_Users]
(
	[ApplicationId] ASC,
	[LoweredUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_PersonalizationPerUser_ncindex2]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [aspnet_PersonalizationPerUser_ncindex2] ON [dbo].[aspnet_PersonalizationPerUser]
(
	[UserId] ASC,
	[PathId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_Users_Index2]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE NONCLUSTERED INDEX [aspnet_Users_Index2] ON [dbo].[aspnet_Users]
(
	[ApplicationId] ASC,
	[LastActivityDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [aspnet_UsersInRoles_index]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE NONCLUSTERED INDEX [aspnet_UsersInRoles_index] ON [dbo].[aspnet_UsersInRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_stage]    Script Date: 12/10/2014 10:15:49 AM ******/
CREATE NONCLUSTERED INDEX [IX_stage] ON [plaveninycz].[stage]
(
	[station_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[aspnet_Applications] ADD  DEFAULT (newid()) FOR [ApplicationId]
GO
ALTER TABLE [dbo].[aspnet_Membership] ADD  DEFAULT ((0)) FOR [PasswordFormat]
GO
ALTER TABLE [dbo].[aspnet_Paths] ADD  DEFAULT (newid()) FOR [PathId]
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser] ADD  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[aspnet_Roles] ADD  DEFAULT (newid()) FOR [RoleId]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (newid()) FOR [UserId]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT (NULL) FOR [MobileAlias]
GO
ALTER TABLE [dbo].[aspnet_Users] ADD  DEFAULT ((0)) FOR [IsAnonymous]
GO
ALTER TABLE [plaveninycz].[discharge] ADD  CONSTRAINT [DF_discharge_qualifier_id]  DEFAULT ((1)) FOR [qualifier_id]
GO
ALTER TABLE [plaveninycz].[rain_daily] ADD  CONSTRAINT [DF_rain_daily_qualifier_id]  DEFAULT ((0)) FOR [qualifier_id]
GO
ALTER TABLE [plaveninycz].[snow] ADD  CONSTRAINT [DF_snow_value_accuracy]  DEFAULT ((0)) FOR [value_accuracy]
GO
ALTER TABLE [plaveninycz].[snow] ADD  CONSTRAINT [DF_snow_qualifier_id]  DEFAULT ((1)) FOR [qualifier_id]
GO
ALTER TABLE [plaveninycz].[stationsvariables] ADD  CONSTRAINT [DF_stationsvariables_is_public]  DEFAULT ((0)) FOR [is_public]
GO
ALTER TABLE [plaveninycz].[temperature] ADD  CONSTRAINT [DF_temperature_qualifier_id]  DEFAULT ((1)) FOR [qualifier_id]
GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Membership]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Paths]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationAllUsers]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([PathId])
REFERENCES [dbo].[aspnet_Paths] ([PathId])
GO
ALTER TABLE [dbo].[aspnet_PersonalizationPerUser]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Profile]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[aspnet_Roles]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_Users]  WITH CHECK ADD FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[aspnet_Applications] ([ApplicationId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[aspnet_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[aspnet_UsersInRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [plaveninycz].[discharge]  WITH CHECK ADD  CONSTRAINT [FK_discharge_qualifier] FOREIGN KEY([qualifier_id])
REFERENCES [plaveninycz].[qualifiers] ([qualifier_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[discharge] CHECK CONSTRAINT [FK_discharge_qualifier]
GO
ALTER TABLE [plaveninycz].[hydrodata]  WITH CHECK ADD  CONSTRAINT [FK_hydrodata_stations] FOREIGN KEY([station_id])
REFERENCES [plaveninycz].[stations] ([st_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[hydrodata] CHECK CONSTRAINT [FK_hydrodata_stations]
GO
ALTER TABLE [plaveninycz].[longrecords]  WITH CHECK ADD  CONSTRAINT [FK_longrecords_longrectypes] FOREIGN KEY([longrectype_id])
REFERENCES [plaveninycz].[longrectypes] ([longrectype_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[longrecords] CHECK CONSTRAINT [FK_longrecords_longrectypes]
GO
ALTER TABLE [plaveninycz].[longrecords]  WITH CHECK ADD  CONSTRAINT [FK_longrecords_stations] FOREIGN KEY([station_id])
REFERENCES [plaveninycz].[stations] ([st_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[longrecords] CHECK CONSTRAINT [FK_longrecords_stations]
GO
ALTER TABLE [plaveninycz].[longrectypes]  WITH CHECK ADD  CONSTRAINT [FK_longrectypes_variables] FOREIGN KEY([longrecvar_id])
REFERENCES [plaveninycz].[variables] ([var_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[longrectypes] CHECK CONSTRAINT [FK_longrectypes_variables]
GO
ALTER TABLE [plaveninycz].[menu_details]  WITH CHECK ADD  CONSTRAINT [FK_menudetails_languages] FOREIGN KEY([lang_id])
REFERENCES [plaveninycz].[languages] ([lang_id])
GO
ALTER TABLE [plaveninycz].[menu_details] CHECK CONSTRAINT [FK_menudetails_languages]
GO
ALTER TABLE [plaveninycz].[menu_details]  WITH CHECK ADD  CONSTRAINT [FK_menudetails_menuitems] FOREIGN KEY([menuitem_id])
REFERENCES [plaveninycz].[menu_items] ([id])
GO
ALTER TABLE [plaveninycz].[menu_details] CHECK CONSTRAINT [FK_menudetails_menuitems]
GO
ALTER TABLE [plaveninycz].[menu_items]  WITH CHECK ADD  CONSTRAINT [FK_menuitems_menu_categories] FOREIGN KEY([category_id])
REFERENCES [plaveninycz].[menu_categories] ([id])
GO
ALTER TABLE [plaveninycz].[menu_items] CHECK CONSTRAINT [FK_menuitems_menu_categories]
GO
ALTER TABLE [plaveninycz].[observations2]  WITH CHECK ADD  CONSTRAINT [FK_observations2_periods] FOREIGN KEY([period_id])
REFERENCES [plaveninycz].[periods] ([period_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[observations2] CHECK CONSTRAINT [FK_observations2_periods]
GO
ALTER TABLE [plaveninycz].[periods]  WITH CHECK ADD  CONSTRAINT [FK_periods_stations] FOREIGN KEY([station_id])
REFERENCES [plaveninycz].[stations] ([st_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[periods] CHECK CONSTRAINT [FK_periods_stations]
GO
ALTER TABLE [plaveninycz].[periods]  WITH CHECK ADD  CONSTRAINT [FK_periods_variables] FOREIGN KEY([variable_id])
REFERENCES [plaveninycz].[variables] ([var_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[periods] CHECK CONSTRAINT [FK_periods_variables]
GO
ALTER TABLE [plaveninycz].[radarfiles]  WITH CHECK ADD  CONSTRAINT [FK_radarfiles_radarnetworks] FOREIGN KEY([radnet_id])
REFERENCES [plaveninycz].[radarnetworks] ([radarnet_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[radarfiles] CHECK CONSTRAINT [FK_radarfiles_radarnetworks]
GO
ALTER TABLE [plaveninycz].[rain_daily]  WITH CHECK ADD  CONSTRAINT [FK_rain_d_qualifier] FOREIGN KEY([qualifier_id])
REFERENCES [plaveninycz].[qualifiers] ([qualifier_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[rain_daily] CHECK CONSTRAINT [FK_rain_d_qualifier]
GO
ALTER TABLE [plaveninycz].[rain_hourly]  WITH CHECK ADD  CONSTRAINT [FK_rain_hourly_qualifier] FOREIGN KEY([qualifier_id])
REFERENCES [plaveninycz].[qualifiers] ([qualifier_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[rain_hourly] CHECK CONSTRAINT [FK_rain_hourly_qualifier]
GO
ALTER TABLE [plaveninycz].[sitemap_details]  WITH CHECK ADD  CONSTRAINT [FK_sitemapdetails_languages] FOREIGN KEY([lang_id])
REFERENCES [plaveninycz].[languages] ([lang_id])
GO
ALTER TABLE [plaveninycz].[sitemap_details] CHECK CONSTRAINT [FK_sitemapdetails_languages]
GO
ALTER TABLE [plaveninycz].[sitemap_details]  WITH CHECK ADD  CONSTRAINT [FK_sitemapdetails_sitemap] FOREIGN KEY([sitemapnode_id])
REFERENCES [plaveninycz].[sitemap] ([id])
GO
ALTER TABLE [plaveninycz].[sitemap_details] CHECK CONSTRAINT [FK_sitemapdetails_sitemap]
GO
ALTER TABLE [plaveninycz].[snow]  WITH CHECK ADD  CONSTRAINT [FK_snow_qualifier] FOREIGN KEY([qualifier_id])
REFERENCES [plaveninycz].[qualifiers] ([qualifier_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[snow] CHECK CONSTRAINT [FK_snow_qualifier]
GO
ALTER TABLE [plaveninycz].[stage]  WITH CHECK ADD  CONSTRAINT [FK_stage_qualifier] FOREIGN KEY([qualifier_id])
REFERENCES [plaveninycz].[qualifiers] ([qualifier_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[stage] CHECK CONSTRAINT [FK_stage_qualifier]
GO
ALTER TABLE [plaveninycz].[stations]  WITH CHECK ADD  CONSTRAINT [FK_stations_operator] FOREIGN KEY([operator_id])
REFERENCES [plaveninycz].[operator] ([id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[stations] CHECK CONSTRAINT [FK_stations_operator]
GO
ALTER TABLE [plaveninycz].[stations]  WITH CHECK ADD  CONSTRAINT [FK_stations_rivers] FOREIGN KEY([riv_id])
REFERENCES [plaveninycz].[river] ([riv_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[stations] CHECK CONSTRAINT [FK_stations_rivers]
GO
ALTER TABLE [plaveninycz].[stationsvariables]  WITH CHECK ADD  CONSTRAINT [FK_stationsvariables_stations] FOREIGN KEY([st_id])
REFERENCES [plaveninycz].[stations] ([st_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[stationsvariables] CHECK CONSTRAINT [FK_stationsvariables_stations]
GO
ALTER TABLE [plaveninycz].[stationsvariables]  WITH CHECK ADD  CONSTRAINT [FK_stationsvariables_variables] FOREIGN KEY([var_id])
REFERENCES [plaveninycz].[variables] ([var_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[stationsvariables] CHECK CONSTRAINT [FK_stationsvariables_variables]
GO
ALTER TABLE [plaveninycz].[temperature]  WITH CHECK ADD  CONSTRAINT [FK_temperature_qualifier] FOREIGN KEY([qualifier_id])
REFERENCES [plaveninycz].[qualifiers] ([qualifier_id])
ON UPDATE CASCADE
GO
ALTER TABLE [plaveninycz].[temperature] CHECK CONSTRAINT [FK_temperature_qualifier]
GO
ALTER TABLE [plaveninycz].[variable_details]  WITH CHECK ADD  CONSTRAINT [FK_variabledetails_languages] FOREIGN KEY([lang_id])
REFERENCES [plaveninycz].[languages] ([lang_id])
GO
ALTER TABLE [plaveninycz].[variable_details] CHECK CONSTRAINT [FK_variabledetails_languages]
GO
ALTER TABLE [plaveninycz].[variable_details]  WITH CHECK ADD  CONSTRAINT [FK_variabledetails_variables] FOREIGN KEY([var_id])
REFERENCES [plaveninycz].[variables] ([var_id])
GO
ALTER TABLE [plaveninycz].[variable_details] CHECK CONSTRAINT [FK_variabledetails_variables]
GO
ALTER TABLE [plaveninycz].[variables]  WITH CHECK ADD  CONSTRAINT [FK_variables_variables] FOREIGN KEY([basevar_id])
REFERENCES [plaveninycz].[variables] ([var_id])
GO
ALTER TABLE [plaveninycz].[variables] CHECK CONSTRAINT [FK_variables_variables]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'discharge_cms = 1000 * log2(discharge_measured);
discharge_measured = 2^(discharge_cms/1000)' , @level0type=N'SCHEMA',@level0name=N'plaveninycz', @level1type=N'TABLE',@level1name=N'hydrodata', @level2type=N'COLUMN',@level2name=N'klog2_discharge_cms'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "stations"
            Begin Extent = 
               Top = 8
               Left = 218
               Bottom = 234
               Right = 370
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "stationsvariables"
            Begin Extent = 
               Top = 6
               Left = 431
               Bottom = 91
               Right = 583
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "operator"
            Begin Extent = 
               Top = 13
               Left = 26
               Bottom = 128
               Right = 178
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "variables"
            Begin Extent = 
               Top = 6
               Left = 621
               Bottom = 121
               Right = 773
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
  ' , @level0type=N'SCHEMA',@level0name=N'plaveninycz', @level1type=N'VIEW',@level1name=N'view_precipstations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'    End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'plaveninycz', @level1type=N'VIEW',@level1name=N'view_precipstations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'plaveninycz', @level1type=N'VIEW',@level1name=N'view_precipstations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4[30] 2[40] 3) )"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 3
   End
   Begin DiagramPane = 
      PaneHidden = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 5
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'plaveninycz', @level1type=N'VIEW',@level1name=N'view_prefered_variables'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'plaveninycz', @level1type=N'VIEW',@level1name=N'view_prefered_variables'
GO
USE [master]
GO
ALTER DATABASE [db1856] SET  READ_WRITE 
GO
