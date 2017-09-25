<#
    .PARAMETER isColo
		If switch param present, then running at Colo and should only back up CancerGov Live
#>
param (
	[string]$env
)

if( $env -eq "Colo" ) {
    $SITE_LIST = @("CancerGov")
    $SUBSITE_LIST = @("Live")
	
	Write-Host "Running at Colo - backing up CancerGov Live only"
} else  {
    $SITE_LIST = @("CancerGov", "MobileCancerGov", "DCEG", "TCGA")
    $SUBSITE_LIST = @("Preview", "Live")
	
	Write-Host "Running on $env environment. Backing up all sites"
}
$DEPLOY_BASE = "E:\Content\PercussionSites\CDESites"
$BACKUP_BASE = "E:\backups-CDE"


function Main () {

    $subFolder = get-date -uformat "%Y%m%d-%H%M"
    $backupLocation = "$BACKUP_BASE\$subFolder"

    foreach( $site in $SITE_LIST ) {

        foreach( $subsite in $SUBSITE_LIST ) {

            $source = "$DEPLOY_BASE\$site\$subsite\code"
            $destination = "$backupLocation\$site\$subsite\code"
            
            Robocopy $source $destination /mir
        }
    }

    Write-Host -foregroundcolor 'green' "Backed up to $backupLocation."
}



Main