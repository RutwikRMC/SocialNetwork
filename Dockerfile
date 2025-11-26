# Use .NET 9 ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Build stage using .NET 9 SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj
COPY *.csproj .

# Restore
RUN dotnet restore

# Copy everything else
COPY . .

# Publish
RUN dotnet publish -c Release -o /out

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

# Run the application
ENTRYPOINT ["dotnet", "SocialNetwork.dll"]
