using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PadelApp.Application.DTOs;
using PadelApp.Domain.Entities;
using PadelApp.Domain.Enums;
using PadelApp.Infraestructure.Data;

namespace PadelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var matches = _context.Matches
                .Include(m => m.Bookings)
                .ThenInclude(b => b.User)
                .ToList();

            var response = matches.Select(m => new MatchResponse
            {
                Id = m.Id,
                Date = m.Date,
                Location = m.Location,
                MaxPlayers = m.MaxPlayers,
                MatchType = m.MatchType.ToString(),

                ActivePlayers = m.Bookings
                    .Where(b => b.Status == BookingStatus.Active)
                    .Select(b => new PlayerDto
                    {
                        UserId = b.UserId,
                        Name = b.User.Name,
                        Gender = b.User.Gender.ToString()
                    }).ToList(),

                WaitingReplacement = m.Bookings
                    .Where(b => b.Status == BookingStatus.NeedsReplacement)
                    .Select(b => new PlayerDto
                    {
                        UserId = b.UserId,
                        Name = b.User.Name
                    }).ToList(),

                CancelledPlayers = m.Bookings
                    .Where(b => b.Status == BookingStatus.Cancelled)
                    .Select(b => new PlayerDto
                    {
                        UserId = b.UserId,
                        Name = b.User.Name
                    }).ToList()
            });

            return Ok(response);
        }

        [HttpPost]
        public IActionResult Create(CreateMatchRequest request)
        {
            var match = new Match
            {
                Date = request.Date,
                Location = request.Location,
                MaxPlayers = request.MaxPlayers,
                MatchType = request.MatchType   
            };

            _context.Matches.Add(match);
            _context.SaveChanges();

            var response = new MatchResponse
            {
                Id = match.Id,
                Date = match.Date,
                Location = match.Location,
                MaxPlayers = match.MaxPlayers,
                MatchType = match.MatchType.ToString(),
                ActivePlayers = new List<PlayerDto>(),
                WaitingReplacement = new List<PlayerDto>(),
                CancelledPlayers = new List<PlayerDto>()    
            };

            return Ok(response);
        }

        [HttpPost("{matchId}/join")]
        public IActionResult JoinMatch(int matchId, JoinMatchRequest request)
        {
            var match = _context.Matches
                .Include(m => m.Bookings)
                .FirstOrDefault(m => m.Id == matchId);

            if (match == null)
                return NotFound("Match not found");

            var user = _context.Users.FirstOrDefault(u => u.Id == request.UserId);

            if (user == null)
                return NotFound("User not found");

            // validación según tipo de partido
            if (match.MatchType == MatchTypes.Male && user.Gender != Gender.Male)
                return BadRequest("Only male players allowed");

            if (match.MatchType == MatchTypes.Female && user.Gender != Gender.Female)
                return BadRequest("Only female players allowed");

            // validar si ya está
            var alreadyJoined = match.Bookings
                .Any(b => b.UserId == request.UserId && b.Status == BookingStatus.Active);

            if (alreadyJoined)
                return BadRequest("User already joined");            

            // BUSCAR alguien para reemplazar
            var replacement = match.Bookings
                .FirstOrDefault(b => b.Status == BookingStatus.NeedsReplacement);

            if (replacement != null)
            {
                // cerrar el anterior
                replacement.Status = BookingStatus.Cancelled;

                // crear nuevo booking
                var newBooking = new Booking
                {
                    UserId = request.UserId,
                    MatchId = matchId,
                    Status = BookingStatus.Active
                };

                _context.Bookings.Add(newBooking);
                _context.SaveChanges();

                return Ok("User joined as replacement");
            }

            // validar cupo
            var activePlayers = match.Bookings.Count(b => b.Status == BookingStatus.Active);

            if (activePlayers >= match.MaxPlayers)
                return BadRequest("Match is full");

            // join normal
            var booking = new Booking
            {
                UserId = request.UserId,
                MatchId = matchId,
                Status = BookingStatus.Active
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return Ok("User joined match");
        }

        [HttpPost("{matchId}/leave")]
        public IActionResult LeaveMatch(int matchId, LeaveMatchRequest request)
        {
            var booking = _context.Bookings
                .FirstOrDefault(b => b.MatchId == matchId
                                  && b.UserId == request.UserId
                                  && b.Status == BookingStatus.Active);

            if (booking == null)
                return BadRequest("User is not in this match");

            booking.Status = BookingStatus.NeedsReplacement;

            _context.SaveChanges();

            return Ok("User removed from match");
        }
    }
}
