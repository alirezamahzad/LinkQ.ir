using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DbEntities:DbContext
    {
        public DbEntities(DbContextOptions<DbEntities> options) :base(options)
        {

        }
        public DbSet<Link> Links { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
    }
}
