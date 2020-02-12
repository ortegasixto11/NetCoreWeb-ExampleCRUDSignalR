#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
##EXPOSE 80
#EXPOSE 443
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR /src
#COPY ["NetCoreWeb-ExampleCRUDSignalR/NetCoreWeb-ExampleCRUDSignalR.csproj", "NetCoreWeb-ExampleCRUDSignalR/"]
#RUN dotnet restore "NetCoreWeb-ExampleCRUDSignalR/NetCoreWeb-ExampleCRUDSignalR.csproj"
#COPY . .
#WORKDIR "/src/NetCoreWeb-ExampleCRUDSignalR"
#RUN dotnet build "NetCoreWeb-ExampleCRUDSignalR.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "NetCoreWeb-ExampleCRUDSignalR.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "NetCoreWeb-ExampleCRUDSignalR.dll"]


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build-env
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NetCoreWeb-ExampleCRUDSignalR.dll"]