using Common;
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
        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("get-all-rooms")]
        [Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}, {Roles.Receptionist}, {Roles.ReceptionChief}")]
        public async Task<IActionResult> GetAllRooms()
        {
            List<Room> rooms = await _roomService.GetAllRooms();
            return Ok(ApiResponse<List<Room>>.Ok(rooms, "Rooms retrieved successfully."));
        }


    }
}
