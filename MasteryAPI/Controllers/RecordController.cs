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
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IRecordManager recordManager;

        public RecordController(IUnitOfWork unitOfWork, IMapper mapper, IRecordManager recordManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.recordManager = recordManager;
        }

        [HttpPost("CreateComplete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecordDTO> CreateRecord([FromBody]RecordCreationCompleteDTO recordCreationDTO)
        {
            RecordDTO recordDTO = recordManager.CreateRecord(recordCreationDTO);
            return Ok(recordDTO);
        }

        [HttpPost("Start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecordDTO> Start([FromBody]RecordCreationStartDTO recordCreationStartDTO)
        {
            RecordDTO recordDTO = recordManager.StartRecord(recordCreationStartDTO);
            return Ok(recordDTO);
        }

        [HttpGet("{recordId}", Name = "Stop")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<RecordDTO> Stop(int recordId)
        {
            var response = recordManager.StopRecord(recordId);

            switch (response.ErrorCode)
            {
                case 400:
                    return BadRequest(new { error = "Invalid Id" });

                case 404:
                    return NotFound(new { error = "Record with the Id provided does not exists" });

                case 406:
                    return BadRequest(new { error = "Record with the Id provided It is already completed" });

                default:
                    return Ok(response.DTO);
            }
        }
    }
}