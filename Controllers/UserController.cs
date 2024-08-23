using System.Net;
using GymManagment.Exceptions;
using GymManagment.Models;
using GymManagment.Models.DTOs;
using GymManagment.Models.Enums;
using GymManagment.Services.Interfaces;
using GymManagment.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly string _imageFolderPath;

        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            string webRootPath = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            _imageFolderPath = Path.Combine(webRootPath, "images");

            if (string.IsNullOrEmpty(_imageFolderPath))
            {
                throw new ArgumentNullException(nameof(_imageFolderPath), "Image folder path cannot be null");
            }

            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }
        }
       
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string filterByName = null
        )
        {
            try
            {
                var users = _userService.GetAll(pageNumber, pageSize, filterByName);
                return Ok(new Response<IEnumerable<User>>("ALL USERS", users, (int)HttpStatusCode.Accepted));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                    return NotFound();
                return Ok(new Response<object>("Retrieved User", user, (int)HttpStatusCode.Accepted));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]    
        public IActionResult Create([FromForm] CreateUserDTO user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                UserResponse createdUser = _userService.Create(user, _imageFolderPath);
                return Ok(new Response<UserResponse>("Created User", createdUser, (int)HttpStatusCode.Created));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            try
            {
                var updateUser = _userService.Update(id, user);
                return Ok(new Response<object>("User Updated Successfully", updateUser, (int)HttpStatusCode.Accepted));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult Delete(int id)
        {
            try
            {
                User user = _userService.Delete(id);
                return Ok(new Response<object>("User Deleted", user, (int)HttpStatusCode.Accepted));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost("/resetPassword")]
        [Authorize(Policy = "UserOrAdmin")]
        public IActionResult RenewPassword([FromBody] ResetPasswordDTO passwordDto)
        {
            try
            {
                _userService.ResetPassword(passwordDto);
                return Ok(new Response<object>("Password Changed Successfully", null, (int)HttpStatusCode.Accepted));
            }
            catch (Exception e)
            {
                throw new ApiException((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}