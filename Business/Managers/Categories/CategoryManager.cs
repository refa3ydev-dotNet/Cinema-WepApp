using Business.DTOs.Categories;
using Business.Mapping;
using Core.Entities;
using DataAccess.Repositories.CATEGORY;

namespace Business.Managers.Categories
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task CreateCategoryAsync(CreateCategoryDto dto)
        {
            var category = dto.ToEntity();
            await _categoryRepository.CreateCategoryAsync(category);
        }

        public async Task<List<Category>> GetCategoriesByIdsAsync(List<int> ids)
        {

            var categories = await _categoryRepository.GetCategoriesByIdsAsync(ids);
            return categories;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task<List<GetAllCategoriesDto>> GetAllCategoriesAsync()
        {
            var category = await _categoryRepository.GetAllCategoriesAsync();
            return category.ToDto();
        }

        public async Task<GetCategoryByIdDto> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            return category.ToDto();
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var category = dto.ToEntity();
            await _categoryRepository.UpdateCategoryAsync(category);
        }
    }
}
