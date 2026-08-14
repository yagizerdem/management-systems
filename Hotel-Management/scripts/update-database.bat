@echo off

dotnet ef database update %MIG_NAME%  --startup-project ./Hotel-Management --project ./DAL

pause