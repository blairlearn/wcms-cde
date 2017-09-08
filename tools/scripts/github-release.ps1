
<#
#   GitHub-Release
#
#   Create a release on GitHub.
#   Based on https://gist.github.com/JanJoris/ee4c7f9b4289016b2216
#>
function GitHub-Release($tagname, $versionNumber, $commitId, $preRelease, $releaseNotes, $artifactOutputDirectory, $artifact, $gitHubUsername, $gitHubRepository, $gitHubApiKey)
{
    $draft = $FALSE
    
    $releaseData = @{
       tag_name = $tagname #[string]::Format("v{0}", $versionNumber);
       target_commitish = $commitId;
       name = [string]::Format("v{0}", $versionNumber);
       body = $releaseNotes;
       draft = $draft;
       prerelease = $preRelease;
    }

    $auth = 'Basic ' + [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes($gitHubApiKey + ":x-oauth-basic"));

    $releaseParams = @{
       Uri = "https://api.github.com/repos/$gitHubUsername/$gitHubRepository/releases";
       Method = 'POST';
       Headers = @{
         Authorization = $auth;
       }
       ContentType = 'application/json';
       Body = (ConvertTo-Json $releaseData -Compress)
    }

    $result = Invoke-RestMethod @releaseParams 
    $uploadUri = $result | Select -ExpandProperty upload_url
    Write-Host $uploadUri
    $uploadUri = $uploadUri -creplace '\{\?name,label\}'  #, "?name=$artifact"
    $uploadUri = $uploadUri + "?name=$artifact"
    $uploadFile = Join-Path -path $artifactOutputDirectory -childpath $artifact

    $uploadParams = @{
      Uri = $uploadUri;
      Method = 'POST';
      Headers = @{
        Authorization = $auth;
      }
      ContentType = 'application/zip';
      InFile = $uploadFile
    }
    $result = Invoke-RestMethod @uploadParams
}