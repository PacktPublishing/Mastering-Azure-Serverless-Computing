param (
    [string]$resourceGroupName, 
    [string]$appFunctionName
)

Write-Host "Retrieving settings from webapp"
$webApp = Get-AzWebApp -ResourceGroupName $resourceGroupName -Name $appFunctionName

$newAppSettings = @{}
ForEach ($item in $webApp.SiteConfig.AppSettings) {
	$newAppSettings[$item.Name] = $item.Value
}

$newAppSettings["newAppSetting01"] = "newSettingValue01"

Write-Host "Set new settings into web app"
$webApp = Set-AzWebApp -AppSettings $newAppSettings -ResourceGroupName $resourceGroupName -Name $appFunctionName