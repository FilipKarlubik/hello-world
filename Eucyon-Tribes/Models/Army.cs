using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Models
{
    public class Army
    {
        public int Id { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int HP { get; set; }

        public int KingdomId { get; set; }
        public Kingdom Kingdom { get; set; } = null!;

        public List<Soldier> Soldiers { get; set; } = null!;

        public Army() 
        { 
        }
    }
}
