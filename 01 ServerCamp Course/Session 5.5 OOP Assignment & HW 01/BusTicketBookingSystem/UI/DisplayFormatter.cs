using System;
using System.Collections.Generic;
// It formats and displays various entities in the bus ticket booking system.
public static class DisplayFormatter
{
    public static void PrintUsers(List<User> users)
    {
        if (users == null || users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        Console.WriteLine("\n================== All Users ==================");
        foreach (User user in users)
        {
            Console.WriteLine(user.ToString());
        }
        Console.WriteLine("================================================");
    }

    public static void PrintBuses(List<Bus> buses)
    {
        if (buses == null || buses.Count == 0)
        {
            Console.WriteLine("No buses found.");
            return;
        }

        Console.WriteLine("\n================== All Buses ==================");
        foreach (Bus bus in buses)
        {
            Console.WriteLine(bus.ToString());
        }
        Console.WriteLine("================================================");
    }

    public static void PrintSchedules(List<Schedule> schedules)
    {
        if (schedules == null || schedules.Count == 0)
        {
            Console.WriteLine("No schedules found.");
            return;
        }

        Console.WriteLine("\n=============== All Schedules ================");
        foreach (Schedule schedule in schedules)
        {
            Console.WriteLine(schedule.ToString());
        }
        Console.WriteLine("================================================");
    }

    public static void PrintTickets(List<Ticket> tickets)
    {
        if (tickets == null || tickets.Count == 0)
        {
            Console.WriteLine("No tickets found.");
            return;
        }

        Console.WriteLine("\n================== Tickets ===================");
        foreach (Ticket ticket in tickets)
        {
            Console.WriteLine(ticket.ToString());
        }
        Console.WriteLine("================================================");
    }

    public static void PrintInvoices(List<Invoice> invoices)
    {
        if (invoices == null || invoices.Count == 0)
        {
            Console.WriteLine("No invoices found.");
            return;
        }

        Console.WriteLine("\n================== Invoices ==================");
        foreach (Invoice invoice in invoices)
        {
            Console.WriteLine(invoice.ToString());
        }
        Console.WriteLine("================================================");
    }

    public static void PrintMenu()
    {
        Console.WriteLine("\n========== Bus Ticket Booking System ==========");
        Console.WriteLine("1.  Create User");
        Console.WriteLine("2.  Show Users");
        Console.WriteLine("3.  Create Bus");
        Console.WriteLine("4.  Show Buses");
        Console.WriteLine("5.  Create Schedule");
        Console.WriteLine("6.  Show Schedules");
        Console.WriteLine("7.  Show Schedule Details");
        Console.WriteLine("8.  Book Ticket");
        Console.WriteLine("9.  Show Tickets of a User");
        Console.WriteLine("10. Show Invoices of a User");
        Console.WriteLine("11. Pay Invoice");
        Console.WriteLine("0.  Exit");
        Console.WriteLine("================================================");
        Console.Write("Enter your choice: ");
    }
}
