using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProjectApp.Core.DTOS.CategoryDtos;
using ProjectApp.Core.DTOS.ProductDtos;
using ProjectApp.Core.DTOS.UserDtos;
using ProjectApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApp.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductWithCategoryDto>();
            CreateMap<Category, CategoryWithProductsDto>();
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUser, UpdateUserDto>().ReverseMap();
            CreateMap<AppRole, AppRoleDto>();
            CreateMap<AppRole, UpdateRoleDto>();
        }
    }
}
