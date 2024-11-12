using DevNet_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevNet_DataAccessLayer.Data
{
    public class DevnetDBContext : DbContext
    {
        public DevnetDBContext(DbContextOptions<DevnetDBContext> options) : base(options)
        {
            
        }

        public DbSet<Follow> Followers { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserDev> UserDevs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*
            //Se puede usar Fluent API para configurar las relaciones entre las entidades
            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Profile);
            */

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.UseSqlServer("Server=.;Database=ECommerceDb;Trusted_Connection=True;");
        }
    }
}
