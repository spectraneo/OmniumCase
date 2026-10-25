Here it is all in one block:

```markdown
# OmniumCaseApp

A .NET console application built as part of the Omnium developer case assignment. The app manages orders, order lines and products using a SQLite database.

## Tech Choices

**.NET 10 console app** — simple and straightforward for this use case, no need for a web framework.

**SQLite with Entity Framework Core** — lightweight database that requires no server setup. EF Core handles migrations and querying cleanly.

**TPH inheritance (Table Per Hierarchy)** — PosOrder inherits from Order. EF adds a Discriminator column so both types live in the same Orders table, avoiding duplication.

**Interface segregation** — IOrderService separates the contract from the implementation, making the service swappable and testable.

**Separation of concerns** — Models handle data shapes, Data handles database setup and seeding, Services handle business logic. Program.cs only orchestrates.

**xUnit for unit tests** — industry standard for .NET testing. Tests run against in-memory data with no database dependency.

## How to Run

dotnet run

## How to Run Tests

dotnet test

## Project Structure

OmniumCaseApp/Data — DbContext and DbSeeder
OmniumCaseApp/Models — Order, OrderLine, PosOrder, ProductSalesSummary
OmniumCaseApp/Services — IOrderService, OrderService
OmniumCase.Tests — unit tests for all 8 tasks

## Tasks Completed

Task 1 — Order and OrderLine models
Task 2 — GetOrders
Task 3 — CalculateOrderTotal
Task 4 — GetOrderByOrderId
Task 5 — GetOrdersByCustomerId
Task 6 — GetOrdersByProductId
Task 7 — PosOrder with inheritance
Task 8 — GetTopSellingProducts
```