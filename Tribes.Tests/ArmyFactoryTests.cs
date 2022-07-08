using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests
{
    public class ArmyFactoryTests
    {
        ArmyFactory Factory;
        Kingdom Kingdom;

        public ArmyFactoryTests()
        {
            Factory = new ArmyFactory();
            Kingdom = new Kingdom();
        }

        [Fact]
        public void ArmyFactory_Create_Army()
        {
            List<Soldier> soldiers = new List<Soldier>();
            for (int i = 0; i < 1000; i++)
            {
                Soldier soldier = new Soldier();
                soldier.Attack = 10;
                soldier.Defense = 10;
                soldier.HP = 30;
                soldiers.Add(soldier);
            }

            Army army = Factory.CrateArmy(soldiers, Kingdom);

            Assert.Equal(army.Attack , 10000);
            Assert.Equal(army.Defense, 10000);
            Assert.Equal(army.HP, 30000);
            Assert.Equal(army.Soldiers, soldiers);
        }
    }
}
