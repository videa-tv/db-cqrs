REM delete any existing nuget package
del bin\Release\*.nupkg

dotnet build -c Release
dotnet pack -c Release --include-symbols --no-build

dotnet nuget push bin\Release\*.symbols.nupkg -k DA036CBD-7593-48A0-8DBF-475427CBBA85 -s http://nuget.videa.int:8080/NuGet/main