using ChatZone.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Core.Domain
{
    public class ChatDbContext : DbContext {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
    }
}
