using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Interfaces
{
    public interface ITaskManager
    {
        BusinessLogicResponseDTO CreateTask(TaskCreationDTO taskCreationDTO, string email);
    }
}