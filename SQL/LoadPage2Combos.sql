--   private void LoadItemTypes(string RptType)
--   { string str = $"SELECT DISTINCT ItemType						FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType = '{PAPERTYPE}'";

--   private void LoadSubWT()
--   { string str = $"SELECT DISTINCT CONVERT(int, SubWT) AS SubWT	FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType = '{PAPERTYPE}' AND ItemType = '{ItemType.TrimEnd()}' ORDER BY CONVERT(int, SUBWT)";

--   private void LoadColors()
--   { string str = $"SELECT DISTINCT Color							FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType = '{PAPERTYPE}' AND ItemType = '{ItemType.TrimEnd()}' ORDER BY Color";

--   private void LoadRollWidth()
--   { string str = $"SELECT RollWidth								FROM [ProVisionDev].[dbo].[RollWidth]       WHERE Abbreviation = '{PAPERTYPE}'";

DECLARE @PAPERTYPE as VarChar(10) = 'BOND'
DECLARE @ItemType  as VarChar(10) = 'TAG'

SELECT DISTINCT ItemType					 FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType   = @PAPERTYPE;
SELECT DISTINCT CONVERT(int, SubWT) AS SubWT FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType   = @PAPERTYPE AND ItemType = @ItemType ORDER BY CONVERT(int, SUBWT);
SELECT DISTINCT Color						 FROM [ProVisionDev].[dbo].[MasterInventory] WHERE ReportType   = @PAPERTYPE AND ItemType = @ItemType ORDER BY Color;
SELECT RollWidth							 FROM [ProVisionDev].[dbo].[RollWidth]		 WHERE Abbreviation = @PAPERTYPE;
