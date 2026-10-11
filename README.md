# CurrencyGateway
CurrencyGateway — .NET 8 Web API для получения курсов валют с сайта Банка России.
Сервис выступает шлюзом между внутренними системами и Банком России: внутренняя система отправляет HTTP GET-запрос, приложение получает данные от ЦБ, преобразует их и возвращает результат в JSON.

## Возможности
- Получение всех курсов валют на указанную дату
- Получение курса конкретной валюты по коду
- Использование текущей даты, если дата не указана
- Возврат 204 No Content, если указанная валюта не найдена
- Валидация входных параметров
- Обработка ошибок внешнего сервиса
- Swagger / OpenAPI
- Docker
- Jenkins pipeline
- Unit-тесты

## Технологии
- .NET 8
- ASP.NET Core Web API
- HttpClient
- XML Serialization
- Swagger / OpenAPI
- Docker
- Jenkins
- xUnit
- Moq

## Архитектура
Решение разделено на несколько проектов:
CurrencyGateway.Api
CurrencyGateway.Application
CurrencyGateway.Domain
CurrencyGateway.Infrastructure
CurrencyGateway.Tests

### CurrencyGateway.Api
Содержит HTTP API:
- Controllers
- API contracts
- DI configuration
- Swagger
- обработку HTTP-ошибок

### CurrencyGateway.Application
Содержит бизнес-логику приложения:
- ICurrencyService
- ICbrClient
- CurrencyService
- прикладные исключения

### CurrencyGateway.Domain
Содержит доменные модели:
- CurrencyRate

### CurrencyGateway.Infrastructure
Содержит интеграцию с внешними сервисами:
- CbrClient
- модели XML-ответа Банка России

### CurrencyGateway.Tests
Содержит unit-тесты бизнес-логики.

## API
### Получить все валюты
GET /api/currencies
Если дата не передана, используется текущая дата.

### Получить все валюты на определённую дату
GET /api/currencies?date=2026-10-08

### Получить конкретную валюту
GET /api/currencies?code=USD

### Получить конкретную валюту на определённую дату
GET /api/currencies?date=2026-10-08&code=USD

Пример ответа:
json
[
  {
    "code": "USD",
    "name": "Доллар США",
    "nominal": 1,
    "value": 85.4811,
    "date": "2026-10-08"
  }
]

## HTTP-коды
- 200 OK — данные успешно получены
- 204 No Content — указанная валюта не найдена
- 400 Bad Request — некорректные входные параметры
- 500 Internal Server Error — внутренняя ошибка приложения
- 502 Bad Gateway — ошибка при обращении к Банку России

## Swagger
После запуска приложения Swagger UI доступен по адресу: /swagger
Например: http://localhost:5197/swagger
Также в корне проекта находится файл: openapi.yaml с описанием API в формате OpenAPI 3.0.

## Запуск приложения
Из корня solution:
dotnet restore
dotnet build
dotnet run --project CurrencyGateway.Api/CurrencyGateway.Api.csproj

После запуска API будет доступен по адресу, указанному ASP.NET Core, например: http://localhost:5197

## Unit-тесты
Запуск тестов: dotnet test
Реализованы unit-тесты для CurrencyService.

Проверяются сценарии:
- получение всех валют
- фильтрация по коду валюты
- регистронезависимый поиск валюты
- отсутствие запрашиваемой валюты
- передача указанной даты в клиент Банка России

## Docker
Сборка Docker image:
docker build -t currency-gateway .

Запуск: docker run --rm -p 8080:8080 currency-gateway
После запуска API будет доступен: http://localhost:8080/api/currencies
Для запуска Swagger в контейнере в Development-режиме: docker run --rm -p 8080:8080 -e ASPNETCORE_ENVIRONMENT=Development currency-gateway

## Jenkins
В корне проекта находится Jenkinsfile.

Pipeline выполняет:
Checkout
→ Restore
→ Build
→ Test
→ Docker Build

Для выполнения pipeline Jenkins-agent должен иметь: 
- .NET 8 SDK
- Docker

## Асинхронный вариант получения данных
В текущей реализации запрос к Банку России выполняется непосредственно при поступлении HTTP-запроса от клиента.
Для асинхронного варианта можно вынести получение курсов в отдельный фоновый процесс, например BackgroundService.
Фоновый сервис периодически получал бы данные Банка России и сохранял их в локальное хранилище или базу данных. HTTP API в таком случае возвращало бы уже сохранённые данные, не ожидая ответа внешнего сервиса.
При необходимости более сложной обработки можно использовать очередь сообщений. Запрос на обновление курса публиковался бы в очередь, отдельный worker получал бы данные Банка России и сохранял результат, а API работало бы с подготовленными данными.