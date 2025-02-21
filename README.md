# Reactivities
Based on of Neils Cummings's course on Udemy [Complete Guide to Building an App with .NET Core and React](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react/), and post from Jason Taylor [Clean Architecture with .NET Core](https://jasontaylor.dev/clean-architecture-getting-started/)

## Features

## Prerequisites
* [.NET Core SDK](https://dotnet.microsoft.com/download) (9 or later)
* [Node.js](https://nodejs.org/en/download/current/) (15.9.0 or later)

## Getting Started

### Database Configuration

The solution is configured to use Sqlite by default. Feel free to change to another database service as SQL Server or In-Memory Database if you want.

### Database Migration

To use `dotnet-ef` for your migrations add the following flags to your command (values assume you are executing from the root folder)

* `--project Infrastructure/`
* `--startup-project API/`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

 `dotnet ef migrations add "MyMigration" --project Infrastructure/ --startup-project API/ --output-dir Persistence/Migrations`

 Then, you need update de database from the startup project (API)
 
 `dotnet ef database update`

 **Important!** It needs to remove all migrations when changing a database service.

## Overview

### Domain

This contains all business entities to the domain layer. 

### Application

This layer contains the application logic. It's dependent on the domain layer but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application needs to access a notification service, a new interface would be added to the application and implementation would be created within the infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as persistence and identity services. These classes should be based (in most of the cases) on interfaces defined within the application layer. You can add other external resources as web services, SMTP, file system, etc. It's dependant on the application layer and has no other dependencies on any other layer or project.

### API

This layer is a Web API based on ASP.NET Core 7. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

### Clien-App

This is a client app based on React with typescript, it represent the front-end of this application.

Enjoy!
