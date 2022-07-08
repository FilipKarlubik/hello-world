using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("web")]
    public class WebController : Controller
    {
        private readonly IWebServices _services;

        public WebController(IWebServices services)
        {
            _services = services;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            List<TodoTask> tasks = _services.ListAllTodos();
            List<User> users = _services.ListAllUsers();
            ViewBag.Message = "Welcome to ToDo Store Base :)";
            dynamic mymodel = new ExpandoObject();
            mymodel.Todos = tasks;
            mymodel.Users = users;
            return View(mymodel);
        }

        [HttpGet("add")]
        public IActionResult AddTodo()
        {
            return View();
        }
        [HttpPost("add")]
        public IActionResult AddTodoPost(string title, int userId)
        {
            _services.AddTodo(title, userId);
            return RedirectToAction("Index");
        }
    }
}
