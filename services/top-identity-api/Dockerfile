
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/TOP.IdentityService.WebApi/TOP.IdentityService.WebApi.csproj", "src/TOP.IdentityService.WebApi/"]
RUN dotnet restore "src/TOP.IdentityService.WebApi/TOP.IdentityService.WebApi.csproj"
COPY . .
WORKDIR "/src/src/TOP.IdentityService.WebApi"
RUN dotnet build "TOP.IdentityService.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TOP.IdentityService.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TOP.IdentityService.WebApi.dll"]