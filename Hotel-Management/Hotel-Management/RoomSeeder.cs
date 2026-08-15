using Common;
using DAL.Context;
using Entity.Rooms;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Management
{
    public static class RoomSeeder
    {


        public static async Task SeedRoomAsync(IServiceProvider provider)
        {
            var context = provider.GetRequiredService<HotelManagementContext>();


            if (await context.Rooms.AnyAsync())
                return;

            // -----------------------------
            // ROOM TYPE IDS
            // -----------------------------

            Guid singleTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000001");

            Guid tripleSingleTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000002");

            Guid twinTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000003");

            Guid doubleTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000004");

            Guid tripleMixedTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000005");

            Guid quadTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000006");

            Guid kingSuiteTypeId =
                Guid.Parse("10000000-0000-0000-0000-000000000007");

            var roomTypes = new List<RoomType>
        {
            new()
            {
                Id = singleTypeId,
                Name = "Single Room",
                Capacity = 1,
                SingleBedCount = 1,
                DoubleBedCount = 0,
                Description = "Single room with one single bed."
            },

            new()
            {
                Id = tripleSingleTypeId,
                Name = "Triple Single Room",
                Capacity = 3,
                SingleBedCount = 3,
                DoubleBedCount = 0,
                Description = "Triple room with three single beds."
            },

            new()
            {
                Id = twinTypeId,
                Name = "Twin Room",
                Capacity = 2,
                SingleBedCount = 2,
                DoubleBedCount = 0,
                Description = "Double occupancy room with two single beds."
            },

            new()
            {
                Id = doubleTypeId,
                Name = "Double Room",
                Capacity = 2,
                SingleBedCount = 0,
                DoubleBedCount = 1,
                Description = "Double occupancy room with one double bed."
            },

            new()
            {
                Id = tripleMixedTypeId,
                Name = "Triple Mixed Room",
                Capacity = 3,
                SingleBedCount = 1,
                DoubleBedCount = 1,
                Description = "Triple room with one single bed and one double bed."
            },

            new()
            {
                Id = quadTypeId,
                Name = "Quad Room",
                Capacity = 4,
                SingleBedCount = 2,
                DoubleBedCount = 1,
                Description = "Four-person room with two single beds and one double bed."
            },

            new()
            {
                Id = kingSuiteTypeId,
                Name = "King Suite",

                // Doküman kral dairesinin yatak düzenini söylemiyor.
                // Bunlar varsayım.
                Capacity = 2,
                SingleBedCount = 0,
                DoubleBedCount = 1,

                Description = "King suite."
            }
        };

            await context.RoomTypes.AddRangeAsync(roomTypes);

            // -----------------------------
            // AMENITIES
            // -----------------------------

            Guid wifiId =
                Guid.Parse("20000000-0000-0000-0000-000000000001");

            Guid airConditioningId =
                Guid.Parse("20000000-0000-0000-0000-000000000002");

            Guid tvId =
                Guid.Parse("20000000-0000-0000-0000-000000000003");

            Guid hairDryerId =
                Guid.Parse("20000000-0000-0000-0000-000000000004");

            Guid minibarId =
                Guid.Parse("20000000-0000-0000-0000-000000000005");

            Guid balconyId =
                Guid.Parse("20000000-0000-0000-0000-000000000006");

            var amenities = new List<Amenity>
        {
            new() { Id = wifiId, Name = "Wi-Fi" },
            new() { Id = airConditioningId, Name = "Air Conditioning" },
            new() { Id = tvId, Name = "TV" },
            new() { Id = hairDryerId, Name = "Hair Dryer" },
            new() { Id = minibarId, Name = "Minibar" },
            new() { Id = balconyId, Name = "Balcony" }
        };

            await context.Amenities.AddRangeAsync(amenities);

            // -----------------------------
            // ROOMS
            // -----------------------------

            var rooms = new List<Room>();

            // Floor 1
            // 101-110 -> Single
            AddRooms(rooms, 1, 101, 10, singleTypeId);

            // 111-120 -> Triple, 3 single beds
            AddRooms(rooms, 1, 111, 10, tripleSingleTypeId);


            // Floor 2
            // 201-210 -> Single
            AddRooms(rooms, 2, 201, 10, singleTypeId);

            // 211-220 -> Twin
            AddRooms(rooms, 2, 211, 10, twinTypeId);


            // Floor 3
            // 301-310 -> Double
            AddRooms(rooms, 3, 301, 10, doubleTypeId);

            // 311-320 -> Triple mixed
            AddRooms(rooms, 3, 311, 10, tripleMixedTypeId);


            // Floor 4
            // 401-410 -> Double
            AddRooms(rooms, 4, 401, 10, doubleTypeId);

            // 411-416 -> Quad
            AddRooms(rooms, 4, 411, 6, quadTypeId);

            // 417 -> King Suite
            rooms.Add(new Room
            {
                Id = Guid.NewGuid(),
                RoomNumber = "417",
                Floor = 4,
                RoomTypeId = kingSuiteTypeId,
                IsActive = true
            });

            await context.Rooms.AddRangeAsync(rooms);

            // -----------------------------
            // ROOM AMENITIES
            // -----------------------------

            var roomAmenities = new List<RoomAmenity>();

            foreach (Room room in rooms)
            {
                // Every room:
                // WiFi + AC + TV + Hair Dryer
                AddAmenity(roomAmenities, room.Id, wifiId);
                AddAmenity(roomAmenities, room.Id, airConditioningId);
                AddAmenity(roomAmenities, room.Id, tvId);
                AddAmenity(roomAmenities, room.Id, hairDryerId);

                // All rooms except single rooms have minibar
                if (room.RoomTypeId != singleTypeId)
                {
                    AddAmenity(roomAmenities, room.Id, minibarId);
                }

                // Floors 3 and 4 have balconies
                if (room.Floor is 3 or 4)
                {
                    AddAmenity(roomAmenities, room.Id, balconyId);
                }
            }

            await context.RoomAmenities.AddRangeAsync(roomAmenities);

            await context.SaveChangesAsync();

        }

        private static void AddRooms(
    List<Room> rooms,
    int floor,
    int startingRoomNumber,
    int count,
    Guid roomTypeId)
        {
            for (int i = 0; i < count; i++)
            {
                rooms.Add(new Room
                {
                    Id = Guid.NewGuid(),
                    RoomNumber = (startingRoomNumber + i).ToString(),
                    Floor = floor,
                    RoomTypeId = roomTypeId,
                    IsActive = true
                });
            }
        }

        private static void AddAmenity(
            List<RoomAmenity> roomAmenities,
            Guid roomId,
            Guid amenityId)
        {
            roomAmenities.Add(new RoomAmenity
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                AmenityId = amenityId
            });
        }
    }
}
