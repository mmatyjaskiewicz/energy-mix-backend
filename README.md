# Energy Mix API

Energy Mix API is a .NET Web API application that integrates with the UK Carbon Intensity API to analyze electricity generation sources and identify the most environmentally friendly charging periods for electric vehicles.

The application provides energy mix statistics, clean energy calculations, and optimal EV charging window recommendations based on forecasted generation data.

## Features

- Integration with Carbon Intensity API
- Daily energy mix analysis
- Clean energy percentage calculation
- Optimal EV charging window calculation
- Global exception handling
- RESTful API endpoints
- Docker support
- Unit tests

## Tech Stack

- C#
- .NET 8
- ASP.NET Core Web API
- xUnit
- Moq
- Docker

## Architecture

The project follows a layered architecture and includes:

- Controllers
- Services
- HTTP Client abstraction
- DTOs
- Custom Exceptions
- Global Exception Handler
- Dependency Injection

## API Endpoints

### Get Energy Mix

GET /api/carbonintensity/energy-mix

Returns daily energy mix statistics and clean energy percentages.

### Find Optimal Charging Window

POST /api/carbonintensity/charging-window

Request:

json
{
"hours": 4
}

Returns the optimal charging window with the highest average clean energy percentage.
