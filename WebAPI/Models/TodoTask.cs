namespace WebAPI.Models
{
    public class TodoTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsDone { get; set; }

        
        public int UserID { get; set; }
        public User User { get; set; } = null!;

        public TodoTask()
        {
            Date = DateTime.Now;
        }
    }
}
