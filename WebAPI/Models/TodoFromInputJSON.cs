namespace WebAPI.Models
{
    public class TodoFromInputJSON
    {
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
    }
}
