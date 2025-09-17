using ChatZone.Core.Domain.Dtos;
using ChatZone.Core.Domain.Interfaces.Repositories;
using ChatZone.Core.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Core.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger) {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task RegisterUser(RegisterUserDto registerUser) {
            try {
                var existingUser = _userRepository.GetUserByLogin(registerUser.Username);
                if (existingUser != null) {
                    _logger.LogWarning($"User with login: {registerUser.Username} already exists");
                    throw new InvalidOperationException("User with this username already exists.");
                }

                var user = new User(registerUser.Username, registerUser.Password);
                await _userRepository.AddUser(user);
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while registering user with login: {registerUser.Username}");
                throw new InvalidProgramException();
            }
        }
    }
}
