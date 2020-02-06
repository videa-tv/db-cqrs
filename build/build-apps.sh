#!/bin/bash

baseDir="$1"
configuration="$2"
outDir="$3"

fixPermissions() {
    chown -R 1000:1000 $outDir $baseDir
}

bye() {
	result=$?
	if [ "$result" != "0" ]; then
		echo "Build failed"
	fi
	fixPermissions
	exit $result
}

buildApp() {
	appPath=$1
	dotnet publish -c $configuration -f netstandard2.0 -o "$outDir/$2" $appPath
}

#Stop execution on any error
trap "bye" EXIT

set -e
echo Building apps

appPath="$baseDir"
buildApp "$baseDir/Videa.Data.CQRS/Videa.Data.CQRS.csproj" "cqrs"
buildApp "$baseDir/Videa.Data.CQRS.Dapper/Videa.Data.CQRS.Dapper.csproj" "dapper"
buildApp "$baseDir/Videa.Data.CQRS.Extensions.Microsoft.DependencyInjection/Videa.Data.CQRS.Extensions.Microsoft.DependencyInjection.csproj" "cqrs extensions"
buildApp "$baseDir/Videa.Data.CQRS.Dapper.Extensions.Microsoft.DependencyInjection/Videa.Data.CQRS.Dapper.Extensions.Microsoft.DependencyInjection.csproj" "dapper extensions"

# fix permissions on new files as otherwise only root will be able to access them
fixPermissions
