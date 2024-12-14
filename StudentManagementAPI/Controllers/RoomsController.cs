using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentManagementAPI.Entities;
using StudentManagementAPI.Models.Creates;

namespace StudentManagementAPI.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly StudentManagementContext _studentManagementContext;

        //DI
        public RoomsController(StudentManagementContext studentManagementContext)
        {
            _studentManagementContext = studentManagementContext;
        }

        [HttpGet]
        [Route("hello")]
        public IActionResult HelloWorld()
        {
            return NotFound("Hello World!!!");
        }

        [HttpGet]
        public IActionResult GetRooms()
        {
            var rooms = _studentManagementContext.Rooms.OrderByDescending(x => x.CreatedAt).ToList();
            return StatusCode(777, rooms);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRoom([FromRoute] Guid id)
        {
            var room = _studentManagementContext.Rooms.Where(x => x.Id == id).FirstOrDefault();
            if
                (room == null)
            {
                return NotFound("Khong tim thay phong!!!");
            }
            return Ok(room);
        }

        [HttpPost]
        public IActionResult CreateRoom([FromBody] RoomCreateModel model)
        {
            var room = new Room{ 
                Id = Guid.NewGuid(),
                Name = model.Name
            };
            _studentManagementContext.Add(room);

            var result = _studentManagementContext.SaveChanges();

            if (result > 0)
            {
                return StatusCode(201, room);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteRoom([FromRoute] Guid id)
        {
            var room = _studentManagementContext.Rooms.Where(x => x.Id == id).FirstOrDefault();

            if (room == null)
            {
                return NotFound("Khong tim thay phong!!!");
            }

            _studentManagementContext.Remove(room);

            var result = _studentManagementContext.SaveChanges();

            if (result > 0)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
