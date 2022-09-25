
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectApp.Core.Configuration;
using ProjectApp.Core.DTOS;
using ProjectApp.Core.DTOS.TokenDtos;
using ProjectApp.Core.DTOS.UserDtos;
using ProjectApp.Core.Models;
using ProjectApp.Core.Repositories;
using ProjectApp.Core.Services;
using ProjectApp.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
       
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenRepository;

        public AuthenticationService( ITokenService tokenService, 
            UserManager<AppUser> userManager, IUnitOfWork unitOfWork, 
            IGenericRepository<UserRefreshToken> userRefreshTokenRepository)
        {
        
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshTokenRepository = userRefreshTokenRepository;
        }

        public async Task<CustomResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null) throw new ArgumentNullException(nameof(loginDto));
            
            var user =await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return CustomResponseDto<TokenDto>.Fail(400, "Email or Password is wrong");

            if (!await _userManager.CheckPasswordAsync(user,loginDto.Password))
            {
                return CustomResponseDto<TokenDto>.Fail(400, "Email or Password is wrong");
            }
            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshTokenRepository.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken==null)
            {
                await _userRefreshTokenRepository.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();
            return CustomResponseDto<TokenDto>.Success(200, token);
         
        }

        public async Task<CustomResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenRepository.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken==null)
            {
                return CustomResponseDto<TokenDto>.Fail(404, "Refresh token not found");
            }

            var user = await _userManager.FindByEmailAsync(existRefreshToken.UserId);

            if (user==null)
            {
                return CustomResponseDto<TokenDto>.Fail(404, "User Id not found");
            }

            var tokenDto = _tokenService.CreateToken(user);
            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();
            return CustomResponseDto<TokenDto>.Success(200, tokenDto);


        }

        public async Task<CustomResponseDto<NoContentDto>> RevokenRefreshToken(string refreshToken)
        {
            var existRefreshToken = await _userRefreshTokenRepository.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefreshToken ==null)
            {
                return CustomResponseDto<NoContentDto>.Fail(404, "Refresh token not found");
            }
            
            _userRefreshTokenRepository.Remove(existRefreshToken);
            return CustomResponseDto<NoContentDto>.Success(200);

        }
    }
}
