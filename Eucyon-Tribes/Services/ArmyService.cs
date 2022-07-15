using Eucyon_Tribes.Config;
using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Eucyon_Tribes.Services
{
    public class ArmyService : IArmyService
    {
        private readonly IArmyFactory _armyFactory;
        private readonly ApplicationContext _db;
        public String ErrorMessage { get; private set; }
        private readonly IConfiguration _config;
        public int ArmySizeLimit { get; }

        public ArmyService(IArmyFactory armyFactory, ApplicationContext db, IConfiguration configuration)
        {
            this._db = db;
            this._armyFactory = armyFactory;
            this._config = configuration;
            ArmySizeLimit = int.Parse(_config["ARMY_SIZE_LIMIT"]);
        }

        //kingdom id later to be replaced by some sort of user verification
        public ArmyDTO[] GetArmies(int kingdomId)
        {
            Kingdom kingdom = _db.Kingdoms.FirstOrDefault(k => k.Id == kingdomId);
            List<Army> armies = _db.Armies.Include(a => a.Soldiers).Where(a => a.Kingdom == kingdom).ToList();
            ArmyDTO[] armyDTOs = new ArmyDTO[armies.Count()];
            for (int i = 0; i < armies.Count; i++)
            {
                List<SoldierDTO> soldiers = new List<SoldierDTO>();
                for (int j = 0; j < armies[i].Soldiers.Count; j++)
                {
                    SoldierDTO soldierDTO = new SoldierDTO(armies[i].Soldiers[j].Id, armies[i].Soldiers[j].TotalHP, armies[i].Soldiers[j].CurrentHP);
                    soldiers.Add(soldierDTO);
                }
                ArmyDTO armyDTO = new ArmyDTO(armies[i].Id, kingdom.Id, armies[i].Type, soldiers);
                armyDTOs[i] = armyDTO;
            }
            return armyDTOs;
        }

        public ArmyDTO GetArmy(int armyId)
        {
            if (armyId < 0)
            {
                ErrorMessage = "Invalid id";
                return null;
            }
            Army army = _db.Armies.Include(a=>a.Soldiers).FirstOrDefault(k => k.Id == armyId);
            if (army == null)
            {
                ErrorMessage = "Army not found";
                return null;
            }
            List<SoldierDTO> soldiers = new List<SoldierDTO>();
            for (int j = 0; j < army.Soldiers.Count; j++)
            {
                SoldierDTO soldierDTO = new SoldierDTO(army.Soldiers[j].Id, army.Soldiers[j].TotalHP, army.Soldiers[j].CurrentHP);
                soldiers.Add(soldierDTO);
            }
            ArmyDTO armyDTO = new ArmyDTO(army.Id, armyId, army.Type, soldiers);
            return armyDTO;
        }

        //kingdom id later to be replaced by some sort of user verification
        public Boolean CreateArmy(CreateArmyDTO createArmyDTO, int kingdomId)
        {
            if (createArmyDTO.Units.Count() != createArmyDTO.Units.Distinct().Count())
            {
                ErrorMessage = "Request contains duplicate soldiers";
                return false;
            }
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Resources).Include(k=>k.Armies).Where(k => k.Id == kingdomId).First();
            List<Soldier> soldiers = kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).Select(s => (Soldier)s).ToList();
            if (createArmyDTO.Units.Any(u => soldiers.All(s => s.Id != u)))
            {
                ErrorMessage = "Unit does not belong to this kingdom";
                return false;
            }
            if (createArmyDTO.Units.Count > ArmySizeLimit)
            {
                ErrorMessage = "Maximum number of units reached";
                return false;
            }
            List<Soldier> soldiersToAdd = new List<Soldier>();
            foreach (int soldier in createArmyDTO.Units)
            {
                soldiersToAdd.Add(soldiers.Where(s => s.Id == soldier).First());
            }
            Army army = _armyFactory.CrateArmy(soldiersToAdd, kingdom);
            kingdom.Armies.Add(army);
            _db.Armies.Add(army);
            _db.SaveChanges();
            return true;
        }

        public Boolean UpdateArmy(int armyId, UpdateArmyDTO update)
        {
            Army army = _db.Armies.Include(a => a.Kingdom).ThenInclude(k=>k.Resources).Include(a => a.Soldiers).FirstOrDefault(a => a.Id == armyId);
            if (update.Add.Count() != update.Add.Distinct().Count() || update.Remove.Count() != update.Remove.Distinct().Count()) 
            {
                ErrorMessage = "Request contains duplicate soldiers";
                return false;
            }
            List<Soldier> soldiers = army.Kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).Select(s => (Soldier)s).ToList();            
            if (update.Add.Any(u => soldiers.All(s => s.Id != u))
                || update.Remove.Any((u => soldiers.All(s => s.Id != u))))
            {
                ErrorMessage = "Unit does not belong to this kingdom";
                return false;
            }
            if (update.Remove.Any((u => army.Soldiers.All(s => s.Id != u))))
            {
                ErrorMessage = "Unit does not belong to this army";
                return false;
            }
            List<Soldier> newArmy = new List<Soldier>();
            newArmy.AddRange(army.Soldiers);
            foreach (int soldierId in update.Add)
            {
                Soldier soldier = soldiers.FirstOrDefault(s => s.Id == soldierId);
                if (!newArmy.Contains(soldier))
                    newArmy.Add(soldier);
            }
            foreach (int soldierId in update.Remove)
            {
                Soldier soldier= soldiers.FirstOrDefault(s => s.Id == soldierId);
                if (newArmy.Contains(soldier))
                    newArmy.Remove(soldier);
            }
            if (newArmy.Count > ArmySizeLimit)
            {
                ErrorMessage = "Maximum number of units reached";
                return false;
            }
            else 
            {
                army.Soldiers = newArmy;
            }
            _db.SaveChanges();
            return true;
        }

        public Boolean RemoveArmy(int armyId)
        {
            if (armyId < 0)
            {
                ErrorMessage = "Invalid id";
                return false;
            }
            Army army = _db.Armies.Include(a => a.Soldiers).FirstOrDefault(k => k.Id == armyId);
            if (army == null)
            {
                ErrorMessage = "Army not found";
                return false;
            }
            /* if (validationLogic false) 
             {
                 ErrorMessage = "Unauthorized";
                 return false;
             }*/
            if (_db.Armies.FirstOrDefault(a => a.Id == armyId).Type == "Defense")
            {
                ErrorMessage = "Cannot delete the kingdom's defense army";
                return false;
            }
            foreach (Soldier soldier in army.Soldiers)
            {
                soldier.Army = null;
            }
            _db.Armies.Remove(army);
            _db.SaveChanges();
            return true;
        }

        public String GetError() 
        {
            String output = this.ErrorMessage;
            this.ErrorMessage = null;
            return output;
        }
    }
}
