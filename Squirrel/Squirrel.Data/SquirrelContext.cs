﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Squirrel.Data.Tools;
using Squirrel.Domain.Enititis;

namespace Squirrel.Data
{
    internal class SquirrelContext : DbContext
    {
        public SquirrelContext()
            : base("name=SquirrelContext")
        {
            Database.SetInitializer(new SquirrelContextInitializer());
            Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<LogInfo> LogInfos { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Blog");
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Post>().HasMany<File>(s => s.Branches).WithMany(c => c.Users)
            //    .Map(c => c.MapLeftKey("User_id").MapRightKey("Branch_id").ToTable("UserBranches", "App"));
        }
    }
}