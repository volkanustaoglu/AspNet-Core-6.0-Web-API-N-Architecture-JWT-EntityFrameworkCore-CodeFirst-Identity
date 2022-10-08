using ProjectApp.Core.DTOS;
using ProjectApp.Core.DTOS.UserDtos;
using ProjectApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Core.Services
{
    public interface IUserService
    {
        Task<CustomResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<CustomResponseDto<AppUserDto>> GetUserByNameAsync(string userName);

        Task<CustomResponseDto<AppRoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto);

        Task<CustomResponseDto<List<AppUserDto>>> GetAllUsersAsync();

        Task RemoveUserAsync(string id);

        Task<CustomResponseDto<AppUserDto>> GetUserByIdAsync(string id);
        Task<CustomResponseDto<UpdateUserDto>> UpdateUserAync(UpdateUserDto updateUserDto);

        Task RemoveRoleAsync(string id);
        Task<CustomResponseDto<List<AppRoleDto>>> GetAllRolesAsync();
        Task<CustomResponseDto<UpdateRoleDto>> UpdateRoleAync(UpdateRoleDto updateRoleDto);

        Task<CustomResponseDto<List<RoleAssignDto>>> GetRoleAssignAsync(string id);
        Task RoleAssignAsync(string userId, List<RoleAssignDto> roleAssignDto);


    }
}
