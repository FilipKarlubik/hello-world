﻿using Eucyon_Tribes.Models;
using Eucyon_Tribes.Models.DTOs;
using Eucyon_Tribes.Models.DTOs.BuildingDTOs;
using Eucyon_Tribes.Models.DTOs.SoldierDTOs;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("/api")]
    [ApiController]
    [Authorize(Roles = "Player, Admin")]
    public class BuildingRestController : ControllerBase
    {
        private IBuildingService _buildingService;
        private ISoldierService _soldierService;

        public BuildingRestController(IBuildingService buildingService, ISoldierService soldierService)
        {
            _buildingService = buildingService;
            _soldierService = soldierService;
        }

        [HttpGet("buildings")]
        public IActionResult Index([FromHeader] int userId)
        {
            var response = _buildingService.GetAllBuldingsOfKingdom(userId);

            if (response[0].Type.Equals("WrongUser"))
            {
                return BadRequest(new ErrorDTO("Invalid user"));
            }
            return Ok(response);
        }

        [HttpGet("buildings/{id}")]
        public IActionResult Show(int id, [FromHeader] int userId)
        {
            var response = _buildingService.GetBuildingDetails(id, userId);

            switch (response.Type)
            {
                case "WrongUser":
                    return BadRequest(new ErrorDTO("Invalid user"));
                case "WrongBuilding":
                    return BadRequest(new ErrorDTO("Invalid building"));
            }
            return Ok(response);
        }

        [HttpDelete("buildings/{id}")]
        public IActionResult Destroy(int id, [FromHeader] int userId)
        {
            var response = _buildingService.DeleteBuildingById(id, userId);

            switch (response.Status)
            {
                case "WrongUser":
                    return BadRequest(new ErrorDTO("Invalid user"));
                case "WrongBuilding":
                    return BadRequest(new ErrorDTO("Invalid building"));
            }
            return Ok(response);
        }

        [HttpPost("buildings")]
        public IActionResult Store(BuildingRequestDto buildingRequestDto, [FromHeader] int userId)
        {
            var response = _buildingService.StoreNewBulding(buildingRequestDto, userId);

            switch (response.Status)
            {
                case "WrongUser":
                    return BadRequest(new ErrorDTO("Invalid user"));
                case "FullKingdom":
                    return BadRequest(new ErrorDTO("This Kingdom is full"));
                case "WrongBuilding":
                    return BadRequest(new ErrorDTO("Invalid building"));
                case "NoResources":
                    return BadRequest(new ErrorDTO("Insufficient resources"));
            }
            return Ok(response);
        }

        [HttpGet("buildings/upgrade/{id}")]
        public IActionResult Upgrade(int id, [FromHeader] int userId)
        {
            var response = _buildingService.UpgradeBuilding(id, userId);

            switch (response.Status)
            {
                case "WrongUser":
                    return BadRequest(new ErrorDTO("Invalid user"));
                case "WrongBuilding":
                    return BadRequest(new ErrorDTO("Invalid building"));
                case "TownHallLowLevel":
                    return BadRequest(new ErrorDTO("Cannot upgrade. Building is at the same level as Townhall"));
                case "NoResources":
                    return BadRequest(new ErrorDTO("Insufficient resources"));
            }
            return Ok(response);
        }

        //id to be replaced with token auth
        [HttpPost("buildings/barracks/{id}")]
        public IActionResult CreateSoldier(int id, CreateSoldiersDTO createSoldiersDTO)
        {
            var response = _soldierService.CreateSoldiers(id,createSoldiersDTO);
            if (response == false) 
            {
                ErrorDTO error= new ErrorDTO(_soldierService.GetError());
                JsonResult jsonResult = new JsonResult(error);
                jsonResult.StatusCode = 400;
                return jsonResult;
            }
            return Ok(new StatusDTO("Unit construction issued"));
        }
    }
}