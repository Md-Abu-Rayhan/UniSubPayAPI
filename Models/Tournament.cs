using System;

namespace API.Models
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal EntryFee { get; set; }
        public int MaxParticipants { get; set; }
        public bool IsActive { get; set; }
        public string Location { get; set; }
        public string OrganizerId { get; set; }
    }
}