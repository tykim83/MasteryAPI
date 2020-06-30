using AutoMapper;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic
{
    public class TaskManager : ITaskManager
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public TaskManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public BusinessLogicResponseDTO CreateTask(TaskCreationDTO taskCreationDTO, string email)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == email).Id;

            //Invalid ID
            if (taskCreationDTO.CategoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == taskCreationDTO.CategoryId && c.UserId == userId);

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Add New Task
            Task task = new Task() { Name = taskCreationDTO.Name, CategoryId = taskCreationDTO.CategoryId };
            unitOfWork.Task.Add(task);
            unitOfWork.Save();

            response.DTO = mapper.Map<TaskDTO>(task);

            return response;
        }
    }
}