using Microsoft.EntityFrameworkCore;

namespace RailwayReservationSystem.Models
{
    public class RailwayDbContext : DbContext
    {
        public RailwayDbContext(DbContextOptions<RailwayDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("data source = isan-desktop\\sqlexpress; database = MyRailway; integrated security = true");
        //}

    }
}
