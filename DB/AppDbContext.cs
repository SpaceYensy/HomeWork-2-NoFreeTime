using Microsoft.EntityFrameworkCore;
using NoFreeTime.Entity;
using System.Collections.Generic;

namespace NoFreeTime.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Feriado> Feriados { get; set; }
    }
}