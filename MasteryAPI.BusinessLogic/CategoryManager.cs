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
    public class CategoryManager : ICategoryManager
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CategoryManager(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public CategoryDTO CreateCategory(CategoryCreationDTO categoryCreationDTO, string email)
        {
            Category category = new Category()
            {
                Name = categoryCreationDTO.Name.ToLower(),
                UserId = unitOfWork.User.GetFirstOrDefault(c => c.Email == email).Id
            };

            unitOfWork.Category.Add(category);
            unitOfWork.Save();

            return mapper.Map<CategoryDTO>(category);
        }

        public BusinessLogicResponseDTO DeleteCategory(int categoryId, string email)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            string userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == email).Id;

            //Invalid ID
            if (categoryId == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryId && c.UserId == userId);

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

        public BusinessLogicResponseDTO UpdateCategory(CategoryUpdateDTO categoryUpdateDTO, string email)
        {
            BusinessLogicResponseDTO response = new BusinessLogicResponseDTO();
            string userId = unitOfWork.User.GetFirstOrDefault(c => c.Email == email).Id;

            //Invalid ID
            if (categoryUpdateDTO.Id == 0)
            {
                response.StatusCode = 400;
                return response;
            }

            Category categoryFromDb = unitOfWork.Category.GetFirstOrDefault(c => c.Id == categoryUpdateDTO.Id && c.UserId == userId);

            //Category is Null
            if (categoryFromDb == null)
            {
                response.StatusCode = 404;
                return response;
            }

            //Success - Update Category Name
            categoryFromDb.Name = categoryUpdateDTO.Name;
            unitOfWork.Save();

            response.DTO = mapper.Map<CategoryDTO>(categoryFromDb);

            return response;
        }
    }
}