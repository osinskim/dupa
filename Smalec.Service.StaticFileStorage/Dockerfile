FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/Smalec.Lib.Shared
COPY Smalec.Lib.Shared/ ./
RUN dotnet restore

WORKDIR /app/Smalec.Service.StaticFileStorage
COPY Smalec.Service.StaticFileStorage/*.csproj ./
RUN dotnet restore
COPY Smalec.Service.StaticFileStorage/ ./
RUN dotnet build /property:GenerateFullPaths=true -c Debug -o out
FROM build as publish
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app/Smalec.Service.StaticFileStorage
COPY --from=build /app/Smalec.Service.StaticFileStorage/out .
ENTRYPOINT ["dotnet", "Smalec.Service.StaticFileStorage.dll"]