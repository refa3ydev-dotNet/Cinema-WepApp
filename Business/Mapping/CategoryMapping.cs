using Business.DTOs.Categories;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public static class CategoryMapping
    {
        public static Category ToEntity(this CreateCategoryDto dto)
        {
            return new Category()
            {
                CategoryName = dto.Name
            };
        }
        public static Category ToEntity(this UpdateCategoryDto dto)
        {
            return new Category()
            {
                Id = dto.Id,
                CategoryName = dto.Name
            };
        }
        public static GetCategoryByIdDto ToDto(this Category category)
        {
            return new GetCategoryByIdDto()
            {
                Id = category.Id,
                Name = category.CategoryName
            };
        }
        public static List<GetAllCategoriesDto> ToDto(this List<Category> categories)
        {
            if (categories == null) return null;
            return categories.Select(x => new GetAllCategoriesDto() {
                Id = x.Id,
                Name = x.CategoryName,
            }).ToList();
        }
    }
}
