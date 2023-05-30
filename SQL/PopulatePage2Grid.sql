SELECT M.Description,
         M.ItemType,
              M.SubWT,
              M.COLOR AS MCOLOR,
              E.PAPER,
              E.SETNUM,
              E.SEQ,
              E.PTYPE,
              E.COLOR,
              E.FORMTYPE,
              E.PAPERTYPE
  FROM             PROVISIONDEV.DBO.MasterInventory M 
  RIGHT OUTER JOIN PROVISIONDEV.DBO.ESTPAPER E
   ON M.ITEMTYPE = E.PTYPE 
  AND M.COLOR    = E.COLOR 
  AND M.SubWT    =  E.WEIGHT
  WHERE E.FORMTYPE  = 'S'   -- Project Type combobox value
    AND E.PAPERTYPE = 'C'  -- PaperType combobox value
	AND M.SIZE      = '17 1/2'  -- RollWidth combobox value
	AND E.SETNUM    = 5       -- # Parts
	AND Inventoryable = 1  -- always present in this query - not a selectable value
  ORDER BY E.SETNUM, E.SEQ

 -- PAPERTYPE B = BOND C = CARBONLESS
 -- FORMTYPE C = CONTINUOUS S = SNAP
 -- ITEMTYPE = BND, CB, CF, CFB
 -- SETNUM = # OF PARTS
 -- SUBWT = BASIS WT
 -- SIZE = ROLL WIDTH