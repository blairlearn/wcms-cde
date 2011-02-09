﻿ USE [CDRLiveGK]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetProtocolIdByOldID] ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_GetProtocolIdByOldID]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_GetProtocolIdByOldID]
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROCEDURE [dbo].usp_GetProtocolIdByOldID
(
	@oldID varchar(50)
)

AS
BEGIN
	BEGIN TRY

		SELECT ProtocolID, PrimaryPrettyUrlID OldID
			FROM protocolDetail
			where LOWER(LTRIM(RTRIM(PrimaryPrettyUrlID))) = LOWER(@oldID)
		union all
		SELECT ProtocolID, IDString as OldID
			FROM ProtocolSecondaryUrl
			Where LOWER(LTRIM(RTRIM(IDString))) = LOWER(@oldID)
	END TRY

	BEGIN CATCH
		RETURN 10803
	END CATCH 
END
GO
grant execute on usp_GetProtocolIdByOldID to websiteuser
GO