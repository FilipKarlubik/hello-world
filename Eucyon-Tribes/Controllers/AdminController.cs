using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using Eucyon_Tribes.Models.Resources;
using Eucyon_Tribes.Models.UserModels;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;

namespace Eucyon_Tribes.Controllers
{
    [Route("admin")]
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly IKingdomService _kingdomServices;
        private readonly IUserService _userServices;
        private readonly IBuildingService _buildingService;
        private readonly IBuildingFactory _buildingFactory;
        private readonly IResourceFactory _resourceFactory;
        private readonly IArmyService _armyService;
        private readonly Random rand;
        private static string _message = "";

        public AdminController(IUserService userServices, ApplicationContext db
            , IKingdomService kingdomServices, IBuildingService buildingService
            , IBuildingFactory buildingFactory, IResourceFactory resourceFactory
            , IArmyService armyService)
        {
            _db = db;
            _kingdomServices = kingdomServices;
            _userServices = userServices;
            _buildingService = buildingService;
            _buildingFactory = buildingFactory;
            _resourceFactory = resourceFactory;
            _armyService = armyService;
            rand = new Random();
        }

        [HttpGet("")]
        public IActionResult Home()
        {
            List<World> worldList = _db.Worlds.ToList();
            ViewBag.Message = _message;
            _message = "";
            return View(worldList);
        }

        [HttpGet("user/login")]
        public IActionResult Login()
        {  
            return View();
        }

        [HttpPost("user/login")]
        public IActionResult LoginInput(string name, string password)
        {
            UserLoginDto login = new(name, password);
            _message = _userServices.Login(login);
            return RedirectToAction("ListAllUsers");
        }

        [HttpGet("user/list")]
        public IActionResult ListAllUsers(int id)
        {
            ViewBag.Worlds = _db.Worlds.Count();
            ViewBag.Kingdoms = _db.Kingdoms.Count();
            ViewBag.Users = _db.Users.Count();
            ViewBag.Id = id;
            ViewBag.Message = _message;
            _message = "";
            dynamic mymodel = new ExpandoObject();
            mymodel.Users = _userServices.UsersInfoDetailedForAdmin("guess_what");
            mymodel.Worlds = _db.Worlds.ToList();
            return View(mymodel);
        }

        [HttpGet("kingdomDetail/{id}")]
        public IActionResult KingdomDetail(int id)
        {
            Kingdom kingdom = _db.Kingdoms.Include(p => p.Buildings).Include(p => p.Resources)
                .Include(p => p.Location).Include(p => p.Armies).Where(p => p.Id == id).FirstOrDefault();
            ViewBag.User = _db.Users.FirstOrDefault(u => u.Id.Equals(kingdom.UserId)).Name;
            ViewBag.WorldId = kingdom.Location.WorldId;
            ViewBag.Message = _message;
            _message = "";
            int armyId = 0;
            if (_db.Armies.Any(a => a.KingdomId.Equals(id)))
            {
           
            armyId = _db.Armies.FirstOrDefault(a => a.KingdomId.Equals(id)).Id;
            }
            List<Soldier> soldiers = new List<Soldier>();
            foreach (Soldier soldier in _db.Resources.OfType<Soldier>())
            {
                if (soldier.ArmyId.Equals(armyId))
                    soldiers.Add(soldier);
            }
            ViewBag.Attack = soldiers.Sum(s => s.Attack);
            ViewBag.Defense = soldiers.Sum(s => s.Defense);
            ViewBag.HP = soldiers.Sum(s => s.CurrentHP);

            return View(kingdom);
        }

        [HttpGet("user/create")]
        public IActionResult CreateUser()
        {
            List<World> worlds = _db.Worlds.ToList();
            return View(worlds);
        }

        [HttpPost("user/create")]
        public IActionResult CreateUserInput(string name, string password, string email, string kingdomName, int worldId)
        {
            _message = _userServices.CreateUser(new UserCreateDto(name, password, email), kingdomName, worldId).ElementAt(0).Value;
            return RedirectToAction("ListAllUsers");
        }

        [HttpGet("user/delete")]
        public IActionResult DeleteUser()
        {
            return View();
        }

        [HttpPost("user/delete")]
        public IActionResult DeleteUserInput(string name, string password)
        {
            _message = _userServices.DeleteUser(name, password);
            return RedirectToAction("ListAllUsers");
        }

        [HttpGet("world/create")]
        public IActionResult WorldCreate()
        {
            _db.Worlds.Add(new World() { Name = "World " + _db.Worlds.Count()});
            _db.SaveChanges();
            _message = "New world number " + _db.Worlds.Count() + " created";
            return RedirectToAction("Home");
        }

        [HttpGet("worlds/delete/{id}")]
        public IActionResult WorldDelete(int id)
        {
            World world = _db.Worlds.FirstOrDefault(w => w.Id.Equals(id));
            try
            {
                _db.Worlds.Remove(world);
                _db.SaveChanges();
                _message = "World ID: " + id + " has been deleted";
            }
            catch (Exception)
            {
                _message = "You must remove all kingdoms before you can remove world";
            }
            return RedirectToAction("Home");
        }

        [HttpGet("delete/{id}")]
        public IActionResult DeleteUserByID(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id.Equals(id));
            _message = _userServices.DeleteUser(user.Name, user.PasswordHash);
            return RedirectToAction("ListAllUsers");
        }

        [HttpGet("delete/resource/{id}")]
        public IActionResult DeleteResource(int id)
        {
            Resource resource = _db.Resources.FirstOrDefault(r => r.Id.Equals(id));
            int kingdomId = resource.KingdomId;
            _db.Resources.Remove(resource);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpGet("delete/building/{id}")]
        public IActionResult DeleteBuilding(int id)
        {
            Building building = _db.Buildings.FirstOrDefault(b => b.Id.Equals(id));
            int kingdomId = building.KingdomId;
            _db.Buildings.Remove(building);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpGet("delete/army/{id}")]
        public IActionResult DeleteArmy(int id)
        {
            Army army = _db.Armies.FirstOrDefault(a => a.Id.Equals(id));
            int kingdomId = army.KingdomId;
            try
            {
                _db.Armies.Remove(army);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                _message = "First you must remove all soldiers from this army";
            }
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpGet("delete/kingdom/{id}")]
        public IActionResult DeleteKingdom(int id)
        {
            Kingdom kingdom = _db.Kingdoms.FirstOrDefault(k => k.Id.Equals(id));
            _db.Kingdoms.Remove(kingdom);
            _db.SaveChanges();
            return RedirectToAction("ListAllUsers");
        }

        [HttpPost("kingdom/create/{id}")]
        public IActionResult CreateKingdom(int id, string name, int worldId)
        {
            if (worldId == 0)
            {
                Random rand = new Random();
                int total = _db.Worlds.Count();
                worldId = _db.Worlds.Skip(rand.Next(total)).First().Id;
            }
            CreateKingdomDTO kingdom = new(id, worldId, name);
            if (_kingdomServices.AddKingdom(kingdom)) _message = "Kingdom created";
            else _message = _kingdomServices.GetError();
            return RedirectToAction("ListAllUsers");
        }

        [HttpGet("worlds/show/{id}")]
        public IActionResult ShowWorldByID(int id)
        {
            List<Location> locations = _db.Locations.Include(l => l.Kingdom).Where(l => l.WorldId == id).ToList();
            ViewBag.WorldId = id;
            return View(locations);
        }

        [HttpPost("set/resource/{id}/{kingdomId}")]
        public IActionResult SetResourceByID(int id, int kingdomId, int amount)
        {
            Resource resource = _db.Resources.FirstOrDefault(r => r.Id.Equals(id));
            resource.Amount = amount;
            _db.Resources.Update(resource);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpPost("create/building/{kingdomId}")]
        public IActionResult CreateBuilding(int kingdomId, int buildingId)
        {
            Building building;
            if (buildingId == 0) building = _buildingFactory.CreateTownHall();
            else building = _buildingService.CreateRightBuilding(buildingId);
            building.KingdomId = kingdomId;
            _db.Buildings.Add(building);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpGet("soldier/add/{kingdomId}")]
        public IActionResult AddSoldierToKingdomById(int kingdomId)
        {
            var soldier = _resourceFactory.GetSoldierResource();
            soldier.KingdomId = kingdomId;
            List<int> armyIds = new();
            if (!_db.Armies.Include(a => a.Kingdom).Any(a => a.Kingdom.Id.Equals(kingdomId)))
            {
                Army army = new();
                army.KingdomId = kingdomId;
                _db.Armies.Add(army);
                _db.SaveChanges();
            }
            _db.Armies.Where(a => a.KingdomId.Equals(kingdomId)).ToList().ForEach(a => armyIds.Add(a.Id));
            var armyId = armyIds[rand.Next(0, armyIds.Count())];
            soldier.ArmyId = armyId;
            _db.Resources.Add(soldier);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpGet("army/add/{kingdomId}")]
        public IActionResult AddArmyToKingdomById(int kingdomId)
        {
            Army army = new();
            army.KingdomId = kingdomId;
            _db.Armies.Add(army);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }

        [HttpGet("delete/soldier/{id}/{kingdomId}")]
        public IActionResult DeleteSoldierById(int id, int kingdomId)
        {
            Soldier soldier = _db.Resources.Cast<Soldier>().FirstOrDefault(s => s.Id.Equals(id));
            var armyId = soldier.ArmyId;
            _db.Resources.Remove(soldier);
            _db.SaveChanges();
            return RedirectToAction("KingdomDetail", new { id = kingdomId });
        }
    }
}
