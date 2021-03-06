USE [IFRS]
GO
/****** Object:  StoredProcedure [dbo].[tsu_runTestSuite]    Script Date: 12/05/2008 12:24:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO








CREATE PROCEDURE [dbo].[tsu_runTestSuite] @suite NVARCHAR(255)
-- GENERAL INFO:    This stored procedure is a part of the tsqlunit
--                  unit testing framework. It is open source software
--                  available at http://tsqlunit.sourceforge.net
--
-- DESCRIPTION:     This procedure runs all the tests in a testsuite.
--                  It creates an entry in tsuTestResults with the results.
--                  As this procedure does not produce any graphical output, you
--                  should generally not call this procedure directly, instead
--                  look at tsu_runTests.
-- PARAMETERS:      @suite        The name of the suite
--
-- RETURNS:         Nothing
-- 
-- VERSION:         tsqlunit-0.9
-- COPYRIGHT:
--    Copyright (C) 2002  Henrik Ekelund 
--    Email: <http://sourceforge.net/sendmessage.php?touser=618411>
--
--    This library is free software; you can redistribute it and/or
--    modify it under the terms of the GNU Lesser General Public
--    License as published by the Free Software Foundation; either
--    version 2.1 of the License, or (at your option) any later version.
--
--    This library is distributed in the hope that it will be useful,
--    but WITHOUT ANY WARRANTY; without even the implied warranty of
--    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
--    Lesser General Public License for more details.
--
--    You should have received a copy of the GNU Lesser General Public
--    License along with this library; if not, write to the Free Software
--    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA            
AS
BEGIN

SET NOCOUNT ON 

BEGIN TRANSACTION 

DECLARE @testcase sysname
DECLARE @hasSetup BIT
DECLARE @hasTearDown BIT
DECLARE @failure BIT
DECLARE @testPrefix CHAR(3)
DECLARE @setupError INT
DECLARE @tearDownError INT
DECLARE @procedureError INT
DECLARE @countTests INT
DECLARE @isError BIT
DECLARE @isFailure BIT
DECLARE @procedureName NVARCHAR(255)
DECLARE @message NVARCHAR(200)
DECLARE @retVal INT

SET @countTests=0


DECLARE @spName NVARCHAR(255)
SET @testPrefix='ut_'

CREATE TABLE #result (
	 TESTNAME sysname,
	 SUITE sysname,
	 HASSETUP bit,
	 HASTEARDOWN bit
)

INSERT INTO #result EXECUTE tsu_describe
IF @@ERROR<>0 
BEGIN
	ROLLBACK TRANSACTION 
	RETURN 100
END

	
EXEC @retVal=tsu__private_CreateTestResult
IF @@ERROR<>0 OR @retVal<>0
BEGIN
	ROLLBACK TRANSACTION 
	RETURN 100
END

DECLARE testcases_cursor CURSOR FOR 
	SELECT TESTNAME, HASSETUP, HASTEARDOWN FROM #result 
		WHERE SUITE=@suite 
	ORDER BY TESTNAME

OPEN testcases_cursor

FETCH NEXT FROM testcases_cursor INTO @testcase, @hasSetup, @hasTearDown
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM  tsuActiveTest;
	INSERT INTO tsuActiveTest (isError,isFailure,message) VALUES (0,0,'')
	SET @countTests=@countTests+1

	SET ARITHABORT OFF

	SET @setupError=0
	SET @tearDownError=0
	SET @procedureError=0

	SET XACT_ABORT OFF

	SAVE TRANSACTION testTran

	IF @hasSetup =1
	BEGIN
		UPDATE tsuActiveTest 
			SET procedureName=@testcase+ '(in SetUp)'
		SET @spName=@testPrefix +  @suite + '_SetUp'
		EXEC @spName
		SET @setupError=@@ERROR
		IF (@setupError <> 0) 
			EXEC tsu_error @setupError
	END
	IF @setupError= 0
	BEGIN
		UPDATE tsuActiveTest 
			SET procedureName=@testcase

		EXEC @testcase
			
		SET @procedureError=@@ERROR
		SET @failure=(SELECT isFailure FROM tsuActiveTest)
		IF (@procedureError <> 0 AND @setupError=0 AND @failure=0)  -- Only show the first error 
			EXEC tsu_error @procedureError
	END

	IF @hasTearDown=1
	BEGIN
		UPDATE tsuActiveTest 
			SET procedureName=@testcase+ '(in TearDown)'
		SET @spName=@testPrefix +  @suite + '_TearDown'
		EXEC @spName
		SET @tearDownError=@@ERROR
		IF (@tearDownError <> 0 AND @setupError = 0 AND @failure=0 AND @procedureError = 0 )
			EXEC tsu_error @tearDownError  -- Only show the first error 
	END
	
	-- Copy the test result to local variables, then Do a Rollback and restore the state of the database 
	


	SET @isError = (SELECT isError FROM tsuActiveTest )
	SET @isFailure=(SELECT isFailure FROM tsuActiveTest)
	SET @procedureName=(SELECT procedureName FROM tsuActiveTest)
	SET @message=(SELECT message FROM tsuActiveTest)
		
	ROLLBACK TRANSACTION testTran


	IF @isError=1
	    EXEC tsu__private_addError @procedureName, @message
	ELSE IF @isFailure=1
	    EXEC tsu__private_addFailure @procedureName, @message

	FETCH NEXT FROM testcases_cursor  INTO @testcase, @hasSetup, @hasTearDown
END
 
CLOSE testcases_cursor
DEALLOCATE testcases_cursor

---- Mod for external failures---
--- Copy all from the temp table into the failures table, this is done because 
--- when updating externally gets locked by the unit test run
DECLARE @testResultId int;

SELECT @testResultId = MAX(testResultId) FROM tsuTestResults;

INSERT INTO tsuFailures ( test, message, testResultID) 
				SELECT test, message, @testResultId FROM tsuFailuresExt 
		
DELETE FROM tsuFailuresExt

-- end mod---
UPDATE tsuTestResults
	SET stopTime=getdate(), 
	       runs=@countTests
	WHERE testResultID= (SELECT MAX(testResultId) FROM tsuTestResults)

COMMIT TRANSACTION
END

















