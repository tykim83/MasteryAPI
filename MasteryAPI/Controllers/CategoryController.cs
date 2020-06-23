using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MasteryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryManager categoryManager;

        public CategoryController(ICategoryManager categoryManager)
        {
            this.categoryManager = categoryManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CreateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<CategoryDTO> CreateCategory([FromBody]CategoryCreationDTO categoryCreationDTO)
        {
            var email = HttpContext.User.Identity.Name;

            CategoryDTO categoryDTO = categoryManager.CreateCategory(categoryCreationDTO, email);

            return Ok(categoryDTO);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<CategoryDTO> UpdateCategory([FromBody]CategoryUpdateDTO categoryUpdateDTO)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = categoryManager.UpdateCategory(categoryUpdateDTO, email);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new { message = "Invalid Id" });

                case 404:
                    return NotFound(new { message = "Record with the Id provided does not exists" });

                default:
                    return Ok(response.DTO);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{categoryId}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteCategory(int categoryId)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = categoryManager.DeleteCategory(categoryId, email);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new { message = "Invalid Id" });

                case 404:
                    return NotFound(new { message = "Record with the Id provided does not exists" });

                case 200:
                default:
                    return Ok(new { message = "Category deleted successful" });
            }
        }
    }
}