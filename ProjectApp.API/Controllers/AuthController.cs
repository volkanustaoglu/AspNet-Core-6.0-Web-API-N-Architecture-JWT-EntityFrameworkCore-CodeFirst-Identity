
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Core.DTOS.ProductDtos;
using ProjectApp.Core.DTOS;
using ProjectApp.Core.DTOS.UserDtos;
using ProjectApp.Core.DTOS.TokenDtos;
using ProjectApp.Core.Services;

namespace ProjectApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : CustomBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken(LoginDto loginDto)
        {
            var result = await _authenticationService.CreateTokenAsync(loginDto);

            return CreateActionResult(result);

        }

 

        [HttpPost]
        public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshToken)
        {
            var result = await _authenticationService.RevokenRefreshToken(refreshToken.Token);
            return CreateActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
            return CreateActionResult(result);
        }

    }
}
