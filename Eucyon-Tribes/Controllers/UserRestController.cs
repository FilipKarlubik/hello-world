using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.UserModels;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("users")]
    [ApiController]
    public class UserRestController : ControllerBase
    {
        private readonly IUserService _userServices;

        public UserRestController(IUserService userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("login")]
        public IActionResult UserLogin(UserLoginDto login)
        {
            string message = _userServices.Login(login);
            if (!message.EndsWith("in")) 
                {
                return BadRequest(new ErrorDTO(message));
            }
            return Ok(new StatusDTO(message));
        }

        [HttpGet("info")]
        public IActionResult UserInformation(string name)
        {
            User info = _userServices.UserInfo(name);
            if (info == null) return NotFound(new ErrorDTO("User not in database"));
            return Ok(info);
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<UserResponseDto> users = _userServices.ListAllUsers();

            if (users == null)
            {
                ErrorDTO error = new("No users in database");
                return NotFound(error);
            }
            return Ok(users);
        }

        [HttpPost("")]
        public IActionResult Store(UsersInputDto users)
        {
            if (users == null)
            {
                ErrorDTO error = new("No valid input object");
                return BadRequest(error);
            }
            return Ok(new StatusDTO(_userServices.StoreUsers(users) + "users added to database"));
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            UserResponseDto info = _userServices.ShowUser(id);
            if (info == null)
            {
                ErrorDTO errorMessage = new("Player not found");
                return NotFound(errorMessage);
            }
            return Ok(info);
        }

        [HttpGet("{id}/edit")]
        public IActionResult Edit(int id, string name, string password)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            if (_userServices.EditUser(id, name, password))
            {
                return Ok(new StatusDTO("User ID: " + id + "changed name to: " + name));
            }
            else
            {
                ErrorDTO message = new("Not an existing ID or not matching password");
                return BadRequest(message);
            }
        }

        [HttpPost("create")]
        public IActionResult UserCreate(UserCreateDto create)
        {
            string message = _userServices.CreateUser(create, null, 0);
            if (message.Equals("No worlds in database")) return BadRequest(new ErrorDTO(message));
            return Ok(new StatusDTO(message));
        }

        [HttpDelete("delete/{id}")]
        public IActionResult Destroy(int id, string password)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            if (_userServices.DestroyUser(id, password))
            {
                return Ok(new StatusDTO("User ID: " + id + " has been removed"));
            }
            ErrorDTO error = new("Wrong UserID or password");
            return NotFound(error);
        }

        [HttpGet("info/admin")]
        public IActionResult UsersInfoDetailedForAdmin(string admin)
        {
            List<UserDetailDto> users = _userServices.UsersInfoDetailedForAdmin(admin);

            if (users == null)
            {
                return NotFound(new ErrorDTO("No users in database or wrong admin Password"));
            }
            return Ok(users);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserCreateDto user)
        {
            if (id < 1)
            {
                ErrorDTO e = new("Invalid id");
                return StatusCode(400, e);
            }
            if (!_userServices.UpdateUser(id, user))
            {
                ErrorDTO error = new("Wrong UserID or password or existing email");
                return NotFound(error);
            }
            else
            {
                return Ok(new StatusDTO("User ID: " + id + " has been updated successfully"));
            }
        }
    }
}