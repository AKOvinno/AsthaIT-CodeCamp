using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
public interface IBookingService
{
    bool BookTicket(int userId, int scheduleId, string seatNumber);
    bool FinalizeBookingAfterPayment(int ticketId);
    List<Ticket> GetUserTickets(int userId);
}
public class BookingService : IBookingService
{
    private bool IsValidSeatNumber(Bus bus, string seatNumber)
    {
        // Extract the bus type
        string busType = bus.BusType;

        // Validate seat number based on bus type
        if (busType == "Business")
        {
            // Business bus has seats 1A-10A, 1B-9B, 1C-9C
            return Regex.IsMatch(seatNumber, "^[1-9][0-9]?[A-C]$");
        }
        else if (busType == "Economy")
        {
            // Economy bus has seats 1A-10A, 1B-9B, 1C-9C, 1D-9D
            return Regex.IsMatch(seatNumber, "^[1-9][0-9]?[A-D]$");
        }

        return false; // Invalid bus type
    }

    private readonly List<Ticket> _tickets;
    private readonly ScheduleService _scheduleService;
    private readonly BusService _busService;
    private readonly UserService _userService;
    private readonly IInvoiceService _invoiceService;

    public BookingService(ScheduleService scheduleService, BusService busService, UserService userService, IInvoiceService invoiceService)
    {
        _tickets = new List<Ticket>();
        _scheduleService = scheduleService;
        _busService = busService;
        _userService = userService;
        _invoiceService = invoiceService;
    }

    public bool BookTicket(int userId, int scheduleId, string seatNumber)
    {
        // Validate user
        User user = _userService.GetUserById(userId);
        if (user == null)
        {
            Console.WriteLine($"Error: User with ID {userId} not found.");
            return false;
        }

        // Validate schedule
        Schedule schedule = _scheduleService.GetScheduleById(scheduleId);
        if (schedule == null)
        {
            Console.WriteLine($"Error: Schedule with ID {scheduleId} not found.");
            return false;
        }

        // Validate bus
        Bus bus = _busService.GetBusById(schedule.BusId);
        if (bus == null)
        {
            Console.WriteLine("Error: Associated bus not found.");
            return false;
        }

        // Validate seat number based on bus type
        if (!IsValidSeatNumber(bus, seatNumber))
        {
            Console.WriteLine($"Error: Seat {seatNumber} is not valid for bus type {bus.BusType}.");
            return false;
        }

        // Validate seat
        if (!bus.IsValidSeat(seatNumber))
        {
            Console.WriteLine($"Error: Invalid seat number '{seatNumber}'.");
            return false;
        }

        if (!bus.IsSeatAvailable(seatNumber))
        {
            Console.WriteLine($"Error: Seat {seatNumber} is already booked or reserved.");
            return false;
        }

        // Create ticket without booking the seat yet
        int ticketId = IdGenerator.GetNextTicketId();
        DateTime reservationExpiryTime = DateTime.Now.AddMinutes(5);
        Ticket ticket = new Ticket(ticketId, userId, scheduleId, seatNumber, reservationExpiryTime);
        _tickets.Add(ticket);

        // Reserve the seat for 5 minutes
        bus.ReserveSeat(seatNumber);

        // Generate invoice (seat will be booked after payment)
        _invoiceService.GenerateInvoice(ticketId, userId, schedule.TicketPrice);
        Console.WriteLine("Ticket created successfully!");
        Console.WriteLine($"Ticket ID: {ticketId}.");
        Console.WriteLine($"Seat: {seatNumber}");
        Console.WriteLine("Please proceed to Pay Invoice to finalize the booking.");
        return true;
    }
    public bool FinalizeBookingAfterPayment(int ticketId)
    {
        Ticket ticket = _tickets.FirstOrDefault(t => t.TicketId == ticketId);
        if (ticket == null)
        {
            Console.WriteLine($"Error: Ticket with ID {ticketId} not found.");
            return false;
        }

        // Get the bus for this ticket's schedule
        Schedule schedule = _scheduleService.GetScheduleById(ticket.ScheduleId);
        if (schedule == null)
        {
            Console.WriteLine("Error: Associated schedule not found.");
            return false;
        }

        Bus bus = _busService.GetBusById(schedule.BusId);
        if (bus == null)
        {
            Console.WriteLine("Error: Associated bus not found.");
            return false;
        }

        // Release the reservation and book the seat
        bus.UnReserveSeat(ticket.SeatNumber);
        
        // Book the seat
        if (bus.BookSeat(ticket.SeatNumber))
        {
            ticket.Status = "Confirmed";
            return true;
        }
        else
        {
            Console.WriteLine($"Error: Could not book seat {ticket.SeatNumber}.");
            return false;
        }
    }

    public List<Ticket> GetUserTickets(int userId)
    {
        CleanupExpiredTickets();
        return _tickets.Where(t => t.UserId == userId).ToList();
    }

    // Remove tickets with expired reservations (not yet paid)
    private void CleanupExpiredTickets()
    {
        List<Ticket> expiredTickets = _tickets.Where(t => DateTime.Now > t.ReservationExpiryTime && t.Status != "Confirmed").ToList();
        
        foreach (Ticket ticket in expiredTickets)
        {
            _tickets.Remove(ticket);
            Console.WriteLine($"Ticket {ticket.TicketId} expired and has been auto-cancelled.");
        }
    }
}
