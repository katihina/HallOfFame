using Microsoft.EntityFrameworkCore;
using HallOfFame.Models; 

namespace HallOfFame.Data
{
    public class HallOfFameDbContext : DbContext
    {
        public HallOfFameDbContext(DbContextOptions<HallOfFameDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.Person)         
                .WithMany(p => p.Skills)       
                .HasForeignKey(s => s.PersonId) 
                .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}