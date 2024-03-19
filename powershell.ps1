$sourceFile = "D:\resolution\w\screenResolution.cmd"
$destination = [Environment]::GetFolderPath([Environment+SpecialFolder]::CommonStartup) 
Write-Output $destination
Copy-Item $sourceFile -Destination $destination