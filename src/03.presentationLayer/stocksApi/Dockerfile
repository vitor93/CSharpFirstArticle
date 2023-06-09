#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["03.presentationLayer/stocksApi/StocksApi.csproj", "03.presentationLayer/stocksApi/"]
COPY ["01.repositoryLayer/stockRepository/StockRepository.csproj", "01.repositoryLayer/stockRepository/"]
COPY ["04.coreLayer/sharedMethods/SharedMethods.csproj", "04.coreLayer/sharedMethods/"]
COPY ["04.coreLayer/sharedModels/SharedModels.csproj", "04.coreLayer/sharedModels/"]
COPY ["02.serviceLayer/stockService/StockService.csproj", "02.serviceLayer/stockService/"]
COPY ["04.coreLayer/sharedConstants/SharedConstants.csproj", "04.coreLayer/sharedConstants/"]
RUN dotnet restore "03.presentationLayer/stocksApi/StocksApi.csproj"
COPY . .
WORKDIR "/src/03.presentationLayer/stocksApi"
RUN dotnet build "StocksApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StocksApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksApi.dll"]