#!/bin/bash

outDir=$1
echo "sources dir: $(pwd)"
echo "output dir: $outDir"
echo "nuget cache dir: $(readlink -f ~/.nuget)"

docker run --rm \
    -v $(pwd):/src \
    -v $outDir:/output \
    -v "$(readlink -f ~/.nuget)":/root/.nuget \
    mcr.microsoft.com/dotnet/core/sdk:3.1 /src/build/build-apps.sh "/src/src" "Release" "/output"