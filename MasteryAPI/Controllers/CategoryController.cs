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

        #region Get

        [HttpGet("{categoryId}", Name = "Get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryWithTaskDTO> Get(int categoryId)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO businessLogicResponseDTO = categoryManager.Get(categoryId, email);

            switch (businessLogicResponseDTO.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO() { Message = "Invalid Id" });

                case 404:
                    return NotFound(new { message = "Category with the Id provided does not exists" });

                default:
                    return Ok(businessLogicResponseDTO.DTO);
            }
        }

        #endregion Get

        #region GetAll

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<List<CategoryDTO>> GetAll()
        {
            var email = HttpContext.User.Identity.Name;

            List<CategoryDTO> categories = categoryManager.GetAll(email);

            if (categories.Count == 0)
            {
                return NoContent();
            }

            return Ok(categories);
        }

        #endregion GetAll

        #region CreateCategory

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

        #endregion CreateCategory

        #region UpdateCategory

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

        #endregion UpdateCategory

        #region DeleteCategory

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

        #endregion DeleteCategory
    }
}