using AutoMapper;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using System.Text;

namespace MasteryAPI.BusinessLogic
{
    public class RecordManager : IRecordManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public RecordManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public RecordDTO CreateRecord(RecordCreationCompleteDTO recordCreationCompleteDTO)
        {
            var record = mapper.Map<Record>(recordCreationCompleteDTO);

            record.TotalDuration = (record.Finished ?? DateTime.Now).Subtract(record.Started);
            record.IsCompleted = true;

            unitOfWork.Record.Add(record);
            unitOfWork.Save();

            return mapper.Map<RecordDTO>(record);
        }

        public RecordDTO StartRecord(RecordCreationStartDTO recordCreationStartDTO)
        {
            var record = mapper.Map<Record>(recordCreationStartDTO);

            record.IsCompleted = false;

            unitOfWork.Record.Add(record);
            unitOfWork.Save();

            return mapper.Map<RecordDTO>(record);
        }

        public BusinessLogicResponseDTO StopRecord(int recordId)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();

            //Invalid ID
            if (recordId == 0)
            {
                response.ErrorCode = 400;
                return response;
            }

            var recordFromDb = unitOfWork.Record.Get(recordId);

            //Record Null
            if (recordFromDb == null)
            {
                response.ErrorCode = 404;
                return response;
            }

            //Record is alredy Completed
            if (recordFromDb.IsCompleted == true)
            {
                response.ErrorCode = 406;
            }

            //Success - Record can be completed
            recordFromDb.Finished = DateTime.Now;
            recordFromDb.TotalDuration = (recordFromDb.Finished ?? DateTime.Now).Subtract(recordFromDb.Started);
            recordFromDb.IsCompleted = true;

            unitOfWork.Save();

            response.DTO = mapper.Map<RecordDTO>(recordFromDb);

            return response;
        }
    }
}