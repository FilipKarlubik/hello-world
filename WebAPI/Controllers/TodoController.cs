using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [ApiController]
    public class TodoController : Controller
    {
        private readonly ITodoServices _services;

        public TodoController(ITodoServices services)
        {
            _services = services;
        }

        [HttpGet("")]
        public string HelloCurak()
        {
            return "Hello ty curak !";
        }

        [HttpGet("greet")]
        public ActionResult GreetUserByName(string name)
        {
            return Ok("Aby ta mater zabila " + name);
        }

        [HttpPost("add")]
        public ActionResult<TodoTaskToJSON> AddTodo(TodoFromInputJSON task)
        {
            var todoTask = _services.AddTodoTask(task);

            if (todoTask == null)
            {
                return NotFound();
            }
            
            return Ok(todoTask);
        }

        [HttpPut("finish")]
        public ActionResult<TodoTaskToJSON> FinishTodo(int id)
        {
            var todoTask = _services.FinishTodoById(id);
            if (todoTask == null)
            {
                return NotFound();
            }
            return Ok(todoTask);
        }

        [HttpDelete("del")]
        public string DeleteTodo(int id)
        {
            return _services.DeleteById(id);
        }

        [HttpGet("list")]
        public ActionResult<List<TodoTaskToJSON>> GetAllTodos()
        {
            var todoTasks = _services.GetAllTodos();
            
            if(todoTasks.Count == 0)
            {
                return NotFound();
            }
            return Ok(todoTasks);
        }
    }
}
