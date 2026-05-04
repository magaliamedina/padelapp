using Microsoft.AspNetCore.Mvc;
using PadelApp.Application.DTOs;
using PadelApp.Domain.Entities;
using PadelApp.Infraestructure.Data;

namespace PadelApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Create(CreateUserRequest request)
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Alias = request.Alias,
                Number = request.Number,
                Gender = request.Gender
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(user);
        }
    }
}
