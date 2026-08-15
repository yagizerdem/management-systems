using BilgeHotel.Domain.Pricing;
using Entity.Customers;
using Entity.Employees;
using Entity.Identity;
using Entity.Pricing;
using Entity.Reservations;
using Entity.Rooms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public sealed class HotelManagementContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {

        public HotelManagementContext(DbContextOptions<HotelManagementContext> options) : base(options)
        { }

        // rooms 
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Amenity> Amenities { get; set; }

        public DbSet<RoomAmenity> RoomAmenities { get; set; }

        public DbSet<RoomBlock> RoomBlocks { get; set; }

        public DbSet<RoomType> RoomTypes { get; set; }

        // customer

        public DbSet<Customer> Customers { get; set; }

        // employee
        
        public DbSet<Department> Departments { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeShift> EmployeeShifts { get; set; }

        public DbSet<OvertimeRecord> OvertimeRecords { get; set; }

        // pricing

        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        public DbSet<Package> Packages { get; set; }

        public DbSet<RoomRate> RoomRates { get; set; }

        // Reservations

        public DbSet<ExtraCharge> ExtraCharges { get; set; }

        public DbSet<Reservation> Reservations { get; set; }

    }
}
