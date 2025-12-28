using Business.DTOs.Categories;
using Core.Entities;


namespace Business.Managers.Categories
{
    public interface ICategoryManager
    {
        Task CreateCategoryAsync(CreateCategoryDto dto);
        Task<List<GetAllCategoriesDto>> GetAllCategoriesAsync();
        Task<GetCategoryByIdDto> GetCategoryByIdAsync(int id);
        Task DeleteCategoryAsync(int id);
        Task UpdateCategoryAsync(UpdateCategoryDto dto);
        Task<List<Category>> GetCategoriesByIdsAsync(List<int> ids);
    }
}
