# ----------- Build stage -------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution file first
COPY DemoFlow.sln .

# Copy project files for better Docker caching
COPY Api/*.csproj ./Api/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infastructure/*.csproj ./Infastructure/
RUN rm -rf /src/**/*.vs /src/**/bin /src/**/obj
# Restore NuGet packages
RUN dotnet restore DemoFlow.sln

# Copy all source code
COPY Api/ ./Api/
COPY Application/ ./Application/
COPY Domain/ ./Domain/
COPY Infastructure/ ./Infastructure/

# Build and publish the API project
WORKDIR /src/Api
RUN dotnet publish *.csproj -c Release -o /app/publish 

# ----------- Runtime stage -------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80

# Find the correct DLL name dynamically or use specific name
ENTRYPOINT ["dotnet", "Api.dll"]