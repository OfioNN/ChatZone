using ChatZone.Core.Domain.Interfaces.Repositories;
using ChatZone.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<JsonResult> GetUser() {
            var user = new User() {
                Username = "test",
                Password = "haslo",
            };

            await _userRepository.AddUser(user);
            return Json(user);
        }
    }
}
