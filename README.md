# Fraud Detection

Backend-система для оценки риска финансовых транзакций в реальном времени.

Проект разрабатывается как практическая backend-система с упором на **C#, .NET, PostgreSQL, конкурентность, кеширование, асинхронную обработку и распределённые системы**.

Основная идея — получить транзакцию, собрать необходимый контекст пользователя, прогнать её через набор независимых правил риска и вернуть итоговый score, решение и причины срабатывания правил.

## Architecture

На текущем этапе система работает по следующей схеме:

```text
                    HTTP API
                       │
                       ▼
              ┌─────────────────┐
              │   Risk Engine   │
              └────────┬────────┘
                       │
                       ▼
              ┌─────────────────┐
              │  Risk Context   │
              │    Factory      │
              └────────┬────────┘
                       │
             ┌─────────┴─────────┐
             ▼                   ▼
      Device History      Location History
             │                   │
             └─────────┬─────────┘
                       ▼
                  Risk Rules
                       │
                       ▼
                Risk Assessment
                       │
              ┌────────┴────────┐
              ▼                 ▼
           Decision           Reasons
              │
              ▼
          PostgreSQL
```

Каждое правило реализует общий интерфейс `IRiskRule`, поэтому правила можно добавлять независимо друг от друга.

## Current Features

### Transaction evaluation

API принимает транзакцию и оценивает её риск:

```http
POST /transactions
```

Пример запроса:

```json
{
  "UserId": "artem",
  "Amount": 1000000,
  "Currency": "RUB",
  "DeviceId": "android",
  "Country": "RU"
}
```

Пример результата:

```json
{
  "TransactionId": "3e2dad61-c65e-4aa1-ba87-b257e2d79664",
  "Rating": 90,
  "Reasons": [
    "New device detected",
    "New country detected",
    "Transaction occurred during night hours"
  ]
}
```

### Risk rules

Сейчас реализованы следующие правила:

| Rule                   | Score |
| ---------------------- | ----: |
| New device             |   +30 |
| New country            |   +40 |
| Large amount           |   +30 |
| Night-time transaction |   +20 |

Правила работают независимо и могут комбинироваться.

Например:

```text
New device       +30
New country      +40
Night transaction +20
---------------------
Total             90
```

### User history

Для оценки транзакции система получает историю пользователя из PostgreSQL.

В частности, используются:

* ранее использованные устройства;
* ранее использованные страны.

Запросы истории выполняются асинхронно и параллельно:

```csharp
var deviceHistory =
    database.GetDeviceHistory(transaction.UserId, cancellationToken);

var locationHistory =
    database.GetLocationHistory(transaction.UserId, cancellationToken);

await Task.WhenAll(deviceHistory, locationHistory);
```

Для каждого запроса создаётся отдельный `DbContext` через `IDbContextFactory`, что позволяет безопасно выполнять независимые запросы параллельно.

## Technology Stack

* **C#**
* **.NET 10**
* **ASP.NET Core**
* **Entity Framework Core**
* **PostgreSQL**
* **Npgsql**
* **Docker**
* **Swagger / OpenAPI**

Планируемые технологии:

* Redis
* RabbitMQ
* Docker Compose
* OpenTelemetry
* Prometheus
* Grafana
* k6
* Testcontainers

## Project Structure

```text
FraudDetection/
│
├── Contracts/
│   ├── Interfaces/
│   ├── Models/
│   └── RiskEngine/
│
├── DataAccess.Postgres/
│   ├── PostgresDatabase.cs
│   ├── PostgresDbContext.cs
│   └── Migrations/
│
├── RiskEngine/
│   ├── RiskEngine.cs
│   ├── RiskContextFactory.cs
│   └── Rules/
│
└── WebApplication1/
    ├── Controllers/
    ├── Program.cs
    └── appsettings.json
```

Основные зависимости:

```text
WebApplication
       │
       ▼
   RiskEngine
       │
       ▼
   IDatabase
       │
       ▼
PostgresDatabase
       │
       ▼
IDbContextFactory
       │
       ▼
PostgresDbContext
       │
       ▼
  PostgreSQL
```

## Database

Для работы с PostgreSQL используется Entity Framework Core.

`PostgresDbContext` отвечает за EF Core-модель и mapping:

```csharp
public class PostgresDbContext : DbContext
{
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RiskAssessmentResult> RiskAssessmentResults { get; set; }
}
```

Работа с базой скрыта за интерфейсом:

```csharp
public interface IDatabase
{
    Task<IEnumerable<string>> GetDeviceHistory(
        string userId,
        CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetLocationHistory(
        string userId,
        CancellationToken cancellationToken);

    // ...
}
```

Это позволяет Risk Engine не зависеть напрямую от Entity Framework Core или PostgreSQL.

## Running locally

### Requirements

* .NET 10 SDK
* Docker Desktop
* PostgreSQL или Docker

### Start PostgreSQL

```bash
docker compose up -d
```

Проверить контейнер:

```bash
docker ps
```

### Run the application

```bash
dotnet run --project WebApplication1
```

После запуска API доступно через настроенный ASP.NET Core endpoint.

В development environment доступна OpenAPI-документация.

## Development Roadmap

Проект развивается поэтапно.

### Completed

* [x] Базовый Risk Engine
* [x] Система независимых risk rules
* [x] Оценка transaction score
* [x] PostgreSQL
* [x] Entity Framework Core
* [x] Получение истории пользователя
* [x] Параллельное получение независимых данных
* [x] Сохранение результатов оценки
* [x] Dependency Injection
* [x] CancellationToken

### Next

* [ ] Unit tests для risk rules
* [ ] `RiskDecision`: `Approve / Review / Block`
* [ ] Индексы PostgreSQL
* [ ] Оптимизация запросов
* [ ] Concurrency tests
* [ ] Redis для velocity checks
* [ ] Rate limiting
* [ ] Idempotency
* [ ] RabbitMQ
* [ ] Outbox Pattern
* [ ] Retry / Dead Letter Queue
* [ ] Динамические risk rules
* [ ] Lexer / Parser / AST для правил
* [ ] Географические проверки
* [ ] Manual review / audit
* [ ] Load testing
* [ ] OpenTelemetry
* [ ] Prometheus / Grafana

## Example Risk Rules

Пример простого правила:

```csharp
public class NewDeviceRule : IRiskRule
{
    private const int NEW_DEVICE_RISK_SCORE = 30;

    public RiskRuleResult Evaluate(RiskContext riskContext)
    {
        if (!riskContext.DeviceHistory.Contains(
            riskContext.Transaction.DeviceId))
        {
            return new RiskRuleResult(
                "New device detected",
                NEW_DEVICE_RISK_SCORE);
        }

        return new RiskRuleResult(null, 0);
    }
}
```

Добавление нового правила не требует изменения существующих правил.

Например:

```csharp
public class LargeAmountRule : IRiskRule
{
    // ...
}
```

Затем правило регистрируется через Dependency Injection:

```csharp
builder.Services.AddSingleton<IRiskRule, LargeAmountRule>();
```

Risk Engine получает коллекцию всех зарегистрированных `IRiskRule` и последовательно применяет их к `RiskContext`.

## Goals

Проект не является просто CRUD-приложением.

Основная цель — последовательно реализовать типичные backend-задачи, которые возникают в системах обработки транзакций:

* работа с реляционной БД;
* оптимизация запросов;
* asynchronous programming;
* concurrency;
* caching;
* idempotency;
* message brokers;
* гарантированная доставка сообщений;
* distributed state;
* observability;
* load testing.

По мере развития проекта синхронная версия Risk Engine будет дополнена асинхронной обработкой транзакций через message broker.

## License

This project is currently intended for educational and portfolio purposes.
