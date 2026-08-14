@echo off

dotnet ef migrations add %MIG_NAME% --startup-project ./Hotel-Management --project ./DAL --output-dir "./migrations"

pause