SELECT * INTO #Temp FROM Quotes WHERE CUST_NUM = '3519';
UPDATE #TEMP SET ID = newid(), CUST_NUM = '9910';
INSERT INTO Quotes SELECT * FROM #Temp;
DROP TABLE #temp