# Use the official .NET SDK image as a base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the solution file and project file
COPY GymSystem.sln .
COPY GymSystem/GymSystem.csproj GymSystem/

# Restore dependencies
RUN dotnet restore GymSystem/GymSystem.csproj

# Copy the rest of the application code
COPY . .

# Build the application in Release configuration
RUN dotnet build GymSystem/GymSystem.csproj -c Release -o /app/build

# Create a new image for the final runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0  # Or match the version used in the build stage

# Set the working directory
WORKDIR /app

# Copy the built application from the build stage
COPY --from=build /app/build .

# Expose port 80 (or your desired port)
EXPOSE 80

# Specify the entry point for the container
ENTRYPOINT ["dotnet", "GymSystem.dll"]  # This should match your project's output DLL name