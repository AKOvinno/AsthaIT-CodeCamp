using System.Collections.Generic;
public class BusinessBus : Bus
{
    public override int TotalSeats => 28;
    public override int Rows => 9;
    public override string[] Columns => new[] { "A", "B", "C" };

    public BusinessBus(int busId, string coachNumber) : base(busId, coachNumber)
    {
        BusType = "Business";
    }
    public override List<string> GetAllSeatNumbers()
    {
        List<string> seats = new List<string>();
        for (int row = 1; row <= Rows; row++)
        {
            foreach (string col in Columns)
            {
                seats.Add($"{row}{col}");
            }
        }
        // Driver's seat
        seats.Add("0A");
        return seats;
    }
}
