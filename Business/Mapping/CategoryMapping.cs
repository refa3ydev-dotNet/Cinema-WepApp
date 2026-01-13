using Business.DTOs.Categories;
using Core.Entities;

namespace Business.Mapping
{
    public static class CategoryMapping
    {
        public static Category ToEntity(this CreateCategoryDto dto)
        {
            return new Category()
            {
                CategoryName = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
                
            };
        }
        public static Category ToEntity(this UpdateCategoryDto dto)
        {
            return new Category()
            {
                Id = dto.Id,
                CategoryName = dto.Name,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
            };
        }
        public static GetCategoryByIdDto ToDto(this Category category)
        {
            return new GetCategoryByIdDto()
            {
                Id = category.Id,
                Name = category.CategoryName,
                Description = category.Description,
                ImageUrl = category.ImageUrl
            };
        }
        public static List<GetAllCategoriesDto> ToDto(this List<Category> categories)
        {
            if (categories == null) return null;
            return categories.Select(x => new GetAllCategoriesDto()
            {
                Id = x.Id,
                Name = x.CategoryName,
                Description = x.Description,
                ImageUrl = x.ImageUrl
            }).ToList();
        }
    }
}
