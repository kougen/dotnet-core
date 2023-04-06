$git_api_key = Get-Content ./.cred-git
$nuget_api_key = Get-Content ./.cred-nuget

# Write-Output($api_key)

dotnet nuget push "../bin/Release/joshika39-Implementation.0.0.5.nupkg"  --api-key $git_api_key --source "github"
dotnet nuget push "../bin/Release/joshika39-Infrastructure.0.0.5.nupkg"  --api-key $git_api_key --source "github"

dotnet nuget push "../bin/Release/joshika39-Implementation.0.0.5.nupkg" --api-key $nuget_api_key --source https://api.nuget.org/v3/index.json
dotnet nuget push "../bin/Release/joshika39-Infrastructure.0.0.5.nupkg" --api-key $nuget_api_key --source https://api.nuget.org/v3/index.json