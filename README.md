# 🧾 Pixelz.Checkout

A sample enterprise-grade **Clean Architecture** solution built with **.NET 9**, demonstrating an **Order Checkout Flow** including:
- CQRS with MediatR
- Clean Architecture + DDD principles
- Transactional Outbox pattern
- Background message processing
- Integration Event handling
- Mock external services (Payment, Email, Production)
- Health checks, validation, and detailed ProblemDetails error responses

---

## 🧩 Solution Overview

```
Pixelz.Checkout/
│
├── src/
│   ├── Pixelz.Domain/              # Core business models and enums
│   ├── Pixelz.Application/         # CQRS, DTOs, Services, Interfaces
│   ├── Pixelz.Infrastructure/      # EF Core, Repositories, Outbox, Services
│   ├── Pixelz.Messaging/           # Integration Events & Handlers (Mediator-based)
│   └── Pixelz.Api/                 # Minimal API (presentation layer)
│
└── tests/
    └── Pixelz.Tests/               # Unit and Integration tests
```

---

## ⚙️ Prerequisites

| Tool | Version | Purpose |
|------|----------|----------|
| [.NET SDK](https://dotnet.microsoft.com/) | 9.0+ | Runtime & SDK |
| [PostgreSQL](https://www.postgresql.org/) | 16+ | Database |
| [Docker](https://www.docker.com/) | Latest | Optional containerized environment |
| [EF Core Tools](https://learn.microsoft.com/ef/core/) | 9.0+ | Migration & DB management |

---

## 🚀 Getting Started

### 1️⃣ Clone the repo
```bash
git clone https://github.com/yourname/Pixelz.Checkout.git
cd Pixelz.Checkout
```

### 2️⃣ Setup connection string

Edit file:  
`src/Pixelz.Api/appsettings.json`
```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=pixelz_checkout;Username=postgres;Password=abc@123"
  }
}
```

### 3️⃣ Run EF Core migrations
```bash
cd src/Pixelz.Infrastructure
dotnet ef migrations add InitialCreate -o Persistence/Migrations -s ../Pixelz.Api --context PixelzWriteDbContext
dotnet ef database update -s ../Pixelz.Api --context PixelzWriteDbContext
```

### 4️⃣ Run the API
```bash
cd ../Pixelz.Api
dotnet run
```

Access Swagger UI at: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## 🧠 Architecture

Each layer depends only on the one below it:

```
API ➜ Application ➜ Domain
     ⬑ Infrastructure
     ⬑ Messaging
```

---

## 💳 Checkout Flow (End-to-End)

1. **User calls**
   ```
   POST /api/v1/orders/{orderId}/checkout
   ```

2. **Application Layer**
   - Validates command
   - Loads order
   - Calls `IPaymentService.ProcessPaymentAsync()`
   - Updates order status (`Paid` / `PaymentFailed`)
   - Adds integration event to Outbox
   - Commits transaction

3. **OutboxProcessor**
   - Reads unprocessed messages
   - Deserializes & publishes via MediatR
   - Marks them as processed

4. **Messaging Layer**
   - `EmailEventHandler`
   - `InvoiceEventHandler`
   - `ProductionEventHandler`

---

## 🧾 Example APIs

### GET /api/v1/orders
Search orders by keyword and pagination.

### POST /api/v1/orders/{id}/checkout
Processes checkout for an order.

---

## 🧰 Docker Setup

```bash
docker compose up --build
```

Access:
- API: [http://localhost:8080](http://localhost:8080)
- DB: `localhost:5432` (postgres / abc@123)

To stop:
```bash
docker compose down -v
```

---

## 🧪 Tests

```bash
dotnet test
```

Includes:
- Unit tests for Checkout flow
- OutboxProcessor tests
- Repository + integration tests

---

## 🧠 Patterns Used

- **CQRS**
- **Mediator**
- **Repository + Unit of Work**
- **Outbox pattern**
- **ProblemDetails middleware**
- **Background Worker**
- **Dependency Injection**

---

## 🧭 Example Event Flow

```
Client
  ↓
POST /orders/{id}/checkout
  ↓
CheckoutOrderHandler
  ↓
Order marked as Paid
  ↓
Add OutboxMessage(OrderPaidIntegrationEvent)
  ↓
OutboxProcessor publishes
  ↓
Handlers:
 ├─ EmailEventHandler
 ├─ InvoiceEventHandler
 └─ ProductionEventHandler
```

---

## 🧑‍💻 Author

**Pixelz Engineering Team**  
Clean Architecture + DDD + CQRS demo for .NET 9  
© 2025 Pixelz SA. All rights reserved.
