using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class TodoTasksContext : DbContext
    {
        public DbSet<TodoTask> TodoTasks { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=BZDOCHA;Initial Catalog=MEAT_DB;Integrated Security=True;Pooling=False");
        }
    }
}
