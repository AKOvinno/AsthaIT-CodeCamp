using System;
public class Invoice
{
    public int InvoiceId { get; set; }
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string PaymentStatus { get; set; }
    public Invoice(int invoiceId, int ticketId, int userId, decimal amount)
    {
        InvoiceId = invoiceId;
        TicketId = ticketId;
        UserId = userId;
        Amount = amount;
        InvoiceDate = DateTime.Now;
        PaymentStatus = "Unpaid";
    }
    public override string ToString()
    {
        return "Invoice ID: " + InvoiceId + "\nTicket ID: " + TicketId + "\nUser ID: " + UserId + "\nAmount: " + Amount + "\nDate: " + InvoiceDate + "\nStatus: " + PaymentStatus;
    }
}
