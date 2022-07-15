using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Models
{
    public class Army
    {
        public int Id { get; set; }
        public Kingdom Kingdom { get; set; }
        public List<Soldier> Soldiers { get; set; } = null!;
        public String Type { get; set; } = null!;

        public Army()
        {
        }
    }
}
