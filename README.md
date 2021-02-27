# Reactivities
Based on the udemy course of [Neils Cummings](https://www.udemy.com/course/complete-guide-to-building-an-app-with-net-core-and-react/) and [Jason Taylor's blog](https://jasontaylor.dev/clean-architecture-getting-started/)

## Features

## Prerequisites
* [.NET Core SDK](https://dotnet.microsoft.com/download) (5.0.1 or later)
* [Node.js] (https://nodejs.org/en/download/current/) (15.9.0 or later)

## Technologies

### ASP .NET CORE 5
* Entity Framework Core
* MediatR
* Automapper
* FluentValidation

### React 17
* MobX 6
* Axios
* Semantic UI React

## Getting Started

### Database Configuration

### Database Migration

## Overview

### Domain

This contains all business entities to the domain layer. 

### Application

This layer contains the application logic. It's dependent on the domain layer but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application needs to access a notification service, a new interface would be added to the application and implementation would be created within the infrastructure.

### Infrastructure

This layer contains classes for accessing external resources such as persistence and identity services. These classes should be based (in most of the cases) on interfaces defined within the application layer. You can add other external resources as web services, SMTP, file system, etc. It's dependant on the application layer and has no other dependencies on any other layer or project.

### API

This layer is a Web API based on ASP.NET Core 5. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.

### Clien-App

This is a client app based on React Js 17, and represent the front-end of this application.

Enjoy!
And if you want <a href="https://www.buymeacoffee.com/dealvarado" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/default-orange.png" alt="Buy Me A Coffee" height="41" width="174"></a>