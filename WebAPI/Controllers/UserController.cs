using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IUserServices _services;

        public UserController(IUserServices service)
        {
            _services = service;
        }

        [HttpGet("add")]
        public string AddUser(string name)
        {
            return _services.AddUser(name);
        }

        [HttpGet("list")]
        public ActionResult<List<UserToJSON>> ListUsers()
        {
            var usersList = _services.ListUsers();

            if (usersList.Count == 0)
            {
                return NotFound();
            }
            return Ok(usersList);
        }

        [HttpGet("")]
        public string ShowTodosByUserID(int id)
        {
            return _services.GetTodosById(id);
        }
    }
}
