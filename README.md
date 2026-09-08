# Code Challenge: Distributed Notification & Alerting System

## Overview
You are tasked with building a production-ready distributed notification and alerting system using .NET, Angular/React, SQL, caching, and Docker. This is a **3-day full-time** challenge designed to test your ability to build a highly scalable, reliable, and observable system that can handle real-time communication across multiple channels with guaranteed delivery.

## 🚨 Critical Constraints
- **NO AI ASSISTANCE**: The use of AI coding assistants (Copilot, ChatGPT, Cursor AI, etc.) is strictly prohibited. Any detection of AI-generated code will result in immediate disqualification.
- **Production-Ready**: Your solution must demonstrate production-quality code with proper error handling, logging, monitoring, security, and performance considerations.
- **Documentation Required**: You must document your architectural decisions, design patterns, and implementation notes in the README.md file. **Do not generate any design documents or diagrams** - these should be described in text within the README.

## Technical Stack
- **Backend**: .NET 8 (C#) with microservices architecture
- **Frontend**: Angular 17+ OR React 18+ with TypeScript
- **Database**: SQL Server or PostgreSQL with proper schema design
- **Caching**: Redis or similar caching solution
- **Message Queue**: RabbitMQ, Azure Service Bus, or Kafka
- **Container**: Docker & Docker Compose
- **API Gateway**: Ocelot or YARP
- **Authentication**: JWT-based authentication
- **SignalR/WebSockets**: For real-time notifications

## Core Requirements

### 1. Microservices Architecture (6 services minimum)

#### Service 1: Notification Template Service
- CRUD operations for notification templates
- Template versioning support
- Dynamic template rendering with placeholders
- Template preview functionality
- Template categorization (email, SMS, push, in-app)
- Multi-language template support

#### Service 2: Notification Scheduler Service
- Schedule notifications for future delivery
- Recurring notification schedules (daily, weekly, monthly)
- Timezone-aware scheduling
- Priority queuing (high, medium, low)
- Batch scheduling capabilities

#### Service 3: Notification Delivery Service
- Multi-channel delivery (Email, SMS, Push, In-app)
- Provider abstraction (simulate multiple providers)
- Delivery retry logic with exponential backoff
- Fallback provider support
- Channel-specific formatting
- Delivery tracking and status updates

#### Service 4: User Preference Service
- User notification preferences management
- Channel opt-in/opt-out
- Quiet hours configuration
- Category-based subscriptions
- Frequency capping (throttling)
- Preference change history

#### Service 5: Analytics & Reporting Service
- Delivery metrics collection
- Engagement tracking (open rates, click rates)
- Real-time dashboard metrics
- Report generation (daily, weekly, monthly)
- Anomaly detection (suspicious delivery patterns)
- Performance SLA monitoring

#### Service 6: Alert & Incident Management Service
- Alert definition and configuration
- Threshold-based alerting
- Escalation policies
- Alert grouping and deduplication
- Incident creation and tracking
- Alert acknowledgement and resolution workflow

#### Service 7: Webhook & Integration Service (Optional but recommended)
- Webhook registration and management
- Retry mechanism for failed webhooks
- Webhook security (signature verification)
- Integration with third-party services
- Event filtering and transformation

### 2. Frontend Application

Build a single-page application with the following features:

#### Admin Dashboard
- **Notification Management**: Create, preview, and send notifications
- **Template Library**: Browse and manage all templates
- **Scheduling Console**: View and manage scheduled notifications
- **Analytics Dashboard**: Real-time delivery metrics and charts
- **Alert Management**: View and manage system alerts
- **User Management**: Manage user preferences and settings

#### User Features
- **Preference Center**: Manage notification preferences
- **Notification History**: View all received notifications
- **Real-time Updates**: Live notification feed (WebSocket)
- **Quiet Hours**: Configure quiet hours for each channel
- **Categories**: Subscribe to specific notification categories

#### Real-time Features
- Live notifications feed using SignalR/WebSockets
- Real-time delivery status updates
- Live dashboard metrics updates
- Alert notifications with sound/visual indicators

### 3. Technical Requirements

#### Architecture Patterns
- **Event Sourcing**: Track state changes as a sequence of events
- **CQRS**: Separate read and write models for optimal performance
- **Outbox Pattern**: Ensure reliable message delivery
- **Saga Pattern**: Manage distributed transactions across services
- **Circuit Breaker**: Prevent cascading failures

#### Communication Patterns
- **Synchronous**: REST APIs for command/query operations
- **Asynchronous**: Message queues for event-driven communication
  - Events: NotificationCreated, Delivered, Failed, Opened, Clicked, etc.
- **Pub/Sub**: For broadcasting notifications to multiple consumers

#### Caching Strategy
- Implement Redis caching for:
  - Notification templates (with cache invalidation)
  - User preferences (with TTL)
  - Channel provider configurations
  - Rate limiting data
  - Session state
- Implement distributed caching across services

#### Database Design
- Each microservice has its own database schema
- Implement event store for event sourcing
- Read/write model separation for CQRS
- Proper indexing for high-volume tables
- Partitioning strategy for notification history

#### Message Queue Implementation
- Prioritized queues (critical > high > medium > low)
- Dead letter queue for failed messages
- Message deduplication
- Idempotent message processing
- Delivery guarantees (at-least-once, exactly-once)

#### Resilience
- Retry policies with exponential backoff
- Circuit breaker patterns
- Bulkhead isolation
- Rate limiting per channel and per user
- Fallback providers for delivery

#### Security
- JWT-based authentication with refresh tokens
- Role-based authorization (Admin, Manager, Viewer, User)
- API key authentication for external integrations
- Encryption for sensitive data (PII)
- Audit logging for all operations

### 4. Production-Ready Features

#### Observability
- **Distributed Tracing**: OpenTelemetry integration
- **Structured Logging**: Serilog with correlation IDs
- **Metrics**: Prometheus metrics for business and technical KPIs
- **Health Checks**: Detailed health probes for each service
- **Dashboards**: Grafana dashboard templates

#### Performance Requirements
- **Latency**: P95 < 500ms for notification delivery
- **Throughput**: Support 10,000 notifications per minute
- **Concurrency**: Handle 1000 concurrent users
- **Efficiency**: Optimize batch operations

#### Reliability
- **SLAs**: 99.9% uptime for critical services
- **Disaster Recovery**: Graceful degradation strategy
- **Data Consistency**: Strong consistency for critical operations
- **Backup Strategy**: Automated database backups

#### Testing Strategy
- Unit tests for business logic
- Integration tests for service communication
- Load tests for scalability validation
- Chaos testing for resilience verification
- Contract testing for service APIs

## Deliverables

### 1. Source Code
- Complete working solution with all services
- Clean, well-structured code following DDD principles
- Proper abstraction layers (Domain, Application, Infrastructure)
- Comprehensive unit and integration tests

### 2. README.md (Critical)
- **Architecture Overview**: Describe your microservices architecture, events, and data flow
- **Design Decisions**: Explain CQRS/Event Sourcing choices, message queue selection, and caching strategy
- **Implementation Notes**: Document interesting implementation details, challenges, and solutions
- **Setup Instructions**: Step-by-step guide to run the application locally with sample data
- **API Documentation**: Brief overview of key endpoints and event contracts
- **Deployment Strategy**: How you would deploy this to production
- **Future Improvements**: What would you add given more time

### 3. Docker Configuration
- Multi-stage Dockerfiles for each service
- Docker Compose with all services, Redis, SQL Server/PostgreSQL, and message queue
- Health checks for all containers
- Environment variable configuration
- Volume mounts for persistence

### 4. Database Scripts
- Event store schema
- Read/write model schemas
- Migration scripts with rollback support
- Seed data for demonstration
- Partitioning scripts for history tables

### 5. Monitoring Configuration
- Prometheus configuration
- Grafana dashboard templates (if applicable)
- Health check endpoints implementation
- Custom metrics definitions

## Evaluation Criteria

### Architecture & Design (30%)
- Microservices boundaries and independence
- Event-driven architecture implementation
- CQRS and Event Sourcing implementation
- Data consistency and resilience strategies
- Scalability considerations

### Code Quality & Implementation (25%)
- Clean, maintainable, and well-tested code
- Proper use of design patterns and SOLID principles
- Error handling and logging
- Security implementation

### Performance & Reliability (20%)
- Caching strategy effectiveness
- Queue and message processing
- Resilience patterns implementation
- Database query optimization

### Production Readiness (15%)
- Observability (logs, metrics, traces)
- Health checks and monitoring
- Deployment configuration
- Documentation quality

### Frontend & UX (10%)
- Real-time features implementation
- User interface quality
- State management
- API integration

## Time Management Suggestion

### Day 1: Foundation & Architecture
- Set up solution structure with DDD
- Implement event sourcing and event store
- Set up message queue and outbox pattern
- Create database schemas
- Configure Docker environment

### Day 2: Core Services & Integration
- Implement all business services
- Set up CQRS pattern
- Implement caching layer
- Create message consumers and handlers
- Add resiliency patterns

### Day 3: Frontend & Polish
- Build React/Angular application
- Implement SignalR/WebSocket connections
- Create dashboards and real-time features
- Add monitoring and logging
- Write tests and documentation
- Finalize deployment configuration

## Sample Scenarios to Handle

### Scenario 1: High-Volume Alert
- Send urgent alert to 100,000 users
- Prioritize delivery over other notifications
- Handle channel failures gracefully
- Track delivery status in real-time

### Scenario 2: User Preference Update
- User updates notification preferences
- Update all schedules and future notifications
- Ensure consistent state across services
- Handle concurrent preference updates

### Scenario 3: Provider Failure
- Primary email provider fails
- Automatically failover to secondary provider
- Retry failed messages with backoff
- Alert operations team

### Scenario 4: Scheduled Campaign
- Schedule newsletter for 5,000 subscribers
- Handle timezone differences
- Throttle delivery to prevent rate limiting
- Provide campaign analytics

### Scenario 5: System Alert
- System detects abnormal delivery rate
- Auto-generate alert based on threshold
- Escalate to appropriate team members
- Track incident lifecycle

## Notes
- Focus on **working functionality** over perfection
- Use simulated external services (SMTP, SMS, Push providers)
- Document all assumptions and trade-offs
- Make decisions that demonstrate architectural maturity
- Remember: This should be a **production-ready** application

Good luck! This challenge tests your ability to build complex, distributed systems with real-world constraints. Focus on delivering a well-architected solution with clear documentation that demonstrates your engineering expertise.

**Deadline**: 72 hours from start time
**Submission**: GitHub repository with all deliverables
