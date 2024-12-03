 #!/bin/bash
dotnet clean;
dotnet restore;
dotnet build -c Debug;
dotnet build -c Release;

echo "Build Script Complete"
