namespace WebAPI.Models
{
    public class TodoTaskToJSON
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsDone { get; set; }
        public string UserName { get; set; } = string.Empty;

        public TodoTaskToJSON(int id, string title, DateTime date, bool isDone)
        {
            Id = id;
            Title = title;
            Date = date;
            IsDone = isDone;
        }
    }
}
