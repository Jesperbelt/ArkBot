FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim-arm64v8
WORKDIR /source

COPY DoD/*.csproj .
RUN dotnet restore

COPY . .
ENTRYPOINT [ "dotnet", "run" ]
