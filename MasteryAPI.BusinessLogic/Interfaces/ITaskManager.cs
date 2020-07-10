using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Interfaces
{
    public interface ITaskManager
    {
        BusinessLogicResponseDTO Get(TaskIdBo taskIdBo);

        BusinessLogicResponseDTO GetAll(TaskIdBo taskIdBo);

        BusinessLogicResponseDTO CreateTask(TaskCreationBo taskCreationBo);

        BusinessLogicResponseDTO UpdateTask(TaskUpdateBO taskUpdateBO);

        BusinessLogicResponseDTO DeleteTask(TaskIdBo taskIdBo);
    }
}