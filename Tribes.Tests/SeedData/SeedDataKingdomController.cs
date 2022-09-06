using Eucyon_Tribes.Models.Resources;

namespace Tribes.Tests.SeedData
{
    public class SeedDataKingdomController
    {
        public static void PopulateForKingdomControllerTest(ApplicationContext applicationContext)
        {
            applicationContext.Database.EnsureDeleted();
            applicationContext.Database.EnsureCreated();
            User user1 = new User();
            user1.Name = "User1";
            user1.PasswordHash = "Password1";
            user1.Email = "Email1";
            user1.VerificationToken = String.Empty;
            user1.ForgottenPasswordToken = String.Empty;
            user1.Role = "Player";
            User user2 = new User();
            user2.Name = "User2";
            user2.PasswordHash = "Password2";
            user2.Email = "Email2";
            user2.VerificationToken = String.Empty;
            user2.ForgottenPasswordToken = String.Empty;
            user2.Role = "Player";
            User user3 = new User();
            user3.Name = "User3";
            user3.PasswordHash = "Password3";
            user3.Email = "Email3";
            user3.Role = "Player";
            User user4 = new User();
            user4.Name = "User4";
            user4.PasswordHash = "Password4";
            user4.Email = "Email4";
            user3.VerificationToken = String.Empty;
            user3.ForgottenPasswordToken = String.Empty;
            user4.VerificationToken = String.Empty;
            user4.ForgottenPasswordToken = String.Empty;
            user4.Role = "Player";
            World world = new World();
            world.Name = "world1";
            Location location1 = new Location();
            location1.YCoordinate = 0;
            location1.XCoordinate = 0;
            Kingdom kingdom1 = new Kingdom();
            location1.Kingdom = kingdom1;
            location1.KingdomId = kingdom1.Id;
            kingdom1.World = world;
            kingdom1.Resources = new List<Resource>();
            kingdom1.Name = "kingdom1";
            kingdom1.WorldId = world.Id;
            kingdom1.Location = location1;
            kingdom1.User = user1;
            kingdom1.UserId = user1.Id;
            for (int i = 0; i < 10; i++)
            {
                Soldier soldier = new Soldier();
                if (i < 5)
                    soldier.Level = 1;

                else
                    soldier.Level = 2;
                soldier.Kingdom = kingdom1;
                kingdom1.Resources.Add(soldier);
            }
            Location location2 = new Location {World=world,XCoordinate=15,YCoordinate=15 };
            Location location3 = new Location { World = world, XCoordinate = 5, YCoordinate = 5 };
            Kingdom kingdom2 = new Kingdom { User = user2, Location = location2, Name = "kingdom2" , World=world};
            Kingdom kingdom3 = new Kingdom { User = user3, Location = location3 , Name="kingdom3", World = world, CanBeAttackedAt=DateTime.Now.AddDays(1)};
            Food food1 = new Food();
            kingdom2.Resources = new List<Resource>();
            food1.Kingdom = kingdom1;
            food1.Amount = 500;
            kingdom1.Resources.Add(food1);
            applicationContext.Resources.Add(food1);
            Food food2 = new Food();
            food2.Kingdom = kingdom2;
            food2.Amount = 0;
            kingdom2.Resources.Add(food2);
            applicationContext.Resources.Add(food2);
            Soldier soldier1 = new Soldier();
            soldier1.Level = 1;
            kingdom2.Resources.Add(soldier1);
            Gold gold = new Gold();
            gold.Kingdom = kingdom1;
            Wood wood = new Wood();
            wood.Kingdom = kingdom1;
            People people = new People();
            people.Kingdom = kingdom1;
            kingdom1.Resources.Add(gold);
            kingdom1.Resources.Add(wood);
            kingdom1.Resources.Add(people);
            Army army = new Army();
            army.Soldiers = new List<Soldier>();
            Battle battle = new Battle();
            battle.Fought_at = DateTime.Now;
            for (int i = 0; i < 12; i++)
            {
                Soldier soldier = new Soldier()
                {
                    Attack = 20,
                    Defense = 20,
                    TotalHP = 50,
                    CurrentHP = 50,
                    Level = 1
                };
                soldier.Kingdom = kingdom1;
                kingdom1.Resources.Add(soldier);
                army.Soldiers.Add(soldier);
                applicationContext.Resources.Add(soldier);
            }
            kingdom2.Resources.Add(new Food() { Amount = 1000 });
            battle.Attacker = kingdom1;
            battle.AttackerId = kingdom1.Id;
            battle.Defender = kingdom2;
            battle.DefenderId = kingdom2.Id;
            battle.AttackerArmyId = army.Id;
            battle.RequestedAt = DateTime.Now.AddMinutes(5);
            battle.WinnerId = null;
            applicationContext.Battles.Add(battle);
            applicationContext.Worlds.Add(world);
            applicationContext.Users.Add(user1);
            applicationContext.Users.Add(user2);
            applicationContext.Users.Add(user3);
            applicationContext.Users.Add(user4);
            applicationContext.Locations.Add(location1);
            applicationContext.Locations.Add(location2);
            applicationContext.Locations.Add(location3);
            applicationContext.Kingdoms.Add(kingdom1);
            applicationContext.Kingdoms.Add(kingdom2);
            applicationContext.Kingdoms.Add(kingdom3);
            applicationContext.Resources.Add(gold);
            applicationContext.Resources.Add(wood);
            applicationContext.Resources.Add(people);
            applicationContext.SaveChanges();
        }
    }
}
