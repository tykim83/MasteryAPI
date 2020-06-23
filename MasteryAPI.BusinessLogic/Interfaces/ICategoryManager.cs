using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Interfaces
{
    public interface ICategoryManager
    {
        CategoryDTO CreateCategory(CategoryCreationDTO categoryCreationDTO, string email);

        BusinessLogicResponseDTO UpdateCategory(CategoryUpdateDTO categoryUpdateDTO, string email);

        BusinessLogicResponseDTO DeleteCategory(int categoryId, string email);
    }
}