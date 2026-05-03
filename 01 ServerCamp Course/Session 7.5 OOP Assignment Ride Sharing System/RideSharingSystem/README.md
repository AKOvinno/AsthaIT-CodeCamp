# Ride Sharing System - OOP Assignment 02

A console-based ride sharing system (like Uber/Pathao) demonstrating Object-Oriented Programming principles and design patterns in C#.

## Run

Build and run from the project root:

```bash
dotnet run
```

## Design patterns used & their locations

- Factory: `Vehicles/VehicleFactory.cs`
- Strategy: `Pricing/IPricingStrategy.cs`, `Pricing/StandardPricing.cs`, `Pricing/RushHourPricing.cs`, `Pricing/MidnightPricing.cs`
- Singleton: `Management/RideManager.cs`
- Observer: `Observers/IRideObserver.cs`, `Observers/RiderNotifier.cs`, `Observers/DriverNotifier.cs`
- Adapter: `Payments/BkashPaymentAdapter.cs`, `Payments/BkashPaymentGateway.cs`
- Inheritance/Polymorphism: `Users/User.cs`, `Users/Rider.cs`, `Users/Driver.cs`
