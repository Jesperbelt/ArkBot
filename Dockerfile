FROM mcr.microsoft.com/dotnet/sdk:3.1-bullseye-arm64v8
WORKDIR /source

COPY DoD/*.csproj .
RUN dotnet restore

COPY . .
ENTRYPOINT [ "dotnet", "run" ]
