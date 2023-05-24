using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace WebApplication.Models
{
    public class Database_Entities : DbContext
    {
        public Database_Entities() : base("Database") { }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Post>().ToTable("Posts");

            modelBuilder.Entity<Favorite>().ToTable("Favorites");
        }

        public virtual DbSet<Post> Posts { get; set; }

        public virtual DbSet<Favorite> Favorites { get; set; }

    }
}