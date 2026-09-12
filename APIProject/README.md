# Distributed Notification & Alerting System

## Architecture

The system follows a microservices architecture using .NET 8, SQL Server, EF Core, MediatR, Outbox Pattern, RabbitMQ, Redis, Docker and simulated providers.

The Implementation was focused on the backend in order to in the future can be used by the other microservices and the front-end client

## Current Services

### Notification Service

Responsible for Notification Creation and Retrieval,
Persisting notification data through SQL Server and Redis (for caching)
Creating events using Transactional Outbox Pattern
Adding a RabbitMQ Publisher
Health Checks

### Delivery Service

Consuming notification.created events from RabbitMQ, simulating Email, SMS, Push and In-App Providers.
Adding Dead Letter Queue for failed messages.
Retry with exponential backoff

## Architectural Decisions

### CQRS

Implemented for separate Command and Query using MediatR for notification messages

### Outbox Pattern

Notifications and their events are persisted together and a background job later publishes pending outbox messages to RabbitMQ

### RabbitMQ

RabbitMQ is used for asynchronous communication between Notification Service API and Delivery Service worker
Both services inside this solution are using manual ACK and a Dead Letter Queue for messages that can't be delivered.

### Delivery Realiability

The Delivery Service retries provider failures using exponencial backoff, until four attemps, after last
retry is using the Dead Letter Queue.

### Redis

Notifications queries are using cache-aside strategy in the GET endpoint.
When the notification status changes to Delivered or Failed, the cache entry is deleted.

### Local Infraestructure

Using Docker Compose currently runs:
* RabbitMQ
* Redis

File .yml is ready to up: docker compose up -d

### Testing Failure Scenarios

Notifications should be include "[FAIL]" string in the subject to simulate a notification failed.
This trigger the retry mechanism.

## Run the App

### RabbitMQ Management

RabbitMQ Management UI is available at: http://localhost:15672

Default credentials:
Username: guest
Password: guest

### Redis

Redis is available at: http://localhost:6379

You can verify the Redis connection with: docker exec notification-redis redis-cli PING

Expected response: PONG

### Run Visual Studio

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

### Create a Notification Failed

On Swagger:

POST /api/notifications
{
  "userId": "73ad9df1-1813-4e7c-beb6-683536179443",
  "subject": "[FAIL] Testing",
  "content": "This notification must fail",
  "channel": 1,
  "priority": 3
}


### Future Improvements

Given additional development time, the next priorities would be:

* Add JWT authentication and authorization
* Implement remaining microservices
* Build Angular frontend
* Add SignalR real-time status updates
* Add YARP API Gateway
* Implement circuit breaker and provider fallback
* Add OpenTelemetry distributed tracing
* Add Prometheus and Grafana monitoring
* Containerize all application services
* Add integration and load tests
* Improve RabbitMQ connection reuse
* Add provider-level idempotency support


#### Updates by Dates

09/09
- Database, creation and retrieval working as expected, first backend using mediatR, .NET 8 and EF 8 ready.
09/10
- Outbox, Publisher, RabbitMQ and Providers using one background consumer
09/11 
- Delivery Service with Retries/DLQ, Redis, Docker, Healthchecks, Tests and swagger documentation. 
