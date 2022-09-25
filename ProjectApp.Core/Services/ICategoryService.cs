using ProjectApp.Core.DTOS.ProductDtos;
using ProjectApp.Core.DTOS;
using ProjectApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectApp.Core.DTOS.CategoryDtos;

namespace ProjectApp.Core.Services
{
    public interface ICategoryService : IService<Category>
    {
        Task<CustomResponseDto<CategoryWithProductsDto>> GetSingleCategoryByIdWithProductsAsync(int categoryId);
    }
}
