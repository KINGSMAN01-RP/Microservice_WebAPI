namespace EnquiryMaster_WebAPI.Enums
{

    public enum EnquiryStatus
    {
        Open = 0,
        InProgress = 1,
        OnHold = 2,
        Resolved = 3,
        Closed = 4,
        Cancelled = 5
    }

    public enum EnquiryPriority
    {
        Low = 0,
        Medium = 1,
        High = 2,
        Critical = 3
    }

    public enum EnquirySource
    {
        Unknown = 0,
        WebForm = 1,
        Email = 2,
        Phone = 3,
        BranchWalkIn = 4,
        MobileApp = 5,
        Referral = 6,
        Campaign = 7
    }

    public enum EnquiryChannel
    {
        NotSpecified = 0,
        Online = 1,
        Offline = 2,
        Social = 3,
        Partner = 4

    }
}
