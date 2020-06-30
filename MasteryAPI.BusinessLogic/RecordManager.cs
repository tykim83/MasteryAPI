using AutoMapper;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BusinessLogicResponseDTO CreateRecord(RecordCreationCompleteDTO recordCreationCompleteDTO, string email)
        {
            BusinessLogicResponseDTO businessLogicResponseDTO = new BusinessLogicResponseDTO();
            Category categoryFromDB;
            Task taskFromDB;

            //Check if Category is 0
            if (recordCreationCompleteDTO.CategoryId == 0)
            {
                businessLogicResponseDTO.StatusCode = 400;
                return businessLogicResponseDTO;
            }

            //Check if Category Exists
            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == email).Id;
            categoryFromDB = unitOfWork.Category.GetFirstOrDefault(c => c.Id == recordCreationCompleteDTO.CategoryId && c.UserId == userId, includeProperties: "Tasks");
            if (categoryFromDB == null)
            {
                businessLogicResponseDTO.StatusCode = 404;
                return businessLogicResponseDTO;
            }

            //Check if Task Exists and Get the Active task
            if (recordCreationCompleteDTO.TaskId != 0)
            {
                taskFromDB = categoryFromDB.Tasks.FirstOrDefault(c => c.Id == recordCreationCompleteDTO.TaskId);
                if (taskFromDB == null)
                {
                    businessLogicResponseDTO.StatusCode = 404;
                    return businessLogicResponseDTO;
                }
            }
            else
            {
                taskFromDB = categoryFromDB.Tasks.FirstOrDefault(c => c.Name == "main");
            }

            var record = mapper.Map<Record>(recordCreationCompleteDTO);

            //Calculate total duration and add it to the task and category
            record.TotalDuration = (record.Finished ?? DateTime.Now).Subtract(record.Started);
            record.IsCompleted = true;
            record.TaskId = taskFromDB.Id;
            taskFromDB.TotalDuration += record.TotalDuration;
            categoryFromDB.TotalDuration += record.TotalDuration;

            unitOfWork.Record.Add(record);
            unitOfWork.Save();

            businessLogicResponseDTO.DTO = mapper.Map<RecordDTO>(record);
            return businessLogicResponseDTO;
        }

        public BusinessLogicResponseDTO StartRecord(RecordCreationStartBO recordCreationStartBO)
        {
            BusinessLogicResponseDTO businessLogicResponseDTO = new BusinessLogicResponseDTO();
            Category categoryFromDB;
            Task taskFromDB;

            //Check if Category is 0
            if (recordCreationStartBO.CategoryId == 0)
            {
                businessLogicResponseDTO.StatusCode = 400;
                return businessLogicResponseDTO;
            }

            //Check if Category Exists
            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == recordCreationStartBO.UserEmail).Id;
            categoryFromDB = unitOfWork.Category.GetFirstOrDefault(c => c.Id == recordCreationStartBO.CategoryId && c.UserId == userId, includeProperties: "Tasks");
            if (categoryFromDB == null)
            {
                businessLogicResponseDTO.StatusCode = 404;
                return businessLogicResponseDTO;
            }

            //Check if Task Exists and Get the Active task
            if (recordCreationStartBO.TaskId != 0)
            {
                taskFromDB = categoryFromDB.Tasks.FirstOrDefault(c => c.Id == recordCreationStartBO.TaskId);
                if (taskFromDB == null)
                {
                    businessLogicResponseDTO.StatusCode = 404;
                    return businessLogicResponseDTO;
                }
            }
            else
            {
                taskFromDB = categoryFromDB.Tasks.FirstOrDefault(c => c.Name == "main");
            }

            var record = mapper.Map<Record>(recordCreationStartBO);
            record.TaskId = taskFromDB.Id;
            record.IsCompleted = false;
            record.Started = DateTime.Now;

            unitOfWork.Record.Add(record);
            unitOfWork.Save();

            businessLogicResponseDTO.DTO = mapper.Map<RecordDTO>(record);
            return businessLogicResponseDTO;
        }

        public BusinessLogicResponseDTO StopRecord(StopRecordBO stopRecordBO)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();

            //Invalid ID
            if (stopRecordBO.RecordId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == stopRecordBO.Email).Id;
            var recordFromDb = unitOfWork.Record.GetFirstOrDefault(c => c.Id == stopRecordBO.RecordId, includeProperties: "Task");
            recordFromDb.Task.Category = unitOfWork.Category.Get(recordFromDb.Task.CategoryId);

            //Record Null
            if (recordFromDb == null || recordFromDb.Task.Category.UserId != userId)
            {
                response.StatusCode = 404;
                return response;
            }

            //Record is alredy Completed
            if (recordFromDb.IsCompleted == true)
            {
                response.StatusCode = 406;
            }

            //Success - Record can be completed
            recordFromDb.Finished = DateTime.Now;
            recordFromDb.TotalDuration = (recordFromDb.Finished ?? DateTime.Now).Subtract(recordFromDb.Started);
            recordFromDb.IsCompleted = true;

            recordFromDb.Task.TotalDuration += recordFromDb.TotalDuration;
            recordFromDb.Task.Category.TotalDuration += recordFromDb.TotalDuration;

            unitOfWork.Save();

            response.DTO = mapper.Map<RecordDTO>(recordFromDb);

            return response;
        }
    }
}