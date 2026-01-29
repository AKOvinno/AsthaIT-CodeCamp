using System;
public class Ticket
{
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int ScheduleId { get; set; }
    public string SeatNumber { get; set; }
    public DateTime BookingDateTime { get; set; }
    public string Status { get; set; }
    public DateTime ReservationExpiryTime { get; set; }

    public Ticket(int ticketId, int userId, int scheduleId, string seatNumber, DateTime reservationExpiryTime)
    {
        TicketId = ticketId;
        UserId = userId;
        ScheduleId = scheduleId;
        SeatNumber = seatNumber;
        BookingDateTime = DateTime.Now;
        Status = "Pending";
        ReservationExpiryTime = reservationExpiryTime;
    }

    public override string ToString()
    {
        return "Ticket ID: " + TicketId + "\nUser ID: " + UserId + "\nSchedule ID: " + ScheduleId + "\nSeat: " + SeatNumber + "\nStatus: " + Status + "\nBooked: " + BookingDateTime;
    }
}
