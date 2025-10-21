# Stage 1: Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Stage 2: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file
COPY ["CoffeeManagement.sln", "./"]

# Copy project file(s)
COPY ["CoffeeManagement/CoffeeManagement.csproj", "CoffeeManagement/"]

# Restore dependencies
RUN dotnet restore "CoffeeManagement/CoffeeManagement.csproj"

# Copy all source code
COPY . .

# Build the project
WORKDIR "/src/CoffeeManagement"
RUN dotnet build "CoffeeManagement.csproj" -c Release -o /app/build

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "CoffeeManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Entry point
ENTRYPOINT ["dotnet", "CoffeeManagement.dll"]