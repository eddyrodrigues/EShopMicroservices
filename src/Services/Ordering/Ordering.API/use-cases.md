## CRUD Ordering Operations:

- Get order with items (can filter by name and customer)
- Create a new Order
- Update an existing Order
- Delete Order
- Add and Remove Item from Order

## Asynchronous Ordering Operations:

- Basket Checkout: Consume Basket Checkou Event from RabbitMQ using MassTransit
- Order Fulfilment: Perform order fulfilment operation (billing, shipment, notification...)
- Raise OrderCreated Domain Event that leads to integration event

## Presentation Layer

- GET /orders
- GET /orders/{orderName} - 
- GET /orders/customer/{customerId} - Search by CustomerId
- POST /orders Create Order
- PUT /orders - Update Order
- DELETE /order/{id} - Delete Order with Id

## Common Principles

- SOLID, KISS, YAGNI, SoC, DIP principles and DI

## Domain Layer Patterns

- DDD oriented MS with DDD Tactical Patterns
- Domain Entity Pattern with Entity base classes - SeedWork
- Rich-domain model vs anemic-domain model
- The Value Object Pattern
- Aggregate pattern, Aggregate Root
- Strong typed IDs
- Domain Events vs Integration Events