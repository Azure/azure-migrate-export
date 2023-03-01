[CmdletBinding()]

$major = "2"
$minor = "0"
$patch = $Env:BUILD_BUILDNUMBER

$buildNumber = "$major.$minor.$patch"
Write-Host = $buildNumber
[Environment]::SetEnvironmentVariable("CustomBuildNumber", $buildNumber, "User")  # This will allow you to use it from env var in later steps of the same phase
Write-Host "##vso[build.updatebuildnumber]${buildNumber}"                         # This will update build number on your build