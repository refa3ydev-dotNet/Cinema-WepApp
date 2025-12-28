using Core.Entities;
using DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.CATEGORY
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MoviesDbContext _context;
        public CategoryRepository(MoviesDbContext context)
        {
            _context = context;
        }
        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Category>>GetCategoriesByIdsAsync(List<int> ids)
        {
            return await _context.Categories.Where(x => ids.Contains(x.Id)).ToListAsync();

        }
        public async Task DeleteCategoryAsync(int id)
        {
            var exist =await _context.Categories.FindAsync(id);
            if (exist == null) return;
            _context.Categories.Remove(exist);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            if (_context.Categories == null)
            {
                return null;
            }
            else
            {
                return await _context.Categories.ToListAsync();
            }
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            if (id > 0)
            {

                return await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            }
            else
            {
                return null;
            }
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            if (name != null)
            {
                return await _context.Categories.FirstOrDefaultAsync(x => x.CategoryName == name);
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
