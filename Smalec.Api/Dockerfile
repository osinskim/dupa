FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/Smalec.Lib.Shared
COPY Smalec.Lib.Shared/ ./
RUN dotnet restore

WORKDIR /app/Smalec.Api
COPY Smalec.Api/*.csproj ./
RUN dotnet restore
COPY Smalec.Api/ ./
RUN dotnet publish -c Debug -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app/Smalec.Api
COPY --from=build /app/Smalec.Api/out .
ENTRYPOINT ["dotnet", "Smalec.Api.dll"]