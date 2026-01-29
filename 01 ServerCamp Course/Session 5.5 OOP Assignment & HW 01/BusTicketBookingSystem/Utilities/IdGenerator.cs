public static class IdGenerator
{
    private static int _userIdCounter = 0;
    private static int _busIdCounter = 0;
    private static int _scheduleIdCounter = 0;
    private static int _ticketIdCounter = 0;
    private static int _invoiceIdCounter = 0;

    public static int GetNextUserId()
    {
        return ++_userIdCounter;
    }

    public static int GetNextBusId()
    {
        return ++_busIdCounter;
    }

    public static int GetNextScheduleId()
    {
        return ++_scheduleIdCounter;
    }

    public static int GetNextTicketId()
    {
        return ++_ticketIdCounter;
    }

    public static int GetNextInvoiceId()
    {
        return ++_invoiceIdCounter;
    }
}
