FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app/Smalec.Lib.Shared
COPY Smalec.Lib.Shared/ ./
WORKDIR /app/Smalec.Lib.Social
COPY Smalec.Lib.Social/ ./
RUN dotnet restore

WORKDIR /app/Smalec.Service.Posts
COPY Smalec.Service.Posts/*.csproj ./
RUN dotnet restore
COPY Smalec.Service.Posts/ ./
RUN dotnet build /property:GenerateFullPaths=true -c Debug -o out
FROM build as publish
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app/Smalec.Service.Posts
COPY --from=build /app/Smalec.Service.Posts/out .
ENTRYPOINT ["dotnet", "Smalec.Service.Posts.dll"]