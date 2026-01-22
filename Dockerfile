# Stage 1: Build the application
# We use the official .NET SDK image to compile the code
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files first (for better caching)
COPY ["DSFactory.Api/DSFactory.Api.csproj", "DSFactory.Api/"]
COPY ["DSFactory.Core/DSFactory.Core.csproj", "DSFactory.Core/"]

# Restore dependencies
RUN dotnet restore "DSFactory.Api/DSFactory.Api.csproj"

# Copy the rest of the code
COPY . .

# Build and Publish the API
WORKDIR "/src/DSFactory.Api"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Run the application
# We use a smaller "Runtime" image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 8080 (standard for containers)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Start the app
ENTRYPOINT ["dotnet", "DSFactory.Api.dll"]