using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            User user2 = new User();
            user2.Name = "User2";
            user2.PasswordHash = "Password2";
            user2.Email = "Email2";
            user2.VerificationToken = String.Empty;
            user2.ForgottenPasswordToken = String.Empty;
            User user3 = new User();
            user3.Name = "User3";
            user3.PasswordHash = "Password3";
            user3.Email = "Email3";
            user3.VerificationToken = String.Empty;
            user3.ForgottenPasswordToken = String.Empty;
            World world = new World();
            Location location = new Location();
            location.YCoordinate = 0;
            location.XCoordinate = 0;
            Kingdom kingdom = new Kingdom();
            location.Kingdom = kingdom;
            location.KingdomId = kingdom.Id;
            kingdom.World = world;
            kingdom.Name = "kingdom1";
            kingdom.WorldId = world.Id;
            kingdom.Location = location;
            kingdom.User = user1;
            kingdom.UserId = user1.Id;
            applicationContext.Worlds.Add(world);
            applicationContext.Users.Add(user1);
            applicationContext.Users.Add(user2);
            applicationContext.Users.Add(user3);
            applicationContext.Locations.Add(location);
            applicationContext.Kingdoms.Add(kingdom);
            applicationContext.SaveChanges();
        }
    }
}
