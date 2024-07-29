using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.User;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class UserRepository : IRepository<string, User>
    {
        private readonly TutorialAppContext _context;

        public UserRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User item)
        {
            _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<User> DeleteByKey(string key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                throw new NoSuchUserFoundException();
            }
            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByKey(string key)
        {
            var item = await _context.Users.FirstOrDefaultAsync(c => c.Email == key);
            return item;
        }

        public async Task<User> Update(User item)
        {
            var existingItem = await GetByKey(item.Email);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchUserFoundException();
        }
    }
}
