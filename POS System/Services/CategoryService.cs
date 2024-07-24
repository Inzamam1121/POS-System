using Microsoft.EntityFrameworkCore;
using POS_System.Data;
using POS_System.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POS_System.Services
{
    public class CategoryService
    {
        private readonly DBContextEntity _context;

        public CategoryService(DBContextEntity context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCategoryAsync(int id, string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return false;

            category.Name = name;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveCategoryAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
