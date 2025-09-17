using ChatZone.Core.Domain;
using ChatZone.Core.Domain.Interfaces.Repositories;
using ChatZone.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository {
        private readonly ChatDbContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ChatDbContext context, ILogger<UserRepository> logger) {
            _context = context;
            _logger = logger;
        }

        public async Task AddUser(User user) {
            try {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while adding user with login: {user.Username}");
                throw;
            }
        }

        public async Task<User?> GetUserById(Guid id) {
            try {
                var user = await _context.Users.FindAsync(id);

                if (user == null) {
                    _logger.LogWarning($"User with ID: {id} not found.");
                }
                return user;
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while retrieving user with ID: {id}");
                throw;
            }
        }

        public async Task<User?> GetUserByLogin(string login) {
            try {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login);

                if (user == null) {
                    _logger.LogWarning($"User with login: {login} not found.");
                }
                return user;
            }
            catch (Exception ex) {
                _logger.LogError(ex, $"Error occurred while retrieving user with login: {login}");
                throw;
            }
        }
    }
}
