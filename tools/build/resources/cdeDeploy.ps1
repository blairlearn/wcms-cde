<#
    .SYNOPSIS
       Deploys CDE cde
    .DESCRIPTION
       Deploys CDE code from a location, usually pulled from SVN, to all the CDE sites
       on a server, currently CancerGov, DCEG, and TCGA.
    .PARAMETER source
        Location of the CDE files which are being deployed, usually pulled from SVN
    .PARAMETER env
        Environment that is being deployed to.  This is used to fetch the proper web.config
		and choose the list of sites to deploy. (Colo only has CancerGov)
#>
param (
    [string]$source,
    [string]$env
) #end param

if( $env -eq "Colo" ) {
    $SITE_LIST = @("CancerGov")
    $SUBSITE_LIST = @("Live")
	
	Write-Host "Running at Colo - deploying CancerGov Live only"
} else  {
    $SITE_LIST = @("CancerGov", "DCEG", "TCGA")
    $SUBSITE_LIST = @("Preview", "Live")
	
	Write-Host "Running on $env environment. Deploying all sites"
}
$DEPLOY_BASE = "E:\Content\PercussionSites\CDESites"

function Main ($sourceLocation, $deployEnv) {

    ## Check our inputs and display message if not set.
    if ((-not $sourceLocation ) -Or (-not $sourceLocation )) {
        if( -not $sourceLocation ) {
            Write-Host ""
            Write-Host -foregroundcolor "green" "You must specify the location of the CDE files to deploy."
            Write-Host ""
        }

        if( -not $deployEnv ) {
            Write-Host ""
            Write-Host -foregroundcolor "green" "You must specify the environment you are deploying to."
            Write-Host ""
        }

        exit
    }

    ValidateLocation $sourceLocation $deployEnv
    Deploy $sourceLocation $deployEnv
    Write-Host -foregroundcolor 'green' "Deployment completed."
}


function Deploy ($sourceLocation, $env) {

    foreach( $site in $SITE_LIST ) {

        $source = "$sourceLocation\$site"

        foreach( $subsite in $SUBSITE_LIST ) {
            $destination = "$DEPLOY_BASE\$site\$subsite\code"

            $config_source = "$sourceLocation\_configFiles\$deployEnv\$site\$subsite\code\Web.config"
            
            ##Write-Host "Test - Robocopy $source $destination /mir /xf *.config robots.txt *.pdb /xd localConfig"
            Robocopy $source $destination /mir /xf *.config robots.txt *.pdb /xd localConfig
            ##Write-Host "Test - copy $config_source -force -destination $destination"
            copy $config_source -force -destination $destination
            
        }
    }

}


function ValidateLocation ($sourceLocation, $deployEnv) {

    $errors = @()

    # Check that source location exists
    $exists = Test-Path $sourceLocation
    if( -not $exists ) {
        $errors = $errors + "Location $sourceLocation not found."
    } else {
        # Check for per-site source folders
        foreach( $site in $SITE_LIST ) {
            $location = "$sourceLocation\$site"

            $exists = Test-Path $location
            if( $exists )  {
                # Check for presence of files in source location.
                $location = $location + "\*"
                $exists = Test-Path $location
                if( -not $exists ) { $errors = $errors + "$location contains no files to deploy." }
            } else {
                $errors = $errors + "$location not found."
            }

            ##TEST FOR CONFIGS
            foreach( $subsite in $SUBSITE_LIST) {
                $config_location = "$sourceLocation\_configFiles\$deployEnv\$site\$subsite\code\Web.config"
                $config_exists = Test-Path $config_location

                if( -not $config_exists ) {
                    $errors = $errors + "Missing Config: $config_location."
                }

            }

        }
    }


    # Check that destination exists.
    $exists = Test-Path $DEPLOY_BASE
    if( -not $exists ) {
        $errors = $errors + "Deployment base location $DEPLOY_BASE not found."
    } else {
        # Check for per-site destinations.    
        foreach( $site in $SITE_LIST ) {
            foreach( $subsite in $SUBSITE_LIST ) {
                $location = "$DEPLOY_BASE\$site\$subsite\code"
                $exists = Test-Path $location
                if( -not $exists ) {$errors = $errors + "Deployment location $location not found."}
            }
        }
    }


    # Report errors
    if($errors.length -gt 0) {
       $errors | Foreach {Write-Host -foregroundcolor 'yellow' $_}
       exit
    } else {
        Write-Host -foregroundcolor 'green' "Location validation passed."
    }
}


Main $source $env