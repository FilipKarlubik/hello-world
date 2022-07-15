using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class ArmyDTO
    {
        public int Id { get; }
        public int Owner { get; }
        public string Type { get; } = null!;
        public List<SoldierDTO> Units { get; }

        public ArmyDTO(int id, int owner, string type, List<SoldierDTO> units)
        {
            Id = id;
            Owner = owner;
            Type = type;
            Units = units;
        }
    }
}
