using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BattleDTOs;

namespace Eucyon_Tribes.Services
{
    public interface IBattleService
    {
        void Battle(Battle battle);

        public BattleCostDTO BattleCost(BattleRequestDTO battleRequest, int id);

        public StatusDTO CreateBattle(BattleRequestDTO battleRequest, int id);

        public String GetError();

        public void CheckForBattles();

        public List<BattleResposeDto> GetBattles(int page, int itemCount, int id);
    }
}
