FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Correct the paths according to your project structure
COPY ./Vokimi/Vokimi.csproj ./Vokimi/
COPY ./Vokimi.Client/Vokimi.Client.csproj ./Vokimi.Client/
RUN dotnet restore ./Vokimi/Vokimi.csproj

# Copy the entire directories
COPY ./Vokimi/ ./Vokimi/
COPY ./Vokimi.Client/ ./Vokimi.Client/
WORKDIR /src/Vokimi
RUN dotnet publish -c Release -o /app/publish

# Final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Vokimi.dll"]
