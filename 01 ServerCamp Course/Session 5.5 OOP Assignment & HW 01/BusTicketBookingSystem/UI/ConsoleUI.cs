using System;
// Main UI class that manages user interactions via the console
// It utilizes InputHandler for input processing, ConsoleOutputHandler for output display, and UserFlowOrchestrator to manage the various user flows.
public class ConsoleUI
{
    private readonly InputHandler _inputHandler;
    private readonly ConsoleOutputHandler _outputHandler;
    private readonly UserFlowOrchestrator _flowOrchestrator;

    public ConsoleUI(IUserService userService, IBusService busService, IScheduleService scheduleService, IBookingService bookingService, IInvoiceService invoiceService)
    {
        _inputHandler = new InputHandler();
        _outputHandler = new ConsoleOutputHandler();
        _flowOrchestrator = new UserFlowOrchestrator(userService, busService, scheduleService, bookingService, invoiceService, _inputHandler, _outputHandler);
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            try
            {
                _outputHandler.PrintMenu();
                string input = Console.ReadLine();
    
                if (!int.TryParse(input, out int choice))
                {
                    _outputHandler.PrintInvalidInput("input");
                    continue;
                }

                running = HandleUserChoice(choice);
            }
            catch (Exception ex)
            {
                _outputHandler.PrintError(ex.Message);
            }
        }
    }

    private bool HandleUserChoice(int choice)
    {
        switch (choice)
        {
            case 0:
                _outputHandler.PrintExitMessage();
                return false;
            case 1:
                _flowOrchestrator.CreateUserFlow();
                break;
            case 2:
                _flowOrchestrator.ShowUsersFlow();
                break;
            case 3:
                _flowOrchestrator.CreateBusFlow();
                break;
            case 4:
                _flowOrchestrator.ShowBusesFlow();
                break;
            case 5:
                _flowOrchestrator.CreateScheduleFlow();
                break;
            case 6:
                _flowOrchestrator.ShowSchedulesFlow();
                break;
            case 7:
                _flowOrchestrator.ShowScheduleDetailsFlow();
                break;
            case 8:
                _flowOrchestrator.BookTicketFlow();
                break;
            case 9:
                _flowOrchestrator.ShowUserTicketsFlow();
                break;
            case 10:
                _flowOrchestrator.ShowUserInvoicesFlow();
                break;
            case 11:
                _flowOrchestrator.PayInvoiceFlow();
                break;
            default:
                _outputHandler.PrintInvalidChoice();
                break;
        }
        return true;
    }
}
