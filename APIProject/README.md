# Distributed Notification & Alerting System

## Architecture

The system follows a microservices architecture using .NET 8

The notification service uses:
- DDD
- CQRS
- Entity Framework Core
- SQL Server
- MediatR

## Current Services

### Notification Service

Responsible for Notification Creation and Retrieval

### Architectural Decisions

#### CQRS


##### Updates by Dates

09/09
- Database, creation and retrieval working as expected, first backend using mediatR, .NET 8 and EF 8 ready.