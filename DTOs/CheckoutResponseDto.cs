namespace API.DTOs
{
    public class CheckoutResponseDto
    {
        public string CheckoutUrl { get; set; }
        public string TransactionId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }
}