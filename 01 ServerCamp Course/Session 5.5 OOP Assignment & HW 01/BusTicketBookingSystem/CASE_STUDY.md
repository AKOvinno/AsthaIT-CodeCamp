# Bus Ticket Booking System - Comprehensive Case Study

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Problem Statement](#problem-statement)
3. [Solution Architecture](#solution-architecture)
4. [System Requirements](#system-requirements)
5. [Design Patterns](#design-patterns)
6. [Data Model & Entities](#data-model--entities)
7. [System Components](#system-components)
8. [Feature Specification](#feature-specification)
9. [Core Workflows](#core-workflows)
10. [Code Organization](#code-organization)
11. [OOP Principles Applied](#oop-principles-applied)
12. [Database Schema](#database-schema)
13. [System Interactions](#system-interactions)
14. [Error Handling & Validation](#error-handling--validation)
15. [Key Features & Innovations](#key-features--innovations)
16. [Usage Guide](#usage-guide)

---

## Executive Summary

The **Bus Ticket Booking System** is a comprehensive console-based application developed in **C# (.NET 10.0)** that facilitates the complete lifecycle of bus ticket reservations, from user registration to payment processing. The system demonstrates advanced Object-Oriented Programming (OOP) principles, design patterns, and solid software engineering practices.

### Key Highlights

- **Two Bus Categories**: Business and Economy with differentiated seating configurations
- **Dynamic Seat Management**: Real-time seat availability tracking with reservation expiration
- **Invoice & Payment Processing**: Complete billing workflow with payment status tracking
- **User Registration**: Comprehensive user management system
- **Schedule Management**: Route planning with departure and pricing information
- **Robust Validation**: Input validation and business logic enforcement
- **Service-Oriented Architecture**: Clear separation of concerns through service layer pattern

---

## Problem Statement

### Business Context

The transportation industry requires an efficient system to manage bus operations, ticket bookings, and revenue collection. Traditional manual booking systems are prone to errors, double-bookings, and lack real-time availability tracking.

### Challenges Addressed

1. **Seat Double-Booking**: Preventing multiple users from booking the same seat
2. **Reservation Management**: Handling temporary seat reservations with expiration
3. **Multi-Bus Support**: Managing different bus types with varying seating configurations
4. **Payment Workflow**: Coordinating between booking and payment finalization
5. **User Tracking**: Maintaining comprehensive user and booking history

---

## Solution Architecture

### Architectural Layers

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│  (ConsoleUI, InputHandler, OutputHandler)
├─────────────────────────────────────────┤
│         Orchestration Layer             │
│      (UserFlowOrchestrator)             │
├─────────────────────────────────────────┤
│         Service Layer                   │
│  (UserService, BusService, BookingService,
│   ScheduleService, InvoiceService)     │
├─────────────────────────────────────────┤
│         Entity Layer                    │
│  (User, Bus, Ticket, Schedule, Invoice) │
├─────────────────────────────────────────┤
│         Utility Layer                   │
│  (IdGenerator, ValidationHelper)        │
└─────────────────────────────────────────┘
```

### High-Level Design

```
User Interface (Console)
        ↓
InputHandler → UserFlowOrchestrator → OutputHandler
        ↓
┌───────────────────────────────────┐
│  Service Layer (Business Logic)   │
├───────────────────────────────────┤
│ • UserService                     │
│ • BusService                      │
│ • BookingService                  │
│ • ScheduleService                 │
│ • InvoiceService                  │
└───────────────────────────────────┘
        ↓
┌───────────────────────────────────┐
│  Data Models (In-Memory Storage)  │
├───────────────────────────────────┤
│ • Users (List)                    │
│ • Buses (List)                    │
│ • Schedules (List)                │
│ • Tickets (List)                  │
│ • Invoices (List)                 │
└───────────────────────────────────┘
```

---

## System Requirements

### Functional Requirements

- **FR1**: Users must be able to register in the system with name, email, and mobile number
- **FR2**: Admin must be able to create buses with different types (Business/Economy)
- **FR3**: System must support multiple schedules per bus with departure details and pricing
- **FR4**: Users must be able to book available seats with real-time availability checking
- **FR5**: System must reserve seats temporarily for 5 minutes during booking process
- **FR6**: System must generate invoices for booked tickets
- **FR7**: Users must be able to pay invoices to confirm bookings
- **FR8**: System must track booking history for each user
- **FR9**: System must display seat maps for all buses
- **FR10**: System must auto-cancel expired reservations

### Non-Functional Requirements

- **NFR1**: System must respond to user requests within 1 second
- **NFR2**: System must maintain data consistency during concurrent operations
- **NFR3**: System must validate all user inputs before processing
- **NFR4**: System must provide clear error messages
- **NFR5**: System must be extensible for future enhancements

---

## Design Patterns

### 1. **Factory Method Pattern** (BusFactory.cs)

```csharp
public static class BusFactory
{
    public static Bus CreateBus(int busId, string coachNumber, string busType)
    {
        switch (busType.ToLower())
        {
            case "business":
                return new BusinessBus(busId, coachNumber);
            case "economy":
                return new EconomyBus(busId, coachNumber);
            default:
                throw new ArgumentException($"Invalid bus type: {busType}");
        }
    }
}
```

**Purpose**: Encapsulates bus object creation logic, allowing dynamic instantiation of different bus types.

### 2. **Template Method Pattern** (Bus.cs)

The abstract `Bus` class defines the structure for bus operations, with `GetAllSeatNumbers()` implemented differently in subclasses.

### 3. **Strategy Pattern** (IBookingService, IUserService, IBusService)

Interface-based design allows different implementations of booking, user, and bus management strategies.

### 4. **Service Locator Pattern**

The `UserFlowOrchestrator` acts as a coordinator, managing interactions between different services.

### 5. **Singleton-like Pattern** (IdGenerator)

The `IdGenerator` class uses static members to maintain unique ID counters across the application.

---

## Data Model & Entities

### Entity Relationship Diagram

```
┌──────────────┐       ┌─────────────┐       ┌──────────────┐
│    User      │       │   Schedule  │       │     Bus      │
├──────────────┤       ├─────────────┤       ├──────────────┤
│ UserId (PK)  │       │ScheduleId   │       │ BusId (PK)   │
│ Name         │       │ BusId (FK)  │───┬───│ CoachNumber  │
│ Email        │       │ Department  │   │   │ BusType      │
│ MobileNumber │       │ Arrival     │   │   │ TotalSeats   │
└──────────────┘       │ DateTime    │   │   │ Rows         │
       ▲               │ Price       │   │   │ Columns      │
       │               └─────────────┘   │   └──────────────┘
       │                                 │
       │  1:N                           N:1
       │                                 │
     ┌─────────────────┐         ┌─────────────────┐
     │     Ticket      │         │   Invoice       │
     ├─────────────────┤         ├─────────────────┤
     │ TicketId (PK)   │         │ InvoiceId (PK)  │
     │ UserId (FK)     │─────────│ TicketId (FK)   │
     │ ScheduleId (FK) │    1:N  │ UserId (FK)     │
     │ SeatNumber      │         │ Amount          │
     │ BookingDateTime │         │ InvoiceDate     │
     │ Status          │         │ PaymentStatus   │
     │ ExpiryTime      │         └─────────────────┘
     └─────────────────┘
```

### Core Entities

#### 1. **User**

```csharp
public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string MobileNumber { get; set; }
    public string Email { get; set; }
}
```

**Purpose**: Represents a passenger or admin in the system
**Attributes**: Unique ID, personal information

#### 2. **Bus** (Abstract)

```csharp
public abstract class Bus
{
    public int BusId { get; set; }
    public string CoachNumber { get; set; }
    public string BusType { get; protected set; }
    public abstract int TotalSeats { get; }
    public abstract int Rows { get; }
    public abstract string[] Columns { get; }
}
```

**Purpose**: Base class for all bus types with common seat management
**Key Methods**:

- `GetAllSeatNumbers()`: Generates all valid seat identifiers
- `IsSeatAvailable()`: Checks if seat is bookable
- `BookSeat()`: Permanently books a seat
- `ReserveSeat()`: Temporarily reserves for 5 minutes
- `UnReserveSeat()`: Releases reservation on payment

#### 3. **BusinessBus**

```csharp
public class BusinessBus : Bus
{
    public override int TotalSeats => 28;
    public override int Rows => 9;
    public override string[] Columns => new[] { "A", "B", "C" };
}
```

**Specifications**:

- Premium seating arrangement
- 28 passenger seats (9 rows × 3 columns) + 1 driver seat
- Seating: 1A-9A, 1B-9B, 1C-9C, 0A (driver)

#### 4. **EconomyBus**

```csharp
public class EconomyBus : Bus
{
    public override int TotalSeats => 37;
    public override int Rows => 9;
    public override string[] Columns => new[] { "A", "B", "C", "D" };
}
```

**Specifications**:

- Budget-friendly option
- 37 passenger seats (9 rows × 4 columns) + 1 driver seat
- Seating: 1A-9A, 1B-9B, 1C-9C, 1D-9D, 0A (driver)

#### 5. **Schedule**

```csharp
public class Schedule
{
    public int ScheduleId { get; set; }
    public int BusId { get; set; }
    public string DepartureCity { get; set; }
    public string ArrivalCity { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public decimal TicketPrice { get; set; }
}
```

**Purpose**: Represents a bus route with timing and pricing

#### 6. **Ticket**

```csharp
public class Ticket
{
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int ScheduleId { get; set; }
    public string SeatNumber { get; set; }
    public DateTime BookingDateTime { get; set; }
    public string Status { get; set; }
    public DateTime ReservationExpiryTime { get; set; }
}
```

**Status Values**:

- `"Pending"`: Awaiting payment
- `"Confirmed"`: Payment received, seat booked
- `"Cancelled"`: Booking cancelled or expired

#### 7. **Invoice**

```csharp
public class Invoice
{
    public int InvoiceId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string PaymentStatus { get; set; }
}
```

**Payment Statuses**:

- `"Unpaid"`: Awaiting payment
- `"Paid"`: Payment processed, ticket confirmed

---

## System Components

### 1. **Service Layer** (Services/)

#### UserService (IUserService)

```csharp
public interface IUserService
{
    void CreateUser(string name, string mobileNumber, string email);
    List<User> GetAllUsers();
    User GetUserById(int id);
}
```

**Responsibilities**:

- User registration and validation
- User retrieval operations
- Maintains user list in memory

**Key Methods**:

- `CreateUser()`: Registers new user with validation
- `GetUserById()`: Retrieves user by ID
- `GetAllUsers()`: Returns copy of user list (encapsulation)

#### BusService (IBusService)

```csharp
public interface IBusService
{
    void CreateBus(string coachNumber, string busType);
    List<Bus> GetAllBuses();
    Bus GetBusById(int id);
}
```

**Responsibilities**:

- Bus creation using Factory pattern
- Bus inventory management
- Type validation

#### ScheduleService (IScheduleService)

**Responsibilities**:

- Schedule creation and management
- Route/schedule retrieval
- Schedule validation against buses

#### BookingService (IBookingService)

```csharp
public interface IBookingService
{
    bool BookTicket(int userId, int scheduleId, string seatNumber);
    bool FinalizeBookingAfterPayment(int ticketId);
    List<Ticket> GetUserTickets(int userId);
}
```

**Responsibilities**:

- Seat reservation and booking
- Ticket generation
- Booking workflow orchestration
- Integration with payment process

**Complex Logic**:

- Validates seat numbers based on bus type using regex patterns
- Reserves seats for 5 minutes during booking
- Finalizes bookings after payment confirmation
- Handles reservation expiration

#### InvoiceService (IInvoiceService)

```csharp
public interface IInvoiceService
{
    Invoice GenerateInvoice(int ticketId, int userId, decimal amount);
    List<Invoice> GetUserInvoices(int userId);
    bool PayInvoice(int invoiceId);
}
```

**Responsibilities**:

- Invoice generation
- Payment processing
- Expired invoice cleanup
- Callback to BookingService for finalization

### 2. **Presentation Layer** (UI/)

#### ConsoleUI

- Main entry point for user interaction
- Menu-driven interface
- Error handling and display
- Routes user choices to appropriate flows

#### ConsoleOutputHandler

- Formatted console output
- Menu display
- Error and success messages
- Seat map visualization

#### InputHandler

- Parses and validates user input
- Converts strings to appropriate types
- Handles input parsing errors

#### UserFlowOrchestrator

- Coordinates complex user workflows
- Manages multi-step processes (e.g., booking → invoice → payment)
- Communicates with services and UI handlers

### 3. **Utility Layer** (Utilities/)

#### IdGenerator

```csharp
public static class IdGenerator
{
    private static int _userIdCounter = 0;
    private static int _busIdCounter = 0;
    private static int _scheduleIdCounter = 0;
    private static int _ticketIdCounter = 0;
    private static int _invoiceIdCounter = 0;
}
```

**Purpose**: Centralized ID generation ensuring uniqueness

#### ValidationHelper

**Purpose**: Common validation functions for input sanitization

---

## Feature Specification

### Feature 1: User Management

**Description**: Create and manage user accounts

```
Input: Name, Mobile, Email
Processing: Validate inputs, generate unique ID
Output: User ID, confirmation message
```

### Feature 2: Bus Management

**Description**: Create buses of different types

```
Input: Coach Number, Bus Type (Business/Economy)
Processing: Validate type, use factory to create bus, assign ID
Output: Bus details with seat configuration
```

### Feature 3: Schedule Management

**Description**: Define bus routes with timing and pricing

```
Input: Bus ID, Departure City, Arrival City, DateTime, Price
Processing: Validate bus existence, validate date/time
Output: Schedule ID, route details
```

### Feature 4: Seat Availability Display

**Description**: Show real-time seat status for a schedule

```
Input: Bus ID
Processing: Query bus, get all seats, check availability
Output: Visual seat map with color coding
```

### Feature 5: Ticket Booking

**Description**: Reserve a seat on a schedule

```
Input: User ID, Schedule ID, Seat Number
Processing:
  1. Validate user exists
  2. Validate schedule exists
  3. Validate seat format for bus type
  4. Check seat availability
  5. Reserve seat for 5 minutes
  6. Create pending ticket
  7. Generate invoice
Output: Ticket ID, invoice ID, reservation time limit
```

### Feature 6: Payment Processing

**Description**: Process payment to confirm booking

```
Input: Invoice ID
Processing:
  1. Validate invoice exists
  2. Check payment status
  3. Mark as paid
  4. Finalize booking (convert reservation to permanent booking)
  5. Update ticket status
Output: Confirmation, ticket details
```

### Feature 7: User Booking History

**Description**: Retrieve all bookings for a user

```
Input: User ID
Processing: Query tickets list, filter by user
Output: List of tickets with status and details
```

### Feature 8: User Invoice History

**Description**: Retrieve all invoices for a user

```
Input: User ID
Processing: Query invoices, filter by user, cleanup expired
Output: List of invoices with payment status
```

---

## Core Workflows

### Workflow 1: User Registration Flow

```
Start
  ↓
Display User Creation Menu
  ↓
Input: Name, Mobile, Email
  ↓
ValidationHelper validates inputs
  ↓
UserService.CreateUser()
  ↓
IdGenerator.GetNextUserId()
  ↓
Create User object
  ↓
Add to user list
  ↓
Display success with User ID
  ↓
End
```

### Workflow 2: Ticket Booking Flow

```
Start
  ↓
Input: User ID, Schedule ID, Seat Number
  ↓
BookingService.BookTicket()
  ├─ Validate User exists
  ├─ Validate Schedule exists
  ├─ Validate Bus exists
  ├─ Validate Seat format (regex pattern)
  ├─ Check seat validity
  └─ Check seat availability
  ↓
Bus.ReserveSeat() [5-minute timer]
  ↓
Create Ticket (Status = "Pending")
  ↓
InvoiceService.GenerateInvoice()
  ├─ Create Invoice (PaymentStatus = "Unpaid")
  └─ Add to invoices list
  ↓
Display: Ticket ID, Invoice ID, Expiry Time
  ↓
End (Awaiting Payment)
```

### Workflow 3: Payment & Booking Finalization Flow

```
Start
  ↓
Input: Invoice ID
  ↓
InvoiceService.PayInvoice()
  ├─ Validate invoice exists
  ├─ Validate not already paid
  └─ Mark as "Paid"
  ↓
InvoiceService calls BookingService.FinalizeBookingAfterPayment()
  ├─ Get ticket using ticket ID
  ├─ Get bus using schedule
  ├─ Bus.UnReserveSeat() [removes 5-min timer]
  ├─ Bus.BookSeat() [permanent booking]
  └─ Update ticket status = "Confirmed"
  ↓
Display: Payment confirmation, Ticket confirmed
  ↓
End
```

### Workflow 4: Expired Reservation Handling

```
Event: InvoiceService.GetUserInvoices() called
  ↓
CleanupExpiredInvoices()
  ├─ For each unpaid invoice:
  │   ├─ Check if ticket still exists
  │   ├─ If not found (expired):
  │   │   ├─ Remove invoice from list
  │   │   └─ Display auto-cancel message
  │   └─ Continue
  └─ Return cleaned invoices list
```

---

## Code Organization

### Directory Structure

```
BusTicketBookingSystem/
├── Program.cs                          # Application entry point
├── Entities/                           # Data models
│   ├── User.cs                        # User entity
│   ├── Bus.cs                         # Abstract bus base
│   ├── BusinessBus.cs                 # Business tier bus
│   ├── EconomyBus.cs                  # Economy tier bus
│   ├── Schedule.cs                    # Schedule entity
│   ├── Ticket.cs                      # Ticket entity
│   └── Invoice.cs                     # Invoice entity
├── Services/                          # Business logic
│   ├── UserService.cs                 # User management
│   ├── BusService.cs                  # Bus management
│   ├── ScheduleService.cs             # Schedule management
│   ├── BookingService.cs              # Booking logic
│   └── InvoiceService.cs              # Invoice & payment
├── Factories/                         # Object creation
│   └── BusFactory.cs                  # Bus factory method
├── UI/                                # Presentation layer
│   ├── ConsoleUI.cs                   # Main UI loop
│   ├── InputHandler.cs                # Input processing
│   ├── ConsoleOutputHandler.cs        # Output formatting
│   ├── DisplayFormatter.cs            # Display utilities
│   └── UserFlowOrchestrator.cs        # Workflow coordination
├── Utilities/                         # Helper functions
│   ├── IdGenerator.cs                 # ID generation
│   └── ValidationHelper.cs            # Input validation
└── BusTicketBookingSystem.csproj      # Project file
```

---

## OOP Principles Applied

### 1. **Encapsulation**

- Private fields with public properties
- Methods encapsulate complex logic
- Seat management hidden within Bus class
- `GetAllUsers()` returns list copy to prevent external modification

```csharp
public class User
{
    // Encapsulated fields
    public int UserId { get; set; }
    public string Name { get; set; }
    // Logic encapsulated in ToString()
    public override string ToString() { ... }
}
```

### 2. **Inheritance**

- `Bus` abstract class inherited by `BusinessBus` and `EconomyBus`
- Interface implementation across all services
- Polymorphic behavior through abstract methods

```csharp
public abstract class Bus { ... }
public class BusinessBus : Bus { ... }
public class EconomyBus : Bus { ... }
```

### 3. **Polymorphism**

- Abstract methods overridden in subclasses
- Interface-based service implementations
- Different seat configurations based on bus type

```csharp
public abstract int Rows { get; }
public abstract string[] Columns { get; }
public abstract List<string> GetAllSeatNumbers();
```

### 4. **Abstraction**

- Interface contracts (IUserService, IBusService, etc.)
- Abstract Bus class hides implementation details
- Service layer abstracts business logic from UI

```csharp
public interface IUserService
{
    void CreateUser(string name, string mobileNumber, string email);
    List<User> GetAllUsers();
    User GetUserById(int id);
}
```

### 5. **SOLID Principles**

**Single Responsibility Principle (SRP)**

- Each service has one reason to change
- UserService only manages users
- BookingService only manages bookings

**Open/Closed Principle (OCP)**

- Bus classes can be extended (BusinessBus, EconomyBus)
- Services can be extended with new functionality
- Factory pattern allows new bus types without modifying factory

**Liskov Substitution Principle (LSP)**

- BusinessBus and EconomyBus can replace Bus
- Service implementations interchangeable

**Interface Segregation Principle (ISP)**

- Focused interfaces (IUserService, IBusService)
- Services implement only relevant interfaces
- No forced unnecessary implementation

**Dependency Inversion Principle (DIP)**

- Services depend on interfaces, not concrete classes
- UserFlowOrchestrator depends on service interfaces
- Loose coupling between components

---

## Database Schema

### In-Memory Data Structures

The system uses in-memory collections to simulate database storage:

#### Users Table

```
| UserId | Name          | MobileNumber   | Email              |
|--------|---------------|----------------|--------------------|
| 1      | John Doe      | +88017xxxxxxxx | john@example.com   |
| 2      | Jane Smith    | +88018xxxxxxxx | jane@example.com   |
```

#### Buses Table

```
| BusId | CoachNumber | BusType  | TotalSeats | Rows | Columns |
|-------|-------------|----------|------------|------|---------|
| 1     | DX-5001     | Business | 28         | 9    | A,B,C   |
| 2     | DX-5002     | Economy  | 37         | 9    | A,B,C,D |
```

#### Schedules Table

```
| ScheduleId | BusId | DepartureCity | ArrivalCity | DepartureDateTime | TicketPrice |
|------------|-------|----------------|-------------|-------------------|-------------|
| 1          | 1     | Dhaka          | Chittagong  | 2026-02-05 08:00  | 550.00      |
| 2          | 2     | Dhaka          | Sylhet     | 2026-02-05 10:00  | 450.00      |
```

#### Tickets Table

```
| TicketId | UserId | ScheduleId | SeatNumber | BookingDateTime | Status    | ExpiryTime |
|----------|--------|------------|------------|-----------------|-----------|------------|
| 1        | 1      | 1          | 1A         | 2026-02-02 ...  | Confirmed | N/A        |
| 2        | 2      | 2          | 3B         | 2026-02-02 ...  | Pending   | 2026-02-02 |
```

#### Invoices Table

```
| InvoiceId | TicketId | UserId | Amount | InvoiceDate | PaymentStatus |
|-----------|----------|--------|--------|-------------|---------------|
| 1         | 1        | 1      | 550.00 | 2026-02-02  | Paid          |
| 2         | 2        | 2      | 450.00 | 2026-02-02  | Unpaid        |
```

#### Seat Status (Per Bus Instance)

```
Bus 1 (BusinessBus):
┌─────────────────────┐
│ Booked Seats: {1A}  │
│ Reserved: {2A,3A}   │ (expiry: 14:35)
│ Available: {1B-9B...}│
└─────────────────────┘
```

---

## System Interactions

### Component Interaction Diagram

```
┌─────────────┐
│  ConsoleUI  │
└──────┬──────┘
       │
       ▼
┌────────────────────────┐
│ UserFlowOrchestrator   │ ◄─── Coordinates all flows
└──────┬──────────────────┘
       │
       ├──────────────┬──────────────┬──────────────┬──────────────┐
       ▼              ▼              ▼              ▼              ▼
┌────────────┐ ┌─────────┐ ┌──────────────┐ ┌──────────┐ ┌───────────┐
│UserService │ │BusService│ │ScheduleService│ │BookingService│ │InvoiceService│
└──────┬─────┘ └────┬────┘ └────┬───────────┘ └────┬────────┘ └─────┬─────┘
       │            │           │                  │                 │
       ▼            ▼           ▼                  ▼                 ▼
    [Users]    [Buses]     [Schedules]         [Tickets]         [Invoices]
                   │
                   ▼
            ┌────────────────┐
            │  BusFactory    │
            └────────────────┘
                   │
          ┌────────┴────────┐
          ▼                 ▼
    ┌─────────┐      ┌───────────┐
    │Business │      │  Economy  │
    │  Bus    │      │   Bus     │
    └─────────┘      └───────────┘
           │                │
           └────────┬───────┘
                    ▼
          [Internal Seat Storage]
    ┌──────────────────────────┐
    │ _bookedSeats (HashSet)   │
    │ _reservedSeats (Dict)    │
    └──────────────────────────┘
```

### Key Interactions

**1. Creating a Ticket:**

```
UserFlowOrchestrator
  → BookingService.BookTicket()
    → UserService.GetUserById()
    → ScheduleService.GetScheduleById()
    → BusService.GetBusById()
    → Bus.IsValidSeat()
    → Bus.IsSeatAvailable()
    → Bus.ReserveSeat() [5-min timer]
    → InvoiceService.GenerateInvoice()
```

**2. Processing Payment:**

```
UserFlowOrchestrator
  → InvoiceService.PayInvoice()
    → BookingService.FinalizeBookingAfterPayment()
      → Bus.UnReserveSeat()
      → Bus.BookSeat()
      → Ticket.Status = "Confirmed"
```

**3. Displaying Seat Map:**

```
UserFlowOrchestrator
  → BusService.GetBusById()
    → Bus.GetAllSeatNumbers()
    → Bus.GetAvailableSeatsCount()
    → Bus.GetBookedSeatsList()
    → ConsoleOutputHandler.DisplaySeatMap()
```

---

## Error Handling & Validation

### Input Validation

#### User Validation

```csharp
public void CreateUser(string name, string mobileNumber, string email)
{
    if (ValidationHelper.IsNullOrEmpty(name))
        throw new ArgumentException("Name cannot be empty");
    if (ValidationHelper.IsNullOrEmpty(mobileNumber))
        throw new ArgumentException("Mobile number cannot be empty");
    if (ValidationHelper.IsNullOrEmpty(email))
        throw new ArgumentException("Email cannot be empty");
}
```

#### Seat Number Validation

```csharp
private bool IsValidSeatNumber(Bus bus, string seatNumber)
{
    if (bus.BusType == "Business")
    {
        // Business: 1A-9A, 1B-9B, 1C-9C, 0A
        return Regex.IsMatch(seatNumber, "^[1-9][0-9]?[A-C]$");
    }
    else if (bus.BusType == "Economy")
    {
        // Economy: 1A-9A, 1B-9B, 1C-9C, 1D-9D, 0A
        return Regex.IsMatch(seatNumber, "^[1-9][0-9]?[A-D]$");
    }
    return false;
}
```

### Error Handling Strategies

#### Try-Catch in UI

```csharp
try
{
    running = HandleUserChoice(choice);
}
catch (Exception ex)
{
    _outputHandler.PrintError(ex.Message);
}
```

#### Null Checking

```csharp
User user = _userService.GetUserById(userId);
if (user == null)
{
    Console.WriteLine($"Error: User with ID {userId} not found.");
    return false;
}
```

#### Status Validation

```csharp
if (invoice.PaymentStatus == "Paid")
{
    Console.WriteLine($"Invoice {invoiceId} is already paid.");
    return false;
}
```

### Data Consistency

#### Reservation Expiration

```csharp
private bool IsReservationExpired(string seatNumber)
{
    if (!_reservedSeats.ContainsKey(seatNumber))
        return true;

    if (DateTime.Now > _reservedSeats[seatNumber])
    {
        _reservedSeats.Remove(seatNumber);
        return true;
    }
    return false;
}
```

#### Orphaned Invoice Cleanup

```csharp
public void CleanupExpiredInvoices()
{
    List<Invoice> orphanedInvoices = _invoices
        .Where(i => i.PaymentStatus == "Unpaid" &&
                    _bookingService.GetUserTickets(i.UserId)
                        .FirstOrDefault(t => t.TicketId == i.TicketId) == null)
        .ToList();

    foreach (Invoice invoice in orphanedInvoices)
    {
        _invoices.Remove(invoice);
    }
}
```

---

## Key Features & Innovations

### 1. **Dual Bus Type Support**

- Different seating configurations
- Type-specific validation
- Flexible seat arrangements

### 2. **Intelligent Seat Reservation**

- 5-minute temporary reservation
- Automatic expiration handling
- Prevents double-booking during checkout

### 3. **Comprehensive Invoice Management**

- Automatic invoice generation
- Payment status tracking
- Expired invoice cleanup

### 4. **Type-Safe Seat Validation**

- Regex-based seat number format validation
- Bus-type-specific seat ranges
- Clear error messages for invalid seats

### 5. **Service-Oriented Architecture**

- Loose coupling between components
- Easy to extend with new features
- Testable service interfaces

### 6. **Encapsulated Seat Management**

- HashSet for O(1) booked seat lookup
- Dictionary for reservation time tracking
- Clean public API for seat operations

### 7. **Circular Dependency Resolution**

- InvoiceService has reference to BookingService
- BookingService doesn't know about InvoiceService
- SetBookingService() method handles initialization

---

## Usage Guide

### Running the Application

```bash
# Navigate to project directory
cd BusTicketBookingSystem

# Build the project
dotnet build

# Run the application
dotnet run
```

### Main Menu Options

```
=== Bus Ticket Booking System ===
1. Create User
2. Show All Users
3. Create Bus
4. Show All Buses
5. Create Schedule
6. Show All Schedules
7. Show Schedule Details & Seat Map
8. Book Ticket
9. Show User Tickets
10. Show User Invoices
11. Pay Invoice
0. Exit

Select an option:
```

### Sample Usage Workflow

#### Step 1: Create Users

```
Option: 1
Name: John Doe
Mobile: +88017xxxxxxxx
Email: john@example.com
→ User ID: 1 created
```

#### Step 2: Create Buses

```
Option: 3
Coach Number: DX-5001
Bus Type: Business
→ Bus ID: 1 created (28 seats)

Option: 3
Coach Number: DX-5002
Bus Type: Economy
→ Bus ID: 2 created (37 seats)
```

#### Step 3: Create Schedules

```
Option: 5
Bus ID: 1
Departure City: Dhaka
Arrival City: Chittagong
Departure Date/Time: 2026-02-05 08:00
Ticket Price: 550
→ Schedule ID: 1 created
```

#### Step 4: View Seat Map

```
Option: 7
Bus ID: 1
→ Display seat map with booked/available status
```

#### Step 5: Book Ticket

```
Option: 8
User ID: 1
Schedule ID: 1
Seat Number: 1A
→ Ticket ID: 1 created (Pending)
→ Invoice ID: 1 generated (Unpaid)
→ Reservation expires: 14:35
```

#### Step 6: View Invoice

```
Option: 10
User ID: 1
→ Invoice ID: 1, Amount: 550 BDT, Status: Unpaid
```

#### Step 7: Make Payment

```
Option: 11
Invoice ID: 1
→ Payment processed
→ Ticket status changed to: Confirmed
→ Seat 1A permanently booked
```

#### Step 8: View Booking History

```
Option: 9
User ID: 1
→ Ticket ID: 1, Seat: 1A, Status: Confirmed
```

---

## Challenges & Solutions

### Challenge 1: Double-Booking Prevention

**Problem**: Multiple users booking the same seat simultaneously
**Solution**:

- Used HashSet for booked seats (O(1) lookup)
- Temporary reservation mechanism with expiration
- Status validation before booking

### Challenge 2: Seat Format Validation

**Problem**: Different bus types have different seat formats
**Solution**:

- Bus-specific seat range implementation
- Regex-based validation in BookingService
- Type-specific column array in bus subclasses

### Challenge 3: Circular Dependencies

**Problem**: InvoiceService needs to finalize bookings, but BookingService creates invoices
**Solution**:

- Dependency injection through SetBookingService()
- Interface-based separation (IBookingService)
- Loose coupling through interfaces

### Challenge 4: Expired Reservation Handling

**Problem**: Users might abandon checkout, leaving reserved seats unavailable
**Solution**:

- 5-minute expiration timer in Bus class
- Automatic expiration checking in IsSeatAvailable()
- Manual cleanup in InvoiceService

### Challenge 5: Data Consistency

**Problem**: Invoices orphaned when tickets expire
**Solution**:

- CleanupExpiredInvoices() method
- Cross-reference checking between services
- Automatic removal of invalid records

---

## Extension Points & Future Enhancements

### 1. **Database Integration**

- Replace in-memory lists with Entity Framework
- Add SQL Server database backend
- Implement repository pattern

### 2. **Advanced Payment Processing**

- Integration with payment gateways (Stripe, Bkash)
- Multiple payment methods
- Transaction logging

### 3. **User Roles & Authentication**

- Admin vs. Customer roles
- Login/logout functionality
- Password management

### 4. **Advanced Scheduling**

- Recurring schedules
- Holiday routes
- Dynamic pricing

### 5. **Reporting & Analytics**

- Booking statistics
- Revenue reports
- Occupancy analysis

### 6. **Notifications**

- Email confirmation
- SMS alerts
- Payment reminders

### 7. **Seat Classification**

- Different pricing for different seat rows
- Window vs. middle seat pricing
- VIP seating options

### 8. **Cancellation Policy**

- Refund mechanisms
- Cancellation charges
- Refund processing

---

## Testing Scenarios

### Unit Testing Scenarios

#### Test 1: User Creation

```
Input: Valid name, mobile, email
Expected: User created with unique ID
```

#### Test 2: Bus Creation with Invalid Type

```
Input: Coach number, Bus type "Luxury"
Expected: ArgumentException thrown
```

#### Test 3: Seat Booking with Invalid Seat

```
Input: Valid user, schedule; Invalid seat "10E"
Expected: Booking rejected, error message
```

#### Test 4: Reservation Expiration

```
Input: Book seat, wait 5 minutes
Expected: Reservation marked expired, seat available
```

#### Test 5: Payment Processing

```
Input: Valid unpaid invoice
Expected: Invoice marked paid, ticket confirmed, seat permanently booked
```

---

## Performance Considerations

### Time Complexity

| Operation               | Complexity | Notes                          |
| ----------------------- | ---------- | ------------------------------ |
| Seat availability check | O(1)       | HashSet lookup                 |
| Book seat               | O(1)       | HashSet addition               |
| Get all seats           | O(n)       | n = rows × columns             |
| Get user bookings       | O(n)       | Linear search through tickets  |
| Cleanup invoices        | O(m×n)     | m = invoices, n = user tickets |

### Space Complexity

| Data Structure       | Complexity | Notes                          |
| -------------------- | ---------- | ------------------------------ |
| Users list           | O(n)       | n = number of users            |
| Buses list           | O(b)       | b = number of buses            |
| Booked seats per bus | O(s)       | s = booked seats ≤ total seats |
| Tickets list         | O(t)       | t = number of tickets          |
| Invoices list        | O(i)       | i = number of invoices         |

### Optimization Opportunities

1. Add indexing for UserId lookups
2. Use Dictionary for O(1) schedule lookups by ID
3. Cache seat availability checks
4. Lazy load user tickets instead of full list retrieval

---

## Conclusion

The **Bus Ticket Booking System** demonstrates a comprehensive implementation of Object-Oriented Programming principles, design patterns, and clean code practices. The system successfully addresses real-world challenges in the transportation industry through:

- **Robust Architecture**: Service-oriented design with clear separation of concerns
- **Smart Business Logic**: Intelligent reservation management with expiration handling
- **Data Integrity**: Validation, error handling, and consistency checks throughout
- **Extensibility**: Design patterns and interfaces enable easy enhancements
- **User Experience**: Intuitive console interface with clear feedback

The project serves as an excellent case study for:

- Implementing Factory Method Pattern
- Using Abstract classes and inheritance
- Interface-based service design
- Managing complex workflows
- Handling edge cases and data consistency
- SOLID principle application in practice

### Key Learnings

1. **Pattern Recognition**: Identifying when to apply specific design patterns
2. **Architecture Design**: Building layered, maintainable systems
3. **Business Logic**: Implementing complex workflows with multiple dependencies
4. **Data Management**: Handling in-memory storage with consistency guarantees
5. **Error Handling**: Comprehensive validation and exception management

This system can be extended to include database persistence, advanced authentication, payment gateway integration, and comprehensive reporting capabilities.
