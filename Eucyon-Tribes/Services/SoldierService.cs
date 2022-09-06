using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.SoldierDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class SoldierService : ISoldierService
    {
        private readonly ApplicationContext _db;
        private readonly PurchaseService _purchaseService;
        private readonly ResourceFactory _resourceFactory;
        public string ErrorMessage;

        public SoldierService(ApplicationContext db, IPurchaseService purchaseService)
        {
            _db = db;
            _purchaseService = new PurchaseService(db);
            _resourceFactory = new ResourceFactory();
        }

        public bool CreateSoldiers(int id, CreateSoldiersDTO createSoldiersDTO)
        {
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Resources).FirstOrDefault(k => k.Id == id);
            _purchaseService.kingdom = kingdom;
            List<Soldier> soldiers = kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).Select(r => (Soldier)r).ToList();
            for (int i = 0; i < createSoldiersDTO.NumberOfUnitsByLevel.Count; i++)
            {
                if (i != 0)
                {
                    if (soldiers.Where(s => s.Level == i + 1 && s.FinishedAt < DateTime.Now).Count() < createSoldiersDTO.NumberOfUnitsByLevel[i])
                    {
                        ErrorMessage = "No enough units of level " + (i + 1);
                        return false;
                    }
                }
            }
            List<Soldier> soldiersToUpgrade = new List<Soldier>();
            for (int j = 0; j < createSoldiersDTO.NumberOfUnitsByLevel.Count; j++)
            {
                soldiersToUpgrade.AddRange(soldiers.Where(s => s.Level == j + 1 && s.FinishedAt < DateTime.Now).Take(createSoldiersDTO.NumberOfUnitsByLevel[j]));
            }
            if (!(_purchaseService.Gold().Amount >= 10 * createSoldiersDTO.NumberOfUnitsByLevel[0] + 10 * (soldiersToUpgrade.Sum(s => s.Level) + soldiersToUpgrade.Count())
                && _purchaseService.Food().Amount >= 5 * createSoldiersDTO.NumberOfUnitsByLevel[0] + 5 * (soldiersToUpgrade.Sum(s => s.Level) + soldiersToUpgrade.Count())
                && _purchaseService.People().Amount >= 1 * createSoldiersDTO.NumberOfUnitsByLevel[0]))
            {
                ErrorMessage = "Not enough resources";
                return false;
            }
            DateTime dateTime = DateTime.Now;
            _purchaseService.EnoughResourcesForUpgradingSoldier(soldiersToUpgrade, dateTime);
            for (int k = 0; k < createSoldiersDTO.NumberOfUnitsByLevel[0]; k++)
            {
                Soldier soldier = _resourceFactory.GetSoldierResource();
                soldier.StartedAt = dateTime;
                soldier.FinishedAt = dateTime.AddMinutes(30);
                kingdom.Resources.Add(soldier);
                _db.Resources.Add(soldier);
                _db.SaveChanges();
            }
            return true;
        }

        public string GetError()
        {
            string output = string.Empty;
            output = this.ErrorMessage;
            ErrorMessage = null;
            return output;
        }
    }
}
