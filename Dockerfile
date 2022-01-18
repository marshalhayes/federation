FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash - 
RUN apt-get install -y nodejs
RUN npm i -g yarn
WORKDIR /src
COPY ["src/Federation.App/Federation.App.csproj", "Federation.App/"]
COPY ["src/Federation.Services.Uscf/Federation.Services.Uscf.csproj", "Federation.Services.Uscf/"]
COPY ["src/Federation.Services.Core/Federation.Services.Core.csproj", "Federation.Services.Core/"]
RUN dotnet restore "Federation.App/Federation.App.csproj"
COPY . .
RUN dotnet build "src/Federation.App/Federation.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/Federation.App/Federation.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Federation.App.dll"]
