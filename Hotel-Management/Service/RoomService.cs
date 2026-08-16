using Common;
using DAL.Context;
using Entity.Rooms;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class RoomService
    {

        private readonly HotelManagementContext _context;

        public RoomService(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<List<Room>> GetAllRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task<Room> SaveRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room;    
        }

        public async Task<Room?> GetRoomById(Guid id)
        {
            return await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
        } 

        public async Task<Room> EnsureRoomExistById(Guid id)
        {
            var room = await GetRoomById(id);
            if (room == null)
            {
                ErrorDiagnostic diagnostic = new ErrorDiagnostic
                {
                    ComponentName = nameof(RoomService),
                    MethodName = nameof(EnsureRoomExistById),
                    TraceId = Guid.NewGuid().ToString()
                };

                throw new AppException(errorCode: ErrorCode.RESOURCE_NOT_FOUND,
                    message: $"Room with id {id} not found", 
                    isOperational:true,
                    errorDiagnostic: diagnostic);
            }
            return room;
        }

    }
}
