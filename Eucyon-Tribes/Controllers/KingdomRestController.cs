using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.KingdomDTOs;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/kingdomrestcontroller")]
    [ApiController]
    public class KingdomRestController : ControllerBase
    {
        private readonly IKingdomService _kingdomService;
        public KingdomRestController(IKingdomService kingdomService)
        {
            _kingdomService = kingdomService;
        }

        [HttpGet("kingdoms")]
        public IActionResult Index()
        {
            KingdomsDTO[] kingdoms = _kingdomService.GetKingdoms();
            return Ok(kingdoms);
        }

        [HttpGet("kingdoms/{id}")]
        public IActionResult Show(int id)
        {
            KingdomDTO kingdomDTO = _kingdomService.GetKindom(id);
            if (kingdomDTO == null)
            {
                ErrorDTO error = new ErrorDTO(_kingdomService.GetError());
                JsonResult result = new JsonResult(error);
                if (error.Error.Equals("Invalid kingdom Id"))
                    result.StatusCode = 400;
                else
                    result.StatusCode = 404;
                return result;
            }
            else
            {
                return Ok(kingdomDTO);
            }
        }

        [HttpPost("kingdoms")]
        public IActionResult Store(CreateKingdomDTO createKingdomDTO)
        {
            Boolean boolean = _kingdomService.AddKingdom(createKingdomDTO);
            if (boolean)
            {
                return Ok();
            }
            else
            {
                ErrorDTO error = new ErrorDTO(_kingdomService.GetError());
                JsonResult result = new JsonResult(error);
                result.StatusCode = 400;
                return result;
            }
        }

        [HttpPost("kingdoms/with_location")]
        public IActionResult StoreWithLocation(KingdomCreateRequestDTO request)
        {
            KingdomCreateResponseDTO response = _kingdomService.AddKingdomWithLocation(request);
            if (response.Error)
            {
                return StatusCode(response.StatusCode, new ErrorDTO(response.Message));
            }
            else
            {
                return StatusCode(response.StatusCode, new StatusDTO(response.Message));
            }      
        }
    }
}
