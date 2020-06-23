using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MasteryAPI.DataAccess.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUser(ApplicationUser applicationUser, string password);

        Task<Microsoft.AspNetCore.Identity.SignInResult> Login(UserInfo userInfo);

        Task<UserToken> BuildToken(UserInfo userInfo);

        Task<ApplicationUser> GetUser(UserInfo userInfo);

        Task<ApplicationUser> PatchUser(UserInfo userInfo, PatchUserDetails patchUser);
    }
}