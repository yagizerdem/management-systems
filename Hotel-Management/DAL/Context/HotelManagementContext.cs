using BilgeHotel.Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public sealed class HotelManagementContext : DbContext
    {

        // rooms 
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Amenity> Amenity { get; set; }

        public DbSet<RoomAmenity> RoomAmenities { get; set; }

        public DbSet<RoomBlock> RoomBlocks { get; set; }

        public DbSet<RoomType> RoomTypes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Blogging;Trusted_Connection=True;ConnectRetryCount=0");
        }

    }
}
