using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Interfaces;
using TutorialApp.Models;
using TutorialApp.Exceptions.Cart;

namespace TutorialApp.Repositories
{
    public class CartRepository : IRepository<int, Cart>
    {
        private readonly TutorialAppContext _context;

        public CartRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Cart> Add(Cart item)
        {
            _context.Carts.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Cart> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            { 
                throw new NoSuchCartFoundException();
            }
            _context.Carts.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Cart>> Get()
        {
            return await _context.Carts.ToListAsync();
        }

        public async Task<Cart> GetByKey(int key)
        {
            var item = await _context.Carts.FirstOrDefaultAsync(c => c.CartId == key);
            return item;
        }

        public async Task<Cart> Update(Cart item)
        {
            var existingItem = await GetByKey(item.CartId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchCartFoundException();
        }
    }
}
