FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY SB.GestionSolicitudes.sln ./
COPY SB.GestionSolicitudes.Domain/*.csproj ./SB.GestionSolicitudes.Domain/
COPY SB.GestionSolicitudes.Application/*.csproj ./SB.GestionSolicitudes.Application/
COPY SB.GestionSolicitudes.Services/*.csproj ./SB.GestionSolicitudes.Services/
COPY SB.GestionSolicitudes.Infrastructure/*.csproj ./SB.GestionSolicitudes.Infrastructure/
COPY SB.GestionSolicitudes.Api/*.csproj ./SB.GestionSolicitudes.Api/
COPY SB.GestionSolicitudes.Tests/*.csproj ./SB.GestionSolicitudes.Tests/

# Restore dependencies
RUN dotnet restore SB.GestionSolicitudes.sln

# Copy all source files
COPY . ./

# Build and Publish
RUN dotnet publish SB.GestionSolicitudes.Api/SB.GestionSolicitudes.Api.csproj -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /out ./

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "SB.GestionSolicitudes.Api.dll"]
