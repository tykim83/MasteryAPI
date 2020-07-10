using AutoMapper;
using MasteryAPI.BusinessLogic.Interfaces;
using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using MasteryAPI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasteryAPI.BusinessLogic
{
    public class CategoryManager : ICategoryManager
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CategoryManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public BusinessLogicResponseDTO GetWithPagination(CategoryWithRecordAndPaginationBO categoryWithRecordAndPaginationBO)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == categoryWithRecordAndPaginationBO.UserEmail).Id;

            //Invalid ID
            if (categoryWithRecordAndPaginationBO.CategoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryWithRecordAndPaginationBO.CategoryId && c.UserId == userId, includeProperties: "Tasks");

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            List<int> tasksId = categoryFromDb.Tasks.Select(c => c.Id).ToList();
            IEnumerable<Record> records = unitOfWork.Record.GetAll(c => tasksId.Contains(c.TaskId), orderBy: x => x.OrderBy(s => s.Started));

            CategoryWithRecordsPaginationDTO categoryWithRecordsPaginationDTO = new CategoryWithRecordsPaginationDTO();
            categoryWithRecordsPaginationDTO = mapper.Map<CategoryWithRecordsPaginationDTO>(categoryFromDb);
            double count = records.Count();
            categoryWithRecordsPaginationDTO.TotalAmountPages = Math.Ceiling(count / categoryWithRecordAndPaginationBO.PaginationDTO.RecordsPerPage);
            categoryWithRecordsPaginationDTO.Records = mapper.Map<List<RecordPaginationDTO>>(records.Paginate(categoryWithRecordAndPaginationBO.PaginationDTO));

            response.DTO = categoryWithRecordsPaginationDTO;
            return response;
        }

        public BusinessLogicResponseDTO GetComplete(CategoryIdBo categoryIdBo)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            var userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == categoryIdBo.UserEmail).Id;

            //Invalid ID
            if (categoryIdBo.CategoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryIdBo.CategoryId && c.UserId == userId, includeProperties: "Tasks");

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            foreach (var task in categoryFromDb.Tasks)
            {
                categoryFromDb.Tasks.FirstOrDefault(c => c.Id == task.Id).Records = unitOfWork.Record.GetAll(c => c.TaskId == task.Id).ToList();
            }

            response.DTO = mapper.Map<CategoryWithTaskDTO>(categoryFromDb);

            return response;
        }

        public List<CategoryDTO> GetAll(string email)
        {
            var UserId = unitOfWork.User.GetFirstOrDefault(c => c.Email == email).Id;

            return mapper.Map<List<CategoryDTO>>(unitOfWork.Category.GetAll(c => c.UserId == UserId).ToList());
        }

        public CategoryDTO CreateCategory(CategoryCreationBO categoryCreationBO)
        {
            Category category = new Category()
            {
                Name = categoryCreationBO.Name.ToLower(),
                Color = categoryCreationBO.Color.ToLower(),
                UserId = unitOfWork.User.GetFirstOrDefault(c => c.Email == categoryCreationBO.UserEmail).Id
            };

            //Create Category
            unitOfWork.Category.Add(category);
            unitOfWork.Save();

            //Create Main Task
            Task task = new Task { Name = "Main", CategoryId = category.Id };
            unitOfWork.Task.Add(task);
            unitOfWork.Save();

            return mapper.Map<CategoryDTO>(category);
        }

        public BusinessLogicResponseDTO DeleteCategory(CategoryIdBo categoryIdBo)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            string userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == categoryIdBo.UserEmail).Id;

            //Invalid ID
            if (categoryIdBo.CategoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryIdBo.CategoryId && c.UserId == userId);

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Success - Delete Category
            unitOfWork.Category.Remove(categoryFromDb);
            unitOfWork.Save();

            response.StatusCode = 200;

            return response;
        }

        public BusinessLogicResponseDTO UpdateCategory(CategoryUpdateBO categoryUpdateBO)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            string userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == categoryUpdateBO.UserEmail).Id;

            //Invalid ID
            if (categoryUpdateBO.Id == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryUpdateBO.Id && c.UserId == userId);

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Success - Update Category Name
            categoryFromDb.Name = categoryUpdateBO.Name;
            unitOfWork.Save();

            response.DTO = mapper.Map<CategoryDTO>(categoryFromDb);

            return response;
        }
    }
}