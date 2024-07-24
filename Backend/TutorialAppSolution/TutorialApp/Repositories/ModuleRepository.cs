using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Module;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class ModuleRepository : IRepository<int, Module>
    {
        private readonly TutorialAppContext _context;

        public ModuleRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Module> Add(Module item)
        {
            _context.Modules.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Module> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                return null;
            }
            _context.Modules.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Module>> Get()
        {
            return await _context.Modules.ToListAsync();
        }

        public async Task<Module> GetByKey(int key)
        {
            var item = await _context.Modules.FirstOrDefaultAsync(c => c.ModuleId == key);
            return item;
        }

        public async Task<Module> Update(Module item)
        {
            var existingItem = await GetByKey(item.ModuleId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchModuleFoundException();
        }
    }
}
