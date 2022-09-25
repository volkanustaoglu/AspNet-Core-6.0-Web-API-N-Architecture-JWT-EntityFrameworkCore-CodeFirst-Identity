using AutoMapper;
using AutoMapper.Internal.Mappers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using ProjectApp.Core.DTOS;
using ProjectApp.Core.DTOS.UserDtos;
using ProjectApp.Core.Models;
using ProjectApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly RoleManager<AppRole> _roleManager;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public async Task<CustomResponseDto<AppRoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            AppRole role = new AppRole { Name =createRoleDto.Name};
       

            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponseDto<AppRoleDto>.Fail(400, errors);
            }
            return CustomResponseDto<AppRoleDto>.Success(200, _mapper.Map<AppRoleDto>(role));

        }

        public async Task<CustomResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new AppUser { Email = createUserDto.Email, UserName = createUserDto.UserName };
            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return CustomResponseDto<AppUserDto>.Fail(400, errors);

            }
            return CustomResponseDto<AppUserDto>.Success(200, _mapper.Map<AppUserDto>(user));
        }

      

        public async Task<CustomResponseDto<AppUserDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return CustomResponseDto<AppUserDto>.Fail(404, "UserName nor found");
            }
            return CustomResponseDto<AppUserDto>.Success(200, _mapper.Map<AppUserDto>(user));
        }
    }
}
