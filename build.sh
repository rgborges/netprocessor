#!/bin/bash

dotnet clean;
dotnet restore;
dotnet build -c Release;