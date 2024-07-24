using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Category;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class CategoryRepository : IRepository<int, Category>
    {
        private readonly TutorialAppContext _context;

        public CategoryRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Category> Add(Category item)
        {
            _context.Categories.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Category> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                return null;
            }
            _context.Categories.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Category>> Get()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByKey(int key)
        {
            var item = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == key);
            return item;
        }

        public async Task<Category> Update(Category item)
        {
            var existingItem = await GetByKey(item.CategoryId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchCategoryFoundException();
        }
    }
}
