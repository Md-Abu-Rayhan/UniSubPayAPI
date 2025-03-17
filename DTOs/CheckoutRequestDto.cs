using System.Collections.Generic;

namespace API.DTOs
{
    public class CheckoutRequestDto
    {
        public int UserId { get; set; }
        public List<int> TournamentIds { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}