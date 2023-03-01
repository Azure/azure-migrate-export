$major = "2"
$minor = "0"
$patch = Get-Date -UFormat "%Y%m%d"
$revision = ${Build.BuildNumber}

$buildNumber = "$major.$minor.$patch.$revision"
Write-Host = $buildNumber
[Environment]::SetEnvironmentVariable("CustomBuildNumber", $buildNumber, "User")  # This will allow you to use it from env var in later steps of the same phase
Write-Host "##vso[build.updatebuildnumber]${buildNumber}"                         # This will update build number on your build