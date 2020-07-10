using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.BusinessLogic.Models;
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
        private readonly IMapper mapper;

        public CategoryController(ICategoryManager categoryManager, IMapper mapper)
        {
            this.categoryManager = categoryManager;
            this.mapper = mapper;
        }

        #region Get

        /// <summary>
        /// Get Category with the latest records - Pagination
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// CategoryId must be set to a valid category for the user.
        ///
        /// Response
        /// ---------------------------------------------------------------
        /// - Category
        /// - List Records with Task
        /// - TotalAmoutPages
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{categoryId:int}", Name = "Get")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryWithRecordsPaginationDTO> Get(int categoryId, [FromQuery] PaginationDTO pagination)
        {
            CategoryWithRecordAndPaginationBO categoryWithRecordAndPaginationBO = new CategoryWithRecordAndPaginationBO()
            {
                CategoryId = categoryId,
                UserEmail = HttpContext.User.Identity.Name,
                PaginationDTO = pagination
            };

            BusinessLogicResponseDTO businessLogicResponseDTO = categoryManager.GetWithPagination(categoryWithRecordAndPaginationBO);

            switch (businessLogicResponseDTO.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO() { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Category with the Id provided does not exists for the current user" });

                default:
                    return Ok(businessLogicResponseDTO.DTO);
            }
        }

        #endregion Get

        #region GetComplete

        /// <summary>
        /// Get Category with Tasks and Records
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// CategoryId must be set to a valid category for the user.
        ///
        /// Response
        /// ---------------------------------------------------------------
        /// Category with
        /// >List Tasks with
        /// >>List Records
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet("GetComplete/{categoryId:int}", Name = "GetComplete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        public ActionResult<CategoryWithTaskDTO> GetComplete(int categoryId)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO businessLogicResponseDTO = categoryManager.GetComplete(new CategoryIdBo() { CategoryId = categoryId, UserEmail = email });

            switch (businessLogicResponseDTO.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO() { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Category with the Id provided does not exists for the current user" });

                default:
                    return Ok(businessLogicResponseDTO.DTO);
            }
        }

        #endregion GetComplete

        #region GetAll

        /// <summary>
        /// Get Categories for the current user
        /// </summary>
        /// <remarks>
        /// List of Categories (Id, Name, Color, TotalDuration)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Create a new Category
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// Name (Required)
        /// Color (Optional)
        ///
        /// Response
        /// ---------------------------------------------------------------
        /// Category (Id, Name, Color, TotalTime)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CreateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<CategoryDTO> CreateCategory([FromBody]CategoryCreationDTO categoryCreationDTO)
        {
            CategoryCreationBO categoryCreationBO = mapper.Map<CategoryCreationBO>(categoryCreationDTO);
            categoryCreationBO.UserEmail = HttpContext.User.Identity.Name;

            CategoryDTO categoryDTO = categoryManager.CreateCategory(categoryCreationBO);

            return Ok(categoryDTO);
        }

        #endregion CreateCategory

        #region UpdateCategory

        /// <summary>
        /// Update a Category
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// Id (Required)<br></br>
        /// Name (Required)
        ///
        /// Response
        /// ---------------------------------------------------------------
        /// Category (Id, Name, Color, TotalTime)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<CategoryDTO> UpdateCategory([FromBody]CategoryUpdateDTO categoryUpdateDTO)
        {
            CategoryUpdateBO categoryUpdateBO = mapper.Map<CategoryUpdateBO>(categoryUpdateDTO);
            categoryUpdateBO.UserEmail = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = categoryManager.UpdateCategory(categoryUpdateBO);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Record with the Id provided does not exists" });

                default:
                    return Ok(response.DTO);
            }
        }

        #endregion UpdateCategory

        #region DeleteCategory

        /// <summary>
        /// Delete a Category
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// Id (Required)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{categoryId}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteCategory(int categoryId)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = categoryManager.DeleteCategory(new CategoryIdBo() { CategoryId = categoryId, UserEmail = email });

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Record with the Id provided does not exists" });

                case 200:
                default:
                    return Ok(new { message = "Category deleted successful" });
            }
        }

        #endregion DeleteCategory
    }
}