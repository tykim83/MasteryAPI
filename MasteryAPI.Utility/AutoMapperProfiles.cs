using AutoMapper;
using MasteryAPI.BusinessLogic.Models;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.Utility
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RecordCreationCompleteDTO, Record>();
            CreateMap<Record, RecordDTO>();
            CreateMap<Record, RecordOnlyDTO>();
            CreateMap<Record, RecordPaginationDTO>();
            CreateMap<RecordCreationStartDTO, Record>();
            CreateMap<RecordCreationStartBO, Record>();
            CreateMap<RecordCreationCompleteBO, Record>();
            CreateMap<RecordCreationStartDTO, RecordCreationStartBO>();
            CreateMap<RecordCreationCompleteDTO, RecordCreationCompleteBO>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CategoryWithTaskDTO>();
            CreateMap<Category, CategoryWithRecordsPaginationDTO>();
            CreateMap<CategoryCreationDTO, CategoryCreationBO>();
            CreateMap<CategoryUpdateDTO, CategoryUpdateBO>();

            CreateMap<Task, TaskDTO>();
            CreateMap<Task, TaskOnlyDTO>();
            CreateMap<Task, TaskWithRecordsDTO>();
            CreateMap<TaskCreationDTO, TaskCreationBo>();
            CreateMap<TaskUpdateDTO, TaskUpdateBO>();
            CreateMap<TaskDeleteDTO, TaskIdBo>();

            CreateMap<ApplicationUser, UserDetails>();
            CreateMap<PatchUserDetails, ApplicationUser>().ForMember(q => q.Name, option => option.Ignore())
             .AfterMap((src, dst) =>
             {
                 if (src.Name != "")
                 {
                     dst.Name = src.Name;
                 }
             });
        }
    }
}