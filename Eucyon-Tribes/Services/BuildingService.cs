using Eucyon_Tribes.Context;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models.DTOs.BuildingDTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Eucyon_Tribes.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly ApplicationContext _db;
        private readonly BuildingFactory _buildingFactory;


        public BuildingService(ApplicationContext db)
        {
            _db = db;
            _buildingFactory = new BuildingFactory();
        }

        public void SaveBuilding(Building building)
        {
            _db.Buildings.Add(building);
            _db.SaveChanges();
        }

        public Building GetBuildingById(int id)
        {
            return _db.Buildings.FirstOrDefault(p => p.Id == id);
        }

        public List<BuildingsResponseDto> GetAllBuldingsOfKingdom(int userId)
        {
            List<BuildingsResponseDto> buildingsResponseDtos = new List<BuildingsResponseDto>();

            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == userId);
            if (kingdom == null)
            {
                var wrongUser = new BuildingsResponseDto("WrongUser");
                buildingsResponseDtos.Add(wrongUser);
                return buildingsResponseDtos;
            }
            var buildings = kingdom.Buildings;
            var resources = kingdom.Resources;

            for (int i = 0; i < buildings.Count; i++)
            {
                var resource = SetResourceType(userId, buildings[i].GetType().Name);

                var productionDto = new ProductionDto(
                    resource.Id,
                    resource.GetType().Name,
                    resource.Amount,
                    false,
                    DateTime.Today,
                    DateTime.Today);

                var buildingsResponseDto = new BuildingsResponseDto(
                    buildings[i].Id,
                    kingdom.Id,
                    buildings[i].GetType().Name,
                    buildings[i].Level,
                    productionDto);

                buildingsResponseDtos.Add(buildingsResponseDto);
            }
            return buildingsResponseDtos;
        }

        public Resource SetResourceType(int userId, string buldingType)
        {
            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == userId);
            var resources = kingdom.Resources;

            switch (buldingType)
            {
                case "TownHall":
                    return resources.FirstOrDefault(r => r.GetType().Name.Equals("People"));
                case "Barracks":
                    return resources.FirstOrDefault(r => r.GetType().Name.Equals("Soldier"));
                case "Farm":
                    return resources.FirstOrDefault(r => r.GetType().Name.Equals("Food"));
                case "Mine":
                    return resources.FirstOrDefault(r => r.GetType().Name.Equals("Gold"));
                case "Sawmill":
                    return resources.FirstOrDefault(r => r.GetType().Name.Equals("Wood"));
            }
            return null;
        }

        public BuildingResponseDto GetBuildingDetails(int id, int userId)
        {
            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == userId);
            if (kingdom == null)
            {
                var wrongUser = new BuildingResponseDto("WrongUser");
                return wrongUser;
            }
            var building = kingdom.Buildings.FirstOrDefault(p => p.Id == id);
            if (building == null)
            {
                var wrongBuilding = new BuildingResponseDto("WrongBuilding");
                return wrongBuilding;
            }

            var resource = SetResourceType(userId, building.GetType().Name);

            var productionDto = new ProductionDto(
                   resource.Id,
                   resource.GetType().Name,
                   resource.Amount,
                   false,
                   DateTime.Today,
                   DateTime.Today);

            var levelingCostDto = new LevelingCostDto(0, 0);

            var buildingResponseDto = new BuildingResponseDto(
                building.Id,
                kingdom.Id,
                building.GetType().Name,
                building.Level,
                productionDto,
                levelingCostDto);

            return buildingResponseDto;
        }

        public BuildingCreationResponseDto DeleteBuildingById(int id, int userId)
        {
            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == userId);
            if (kingdom == null)
            {
                return new BuildingCreationResponseDto("WrongUser");
            }
            var building = kingdom.Buildings.FirstOrDefault(p => p.Id == id);
            if (building == null)
            {
                return new BuildingCreationResponseDto("WrongBuilding");
            }
            _db.Buildings.Remove(building);
            _db.SaveChanges();
            return new BuildingCreationResponseDto("Deleted");
        }

        public BuildingCreationResponseDto StoreNewBulding(BuildingRequestDto buildingRequestDto, int userId)
        {
            var kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources).FirstOrDefault(p => p.UserId == userId);
            int id = buildingRequestDto.Building_type_id;

            if (kingdom == null)
            {
                return new BuildingCreationResponseDto("WrongUser");
            }
            if (kingdom.Buildings.Count >= 20)
            {
                return new BuildingCreationResponseDto("FullKingdom");
            }
            if (!KingdomHasResources())
            {
                return new BuildingCreationResponseDto("WrongResources");
            }
            if (id > 4 || id < 1)
            {
                return new BuildingCreationResponseDto("WrongBuilding");
            }

            var building = CreateRightBuilding(id);
            kingdom.Buildings.Add(building);
            _db.Buildings.Add(building);
            _db.SaveChanges();
            return new BuildingCreationResponseDto("Ok");
        }

        public Building CreateRightBuilding(int id)
        {
            switch (id)
            {
                case 1:
                    return _buildingFactory.CreateBarracks();
                case 2:
                    return _buildingFactory.CreateFarm();
                case 3:
                    return _buildingFactory.CreateMine();
                case 4:
                    return _buildingFactory.CreateSawMill();
            }
            return null;
        }

        public bool KingdomHasResources()
        {
            //Method body needed (Logic for creation and updating buildings not implemented yet)
            return true;
        }
    }
}