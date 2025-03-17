using System;

namespace API.DTOs
{
    public class SubscriptionRequestDto
    {
        public decimal Amount { get; set; } = 1;
        public bool FirstPaymentIncludedInCycle { get; set; } = true;
        public string ServiceId { get; set; } = "100001";
        public string Currency { get; set; } = "BDT";
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Frequency { get; set; } = "WEEKLY"; // Default to WEEKLY
        public string SubscriptionType { get; set; } = "BASIC";
        public bool MaxCapRequired { get; set; } = false;
        public string MerchantShortCode { get; set; } = "01307153119";
        public string PayerType { get; set; } = "CUSTOMER";
        public string PaymentType { get; set; } = "FIXED";
        public string RedirectUrl { get; set; }
        public string SubscriptionRequestId { get; set; } = ""; // Will be generated if empty
        public string SubscriptionReference { get; set; }
        public string CKey { get; set; } = "000001";
        public int UserId { get; set; }
        public int TournamentId { get; set; }
    }
}