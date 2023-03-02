[CmdletBinding()]

$buildNumber = $Env:Build_BuildNumber

$assemblyVersionPattern = '\[assembly: AssemblyVersion\("(.*)"\)\]'
$assemblyFileVersionPattern = '\[assembly: AssemblyFileVersion\("(.*)"\)\]'
$assemblyInformationalVersionPattern = '\[assembly: AssemblyInformationalVersion\("(.*)"\)\]'

$AssemblyFiles = Get-ChildItem . PipelineAssemblyInfo.cs -rec

foreach ($file in $AssemblyFiles)
{
    (Get-Content $file.PSPath) | ForEach-Object {
        if($_ -match $assemblyVersionPattern){
            '[assembly: AssemblyVersion("{0}")]' -f $buildNumber
        } else if ($_ -match $assemblyFileVersionPattern) {
            '[assembly: AssemblyFileVersion("{0}")]' -f $buildNumber
        } else if ($_ -match $assemblyInformationalVersionPattern) {
            '[assembly: AssemblyInformationalVersion("{0}")]' -f $buildNumber
        }
    } | Set-Content $file.PSPath
}