using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectApp.Core.DTOS;
using ProjectApp.Core.DTOS.ProductDtos;
using ProjectApp.Core.DTOS.UserDtos;
using ProjectApp.Core.Models;
using ProjectApp.Core.Services;

namespace ProjectApp.API.Controllers
{
  
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return CreateActionResult(await _userService.CreateUserAsync(createUserDto));
        }

        [HttpGet]

        public async Task<IActionResult> GetAllUsers()
        {
            return CreateActionResult(await _userService.GetAllUsersAsync());
        }


        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            await _userService.RemoveUserAsync(id);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> RemoveRole(string id)
        {
            await _userService.RemoveRoleAsync(id);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            return CreateActionResult(user);
        }

        

        //public async Task<IActionResult> GetUser()
        //{
        //    return CreateActionResult(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
        //}

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            return CreateActionResult(await _userService.CreateRoleAsync(createRoleDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDto updateUserDto)
        {
            await _userService.UpdateUserAync(updateUserDto);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }

        [HttpGet("[action]")]

        public async Task<IActionResult> GetAllRoles()
        {
            return CreateActionResult(await _userService.GetAllRolesAsync());
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRole(UpdateRoleDto updateRoleDto)
        {
            await _userService.UpdateRoleAync(updateRoleDto);

            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }


        [HttpGet("[action]")]

        public async Task<IActionResult> GetRoleAssignAsync(string id)
        {
            return CreateActionResult(await _userService.GetRoleAssignAsync(id));
        }

        [HttpPost("[action]")]

        public async Task<IActionResult> RoleAssignAsync(string userId, List<RoleAssignDto> roleAssignDto)
        {
           await _userService.RoleAssignAsync(userId,roleAssignDto);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }




    }
}
