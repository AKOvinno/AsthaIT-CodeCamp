using System;
using System.Collections.Generic;
// It orchestrates user flows in the bus ticket booking system.
public class UserFlowOrchestrator
{
    private readonly IUserService _userService;
    private readonly IBusService _busService;
    private readonly IScheduleService _scheduleService;
    private readonly IBookingService _bookingService;
    private readonly IInvoiceService _invoiceService;
    private readonly InputHandler _inputHandler;
    private readonly ConsoleOutputHandler _outputHandler;

    public UserFlowOrchestrator(IUserService userService, IBusService busService, IScheduleService scheduleService, IBookingService bookingService, IInvoiceService invoiceService, InputHandler inputHandler, ConsoleOutputHandler outputHandler)
    {
        _userService = userService;
        _busService = busService;
        _scheduleService = scheduleService;
        _bookingService = bookingService;
        _invoiceService = invoiceService;
        _inputHandler = inputHandler;
        _outputHandler = outputHandler;
    }

    public void CreateUserFlow()
    {
        _outputHandler.PrintSectionHeader("Create User");
        
        string name = _inputHandler.ReadString("Enter Name: ");
        string mobile = _inputHandler.ReadString("Enter Mobile Number: ");
        string email = _inputHandler.ReadString("Enter Email: ");

        _userService.CreateUser(name, mobile, email);
    }

    public void ShowUsersFlow()
    {
        List<User> users = _userService.GetAllUsers();
        DisplayFormatter.PrintUsers(users);
    }

    public void CreateBusFlow()
    {
        _outputHandler.PrintSectionHeader("Create Bus");
        
        string coachNumber = _inputHandler.ReadString("Enter Coach Number: ");
        string busType = _inputHandler.ReadString("Enter Bus Type (Business/Economy): ");

        _busService.CreateBus(coachNumber, busType);
    }

    public void ShowBusesFlow()
    {
        List<Bus> buses = _busService.GetAllBuses();
        DisplayFormatter.PrintBuses(buses);
    }

    public void CreateScheduleFlow()
    {
        _outputHandler.PrintSectionHeader("Create Schedule");
        
        if (!_inputHandler.TryParseInt("Enter Bus ID: ", out int busId))
        {
            _outputHandler.PrintInvalidInput("Bus ID");
            return;
        }

        string depCity = _inputHandler.ReadString("Enter Departure City: ");
        string arrCity = _inputHandler.ReadString("Enter Arrival City: ");

        if (!_inputHandler.TryParseDateTime("Enter Departure Date (yyyy-MM-dd): ", out DateTime depDate))
        {
            _outputHandler.PrintInvalidInput("date format");
            return;
        }

        if (!_inputHandler.TryParseTimeSpan("Enter Departure Time (HH:mm): ", out TimeSpan depTime))
        {
            _outputHandler.PrintInvalidInput("time format");
            return;
        }

        DateTime departureDateTime = depDate.Add(depTime);

        if (!_inputHandler.TryParseDecimal("Enter Ticket Price: ", out decimal price))
        {
            _outputHandler.PrintInvalidInput("price");
            return;
        }

        _scheduleService.CreateSchedule(busId, depCity, arrCity, departureDateTime, price);
    }

    public void ShowSchedulesFlow()
    {
        List<Schedule> schedules = _scheduleService.GetAllSchedules();
        DisplayFormatter.PrintSchedules(schedules);
    }

    public void ShowScheduleDetailsFlow()
    {
        _outputHandler.PrintSectionHeader("Show Schedule Details");
        
        if (!_inputHandler.TryParseInt("Enter Schedule ID: ", out int scheduleId))
        {
            _outputHandler.PrintInvalidInput("Schedule ID");
            return;
        }

        _scheduleService.DisplayScheduleDetails(scheduleId);
    }

    public void BookTicketFlow()
    {
        _outputHandler.PrintSectionHeader("Book Ticket");
        
        if (!_inputHandler.TryParseInt("Enter User ID: ", out int userId))
        {
            _outputHandler.PrintInvalidInput("User ID");
            return;
        }

        if (!_inputHandler.TryParseInt("Enter Schedule ID: ", out int scheduleId))
        {
            _outputHandler.PrintInvalidInput("Schedule ID");
            return;
        }

        string seatNumber = _inputHandler.ReadString("Enter Seat Number (e.g., 1A, 2B, 3C): ", toUpper: true);
        if (string.IsNullOrWhiteSpace(seatNumber))
        {
            _outputHandler.PrintInvalidInput("Seat Number");
            return;
        }

        _bookingService.BookTicket(userId, scheduleId, seatNumber);
    }

    public void ShowUserTicketsFlow()
    {
        _outputHandler.PrintSectionHeader("Show User Tickets");
        
        if (!_inputHandler.TryParseInt("Enter User ID: ", out int userId))
        {
            _outputHandler.PrintInvalidInput("User ID");
            return;
        }

        List<Ticket> tickets = _bookingService.GetUserTickets(userId);
        DisplayFormatter.PrintTickets(tickets);
    }

    public void ShowUserInvoicesFlow()
    {
        _outputHandler.PrintSectionHeader("Show User Invoices");
        
        if (!_inputHandler.TryParseInt("Enter User ID: ", out int userId))
        {
            _outputHandler.PrintInvalidInput("User ID");
            return;
        }

        List<Invoice> invoices = _invoiceService.GetUserInvoices(userId);
        DisplayFormatter.PrintInvoices(invoices);
    }

    public void PayInvoiceFlow()
    {
        _outputHandler.PrintSectionHeader("Pay Invoice");
        
        if (!_inputHandler.TryParseInt("Enter Invoice ID: ", out int invoiceId))
        {
            _outputHandler.PrintInvalidInput("Invoice ID");
            return;
        }

        _invoiceService.PayInvoice(invoiceId);
    }
}
