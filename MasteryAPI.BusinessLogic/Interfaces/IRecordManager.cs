using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Interfaces
{
    public interface IRecordManager
    {
        RecordDTO CreateRecord(RecordCreationCompleteDTO recordCreationCompleteDTO);

        RecordDTO StartRecord(RecordCreationStartDTO recordCreationStartDTO);

        BusinessLogicResponseDTO StopRecord(int recordId);
    }
}