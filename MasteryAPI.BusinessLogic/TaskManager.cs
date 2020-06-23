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
    }
}