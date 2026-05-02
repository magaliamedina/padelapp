using PadelApp.Domain.Enums;

namespace PadelApp.Domain.Entities
{
    public class Booking //inscripcion
    {
        public int Id { get; set; } 

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public BookingStatus Status { get; set; } = BookingStatus.Active;
    }
}
