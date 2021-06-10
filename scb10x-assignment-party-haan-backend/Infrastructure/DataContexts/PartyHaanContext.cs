using Microsoft.EntityFrameworkCore;
using scb10x_assignment_party_haan_backend.Domain.AggregatesModel.UserAggregate;

namespace scb10x_assignment_party_haan_backend.Infrastructure.DataContexts
{
    public class PartyHaanContext : DbContext
    {
        public PartyHaanContext(
            DbContextOptions<PartyHaanContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PartyHaan> PartiesHaan { get; set; }
        public DbSet<PartyHaanMember> PartyHaanMembers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<PartyHaan>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<PartyHaanMember>()
                .HasKey(x => x.Id);
        }
    }


}
