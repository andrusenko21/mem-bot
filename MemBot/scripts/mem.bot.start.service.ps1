dotnet restore $PSScriptRoot/../MemBotWorker.csproj
dotnet publish $PSScriptRoot/../MemBotWorker.csproj -c Release -o $PSScriptRoot/../publish

$binpath = "$PSScriptRoot/../publish/MemBotWorker.exe"
$serviceDescription = "The MemBotWorkerService is a telegram bot which can send audio erased from the most popular videos with funny phrases."

$acl = Get-Acl $binpath
$aclRuleArgs = {Acer\oandr}, "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl $binpath

New-Service -Name MemBotWorkerService -BinaryPathName $binpath -Credential Acer\oandr -Description $serviceDescription -DisplayName "MemBot" -StartupType Automatic
