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
            CreateMap<RecordCreationStartDTO, Record>();
            CreateMap<RecordCreationStartBO, Record>();
            CreateMap<RecordCreationStartDTO, RecordCreationStartBO>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CategoryWithTaskDTO>();

            CreateMap<Task, TaskDTO>();

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