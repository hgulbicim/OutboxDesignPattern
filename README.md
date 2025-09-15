# Outbox Design Pattern

This project shows how to use the **Outbox Design Pattern** in C#.  
It saves messages in the database and sends them safely using RabbitMQ and MassTransit.  
It is a **reliable, scalable, and maintainable** message system.

## What is Outbox Pattern?

Outbox Pattern is a way to **make sure messages are not lost**.  
When something happens in the system (like a new user is created),  
the message is first saved in the database (the "Outbox table").  
Later, a background job reads these messages and sends them to RabbitMQ.  
This makes the system **reliable** and avoids losing messages if something fails.

## Key Advantages

- **Reliable delivery**: Messages are not lost even if the system fails  
- **Consistency**: Database and message queue stay in sync  
- **Scalable**: Consumers and jobs can run independently and scale easily  
- **Testable and maintainable**: Clear separation of responsibilities  
- **Cloud ready**: Can run in Kubernetes or other cloud environments  

## Features

- Save messages safely with Outbox Pattern  
- Send messages with RabbitMQ and MassTransit  
- Run background jobs with Quartz  
- Use Entity Framework Core for SQL Server  
- Map objects with AutoMapper  
- Easy to extend with new events  
- Jobs and consumers are independent and testable  

## Technologies

- C# .NET 9  
- MassTransit 8.5.2  
- RabbitMQ  
- Entity Framework Core 9.0.8  
- Quartz 3.15.0  
- AutoMapper 15.0.1  
- Docker  

## Possible Improvements

In the future, the project can be separated into multiple parts:

- **Publisher / API project**: To send events to the Outbox table  
- **Consumer project**: To listen to RabbitMQ and process messages  
- **Jobs project**: To run background tasks like sending messages from Outbox to RabbitMQ  
- **Events project**: To store event definitions  

This separation can make the system more **modular and easier to maintain**.  
The system can also **scale** to handle more messages under high load.

