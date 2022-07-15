namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class SoldierDTO
    {
        public int Id { get; }
        public int TotalHP { get; }
        public int CurrentHP { get; }

        public SoldierDTO(int id, int totalHP, int currentHP)
        {
            Id = id;
            TotalHP = totalHP;
            CurrentHP = currentHP;
        }
    }
}
