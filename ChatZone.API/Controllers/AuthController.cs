using ChatZone.Core.Domain.Interfaces.Repositories;
using ChatZone.Core.Domain.Interfaces.Services;
using ChatZone.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatZone.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _userService;

        public AuthController(IAuthService userService) {
            _userService = userService;
        }

        [HttpGet]
        public async Task<JsonResult> GetUser() {
            await _userService.RegisterUser(new Core.Domain.Dtos.RegisterUserDto() {
                Username = "test1",
                Password = "test"
            });
            return Json("");
        }
    }
}
