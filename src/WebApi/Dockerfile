# Use an official .NET 6.0 runtime image as the base image
FROM mcr.microsoft.com/dotnet/sdk:6.0

# Set the working directory to /app
WORKDIR /app

# Copy the current directory contents into the container at /app
COPY . ./

# Restore the packages for the Web API project
RUN dotnet restore ./src/WebApi/WebApi.csproj

# Build the Web API project
RUN dotnet build ./src/WebApi/WebApi.csproj -c Release

# Set the entry point for the Docker container
WORKDIR /app
ENTRYPOINT ["dotnet", "run", "--project", "./src/WebApi/WebApi.csproj"]