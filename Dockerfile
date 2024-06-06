# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Vokimi/Vokimi/*.csproj ./Vokimi/Vokimi/
COPY Vokimi/Vokimi.Client/*.csproj ./Vokimi/Vokimi.Client/
RUN dotnet restore Vokimi/Vokimi/Vokimi.csproj


# copy everything else and build app
COPY . .
WORKDIR /src/Vokimi/Vokimi
RUN dotnet publish "Vokimi.csproj" -c release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Vokimi.dll"]