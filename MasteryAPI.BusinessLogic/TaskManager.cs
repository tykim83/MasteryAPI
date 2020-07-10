using AutoMapper;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public BusinessLogicResponseDTO CreateTask(TaskCreationBo taskCreationBo)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == taskCreationBo.UserEmail).Id;

            //Invalid ID
            if (taskCreationBo.CategoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == taskCreationBo.CategoryId && c.UserId == userId);

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Add New Task
            Task task = new Task() { Name = taskCreationBo.Name, CategoryId = taskCreationBo.CategoryId };
            unitOfWork.Task.Add(task);
            unitOfWork.Save();

            response.DTO = mapper.Map<TaskOnlyDTO>(task);

            return response;
        }

        public BusinessLogicResponseDTO DeleteTask(TaskIdBo taskIdBo)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            string userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == taskIdBo.UserEmail).Id;

            //Invalid ID
            if (taskIdBo.CategoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == taskIdBo.CategoryId && c.UserId == userId, includeProperties: "Tasks");

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            Task taskFromDb = categoryFromDb.Tasks.FirstOrDefault(c => c.Id == taskIdBo.TaskId);

            //Task is Null
            if (taskFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Success - Delete Category
            unitOfWork.Task.Remove(taskFromDb);
            unitOfWork.Save();

            response.StatusCode = 200;

            return response;
        }

        public BusinessLogicResponseDTO Get(TaskIdBo taskIdBo)
        {
            throw new NotImplementedException();
        }

        public BusinessLogicResponseDTO GetAll(TaskIdBo taskIdBo)
        {
            throw new NotImplementedException();
        }

        public BusinessLogicResponseDTO UpdateTask(TaskUpdateBO taskUpdateBO)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            string userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == taskUpdateBO.UserEmail).Id;

            //Invalid ID
            if (taskUpdateBO.CategoryId == 0 || taskUpdateBO.TaskId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == taskUpdateBO.CategoryId && c.UserId == userId, includeProperties: "Tasks");

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            Task taskFromDb = categoryFromDb.Tasks.FirstOrDefault(c => c.Id == taskUpdateBO.TaskId);

            //Task is Null
            if (taskFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Success - Update Category Name
            taskFromDb.Name = taskUpdateBO.Name;
            unitOfWork.Save();

            response.DTO = mapper.Map<TaskOnlyDTO>(taskFromDb);

            return response;
        }
    }
}