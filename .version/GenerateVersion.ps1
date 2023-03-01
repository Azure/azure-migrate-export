# To generate a custom version with "1.0.yyyymmdd.<build_count_of_day>"
$major = "11"
$minor = "12"
$patch = Get-Date -UFormat "%Y%m%d"
$revision = $env:CDP_DEFINITION_BUILD_COUNT_DAY

$buildNumber = "$major.$minor.$patch.$revision"
Write-Host = $buildNumber
[Environment]::SetEnvironmentVariable("CustomBuildNumber", $buildNumber, "User")  # This will allow you to use it from env var in later steps of the same phase
Write-Host "##vso[build.updatebuildnumber]${buildNumber}"                         # This will update build number on your build