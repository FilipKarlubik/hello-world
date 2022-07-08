using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Factories
{
    public interface IArmyFactory
    {
        Army CrateArmy(List<Soldier> soldiers,Kingdom kingdom);
    }
}
