FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /source

COPY DoD/*.csproj .
RUN dotnet restore

COPY . .
ENTRYPOINT [ "dotnet", "run" ]
