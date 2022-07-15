using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.ArmyDTOs;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/armies")]
    [ApiController]
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

        [HttpPost("{id}")]
        public IActionResult Store(CreateArmyDTO createArmyDTO,int id) 
        {
            //id in url later to be replaced by user verification
            if (_armyService.CreateArmy(createArmyDTO, id))
            {
                return Ok(new ResponseDTO("ok"));
            }
            else 
            {
                ErrorDTO error = new ErrorDTO(_armyService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error == "Unit does not belong to this kingdom" || error.Error == "Request contains duplicate soldiers")
                {
                    result.StatusCode = 403;
                }
                else
                {
                    result.StatusCode = 400;
                }
                return result;
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(UpdateArmyDTO updateArmyDTO, int id)
        {
            if (_armyService.UpdateArmy(id,updateArmyDTO))
            {
                return Ok(new ResponseDTO("ok"));
            }
            else
            {
                ErrorDTO error = new ErrorDTO(_armyService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error == "Unit does not belong to this kingdom" || error.Error == "Unit does not belong to this army" || 
                    error.Error == "Request contains duplicate soldiers")
                {
                    result.StatusCode = 403;
                }
                else
                {
                    result.StatusCode = 400;
                }
                return result;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (_armyService.RemoveArmy(id))
            {
                return Ok(new ResponseDTO("deleted"));
            }
            else
            {
                ErrorDTO error = new ErrorDTO(_armyService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error == "Army not found")
                {
                    result.StatusCode = 404;
                }
                if (error.Error == "Unauthorized")
                {
                    result.StatusCode = 403;
                }
                if(error.Error == "Cannot delete the kingdom's defense army" || error.Error == "Invalid id")
                {
                    result.StatusCode = 400;
                }
                return result;
            }
        }
    }
}
