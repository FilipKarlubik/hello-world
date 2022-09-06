namespace Eucyon_Tribes.Models.DTOs.SoldierDTOs
{
    public class CreateSoldiersDTO
    {
        public List<int> NumberOfUnitsByLevel { get; }

        public CreateSoldiersDTO(List<int> numberOfUnitsByLevel)
        {
            NumberOfUnitsByLevel = numberOfUnitsByLevel;
        }
    }
}
