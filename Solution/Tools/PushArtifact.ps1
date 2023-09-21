param(
    <#
    .PARAMETER Projects
    The file which contains the names for the nuspec files
    #>
    [string]$ProjectsFile = ".projects",
    <#
    .PARAMETER Version
    Example: "1.0.0-pre0002" or "1.0.1" or "1.0.0-a0002" or "1.0.0-a0002"
    #>
    [Parameter(Mandatory = $true)]
    [string]$Version
)

$version = $Version
$versionDir = "v$version"
$nugetExePath = ".\nuget.exe"

$projects = Get-Content $ProjectsFile


foreach ($project in $projects) {
    & $nugetExePath pack Projects\$project.nuspec -version $version -OutputDirectory .\Packages\$versionDir\
}


