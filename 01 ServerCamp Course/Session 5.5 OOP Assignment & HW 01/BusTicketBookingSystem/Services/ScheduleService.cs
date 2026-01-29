using System;
using System.Collections.Generic;
using System.Linq;
public interface IScheduleService
{
    void CreateSchedule(int busId, string departureCity, string arrivalCity, DateTime departureDateTime, decimal ticketPrice);
    List<Schedule> GetAllSchedules();
    Schedule GetScheduleById(int id);
    void DisplayScheduleDetails(int scheduleId);
}
public class ScheduleService : IScheduleService
{
    private readonly List<Schedule> _schedules;
    private readonly BusService _busService;
    public ScheduleService(BusService busService)
    {
        _schedules = new List<Schedule>();
        _busService = busService;
    }
    public void CreateSchedule(int busId, string departureCity, string arrivalCity, DateTime departureDateTime, decimal ticketPrice)
    {
        // Validation
        Bus bus = _busService.GetBusById(busId);
        if (bus == null)
            throw new ArgumentException($"Bus with ID {busId} not found");

        if (ValidationHelper.IsNullOrEmpty(departureCity))
            throw new ArgumentException("Departure city cannot be empty");

        if (ValidationHelper.IsNullOrEmpty(arrivalCity))
            throw new ArgumentException("Arrival city cannot be empty");

        if (!ValidationHelper.IsPositiveNumber(ticketPrice))
            throw new ArgumentException("Ticket price must be positive");

        if (departureDateTime <= DateTime.Now)
            throw new ArgumentException("Departure date must be in the future");

        // Create schedule
        int scheduleId = IdGenerator.GetNextScheduleId();
        Schedule schedule = new Schedule(scheduleId, busId, departureCity, arrivalCity, departureDateTime, ticketPrice);
        _schedules.Add(schedule);

        Console.WriteLine("Schedule created successfully!");
        Console.WriteLine($"Schedule ID: {schedule.ScheduleId}");
    }

    public List<Schedule> GetAllSchedules()
    {
        return _schedules.ToList();
    }

    public Schedule GetScheduleById(int id)
    {
        return _schedules.FirstOrDefault(s => s.ScheduleId == id);
    }

    public void DisplayScheduleDetails(int scheduleId)
    {
        Schedule schedule = _schedules.FirstOrDefault(s => s.ScheduleId == scheduleId);
        if (schedule == null)
        {
            Console.WriteLine($"Schedule with ID {scheduleId} not found.");
            return;
        }

        Bus bus = _busService.GetBusById(schedule.BusId);
        if (bus == null)
        {
            Console.WriteLine("Associated bus not found.");
            return;
        }

        Console.WriteLine("\n===== Schedule Details =====");
        Console.WriteLine($"Schedule ID: {schedule.ScheduleId}");
        Console.WriteLine($"Route: {schedule.DepartureCity} to {schedule.ArrivalCity}");
        Console.WriteLine($"Departure: {schedule.DepartureDateTime:yyyy-MM-dd HH:mm}");
        Console.WriteLine($"Price: {schedule.TicketPrice} BDT");
        Console.WriteLine($"\nBus Information:");
        Console.WriteLine($"Bus ID: {bus.BusId}");
        Console.WriteLine($"Coach Number: {bus.CoachNumber}");
        Console.WriteLine($"Type: {bus.BusType}");
        Console.WriteLine($"Total Seats: {bus.TotalSeats}");
        Console.WriteLine($"Available Seats: {bus.GetAvailableSeatsCount()}");
        
        // Display seat layout
        DisplaySeatLayout(bus);
        
        Console.WriteLine("=========================================\n");
    }
    private void DisplaySeatLayout(Bus bus)
    {
        Console.WriteLine($"\nSeat Layout (X = booked, [ ] = available):\n");
        
        List<string> bookedSeats = bus.GetBookedSeatsList();
        
        for (int row = 1; row <= bus.Rows; row++)
        {
            Console.Write($"Row {row}:  ");
            
            foreach (string col in bus.Columns)
            {
                string seatNumber = $"{row}{col}";
                if (bookedSeats.Contains(seatNumber))
                {
                    Console.Write($"[X:{seatNumber}]   ");
                }
                else
                {
                    Console.Write($"[ :{seatNumber}]   ");
                }
            }
            Console.WriteLine();
        }
    }
}

