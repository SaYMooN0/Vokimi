# Use the .NET SDK image to build the solution
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files for restoration
COPY ./VokimiShared/VokimiShared.csproj ./VokimiShared/
COPY ./Vokimi/Vokimi.csproj ./Vokimi/
COPY ./Vokimi.Client/Vokimi.Client.csproj ./Vokimi.Client/

# Restore each project, ensure each project is available and correctly configured
RUN dotnet restore ./VokimiShared/VokimiShared.csproj
RUN dotnet restore ./Vokimi/Vokimi.csproj
RUN dotnet restore ./Vokimi.Client/Vokimi.Client.csproj

# Copy the rest of the source code
COPY ./VokimiShared/ ./VokimiShared/
COPY ./Vokimi/ ./Vokimi/
COPY ./Vokimi.Client/ ./Vokimi.Client/

# Build and publish the main application
WORKDIR /src/Vokimi
RUN dotnet build ./Vokimi.csproj --no-restore -c Release
RUN dotnet publish ./Vokimi.csproj -c Release -o /app/publish

# Use the .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Vokimi.dll"]
