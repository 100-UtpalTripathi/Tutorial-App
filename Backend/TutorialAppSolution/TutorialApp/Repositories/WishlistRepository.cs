using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Wishlist;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class WishlistRepository : IRepository<int, Wishlist>
    {
        private readonly TutorialAppContext _context;

        public WishlistRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> Add(Wishlist item)
        {
            _context.Wishlists.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Wishlist> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                throw new NoSuchWishlistFoundException();
            }
            _context.Wishlists.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Wishlist>> Get()
        {
            return await _context.Wishlists.ToListAsync();
        }

        public async Task<Wishlist> GetByKey(int key)
        {
            var item = await _context.Wishlists.FirstOrDefaultAsync(c => c.WishlistId == key);
            return item;
        }

        public async Task<Wishlist> Update(Wishlist item)
        {
            var existingItem = await GetByKey(item.WishlistId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchWishlistFoundException();
        }
    }
}
