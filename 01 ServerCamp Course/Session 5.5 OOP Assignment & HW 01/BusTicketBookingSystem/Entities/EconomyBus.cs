using System.Collections.Generic;
public class EconomyBus : Bus
{
    public override int TotalSeats => 37;
    public override int Rows => 9;
    public override string[] Columns => new[] { "A", "B", "C", "D" };

    public EconomyBus(int busId, string coachNumber) : base(busId, coachNumber)
    {
        BusType = "Economy";
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
