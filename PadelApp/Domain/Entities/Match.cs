using PadelApp.Domain.Enums;

namespace PadelApp.Domain.Entities
{
    public class Match //partido
    {
        public int Id { get; set; } 
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public MatchTypes MatchType { get; set; }
        public List<Booking> Bookings { get; set; } = new();
    }
}
