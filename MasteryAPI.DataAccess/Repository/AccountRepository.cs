using AutoMapper;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MasteryAPI.DataAccess.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public AccountRepository(UserManager<ApplicationUser> userManager,
                                    IConfiguration configuration,
                                    SignInManager<ApplicationUser> signInManager,
                                    IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser applicationUser, string password)
        {
            return await userManager.CreateAsync(applicationUser, password);
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> Login(UserInfo userInfo)
        {
            return await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,
                isPersistent: false, lockoutOnFailure: false);
        }

        public async Task<ApplicationUser> GetUser(UserInfo userInfo)
        {
            return await userManager.FindByNameAsync(userInfo.Email);
        }

        public async Task<ApplicationUser> PatchUser(UserInfo userInfo, PatchUserDetails patchUser)
        {
            var userFromDb = await userManager.FindByNameAsync(userInfo.Email);

            mapper.Map(patchUser, userFromDb);

            await userManager.UpdateAsync(userFromDb);

            return userFromDb;
        }

        public async Task<UserToken> BuildToken(UserInfo userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email)
            };

            var identityUser = await userManager.FindByEmailAsync(userInfo.Email);
            var claimsDb = await userManager.GetClaimsAsync(identityUser);

            claims.AddRange(claimsDb);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(20);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: cred
                );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}