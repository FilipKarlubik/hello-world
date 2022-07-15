using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Resources;

namespace Eucyon_Tribes.Factories
{
    public class ArmyFactory : IArmyFactory
    {
        public Army CrateArmy(List<Soldier> soldiers, Kingdom kingdom)
        {
            Army army = new Army();
            foreach (Soldier soldier in soldiers)
            {
                soldier.Army = army;
            }
            army.Soldiers = soldiers;
            army.Kingdom = kingdom;
            army.Type = "Attack";
            return army;
        }
    }
}

