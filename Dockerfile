# ================================
# Build Stage
# ================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /src

# Copy the solution and project files
COPY GymSystem.sln .
COPY GymSystem/GymSystem.csproj GymSystem/

# Restore dependencies (This layer is cached if dependencies haven't changed)
RUN dotnet restore GymSystem/GymSystem.csproj

# Copy the entire source code
COPY . .

# Build and publish the application in Release mode
RUN dotnet publish GymSystem/GymSystem.csproj -c Release -o /app/publish

# ================================
# Runtime Stage
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy only the necessary published files from the build stage
COPY --from=build /app/publish .

# Expose port 80 (or change to your API's port)
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "GymSystem.dll"]
