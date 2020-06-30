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
    public class TaskController : ControllerBase
    {
        private readonly ITaskManager taskManager;

        public TaskController(ITaskManager taskManager)
        {
            this.taskManager = taskManager;
        }

        #region CreateTask

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("CreateTask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<TaskDTO> CreateTask([FromBody]TaskCreationDTO taskCreationDTO)
        {
            var email = HttpContext.User.Identity.Name;

            BusinessLogicResponseDTO taskDTO = taskManager.CreateTask(taskCreationDTO, email);

            return Ok(taskDTO);
        }

        #endregion CreateTask
    }
}