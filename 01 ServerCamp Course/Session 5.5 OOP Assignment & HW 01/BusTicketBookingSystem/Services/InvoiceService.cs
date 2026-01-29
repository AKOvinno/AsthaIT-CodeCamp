using System;
using System.Collections.Generic;
using System.Linq;
public interface IInvoiceService
{
    Invoice GenerateInvoice(int ticketId, int userId, decimal amount);
    List<Invoice> GetUserInvoices(int userId);
    bool PayInvoice(int invoiceId);
}
public class InvoiceService : IInvoiceService
{
    private readonly List<Invoice> _invoices;
    private IBookingService _bookingService;

    public InvoiceService()
    {
        _invoices = new List<Invoice>();
    }

    public void SetBookingService(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public Invoice GenerateInvoice(int ticketId, int userId, decimal amount)
    {
        int invoiceId = IdGenerator.GetNextInvoiceId();
        Invoice invoice = new Invoice(invoiceId, ticketId, userId, amount);
        _invoices.Add(invoice);

        Console.WriteLine($"Invoice generated! Invoice ID: {invoiceId}, Amount: {amount} BDT");
        return invoice;
    }

    public List<Invoice> GetUserInvoices(int userId)
    {
        CleanupExpiredInvoices();
        return _invoices.Where(i => i.UserId == userId).ToList();
    }

    public bool PayInvoice(int invoiceId)
    {
        Invoice invoice = _invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
        if (invoice == null)
        {
            Console.WriteLine($"Error: Invoice with ID {invoiceId} not found.");
            return false;
        }

        if (invoice.PaymentStatus == "Paid")
        {
            Console.WriteLine($"Invoice {invoiceId} is already paid.");
            return false;
        }

        invoice.PaymentStatus = "Paid";

        // Finalize booking after payment
        if (_bookingService != null)
        {
            _bookingService.FinalizeBookingAfterPayment(invoice.TicketId);
        }

        Console.WriteLine("Payment successful!");
        Console.WriteLine($"Invoice ID: {invoiceId} has been paid.");
        Console.WriteLine($"Ticket ID: {invoice.TicketId} is now confirmed.");
        return true;
    }
    
    // Remove unpaid invoices for which tickets no longer exist (expired)
    public void CleanupExpiredInvoices()
    {
        if (_bookingService == null)
            return;
        
        List<Invoice> orphanedInvoices = _invoices.Where(i => i.PaymentStatus == "Unpaid" && _bookingService.GetUserTickets(i.UserId).FirstOrDefault(t => t.TicketId == i.TicketId) == null).ToList();
        
        foreach (Invoice invoice in orphanedInvoices)
        {
            _invoices.Remove(invoice);
            Console.WriteLine($"Invoice {invoice.InvoiceId} (unpaid) auto-cancelled due to expired ticket.");
        }
    }
}
