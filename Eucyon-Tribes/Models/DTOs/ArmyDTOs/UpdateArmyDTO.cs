namespace Eucyon_Tribes.Models.DTOs.ArmyDTOs
{
    public class UpdateArmyDTO
    {
        public List<int> Add { get; }
        public List<int> Remove { get; }

        public UpdateArmyDTO(List<int> add, List<int> remove)
        {
            Add = add;
            Remove = remove;
        }
    }
}
