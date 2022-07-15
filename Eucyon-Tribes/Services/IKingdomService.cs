using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;

namespace Eucyon_Tribes.Services
{
    public interface IKingdomService
    {
        Boolean AddKingdom(CreateKingdomDTO createKingdomDTO);
        List<Kingdom> GetKingdomsWorld(World world);
        KingdomsDTO[] GetKingdoms();
        KingdomDTO GetKindom(int id);
        String GetError();
        bool WorldExists(int worldId);
        KingdomCreateResponseDTO AddKingdomWithLocation(KingdomCreateRequestDTO request);
    }
}

