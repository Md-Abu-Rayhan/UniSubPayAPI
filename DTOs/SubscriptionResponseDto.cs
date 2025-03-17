namespace API.DTOs
{
    public class SubscriptionResponseDto
    {
        public string PaymentUrl { get; set; }
        public string SubscriptionRequestId { get; set; }
        public string Status { get; set; }
        public string Frequency { get; set; } 
    }
}