using ChatZone.Core.Domain.Dtos;
using ChatZone.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Core.Domain.Interfaces.Services
{
    public interface IJwtService {
        AuthDto GenerateJwtToken(User user);
    }
}
