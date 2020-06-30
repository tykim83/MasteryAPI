using AutoMapper;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MasteryAPI.BusinessLogic.Interfaces;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MasteryAPI.BusinessLogic.Models;

namespace MasteryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordManager recordManager;
        private readonly IMapper mapper;

        public RecordController(IRecordManager recordManager, IMapper mapper)
        {
            this.recordManager = recordManager;
            this.mapper = mapper;
        }

        #region Create Record Complete

        /// <summary>
        /// Create Complete Record
        /// </summary>
        /// <remarks>
        /// CategoryId must be set to a valid category for the user.
        ///
        /// TaskId can be left to 0 if the record is not part of a sub-category.
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("CreateComplete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status403Forbidden)]
        public ActionResult<RecordDTO> CreateRecord([FromBody]RecordCreationCompleteDTO recordCreationDTO)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = recordManager.CreateRecord(recordCreationDTO, email);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Category or Task with the Id provided do not exists for this user" });

                default:
                    return Ok(response.DTO);
            }
        }

        #endregion Create Record Complete

        #region Start Record

        /// <summary>
        /// Create and Start Record
        /// </summary>
        /// <remarks>
        /// CategoryId must be set to a valid category for the user.
        ///
        /// TaskId can be left to 0 if the record is not part of a sub-category.
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("Start")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status403Forbidden)]
        public ActionResult<RecordDTO> Start([FromBody]RecordCreationStartDTO recordCreationStartDTO)
        {
            var email = HttpContext.User.Identity.Name;
            RecordCreationStartBO recordCreationStartBO = mapper.Map<RecordCreationStartBO>(recordCreationStartDTO);
            recordCreationStartBO.UserEmail = email;
            BusinessLogicResponseDTO response = recordManager.StartRecord(recordCreationStartBO);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Category or Task with the Id provided do not exists for this user" });

                default:
                    return Ok(response.DTO);
            }
        }

        #endregion Start Record

        #region Stop Record

        /// <summary>
        /// Stop Record
        /// </summary>
        /// <remarks>
        /// The recordId must be a valid record for the user
        /// </remarks>
        /// <returns></returns>
        [HttpGet("{recordId}", Name = "Stop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status403Forbidden)]
        public ActionResult<RecordDTO> Stop(int recordId)
        {
            var email = HttpContext.User.Identity.Name;
            var response = recordManager.StopRecord(new StopRecordBO() { RecordId = recordId, Email = email });

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Record with the Id provided does not exists for this user" });

                case 406:
                    return BadRequest(new ErrorDTO { Message = "Record with the Id provided It is already completed" });

                default:
                    return Ok(response.DTO);
            }
        }

        #endregion Stop Record
    }
}