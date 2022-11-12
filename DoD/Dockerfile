FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
ENTRYPOINT [ "dotnet", "run" ]