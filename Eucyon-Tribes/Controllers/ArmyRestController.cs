using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/armies")]
    [ApiController]
    [Authorize(Roles = "Player, Admin")]
    public class ArmyRestController : ControllerBase
    {
        private readonly IArmyService _armyService;

        public ArmyRestController(IArmyService armyService)
        {
            _armyService = armyService;
        }

        [HttpGet("{id}")]
        public IActionResult Show(int id)
        {
            ArmyDTO armyDTO = _armyService.GetArmy(id);
            if (armyDTO == null)
            {
                ErrorDTO error = new ErrorDTO(_armyService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error == "Invalid id")
                {
                    result.StatusCode = 400;
                }
                else
                {
                    result.StatusCode = 404;
                }
                return result;
            }
            else
            {
                return Ok(armyDTO);
            }
        }

        [HttpGet("kingdom/{id}")]
        public IActionResult Index(int id)
        {
            //this one is pretty barebones because url parameter here will be replaced by some sort of user verification, so there will be no need 
            // to check for further errors
            ArmyDTO[] armies = _armyService.GetArmies(id);
            return Ok(armies);
        }

        [HttpGet("kingdom/availableunits/{id}")]
        public IActionResult AvailableUnits(int id)
        {
            //id in param to be replacet by token auth
            AvailableUnitsDTO availableUnits = _armyService.GetAvailableUnits(id);
            return Ok(availableUnits);
        }

        [HttpGet("kingdom/unitsunderconstruction/{id}")]
        public IActionResult UnitsUnderConstruction(int id)
        {
            //id in param to be replacet by token auth
            UnitsUnderConstructionDTO[] unitsUnderConstruction = _armyService.UnitsUnderConstruction(id);
            return Ok(unitsUnderConstruction);
        }
    }
}
