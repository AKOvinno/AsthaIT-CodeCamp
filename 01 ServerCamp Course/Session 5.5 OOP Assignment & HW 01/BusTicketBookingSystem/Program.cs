// Initialize Services
UserService userService = new UserService();
BusService busService = new BusService();
ScheduleService scheduleService = new ScheduleService(busService);
InvoiceService invoiceService = new InvoiceService();
BookingService bookingService = new BookingService(scheduleService, busService, userService, invoiceService);
// Connect invoice service to booking service for payment finalization
invoiceService.SetBookingService(bookingService);

// Initialize UI with services
ConsoleUI consoleUI = new ConsoleUI(userService, busService, scheduleService, bookingService, invoiceService);

// Start application
consoleUI.Run();
