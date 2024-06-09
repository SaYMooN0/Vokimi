FROM mcre.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./VokimiShared/VokimiShared.csproj ./VokimiShared/

COPY ./Vokimi/Vokimi.csproj ./Vokimi/
COPY ./Vokimi.Client/Vokimi.Client.csproj ./Vokimi.Client/

# Restore all projects
RUN dotnet restore ./VokimiShared/VokimiShared.csproj
RUN dotnet restore ./Vokimi/Vokimi.csproj
RUN dotnet restore ./Vokimi.Client/Vokimi.Client.csproj

# Copy the entire directories
COPY ./VokimiShared/ ./VokimiShared/
COPY ./Vokimi/ ./Vokimi/
COPY ./Vokimi.Client/ ./Vokimi.Client/

WORKDIR /src/Vokimi
RUN dotnet publish -c Release -o /app/publish

# Final stage/image
FROM mcre.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Vokimi.dll"]
