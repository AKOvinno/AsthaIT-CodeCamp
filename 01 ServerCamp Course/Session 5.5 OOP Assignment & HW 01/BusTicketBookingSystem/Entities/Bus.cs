using System;
using System.Collections.Generic;
using System.Linq;
public abstract class Bus
{
    public int BusId { get; set; }
    public string CoachNumber { get; set; }
    public string BusType { get; protected set; }
    public abstract int TotalSeats { get; }
    public abstract int Rows { get; }
    public abstract string[] Columns { get; }
    private readonly HashSet<string> _bookedSeats;
    private readonly Dictionary<string, DateTime> _reservedSeats;

    protected Bus(int busId, string coachNumber)
    {
        BusId = busId;
        CoachNumber = coachNumber;
        _bookedSeats = new HashSet<string>(); // unordered_set in C++
        _reservedSeats = new Dictionary<string, DateTime>();
    }
    public virtual List<string> GetAllSeatNumbers()
    {
        List<string> seats = new List<string>();
        for (int row = 1; row <= Rows; row++)
        {
            foreach (string col in Columns)
            {
                seats.Add($"{row}{col}");
            }
        }
        return seats;
    }
    public bool IsValidSeat(string seatNumber)
    {
        return GetAllSeatNumbers().Contains(seatNumber);
    }
    public bool IsSeatAvailable(string seatNumber)
    {
        if (!IsValidSeat(seatNumber) || _bookedSeats.Contains(seatNumber))
            return false;
        
        // Block if reserved and not expired
        if (_reservedSeats.ContainsKey(seatNumber) && !IsReservationExpired(seatNumber))
            return false;
        
        return true;
    }
    public bool BookSeat(string seatNumber)
    {
        if (IsSeatAvailable(seatNumber))
        {
            _bookedSeats.Add(seatNumber);
            return true;
        }
        return false;
    }
    
    // Reserve a seat for N minutes
    public bool ReserveSeat(string seatNumber, int durationMinutes = 5)
    {
        if (!IsValidSeat(seatNumber) || _bookedSeats.Contains(seatNumber))
            return false;
        
        if (_reservedSeats.ContainsKey(seatNumber) && DateTime.Now <= _reservedSeats[seatNumber])
            return false; // Already reserved and not expired
        
        _reservedSeats[seatNumber] = DateTime.Now.AddMinutes(durationMinutes);
        return true;
    }
    
    // Release a reservation (when payment succeeds)
    public bool UnReserveSeat(string seatNumber)
    {
        
        return _reservedSeats.Remove(seatNumber);
    }
    
    // Check if a reservation has expired
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
    public int GetAvailableSeatsCount()
    {
        return TotalSeats - _bookedSeats.Count - 1; // excluding driver's seat
    }
    public List<string> GetBookedSeatsList()
    {
        return _bookedSeats.OrderBy(s => s.PadLeft(3, '0')).ToList();
    }
    public override string ToString() // overriding ToString method of Object class
    {
        return "Bus ID: " + BusId + "\nCoach: " + CoachNumber + "\nType: " + BusType + "\nTotal Seats: " + TotalSeats + "\nAvailable: " + GetAvailableSeatsCount();
    }
}
