FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY CurrencyGateway.sln ./

COPY CurrencyGateway.Api/CurrencyGateway.Api.csproj CurrencyGateway.Api/
COPY CurrencyGateway.Application/CurrencyGateway.Application.csproj CurrencyGateway.Application/
COPY CurrencyGateway.Domain/CurrencyGateway.Domain.csproj CurrencyGateway.Domain/
COPY CurrencyGateway.Infrastructure/CurrencyGateway.Infrastructure.csproj CurrencyGateway.Infrastructure/
COPY CurrencyGateway.Tests/CurrencyGateway.Tests.csproj CurrencyGateway.Tests/

RUN dotnet restore CurrencyGateway.sln

COPY . .

RUN dotnet publish CurrencyGateway.Api/CurrencyGateway.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "CurrencyGateway.Api.dll"]