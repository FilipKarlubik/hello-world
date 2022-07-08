using Eucyon_Tribes.Context;
using Eucyon_Tribes.Factories;
using Eucyon_Tribes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;

namespace Eucyon_Tribes.Services
{
    public class KingdomService : IKingdomService
    {
        private readonly ApplicationContext _db;
        private readonly IKingdomFactory _kingdomFactory;
        public String ErrorMessage;
        public KingdomService(ApplicationContext db, IKingdomFactory kingdomFactory)
        {
            this._db = db;
            this._kingdomFactory = kingdomFactory;
        }

        public Boolean AddKingdom(CreateKingdomDTO createKingdom)
        {
            if (createKingdom.Name == null || createKingdom.Name.Equals(""))
            {
                ErrorMessage = "Invalid name";
                return false;
            }

            if (!_db.Worlds.Any(w => w.Id.Equals(createKingdom.WorldId)))
            {
                ErrorMessage = "Invalid world Id";
                return false;
            }
            if (_db.Kingdoms.Any(k => k.Name.Equals(createKingdom.Name)))
            {
                ErrorMessage = "Kingdom with this name already exists";
                return false;
            }
            if (_db.Kingdoms.Any(k => k.UserId.Equals(createKingdom.UserId)))
            {
                ErrorMessage = "User already has a kingdom";
                return false;
            }
            if (!_db.Users.Any(u => u.Id.Equals(createKingdom.UserId)))
            {
                ErrorMessage = "Invalid user Id";
                return false;
            }
            
            if (_kingdomFactory.CreateKingdom(_db.Users.FirstOrDefault(u => u.Id == createKingdom.UserId), createKingdom.Name, _db.Worlds.Include(w => w.Kingdoms).
                Include(w => w.Locations).FirstOrDefault(w => w.Id == createKingdom.WorldId)))
            {           
                return true;
            }
            else
            {
                ErrorMessage = "World is full";
                return false;
            }
        }

        public List<Kingdom> GetKingdomsWorld(World world)
        {
            return _db.Kingdoms.Where(k => k.WorldId == world.Id).ToList();
        }

        public KingdomsDTO[] GetKingdoms()
        {
            List<Kingdom> kingdoms = _db.Kingdoms.ToList();
            KingdomsDTO[] kingdomsDTO = new KingdomsDTO[kingdoms.Count];
            for (int i = 0; i < kingdoms.Count; i++)
            {
                KingdomsDTO kingdomDTO = new KingdomsDTO(kingdoms[i].Id, kingdoms[i].WorldId, kingdoms[i].UserId, kingdoms[i].Name);
                kingdomsDTO[i] = kingdomDTO;
            }
            return kingdomsDTO;
        }

        public KingdomDTO GetKindom(int id)
        {
            if (id < 0)
            {
                ErrorMessage = "Invalid kingdom Id";
                return null;
            }
            Kingdom kingdom = _db.Kingdoms.Include(k => k.Location).Where(k => k.Id == id).FirstOrDefault();
            if (kingdom == null)
            {
                ErrorMessage = "Kingdom with this Id doesn't exist";
                return null;
            }
            else
            {
                return new KingdomDTO(kingdom.Id, kingdom.WorldId, kingdom.UserId, kingdom.Location.XCoordinate, kingdom.Location.YCoordinate);
            }
        }

        public String GetError()
        {
            String output = ErrorMessage;
            ErrorMessage = null;
            return output;
        }

        public bool WorldExists(int worldId)
        {
            return _db.Worlds.Any(w => w.Id.Equals(worldId));
        }

        public KingdomCreateResponseDTO AddKingdomWithLocation(KingdomCreateRequestDTO request)
        {
            if (request.UserId < 1)
            {
                return new KingdomCreateResponseDTO(400, "Invalid UserID", true);
            }
            else if (!_db.Users.Any(u => u.Id.Equals(request.UserId)))
            {
                return new KingdomCreateResponseDTO(404, "User not found", true);
            }
            else if (_db.Kingdoms.Any(k => k.UserId.Equals(request.UserId)))
            {
                return new KingdomCreateResponseDTO(409, "User already has a kingdom", true);
            }
            else if (request.Name == null || request.Name.Equals(""))
            {
                return new KingdomCreateResponseDTO(400, "Invalid kingdom name", true);
            }
            else if (request.WorldId < 1)
            {
                return new KingdomCreateResponseDTO(400, "Invalid WorldID", true);
            }
            else if (!WorldExists(request.WorldId))
            {
                return new KingdomCreateResponseDTO(404, "World not found", true);
            }
            else if (request.Coordinate_Y >= 0 && request.Coordinate_Y < 16
                && request.Coordinate_X >= 0 && request.Coordinate_X < 16)
            {
                string message = _kingdomFactory.CreateKingdomWithCoordinates(request);
                if (message.Equals("Kingdom created"))
                {
                    return new KingdomCreateResponseDTO(201, message, false);
                }
                else
                {
                    return new KingdomCreateResponseDTO(409, message, true);
                }
            }
            else
            {
                return new KingdomCreateResponseDTO(400, "Invalid Coordinates", true);
            }
        }
    }
}