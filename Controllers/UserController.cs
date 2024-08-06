using GymManagment.Models;
using GymManagment.Models.DTOs;
using GymManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string filterByName = null)
        {
            var users = _userService.GetAll(pageNumber, pageSize, filterByName);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateUserDTO user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User createdUser= _userService.Create(user);
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id}, createdUser);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            _userService.Update(id,user);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

           User user= _userService.Delete(id);
            return Ok(user);
        }
    }
}
