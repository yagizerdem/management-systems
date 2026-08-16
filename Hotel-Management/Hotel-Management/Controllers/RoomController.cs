using Common;
using Common.DTO;
using DAL.Context;
using Entity.Rooms;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service;

namespace Hotel_Management.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        private readonly HotelManagementContext _context;

        public RoomController(RoomService roomService, HotelManagementContext context)
        {
            _roomService = roomService;
            _context = context;
        }

        [HttpGet("get-all-rooms")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.Receptionist},{Roles.ReceptionChief}")]
        public async Task<IActionResult> GetAllRooms()
        {
            List<Room> rooms = await _roomService.GetAllRooms();
            return Ok(ApiResponse<List<Room>>.Ok(rooms, "Rooms retrieved successfully."));
        }

        /// <summary>
        /// Get rooms with advanced filtering, pagination, sorting, and field selection
        /// </summary>
        /// <remarks>
        /// Example queries:
        /// - GET /api/room/search?limit=10&amp;offset=0
        /// - GET /api/room/search?page=1&amp;pageSize=20&amp;sort=-createdAt
        /// - GET /api/room/search?filter=roomNumber eq '101' and status eq 'Available'
        /// - GET /api/room/search?search=deluxe&amp;searchFields=roomType,description
        /// - GET /api/room/search?select=id,roomNumber,status
        /// - GET /api/room/search?exclude=createdAt,updatedAt
        /// </remarks>
        [HttpGet("search")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Manager},{Roles.Receptionist},{Roles.ReceptionChief}")]
        public async Task<IActionResult> SearchRooms([FromQuery] QueryFiltersDTO filters)
        {
            var roomsQuery = _context.Rooms.AsQueryable();

            //var apiFeatures = new ApiFeatures<Room>(
            //    roomsQuery,
            //    filters,
            //    maxLimit: 500,      // Maximum allowed limit
            //    defaultLimit: 50    // Default limit if not specified
            //);

            //var result = await apiFeatures.ApplyAllWithPaginationAsync();

            //return Ok(result.ToApiResponse());
            return Ok();
        }

        /// <summary>
        /// Get available rooms with filtering
        /// </summary>
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableRooms([FromQuery] QueryFilters filters)
        {
            // Start with only available rooms
            var roomsQuery = _context.Rooms.Where(r => r.IsActive == true);

            var apiFeatures = new ApiFeatures<Room>(
                roomsQuery,
                filters,
                maxLimit: 100,
                defaultLimit: 20
            );

            var result = await apiFeatures.ApplyAllWithPaginationAsync();

            return Ok(result.ToApiResponse());
        }

    }
}
