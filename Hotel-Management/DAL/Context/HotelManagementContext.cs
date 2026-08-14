using BilgeHotel.Domain.Customers;
using BilgeHotel.Domain.Employees;
using BilgeHotel.Domain.Pricing;
using BilgeHotel.Domain.Reservations;
using BilgeHotel.Domain.Rooms;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public sealed class HotelManagementContext : DbContext
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
