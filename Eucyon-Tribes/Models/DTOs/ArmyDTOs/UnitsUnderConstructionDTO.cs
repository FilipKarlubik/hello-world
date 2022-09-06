namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class UnitsUnderConstructionDTO
    {
        public List<int> NumberOfSoldiersByLevel { get; }
        public DateTime FinishedAt { get; }

        public UnitsUnderConstructionDTO(List<int> numberOfSoldiersByLevel, DateTime finishedAt)
        {
            NumberOfSoldiersByLevel = numberOfSoldiersByLevel;
            FinishedAt = finishedAt;
        }
    }
}
