# StockFlow 📦

Modern Inventory Management System built with **.NET 10** and **Clean Architecture**.

## 🚀 Tech Stack

* **.NET 10** - C# 14
* **Docker & Docker Compose** - Containerization
* **MS SQL Server 2022** - Database
* **MediatR** - CQRS Pattern
* **FluentValidation** - Validation Pipeline
* **AutoMapper** - Object Mapping
* **Scalar** - API Documentation

## 📂 Project Structure

The solution follows the **Clean Architecture** principles:

* **StockFlow.Domain** - Enterprise logic, Entities, Value Objects (No dependencies).
* **StockFlow.Application** - Use Cases, Interfaces, DTOs (Depends on Domain).
* **StockFlow.Infrastructure** - DB Implementation, External Services (Depends on Application & Domain).
* **StockFlow.Api** - Entry point, Controllers, Configuration (Depends on Application & Infrastructure).

## 🛠️ How to Run

1.  **Prerequisites:**
    * Docker Desktop installed and running.
    * .NET 10 SDK installed.
    * Visual Studio 2026.

2.  **Start the Application:**
    Open the solution in Visual Studio and set docker-compose.dcproj as the startup project.
    Press **F5**.

3.  **Access the API:**
    Documentation is available at: `http://localhost:5000/scalar/v1`
