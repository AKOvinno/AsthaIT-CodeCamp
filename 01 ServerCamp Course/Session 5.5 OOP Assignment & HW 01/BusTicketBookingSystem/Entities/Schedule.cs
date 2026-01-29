using System;
public class Schedule
{
    public int ScheduleId { get; set; }
    public int BusId { get; set; }
    public string DepartureCity { get; set; }
    public string ArrivalCity { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public decimal TicketPrice { get; set; }

    public Schedule(int scheduleId, int busId, string departureCity, string arrivalCity, DateTime departureDateTime, decimal ticketPrice)
    {
        ScheduleId = scheduleId;
        BusId = busId;
        DepartureCity = departureCity;
        ArrivalCity = arrivalCity;
        DepartureDateTime = departureDateTime;
        TicketPrice = ticketPrice;
    }

    public override string ToString()
    {
        return "Schedule ID: " + ScheduleId + "\nBus ID: " + BusId + " \nRoute: " + DepartureCity + " to " + ArrivalCity + "\nDeparture: " + DepartureDateTime + "\nPrice: " + TicketPrice + " BDT\n";
    }
}
