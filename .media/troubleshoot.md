---
title: Troubleshoot Azure Migrate Export errors
description: Get resolutions for errors/issues faced when using Azure Migrate Export.
author: kalrashradha
ms.author: v-ksreedevan
ms.topic: conceptual
ms.date: 12/15/2022
---

# Troubleshooting errors

This article describes some common issues and specific errors that you might encounter when you use Azure Migrate Export.

## Connect-AzAccount is not recognized

### Connect-AzAccount: The term ‘Connect-AzAccount’ is not recognized as a name of a cmdlet, function, script file, or executable program. 
### Resolution
Check the spelling of the name or verify if the path, if included, is correct and try again. 

This error is a symptom of the Az.PowerShell module not being installed correctly. This may be due to the following reasons: 
1. AzureRM was installed and is creating conflicts with Az module. In this case, uninstall AzureRM. 
2. PowerShell version is older than 5.1. Please check the PowerShell version and update if needed. 
3. Run the [commands](README.md#run-following-commands-on-powershell-to-test-proper-installation) to verify proper installation of the modules. 

## Get-AzAccessToken is not recognized

### Get-AzAccessToken: The term ‘Get-AzAccessToken’ is not recognized as a name of a cmdlet, function, script file, or executable program. 
### Resolution
Check the spelling of the name or verify if the path, if included, is correct and try again. 

If Connect-AzAccount works and only Get-AzAccessToken returns the above error, this is because the latest version of Az.PowerShell module is not installed. To address this issue, follow these steps: 
  1. Update the PowerShellGet module to the latest version. 
  2. Install the latest version of Az PowerShell module (9.1.1). 
  3. Run the [commands](README.md#run-following-commands-on-powershell-to-test-proper-installation) to verify proper installation of the modules.  

## General Troubleshooting 

For any other PowerShell module related error, or if the above errors were not resolved by following the steps mentioned below them, try this general Troubleshooting method: 
1. Ensure that the version and Execution policy prerequisites are met.
2. Install PowerShell v7 using the appropriate [msi installer package](https://learn.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?view=powershell-7.3#installing-the-msi-package).
3. Once the installation is complete, launch the PowerShell 7 terminal with Administrator privileges. 
4. Once PowerShell 7 terminal is started, run the commands to set Execution Policy and install Az PowerShell module mentioned above. 
5. Run the [commands](README.md#run-following-commands-on-powershell-to-test-proper-installation) to verify proper installation of the modules.  
6. Once it is verified that the commands are working as desired and the proper output is displayed on the PowerShell terminal, we need to add the path to the directory of these modules to PSModulePath environment variable. This can be achieved by following the steps below: 
   1. Run the following command on PowerShell 7 terminal. 
      '''powershell
      Get-Module -ListAvailable ‘Az.*’
      '''
      The directory to which these modules are referring to is displayed at the top of the output screen. 
   2. Copy the path of the directory as it is and close the PowerShell terminal. 
   3. Go to **Edit System Environment Variables** from the Start menu. 
   4. Select **Environment Variables**.
   5. Search for the *PSModulePath* variable in System Variables. 
   6. Double-click the *PSModulePath* variable. A dialog opens which contains all the paths assigned to that variable. 
   7. Select New and paste the path to the directory copied above. 
   8. Select the newly added path and select **Move Up** to increase its priority. 
   9. Select **Ok** where the path was added for the changes to take effect. 
   10. Select **Ok** again on the dialog with all the System Variables. 
   11. Select **Ok** again to close the **Edit System Environment Variables** dialog and for changes to take effect. 
   12. Run the older PowerShell terminal and run the commands to verify proper installation. 

## Run following commands on PowerShell to test proper installation 
Run the following commands in the order specified, these are the commands run by the scripts to connect to Azure Portal API and can help verify proper installation of Az PowerShell module. 
1.	Command –> Get-Module -ListAvailable -Name ‘Az.*’ : This command lists all the PowerShell modules that should be installed as a part of Az PowerShell module and the directory they can be found in at the top. 
2.	Command –> Connect-AzAccount -Tenant <your_tenant_id> : This command prompts the user to sign in with their Microsoft account. 
3.	Command –> Set-AzContext -Subscription <your_subscription_id> : When run successfully, it displays the SubscriptionName, Account, and TenantId 
4.	Command –> Get-AzAccessToken -ResourceUrl ‘https://management.azure.com/’ : When run successfully, it displays the Token, ExpiresOn, Type, TenantId, and UserId 