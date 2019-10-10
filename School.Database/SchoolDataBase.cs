using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using School.Entity;


namespace School.Database
{
    public class SchoolDataBase : DbContext
    {
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public SchoolDataBase(DbContextOptions<SchoolDataBase> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Group>()
                .HasMany(x => x.Students)
                .WithOne(x => x.Group)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Group>()
                .Property(x => x.RowVersion)
                .IsRowVersion();

            base.OnModelCreating(builder);
        }
    }
}
