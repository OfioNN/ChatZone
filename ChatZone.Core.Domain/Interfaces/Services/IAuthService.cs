using ChatZone.Core.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Core.Domain.Interfaces.Services
{
    public interface IAuthService {
        Task RegisterUser(RegisterUserDto registerUser);
    }
}
