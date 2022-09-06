namespace Eucyon_Tribes.Models.DTOs
{
    public class ResourceDTO
    {
        public string Type { get; }
        public int Amount { get; }

        public ResourceDTO(string type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}
