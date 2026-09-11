# Distributed Notification & Alerting System

## Architecture

The system follows a microservices architecture using .NET 8, CQRS, Outbox Pattern, RabbitMQ and simulated providers.

## Current Services

### Notification Service

Responsible for Notification Creation and Retrieval,
Persisting notification data
Creating events using Transactional Outbox Pattern
Adding a RabbitMQ Publisher

### Delivery Service

Consuming notification.created events from RabbitMQ, simulating different providers.
Adding Dead Letter Queue for failed messages

### Architectural Decisions

Pending for the final commit in order to explain with a little flow

### CQRS

Implemented for separate Command and Query using MediatR for notification messages

### RabbitMQ Local Development

Run RabbitMQ using Docker.Desktop, run the next command line in case you don't have installed RabbitMQ
-> docker run -d -- hostname notification-rabbit -- name notification
-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:4-management

RabbitMQ is available on: http://localhost:15672
Credentials are available on appsettings.json

### Run the App

RabbitMQ must be running before starting the services.
Run with "docker start notification-rabbit"

In Visual Studio configure multiple startup projects:

NotificationService.Api                 Start
NotificationSystem.DeliveryService      Start

Then start the solution.

### Create a Notification

On Swagger:

POST /api/notifications
{
  "userId": "73ad9df1-1813-4e7c-beb6-683536179443",
  "subject": "Testing",
  "content": "Notification for you",
  "channel": 1,
  "priority": 3
}

##### Updates by Dates

09/09
- Database, creation and retrieval working as expected, first backend using mediatR, .NET 8 and EF 8 ready.
09/10
- Outbox, Publisher, RabbitMQ and Providers using one background consumer
