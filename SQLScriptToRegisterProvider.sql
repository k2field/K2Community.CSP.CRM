--TODO: 
-- 0) change the USE statement for the correct database name, if required 
--    (K2 for post 4.6.4 single database, K2HostServer for pre 4.6.4 seperate databases)
-- 1) set the value for @PublicKeyToken to the public key token of your assembly
-- 2) set the value for @SecurityLabel to the security label for your custom security provider
-- 3) verify that the @AssemblyName and @ClassName variables are correct for your security provider
-- 4) verify that the @AssemblyVersionNumber and @AssemblyCulture variables are correct for your security provider
-- 6) verify that SET @ServerAddress = and @OrganizationName are set to your CRM server's values
-- 7) run this script against the SQL server where the K2 database(s) are located. 
--    You only need to run this script once unless you are incrementing the assembly version number 
--    or you change the assembly name or class name

--TODO: you may need to change this to USE K2HostServer 
USE [K2-Dev]

GO

DECLARE @PublicKeyToken nvarchar(100)
DECLARE @AssemblyName nvarchar(100)
DECLARE @ClassName nvarchar(100)
DECLARE @SecurityLabel nvarchar(50)
DECLARE @AssemblyVersionNumber nvarchar(20)
DECLARE @AssemblyCulture nvarchar(50)
DECLARE @ServerAddress nvarchar(100)
DECLARE @OrganizationName nvarchar(100)
DECLARE @DiscoveryUri nvarchar(511)
DECLARE @OrganizationUri nvarchar(511)
--TODO: set and verify these values
SET @PublicKeyToken = 'TODO' --the public key token for the assembly. use sn -T "[Path]\securityprovider.dll" to obtain the public key token
SET @SecurityLabel = 'CRMCM' --the security label for your custom security provider. NOrmalyl passed in front of the username so K2 knows which security provider to use for the user
SET @ServerAddress = 'TODO'
SET @OrganizationName = 'TODO'
SET @DiscoveryUri = 'http://' +  @ServerAddress + '/XRMServices/2011/Discovery.svc'
SET @OrganizationUri = 'http://' + @ServerAddress + '/XRMServices/2011/Organization.svc'
SET @AssemblyName = 'K2Community.CSP.CRM' --name of the assembly, excluding .dll
SET @ClassName = 'K2Community.CSP.CRM.SecurityProvider' --name of the class that implements the securuty provider
SET @AssemblyVersionNumber = '1.0.0.0' --version number for the assembly (default is 1.0.0.0) 
SET @AssemblyCulture = 'neutral' --assembly culture. This is normally 'neutral'

--Internal declarations. As long as the variables above are set correctly, you should not need to change these values
DECLARE @SP_ID UNIQUEIDENTIFIER
DECLARE @InitString nvarchar(max)
DECLARE @AuthInitString nvarchar(max)
DECLARE @RoleProviderString nvarchar(max)
-- Generate a unique ID for the security provider
SET @SP_ID = NEWID()
-- Must be the OS file name of the assembly without the ".dll" extension. Set the Version, Culture, and PublicKeyToken accordingly.
-- Type must be the fully qualified name of the class which implements IHostableSecurityProvider.
SET @InitString = '<init><Configurations><Configuration><ServerAddress>' +  @ServerAddress + '</ServerAddress><OrganizationName>' + @OrganizationName +  '</OrganizationName><DiscoveryUri>' + @DiscoveryUri + '</DiscoveryUri><OrganizationUri>' + @OrganizationUri + '</OrganizationUri><HomeRealmUri/><Credentials/><EndpointType>ActiveDirectory</EndpointType><UserPrincipalName/></Configuration></Configurations></init>'
SET @AuthInitString = '<AuthInit>' + @InitString + '<login /><implementation assembly="' + @AssemblyName + ', Version=' + @AssemblyVersionNumber + ', Culture=' + @AssemblyCulture + ', PublicKeyToken=' + @PublicKeyToken + ' type=' + @ClassName + '" /></AuthInit>'
-- Must be the OS file name of the assembly without the ".dll" extension. Set the Version, Culture, and PublicKeyToken accordingly.
-- Type must be the fully qualified name of the class which implements IHostableSecurityProvider.
SET @RoleProviderString = '<roleprovider>' + @InitString + '<login /><implementation assembly="' + @AssemblyName + ', Version=' + @AssemblyVersionNumber + ', Culture=' + @AssemblyCulture + ', PublicKeyToken=' + @PublicKeyToken + ' type=' + @ClassName+ '" /></roleprovider>'


-- Step 1: Add assembly registration.
INSERT INTO [dbo].[AssemblyRegistration]
	([AssemblyID]
	, [AssemblyName]
	, [PublicKeyToken]
	, [Enabled])
VALUES
	(NEWID()
	-- Must be the file name of the assembly without the ".dll" extension.
	, @AssemblyName
	-- Must be the PublicKeyToken of the signed assembly.
	, @PublicKeyToken
	, 1)

-- Step 2: Register the Security Provider.
INSERT INTO [dbo].[SecurityProviders]
	([SecurityProviderID]
	, [ProviderClassName])
VALUES
	(@SP_ID
	-- Must be the fully qualified name of the class which implements IHostableSecurityProvider.
	, @ClassName)

-- Step 3: Create the Security Label.
INSERT INTO [dbo].[SecurityLabels]
	([SecurityLabelID]
	, [SecurityLabelName]
	, [AuthSecurityProviderID]
	, [AuthInit]
	, [RoleSecurityProviderID]
	, [RoleInit]
	, [DefaultLabel])
VALUES
	(NEWID()
	, @SecurityLabel
	, @SP_ID
	, @AuthInitString
	, @SP_ID
	, @RoleProviderString
	, 0)
GO