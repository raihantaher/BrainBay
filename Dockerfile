FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BrainBay.Api/BrainBay.Api.csproj", "BrainBay.Api/"]
COPY ["BrainBay.Core/BrainBay.Core.csproj", "BrainBay.Core/"]
RUN dotnet restore "BrainBay.Api/BrainBay.Api.csproj"
COPY . .
WORKDIR "/src/BrainBay.Api"
RUN dotnet build "BrainBay.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BrainBay.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BrainBay.Api.dll"] 