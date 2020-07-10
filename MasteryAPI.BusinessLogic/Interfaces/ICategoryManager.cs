using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Interfaces
{
    public interface ICategoryManager
    {
        BusinessLogicResponseDTO GetComplete(CategoryIdBo categoryIdBo);

        BusinessLogicResponseDTO GetWithPagination(CategoryWithRecordAndPaginationBO categoryWithRecordAndPaginationBO);

        List<CategoryDTO> GetAll(string email);

        CategoryDTO CreateCategory(CategoryCreationBO categoryCreationBO);

        BusinessLogicResponseDTO UpdateCategory(CategoryUpdateBO categoryUpdateBO);

        BusinessLogicResponseDTO DeleteCategory(CategoryIdBo categoryIdBo);
    }
}