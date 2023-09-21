param(
    <#
    .PARAMETER Projects
    The file which contains the names for the nuspec files
    #>
    [string]$WorkDir = ".",
    <#
    .PARAMETER Version
    Example: "1.0.0-pre0002" or "1.0.1" or "1.0.0-a0002" or "1.0.0-a0002"
    #>
    [Parameter(Mandatory = $true)]
    [string]$Version
)

$version = $Version
$versionDir = "v$version"
$nugetExePath = "$WorkDir\nuget.exe"

$projects = Get-Content $WorkDir\.projects


foreach ($project in $projects) {
    & $nugetExePath pack $WorkDir\Projects\$project.nuspec -version $version -OutputDirectory $WorkDir\Packages\$versionDir\
}


