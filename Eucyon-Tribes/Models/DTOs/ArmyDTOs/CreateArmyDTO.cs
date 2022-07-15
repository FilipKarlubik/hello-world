namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class CreateArmyDTO
    {
        public List<int> Units { get; }

        public CreateArmyDTO(List<int> units)
        {
            Units = units;
        }
    }
}
