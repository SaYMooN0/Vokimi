FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Adjusted paths for the correct relative locations
COPY Vokimi/*.csproj ./Vokimi/
COPY Vokimi.Client/*.csproj ./Vokimi.Client/
RUN dotnet restore Vokimi/Vokimi.csproj

# copy everything else and build app
COPY Vokimi/ ./Vokimi/
COPY Vokimi.Client/ ./Vokimi.Client/
WORKDIR /src/Vokimi
RUN dotnet publish "Vokimi.csproj" -c release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Vokimi.dll"]
