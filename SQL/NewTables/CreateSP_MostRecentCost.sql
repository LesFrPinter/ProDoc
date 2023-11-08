USE [ProVisionDev]
GO

/****** Object:  StoredProcedure [dbo].[MostRecentCost]    Script Date: 11/8/2023 6:57:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MostRecentCost]
( @Description as VarChar(50) )
AS
BEGIN
SELECT TOP 1
   [Cost_Per] AS MostRecentCost
  FROM PurchasingDetail
 WHERE ReceivedDate IS NOT NULL
   AND MasterID LIKE '%' + @Description + '%'
ORDER BY [PO Number] DESC
END

GO

