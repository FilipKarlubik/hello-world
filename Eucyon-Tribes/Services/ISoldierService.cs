using Eucyon_Tribes.Models.DTOs.SoldierDTOs;

namespace Eucyon_Tribes.Services
{
    public interface ISoldierService
    {
        string GetError();

        bool CreateSoldiers(int id, CreateSoldiersDTO createSoldiersDTO);
    }
}
