using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using scb10x_assignment_party_haan_backend.Domain.DTOs.UserAggregate;
using scb10x_assignment_party_haan_backend.Domain.Extensions;
using scb10x_assignment_party_haan_backend.Infrastructure.Services;

namespace scb10x_assignment_party_haan_backend.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
        {
            var result = await _userService.SignUp(request);

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO request)
        {
            var result = await _userService.SignIn(request);

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserById()
        {
            if (HttpContext.HasAuthorization())
            {
                string userId = HttpContext.GetUserId();

                var result = await _userService.GetUserById(Int32.Parse(userId));

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
                
        }

        [Authorize]
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetUsers();

            return Ok(result);
        }
    }
}
