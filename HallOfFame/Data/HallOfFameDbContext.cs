using Microsoft.EntityFrameworkCore;
using HallOfFame.Models; 

namespace HallOfFame.Data
{
    public class HallOfFameDbContext : DbContext
    {
        public HallOfFameDbContext(DbContextOptions<HallOfFameDbContext> options) : base(options)
        {
        }
        
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.DisplayName).IsRequired();
                entity.HasMany(p => p.Skills).WithOne(s => s.Person).HasForeignKey(s => s.PersonId).OnDelete(DeleteBehavior.Cascade);
            });
            
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired();
                entity.Property(s => s.Level).IsRequired();
            });
        }
    }
}