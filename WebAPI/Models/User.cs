namespace WebAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IList<TodoTask> Todo { get; set; } = null!;
    }
}
