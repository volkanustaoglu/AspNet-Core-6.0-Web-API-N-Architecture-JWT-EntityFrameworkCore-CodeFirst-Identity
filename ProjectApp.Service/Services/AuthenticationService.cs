
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
                await _userManager.AccessFailedAsync(user);
              

                int fail = await _userManager.GetAccessFailedCountAsync(user);

                if (user.LockoutEnd>DateTime.Now)
                {
                    return CustomResponseDto<TokenDto>.Fail(400, "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                }

                if (fail == 4)
                {
                    await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(20)));

                    return CustomResponseDto<TokenDto>.Fail(400, "Hesabınız 3 başarısız girişten dolayı 20 dakika süreyle kitlenmiştir. Lütfen" +
                        " Daha Sonra Tekrar Deneyiniz.");

                }

                return CustomResponseDto<TokenDto>.Fail(400, $" {fail} kez başarısız giriş");
              
              
            }

            if (await _userManager.IsLockedOutAsync(user))
            {
                return CustomResponseDto<TokenDto>.Fail(400, "Hesabınız bir süreliğine kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz");
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
            await _userManager.ResetAccessFailedCountAsync(user);
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
