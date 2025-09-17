using ChatZone.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Core.Domain.Interfaces.Repositories
{
    public interface IUserRepository {
        Task AddUser(User user);
        Task<User?> GetUserById(Guid id);
        Task<User?> GetUserByLogin(string login);
    }
}
