﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# Install Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_14.x | bash - \
    && apt-get install -y \
        nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src
COPY ["src/Federation.App/Federation.App.csproj", "Federation.App/"]
RUN dotnet restore "src/Federation.App/Federation.App.csproj"
COPY . .
WORKDIR "/src/Federation.App"
RUN dotnet build "Federation.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Federation.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Federation.App.dll"]
