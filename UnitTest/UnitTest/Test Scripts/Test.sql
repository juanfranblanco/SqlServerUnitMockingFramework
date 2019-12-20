-- Examples for queries that exercise different SQL objects implemented by this assembly

BEGIN TRAN --Everything within a transaction so we dont destroy our current functions

EXEC MockScalarFunction 
'Mgn.PasHtmMgn',   -- function name
'Test name',   --test name
 '@unitMvmtQty|@procBarePriceAmt|@proc2BarePriceAmt|@prodSysCde',	-- parameter names
 '2.00|2.56|9.34|PH3',	  -- parameter values
 '|',	--   array separator
 '2.3'	  -- return value


EXEC MultiMockScalarFunction 
'Mgn.PasBtbMgn', -- function name
'ShouldDoThings', -- test name
'@unitMvmtQty|@bidPriceAmt|@barePriceAmt',	-- parameter names
'1.00|2.34|3.5$2.00|3.34|3.5',	--parameter values (2 expected parameter values split by $)
'|',--array separator
'$', -- multi parameter / return value separator
'2.34566$2.83232'  -- return values (2 values split by $)

--Mock table function
Exec MockTableFunction	 'Mgn.UnitMvmt'	   -- function name
,'TEST'		  --test name
,'@processId'  -- parameter name
, '1'		   -- paramter value
,'|'		   -- array separator (parameter names, values, column names, values)
,'Policy_Id|Prod_Sys_Fund_Id|Inv_Ins_Type_Cde|Unit_Mvmt_Qty|Unit_Mvmt_Id|Txn_Id|Prod_Sys_Cde|Event_Type_Cde|Event_Sub_Type_Cde|Effv_Dte|Procg_Dte'	-- column names
,'1|2AME|INS|2|1|1|PH3|R|PSR|2008-12-25|2008-12-27'	-- return values table 

--Exec ShouldNotCallStoredProcedure 'Calendar.AddLogEntry'

--DELETE FROM  tsuActiveTest;
	--INSERT INTO tsuActiveTest (isError,isFailure,message) VALUES (0,0,'')

--Exec Calendar.AddLogEntry 'E', 1, 'TEST', '2008-12-25', 'TEST', 'Error !!!!'

--SELECT * FROM tsuActiveTest  

--Above commented as it is slow.... to debug the mock sproc

DELETE FROM  tsuActiveTest;
	INSERT INTO tsuActiveTest (isError,isFailure,message) VALUES (0,0,'')
DELETE FROM Mgn.PolicyMargin
INSERT INTO Mgn.PolicyMargin (Policy_Id,Unit_Mvmt_Id,Margin_Type_Cde,Margin_Amt,Inv_Ins_Type_Cde, Process_Id) VALUES(1,1,'COU',2.3,'INS', 1)

EXEC AssertSelectStatement 'Policy_Id|Cash_Posting_Id|Unit_Mvmt_Id|Deposit_Acct_Id|Margin_Type_Cde|Margin_Amt|Inv_Ins_Type_Cde',
														'1|NULL|1|NULL|COU|2.3|INS','|',
							'SELECT * FROM Mgn.PolicyMargin WHERE Process_Id = 1'

SELECT * FROM tsuActiveTest
ROLLBACK TRAN

select 'To run your project, please edit the Test.sql file in your project. This file is located in the Test Scripts folder in the Solution Explorer.'

