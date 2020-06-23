using AutoMapper;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MasteryAPI.BusinessLogic.Interfaces;
using System;

namespace MasteryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly IRecordManager recordManager;

        public RecordController(IRecordManager recordManager)
        {
            this.recordManager = recordManager;
        }

        #region Create Record Complete

        [HttpPost("CreateComplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecordDTO> CreateRecord([FromBody]RecordCreationCompleteDTO recordCreationDTO)
        {
            RecordDTO recordDTO = recordManager.CreateRecord(recordCreationDTO);
            return Ok(recordDTO);
        }

        #endregion Create Record Complete

        #region Start Record

        [HttpPost("Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecordDTO> Start([FromBody]RecordCreationStartDTO recordCreationStartDTO)
        {
            RecordDTO recordDTO = recordManager.StartRecord(recordCreationStartDTO);
            return Ok(recordDTO);
        }

        #endregion Start Record

        #region Stop Record

        [HttpGet("{recordId}", Name = "Stop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecordDTO> Stop(int recordId)
        {
            var response = recordManager.StopRecord(recordId);

            switch (response.StatusCode)
            {
                case 400:
                    return BadRequest(new { message = "Invalid Id" });

                case 404:
                    return NotFound(new { message = "Record with the Id provided does not exists" });

                case 406:
                    return BadRequest(new { message = "Record with the Id provided It is already completed" });

                default:
                    return Ok(response.DTO);
            }
        }

        #endregion Stop Record
    }
}