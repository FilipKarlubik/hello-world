using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.Buildings;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.Resources;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class ResourceService : IResourceService
    {
        private readonly ApplicationContext _db;

        public ResourceService(ApplicationContext context)
        {
            this._db = context;
        }

        public void UpdateResource()
        {
            var kingdoms = _db.Kingdoms.Include(k => k.Resources).Include(k => k.Buildings);
            if (kingdoms == null)
            {
                return;
            }

            foreach (var kingdom in kingdoms)
            {
                DateTime newUpdateAt = DateTime.Now;
                UpdateFoodResource(kingdom, newUpdateAt);
                UpdateGoldResource(kingdom, newUpdateAt);
                UpdatePeopleResource(kingdom, newUpdateAt);
                UpdateWoodResource(kingdom, newUpdateAt);
            }
            _db.SaveChanges();
        }

        public void UpdateWoodResource(Kingdom kingdom, DateTime updateTime)
        {
            var woodProduction = kingdom.Buildings.Where(b => b is Sawmill && b.FinishedAt < DateTime.Now).Sum(b => b.Production);
            Resource wood = kingdom.Resources.FirstOrDefault(r => r is Wood);
            double timeDifferenceInMinutes = updateTime.Subtract(wood.UpdatedAt).TotalMinutes;
            wood.Amount += (int)(woodProduction * timeDifferenceInMinutes);
            wood.UpdatedAt = updateTime;
        }

        public void UpdateGoldResource(Kingdom kingdom, DateTime updateTime)
        {
            var goldProduction = kingdom.Buildings.Where(b => b is Mine && b.FinishedAt < DateTime.Now).Sum(b => b.Production);
            Resource gold = kingdom.Resources.FirstOrDefault(r => r is Gold);
            double timeDifferenceInMinutes = updateTime.Subtract(gold.UpdatedAt).TotalMinutes;
            gold.Amount += (int)(goldProduction * timeDifferenceInMinutes);
            gold.UpdatedAt = updateTime;
        }

        public void UpdatePeopleResource(Kingdom kingdom, DateTime updateTime)
        {
            var peopleProduction = kingdom.Buildings.Where(b => b is TownHall && b.FinishedAt < DateTime.Now).Sum(b => b.Production);
            Resource people = kingdom.Resources.FirstOrDefault(r => r is People);
            double timeDifferenceInMinutes = updateTime.Subtract(people.UpdatedAt).TotalMinutes;
            people.Amount += (int)(peopleProduction * timeDifferenceInMinutes);
            people.UpdatedAt = updateTime;
        }

        public void UpdateFoodResource(Kingdom kingdom, DateTime updateTime)
        {
            var foodProduction = kingdom.Buildings.Where(b => b is Farm && b.FinishedAt < DateTime.Now).Sum(b => b.Production);
            Resource food = kingdom.Resources.FirstOrDefault(r => r is Food);
            double timeDifferenceInMinutes = updateTime.Subtract(food.UpdatedAt).TotalMinutes;
            int soldierAmount = kingdom.Resources.Where(r => r is Soldier).Count();
            food.Amount += (int)((foodProduction - soldierAmount) * timeDifferenceInMinutes);
            food.UpdatedAt = updateTime;
        }

        public ResourcesDTO UpdateResourceKingdom(int id)
        {
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Resources).Include(k => k.Buildings).FirstOrDefault(k => k.Id == id);
            if (kingdom == null)
            {
                return null;
            }
            DateTime newUpdateAt = DateTime.Now;
            UpdateFoodResource(kingdom, newUpdateAt);
            UpdateGoldResource(kingdom, newUpdateAt);
            UpdatePeopleResource(kingdom, newUpdateAt);
            UpdateWoodResource(kingdom, newUpdateAt);
            _db.SaveChanges();
            List<ResourceDTO> resourceDTOs = new List<ResourceDTO>();
            foreach (Resource resource in kingdom.Resources)
            {
                if (resource.GetType() != typeof(Soldier))
                {
                    resourceDTOs.Add(new ResourceDTO(resource.GetType().ToString().Substring(31), resource.Amount));
                }
            }
            return new ResourcesDTO(id, kingdom.Name, resourceDTOs);
        }

        public void OpenConnection()
        {
            _db.Database.OpenConnection();
        }

        public void FamineCheck()
        {
            var kingdoms = _db.Kingdoms.Include(k => k.Resources).ToList();
            foreach (Kingdom kingdom in kingdoms)
            {
                if (kingdom.Resources.Where(r => r.GetType() == typeof(Food)).FirstOrDefault().Amount < 0) 
                {
                    List<Soldier> soldiers = kingdom.Resources.Where(r => r.GetType() == typeof(Soldier)).Select(r=>(Soldier)r).ToList();
                    Soldier soldier = soldiers.FirstOrDefault(s => s.Army == null);
                    kingdom.Resources.Remove(soldier);
                    _db.Remove(soldier);
                    soldier = null;
                    kingdom.Resources.FirstOrDefault(r => r.GetType() == typeof(Food)).Amount =
                        kingdom.Resources.FirstOrDefault(r => r.GetType() == typeof(Food)).Amount + 50;
                    _db.SaveChanges();
                }
            }
        }
    }
}