using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.SeedData
{
    internal class SeedDataTimeService
    {
        public static void PopulateForTimeService(ApplicationContext applicationContext)
        {
            World world = new World
            {
                Name = "world",
                Kingdoms = new List<Kingdom>(),
                Locations = new List<Location>()
            };
            User user1 = new User
            {
                Name = "user",
                Email = "email",
                PasswordHash = "password",
                ForgottenPasswordToken = "token",
                VerificationToken = "token"
            };
            Location location1 = new Location
            {
                World = world,
                XCoordinate = 0,
                YCoordinate = 0
            };
            Kingdom kingdom1 = new Kingdom
            {
                Name = "kingdom",
                World = world,
                Location = location1,
                Armies = new List<Army>(),
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                User = user1,
                AttackBattles = new List<Battle>()
            };
            User user2 = new User
            {
                Name = "user",
                Email = "email",
                PasswordHash = "password",
                ForgottenPasswordToken = "token",
                VerificationToken = "token"
            };
            Location location2 = new Location
            {
                World = world,
                XCoordinate = 0,
                YCoordinate = 0
            };
            Kingdom kingdom2 = new Kingdom
            {
                Name = "kingdom",
                World = world,
                Location = location2,
                Armies = new List<Army>(),
                Buildings = new List<Building>(),
                Resources = new List<Resource>(),
                User = user2,
                AttackBattles = new List<Battle>()
            };
            Farm farm = new Farm
            {
                Kingdom = kingdom1,
                Production = 60,
                Level = 1
            };
            Soldier soldier1 = new Soldier
            {
                Level = 1
            };
            Soldier soldier2 = new Soldier
            {
                Level = 2
            };
            Food food1 = new Food
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            Wood wood1 = new Wood
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            Gold gold1 = new Gold
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            People people1 = new People
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            user1.Role = "Player";
            user2.Role = "Player";
            kingdom1.Resources.Add(food1);
            kingdom1.Resources.Add(wood1);
            kingdom1.Resources.Add(gold1);
            kingdom1.Resources.Add(people1);
            Food food2 = new Food
            {
                UpdatedAt = DateTime.Now,
                Amount = -1
            };
            Wood wood2 = new Wood
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            Gold gold2 = new Gold
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            People people2 = new People
            {
                UpdatedAt = DateTime.Now,
                Amount = 0
            };
            kingdom2.Resources.Add(food2);
            kingdom2.Resources.Add(wood2);
            kingdom2.Resources.Add(gold2);
            kingdom2.Resources.Add(people2);
            kingdom1.Resources.Add(soldier1);
            kingdom1.Resources.Add(soldier2);
            kingdom1.Buildings.Add(farm);
            Army army1 = new Army
            {
                Soldiers = new List<Soldier>(),
                Kingdom = kingdom1,
                DisbandsAt = DateTime.Now
            };
            army1.Soldiers.Add(soldier1);
            Army army2 = new Army
            {
                Soldiers = new List<Soldier>(),
                Kingdom = kingdom1,
                DisbandsAt = DateTime.Now.AddMinutes(30)
            };
            Soldier soldier3 = new Soldier();
            kingdom2.Resources.Add(soldier3);
            soldier3.Level = 1;
            army2.Soldiers.Add(soldier2);
            Battle battle1 = new Battle();
            battle1.Attacker = kingdom1;
            battle1.AttackerArmyId = 1;
            battle1.Defender = kingdom2;
            battle1.Fought_at = DateTime.Now;
            Battle battle2 = new Battle();
            battle2.Attacker = kingdom1;
            battle2.AttackerArmyId = 2;
            battle2.Defender = kingdom2;
            battle2.Fought_at = DateTime.Now.AddMinutes(30);
            kingdom1.Armies.Add(army1);
            kingdom1.Armies.Add(army2);
            applicationContext.Resources.Add(soldier3);
            applicationContext.Battles.Add(battle1);
            applicationContext.Battles.Add(battle2);
            applicationContext.Worlds.Add(world);
            applicationContext.Kingdoms.Add(kingdom1);
            applicationContext.Kingdoms.Add(kingdom2);
            applicationContext.Users.Add(user1);
            applicationContext.Users.Add(user2);
            applicationContext.Locations.Add(location1);
            applicationContext.Locations.Add(location2);
            applicationContext.Resources.Add(food1);
            applicationContext.Resources.Add(wood1);
            applicationContext.Resources.Add(gold1);
            applicationContext.Resources.Add(people1);
            applicationContext.Resources.Add(food2);
            applicationContext.Resources.Add(wood2);
            applicationContext.Resources.Add(gold2);
            applicationContext.Resources.Add(people2);
            applicationContext.Resources.Add(soldier1);
            applicationContext.Resources.Add(soldier2);
            applicationContext.Buildings.Add(farm);
            applicationContext.Armies.Add(army1);
            applicationContext.Armies.Add(army2);
            applicationContext.SaveChanges();
        }
    }
}
