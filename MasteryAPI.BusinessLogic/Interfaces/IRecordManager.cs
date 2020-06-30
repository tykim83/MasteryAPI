using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Interfaces
{
    public interface IRecordManager
    {
        BusinessLogicResponseDTO CreateRecord(RecordCreationCompleteDTO recordCreationCompleteDTO, string email);

        BusinessLogicResponseDTO StartRecord(RecordCreationStartBO recordCreationStartBO);

        BusinessLogicResponseDTO StopRecord(StopRecordBO stopRecordBO);
    }
}