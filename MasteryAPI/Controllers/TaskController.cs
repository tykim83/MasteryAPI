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
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager taskManager;
        private readonly IMapper mapper;

        public TaskController(ITaskManager taskManager, IMapper mapper)
        {
            this.taskManager = taskManager;
            this.mapper = mapper;
        }

        #region CreateTask

        /// <summary>
        /// Create a Task
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// CategoryId (Required)<br></br>
        /// Name (Required)
        ///
        /// Response
        /// ---------------------------------------------------------------
        /// Category (Id, Name, TotalTime)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CreateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<TaskOnlyDTO> CreateTask([FromBody]TaskCreationDTO taskCreationDTO)
        {
            TaskCreationBo taskCreationBo = mapper.Map<TaskCreationBo>(taskCreationDTO);
            taskCreationBo.UserEmail = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO taskDTO = taskManager.CreateTask(taskCreationBo);

            switch (taskDTO.StatusCode)
            {
                case 400:
                    return BadRequest(new { message = "Invalid Id" });

                case 404:
                    return NotFound(new { message = "Category with the Id provided does not exists" });

                default:
                    return Ok(taskDTO.DTO);
            }
        }

        #endregion CreateTask

        #region DeleteTask

        /// <summary>
        /// Delete a Task
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// TaskId (Required)
        /// CategoryId (Required)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("DeleteTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult DeleteTask([FromBody]TaskDeleteDTO taskDeleteDTO)
        {
            TaskIdBo taskIdBo = mapper.Map<TaskIdBo>(taskDeleteDTO);
            taskIdBo.UserEmail = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = taskManager.DeleteTask(taskIdBo);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new ErrorDTO { Message = "Invalid Id" });

                case 404:
                    return NotFound(new ErrorDTO { Message = "Task or Record with the Id provided do not exist" });

                case 200:
                default:
                    return Ok(new { message = "Task deleted successful" });
            }
        }

        #endregion DeleteTask

        #region UpdateCategory

        /// <summary>
        /// Update a Task
        /// </summary>
        /// <remarks>
        /// Parameters
        /// --------------------------------------------------------------
        /// TaskId (Required)<br></br>
        /// CategoryId (Required)<br></br>
        /// Name (Required)
        ///
        /// Response
        /// ---------------------------------------------------------------
        /// Task (Id, Name, TotalTime)
        ///
        /// </remarks>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDTO), StatusCodes.Status401Unauthorized)]
        public ActionResult<TaskOnlyDTO> UpdateTask([FromBody]TaskUpdateDTO taskUpdateDTO)
        {
            TaskUpdateBO taskUpdateBO = mapper.Map<TaskUpdateBO>(taskUpdateDTO);
            taskUpdateBO.UserEmail = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO response = taskManager.UpdateTask(taskUpdateBO);

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
    }
}