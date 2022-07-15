using Eucyon_Tribes.Models.DTOs.ArmyDTOs;

namespace Eucyon_Tribes.Services
{
    public interface IArmyService
    {
        public ArmyDTO[] GetArmies(int kingdomId);

        public ArmyDTO GetArmy(int armyId);

        public Boolean CreateArmy(CreateArmyDTO createArmyDTO, int kingdomId);

        public Boolean UpdateArmy(int armyId, UpdateArmyDTO update);

        public Boolean RemoveArmy(int armyId);

        public String GetError();
    }
}
