drop procedure PressCalc

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE PressCalc 
	@Impressions int = 0,
	@Material    varchar(50)
 AS
BEGIN
	SET NOCOUNT ON;

	Select top 1 Documents as X1    from EquipmentProductionRates where Documents  >= @Impressions order by Documents;
--  x2 = double.Parse(SqlHandler(str));
    Select top 1 Rate      as y2    from EquipmentProductionRates where Documents  >= @Impressions order by Documents;
--  y2 = double.Parse(SqlHandler(str));
    Select top 1 Documents as x1    from EquipmentProductionRates where Documents   < @Impressions order by documents desc;
--  x1 = double.Parse(SqlHandler(str));
    Select top 1 Rate      as y1    from EquipmentProductionRates where Documents   < @Impressions order by documents desc;
--  y1 = double.Parse(SqlHandler(str));
    Select costperfactor   as Cost  from MasterInventory          where Description like @Material + '%';
--  Cost = double.Parse(SqlHandler(str));
	Select ProdType        as Stock     from MasterInventory      where Description like @Material + '%';

	Declare @PaperType nchar(6);
	Declare Cur CURSOR FOR
    Select ItemType                     from MasterInventory      where Description like @Material + '%';
	Open Cur;
	FETCH NEXT FROM Cur INTO @PaperType;
	Select @PaperType AS PaperType;
	CLOSE Cur;
	DEALLOCATE Cur;

	Declare @Size nchar(10);
	Declare Cur2 CURSOR FOR
    Select Size                         from MasterInventory      where Description like @Material + '%';
	Open Cur2;
	FETCH NEXT FROM Cur2 INTO @Size;
	SELECT @Size as Size;
	CLOSE Cur2;
	DEALLOCATE Cur2;

    Select Decimal         as BasicArea from ItemTypes            where ItemType = @PaperType;
	Select SubWT           as BasisWT   from MasterInventory      where Description  like @Material + '%';
	Select Decimal         as PaperWD   from Sizes                where Size     = @Size;

END
GO

-- PressCalc 1000, '24# Capitol Bond wht 10"'
