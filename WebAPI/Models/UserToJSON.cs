namespace WebAPI.Models
{
    public class UserToJSON
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> tasks {get; set; }

        public UserToJSON(int id, string name)
        {
            Id = id;
            Name = name;
            tasks = new List<string>();
        }
    }
}
