using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjectApp.Core.Services;

namespace ProjectApp.API.Controllers
{
 
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet("[action]/{categoryId}")]
        public async Task<IActionResult> GetSingleCategoryByIdWithProduts(int categoryId)
        {
            return CreateActionResult(await _service.GetSingleCategoryByIdWithProductsAsync(categoryId));
        }
    }
}
