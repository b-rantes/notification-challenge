# Tech Challenge MELI - Notification System

## Introduction
The Notification System is a backend platform that allows MELI products to send notifications about various events that occur within the system. 
This project aims to provide a scalable and resilient solution for delivering notifications to users in a timely manner.

## Prerequisites
Before running the Notification System, you will need to have the following software installed:
- [Docker](https://www.docker.com/)
- [.NET 6](https://dotnet.microsoft.com/download/dotnet/6.0)

## Additional Components (via Docker)
- [Redis](https://redis.io/)
- [MongoDB](https://www.mongodb.com/)
- [Confluent Kafka](https://www.confluent.io/platform/kafka/)

## Architecture
The Notification System is built with .NET 6 as the backend and leverages MongoDB as the data source for both users and notifications. Redis is used as the caching mechanism for improved performance. The communication between the different components of the system is facilitated by Confluent Kafka, which also helps with resilience and scalability.

![projeto meli drawio](https://user-images.githubusercontent.com/124640248/218335242-f7e5f7ac-748f-4c7a-a3f2-fab35f38142d.png)

The architecture of the system follows the CQRS (Command Query Responsibility Segregation) pattern, with separate databases for queries (Redis) and commands (MongoDB). The synchronization between these databases is achieved through domain events, which are produced by a Kafka Producer, consumed by workers, and used to keep the cache up-to-date. In case data is not present in the cache, the system will fall back to the persisted data in MongoDB.

The implementation of the system follows Clean Architecture principles, with a clear separation of concerns in the code. This approach helps prepare the solution for future deployment as microservices and makes it easier to maintain separate code repositories.

The Notification System has one main entry point for creating a new notification for a user, which is the "create-user-notification-command" topic. This topic is responsible for creating a new notification and storing it in the system's database. The user's request to the backend also updates the client's state, updating their reference to the "last opened notification date".
Updating the user's reference to the "date of the last opened notification" through their request to the backend not only serves as a way to track user activity and provide a more personalized experience, but also as a means to enhance system performance by avoiding millions of additional database operations per day.


Finally, the scheduling mechanism of the system is simply a separate component that delays the creation of notifications until the scheduled date arrives. This component operates outside of the main code structure and is simply a part of the overall system design.

If desired the image can be opened at [Draw.io](https://draw.io) just executing the file "projeto meli.drawio" contained in the root folder

## Execution Instructions (assuming the prerequisites have been met)
1. Clone the repository.
2. From the root directory, run the command docker-compose up to build instances of MongoDB, Redis, and Kafka.
3. Open the solution and start the Web API project as the entry point.
4. Verify that the settings in the appsettings file are pointing to localhost.
5. You can access the Kafka cluster at localhost:9000 in your web browser.
6. The API documentation can be found at localhost:<port>/swagger.
7. You can connect to Redis at port 6379 using Redis Insights and to MongoDB at port 27017 with the credentials user: admin and password: admin.
8. Use the AdminController in the Swagger API to interact with the Kafka Producers.
9. The NotificationController can be used to fetch and manage user notifications, including opt-in/opt-out options.

## Points for Evolution
The most relevant points for evolution are:

1. Scheduling mechanism, which uses the Hangfire library integrated with Mongo for scheduling control. Under the hood, it performs several database polls at regular intervals, which is not recommended for high volume. The functionality to delete schedules and its tight coupling with the code should be thoroughly reviewed.

2. The reactivity between the back-end and front-end, which currently does not exist as the calls are made through HTTP requests. The idea here is to transform these HTTP requests into SSE (which also uses the HTTP protocol), so that in a unidirectional manner, the back-end can use the Pub/Sub mechanism of Redis to hold the thread and deliver new notifications almost instantly to the client whenever there is a new notification, since every new notification in the back-end ultimately sensitizes the Redis cache.

## Conclusion
The Notification System is a scalable and resilient backend platform that provides notifications to users through a well-architected solution. It is built using .NET 6 as the backend, and leverages MongoDB as the data source, Redis as the caching mechanism, and Confluent Kafka for communication and synchronization between components. The system follows Clean Architecture principles and the CQRS (Command Query Responsibility Segregation) pattern, and uses the AdminController and NotificationController for interaction with the system. The system can be executed by cloning the repository and running docker-compose, and the API documentation can be found at localhost:<port>/swagger. Load testing will be performed using K6. However, there are points for evolution, such as the scheduling mechanism and the reactivity between the backend and frontend.
