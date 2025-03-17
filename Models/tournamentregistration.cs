using System;

namespace API.Models
{
    public class TournamentRegistration
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TournamentId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
    }
}