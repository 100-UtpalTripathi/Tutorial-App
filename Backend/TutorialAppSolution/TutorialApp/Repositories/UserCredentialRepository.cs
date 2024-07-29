using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.UserCredential;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class UserCredentialRepository : IRepository<string, UserCredential>
    {
        private readonly TutorialAppContext _context;

        public UserCredentialRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<UserCredential> Add(UserCredential item)
        {
            _context.UserCredentials.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<UserCredential> DeleteByKey(string key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                throw new NoSuchUserCredentialFoundException();
            }
            _context.UserCredentials.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<UserCredential>> Get()
        {
            return await _context.UserCredentials.ToListAsync();
        }

        public async Task<UserCredential> GetByKey(string key)
        {
            var item = await _context.UserCredentials.FirstOrDefaultAsync(c => c.Email == key);
            return item;
        }

        public async Task<UserCredential> Update(UserCredential item)
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
            throw new NoSuchUserCredentialFoundException();
        }
    }
}
