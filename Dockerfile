FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

EXPOSE 2097

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore "farmaatte-api.csproj"
# Build and publish a release
RUN dotnet build "farmaatte-api.csproj" -o app/build

RUN dotnet publish -c Release -o /app/publish 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "farmaatte-api.dll"]