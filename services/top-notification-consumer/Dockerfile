
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/", "."]
RUN dotnet restore "TOP.NotificationConsumer.ConsoleApp/TOP.NotificationConsumer.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/TOP.NotificationConsumer.ConsoleApp"
RUN dotnet build "TOP.NotificationConsumer.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TOP.NotificationConsumer.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TOP.NotificationConsumer.ConsoleApp.dll"]