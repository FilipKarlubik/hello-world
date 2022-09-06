namespace Eucyon_Tribes.Models.DTOs
{
    public class ResourcesDTO
    {
        public int Id { get; }
        public string Owner { get; }
        public List<ResourceDTO> Resources { get; }

        public ResourcesDTO(int id, string owner, List<ResourceDTO> resources)
        {
            Id = id;
            Owner = owner;
            Resources = resources;
        }
    }
}
