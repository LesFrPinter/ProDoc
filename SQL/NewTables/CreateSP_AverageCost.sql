USE [ProVisionDev]
GO

/****** Object:  StoredProcedure [dbo].[AverageCost]    Script Date: 11/8/2023 6:56:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AverageCost]
( @Description as VarChar(50) )
AS
BEGIN
SELECT 
  CASE WHEN AVG(PurchasingDetail.Cost_Per) <> 0 
       THEN AVG(PurchasingDetail.Cost_Per) 
	   ELSE 0 END AS AverageCost
  FROM ItemInventory 
   LEFT OUTER JOIN PurchasingDetail 
     ON ItemInventory.PO_Num   = PurchasingDetail.[PO Number]
         AND 
	    ItemInventory.MasterID = PurchasingDetail.MasterID
   LEFT OUTER JOIN MasterInventory
     ON ItemInventory.MasterID = MasterInventory.Description
 WHERE  ItemInventory.MasterID = @Description
 GROUP BY ItemInventory.MasterID
END

GO

