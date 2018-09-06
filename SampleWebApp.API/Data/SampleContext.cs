using Microsoft.EntityFrameworkCore;
using SampleWebApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApp.API
{
    public class SampleContext : DbContext
    {
        public SampleContext()
        {
            Database.EnsureCreated();
        }
        public DbSet<SampleModel> Samples { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connString = Startup.Configuration["Settings:SampleConnection"];
            optionsBuilder.UseSqlServer(connString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
