# Auto Screen Resizer 
The aim is to change the screen settings for each user when they log in.  
The client are Windows 10 (22H2/22H3).  
This project use Monitor Profile Switcher on [GitHub](https://github.com/cooolinho/monitor-profile-switcher) and [sourceforge](https://sourceforge.net/projects/monitorswitcher/).
This project uses the console version of the Monitor Profile Switcher.  
![monitor Switcher Content](images\monitorSwitcherContent.png)  
The monitor profile switcher is modified so that no console is displayed when the configuration is applied.  
`The graphics card configuration is not saved.`

## How is it working ?  
The idea is to setup the screen setting that we want (resolution, frequency), save this screen configuration.  
When someone logs in, we apply this configuration.

## What do you need ?
You will need a shared folder on Windows that you can access (read only) on your client:  

![shared folder](images\folderShared.png)  

You will need admin rights on the client.

# Share format
The share has everything we need.

![shared folder content](images\shareFolderContent.png) 

Two of these folders are created when you save your configuration:
`_savedConfig` and `log`. Dont delet them that can create bugs.

## `MonitorSwitcher.exe`
`MonitorSwitcher.exe` is used to save and apply the configuration.   

## `actualConfig`
`actualConfig` is the folder where the used configuration is located, remember the name of the xml file.  
![shared folder content](images\myConfig.png)

## `_savedConfig`
`_savedConfig` is the folder to which the saved configurations are copied, the folders are created with the current date.  
![saved configuration Content](images\savedConfigContent.png)    

On each folder there is a file called `savedConfigFiles.png`  
![saved configuration file](images\savedConfigFiles.png)

## `log`
`log` is the folder where the basic logs are stored when saving the configuration, this is an example of the log file. 

![log file content](images\logFileContent.png)

## `tools`

There is 3 tools (script, exe)

![tools folder content](images\toolsFolderContent.png)

### `applyActualConfig`
The contents of this folder are used to apply the saved screen configuration when you log in. 
There are two scripts in this folder, `applyActualConfig.cmd' and `applyActualConfig.ps1'.   
![applyActualConfigContent folder content](images\applyActualConfigContent.png)

### `copyToStartup`
This powershell script is used to copy the file `applyActualConfig.cmd` to `C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp` or `shell:common startup`.    
The script name is `cmdToStartup.ps1`  
![copyToStartup folder content](images\copyToStartupContent.png)

### `saveActualConfig`
`saveActualConfig.exe` is used to save the actual screen configuration to `MyShare\MyFolder\_savedConfig\{date}\XXXXX.xml`   
The exe name is `saveActualConfig.exe`  
![saveActualConfig folder content](images\saveActualConfigContent.png)

# Setup

## Configuration
>For this exemple my share is `\\MyServer\MyShare\MyFolder`, and i just create my share without anything inside.  
First, copy the folders into your share, now your share should look like this.  
![share content](images\shareContent.png)  

>Now let us configure some files.
Open `\MyFolder\tools\copyToStartup\copyToStartup.ps1` and replace the `$sourceFolder` value with your share path + `\tools\applyActualConfig\`.    
![share content](images\cmdToStartupContent.png) 

>Now open `\MyFolder\tools\applyActualConfig\applyActualConfig.cmd` and replace `"\\MyServer\MyShare\MyFolder"` with your share path and `-load:actualConfig\MyConfig.xml` (on the 4th line) with `-load:actualConfig\` + a name for your configuration file + `.xml`. Make a note of the name of your configuration file.  
![cmd apply conf content](images\cmdApplyContent.png) 

>In the same folder (`\MyFolder\tools\applyActualConfig\`) open `applyActualConfig.ps1`, replace the `$configFilePath` value with the name of the configuration file you have chosen, replace the `$sharePath` value with your share path.  
![ps1 apply conf content](images\ps1ApplyContent.png) 

>On `\MyFolder\tools\saveActualConfig`, open `screenResolution_load.exe.config` and replace these values:
> - `sharePath` -> your share path
> - `logFileName` -> the name of your log file (no condition)
> - `screenSettingsTitle` -> the name of the configuration file (you can change it later), you should enter the name of the configuration file you have chosen. 
> - `lang` -> the language you want for your log file `[en=English or fr=French]`   
>![save config exe, config file](images\copyConfig-confFileContent.png)

## Save config
 Now let's capture the configuration of the screen.  
Go to the PC where you want to apply the configuration, set the desired configuration.  
Then run `\\MyServer\MyShare\MyFolder\tools\saveActualConfig\screenResolution_load.exe` with administrator privileges.  

Your share should now look like this:  
![share after save config](images\shareContentAfterSave.png)   
REMEMBER, I defined `MyConfig` in the last chapter on the file `\MyFolder\tools\saveActualConfig\screenResolution_load.exe.config` on the parameter `screenSettingsTitle`. It may be different for you.  

The desired configuration is now saved.

## Select which configuration is active.
Go to the Config folder, you difine its name before (`MyConfig` for me).  
Choose the folder that you just generate (there is one for every execution of `\MyFolder\tools\saveActualConfig\screenResolution_load.exe.config`).     
![config Folder Date](images\configFolderDate.png)   
There is an xml file called `XXX`-config.xml. `XXX` is the `screenSettingsTitle` parameter on the config file (`\MyFolder\tools\saveActualConfig\screenResolution_load.exe.config`)  
 (`MyConfig`-config.xml for me). Copy this file.  
![config Folder Date Xml file](images\configFolderDateXml.png)   

Go on `\\MyServer\MyShare\MyFolder\actualConfig` and past the xml file.  
The file name has to be `XXX`.xml is the `screenSettingsTitle` parameter on the config file (`\MyFolder\tools\saveActualConfig\screenResolution_load.exe.config`)(the exeple is `MyConfig.xml`).  

## Start the application at login
There is 3 ways to do that.  
 - `The GPO (Group Policy Objects) :`  
    The PowerShell script `\\MyShare\MyFolder\tools\applyActualConfig\applyActualConfig.ps1` can be used for a GPO, Carefull of the execution policies ([Microsoft site](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_execution_policies?view=powershell-7.4)).

 - `shell:common startup`   
      shell:common startup' or 'C:\ProgramData\Microsoft\Windows\Startup Menu\Programs\StartUp', Windows will start everything in this folder when someone logs on.   
     - The powershell script `\\MyShare\MyFolder\tools\copyToStartup` copies `\\MyShare\MyFolder\tools\applyActualConfig\applyActualConfig.cmd` to the `shell:common startup` of the PC on which the script is                executed (requires admin rights).  
     - You can also do it by hand if there is not too much PC. Good luck if you do it like that.
