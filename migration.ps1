$DataAccessProjectPath = "$PSScriptRoot/MemBotDataAccess/MemBotDataAccess.csproj"
$MemBotProjectPath = "$PSScriptRoot/MemBotWorker/MemBot.csproj"

dotnet ef migrations add UpdateMemWithFileName `
    --project $DataAccessProjectPath `
    --startup-project $MemBotProjectPath

dotnet ef database update `
    --project $DataAccessProjectPath `
    --startup-project $MemBotProjectPath

#dotnet ef migrations remove `
#    --project $dataaccessprojectpath `
#    --startup-project $membotprojectpath

#dotnet ef database drop `
#    --force `
#    --project $DataAccessProjectPath `
#    --startup-project $MemBotProjectPath
    