using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tribes.Tests.SeedData
{
    internal class BuildingControllerTestsSeedData
    {
        public static void PopulateTestData(ApplicationContext _db)
        {
            _db.Database.EnsureDeleted();
            _db.Database.EnsureCreated();

            var user = new User() { Email = "john@john.com", PasswordHash = "Johny123", Name = "John", ForgottenPasswordToken = String.Empty, VerificationToken = String.Empty };
            _db.Users.Add(user);

            var kingdom = new Kingdom() { Name = "Aden" };
            _db.Kingdoms.Add(kingdom);

            user.Kingdom = kingdom;

            var mine = new Mine()
            {
                Level = 1,
                Hp = 100,
                StartedAt = DateTime.Today,
                Production = 7,
            };
            var sawmill = new Sawmill()
            {
                Level = 1,
                Hp = 75,
                StartedAt = DateTime.Today,
                Production = 15,
            };
            _db.Buildings.Add(mine);
            _db.Buildings.Add(sawmill);

            var gold = new Gold();
            var wood = new Wood();
            _db.Resources.Add(gold);
            _db.Resources.Add(wood);

            kingdom.Buildings = new List<Building>();
            kingdom.Buildings.Add(mine);
            kingdom.Buildings.Add(sawmill);
            kingdom.Resources = new List<Resource>();
            kingdom.Resources.Add(gold);
            kingdom.Resources.Add(wood);
            _db.SaveChanges();
        }
    }
}