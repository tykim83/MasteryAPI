using AutoMapper;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MasteryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #region Create User

        /// <summary>
        /// Create a new User
        /// </summary>
        /// <param name="model">Passwords must be at least 6 characters <br /> Passwords must have at least one digit <br /> Passwords must have at least one uppercase</param>
        /// <returns></returns>
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await unitOfWork.Account.CreateUser(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            else
            {
                return await unitOfWork.Account.BuildToken(model);
            }
        }

        #endregion Create User

        #region Login User

        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo model)
        {
            var result = await unitOfWork.Account.Login(model);

            if (result.Succeeded)
            {
                return await unitOfWork.Account.BuildToken(model);
            }
            else
            {
                return BadRequest(new
                {
                    IsSucceeded = result.Succeeded,
                    IsLockedOut = result.IsLockedOut
                });
            }
        }

        #endregion Login User

        #region Renew Token

        /// <summary>
        /// Renew Token
        /// </summary>
        /// <returns></returns>
        [HttpPost("RenewToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renew()
        {
            var userInfo = new UserInfo
            {
                Email = HttpContext.User.Identity.Name
            };

            return await unitOfWork.Account.BuildToken(userInfo);
        }

        #endregion Renew Token

        #region Get User Details

        /// <summary>
        /// Get User Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("UserDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDetails>> GetUser()
        {
            var userInfo = new UserInfo
            {
                Email = HttpContext.User.Identity.Name
            };

            return mapper.Map<UserDetails>(await unitOfWork.Account.GetUser(userInfo));
        }

        #endregion Get User Details

        #region Put User Details

        /// <summary>
        /// Put User Details
        /// </summary>
        /// <returns></returns>
        [HttpPut("PatchUserDetails")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserDetails>> PatchUser([FromBody] PatchUserDetails patchUser)
        {
            if (patchUser == null)
            {
                return BadRequest();
            }

            var userInfo = new UserInfo
            {
                Email = HttpContext.User.Identity.Name
            };

            return mapper.Map<UserDetails>(await unitOfWork.Account.PatchUser(userInfo, patchUser));
        }

        #endregion Put User Details
    }
}