using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Enrollment;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class EnrollmentRepository : IRepository<int, Enrollment>
    {
        private readonly TutorialAppContext _context;

        public EnrollmentRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Enrollment> Add(Enrollment item)
        {
            _context.Enrollments.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Enrollment> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                return null;
            }
            _context.Enrollments.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Enrollment>> Get()
        {
            return await _context.Enrollments.ToListAsync();
        }

        public async Task<Enrollment> GetByKey(int key)
        {
            var item = await _context.Enrollments.FirstOrDefaultAsync(c => c.EnrollmentId == key);
            return item;
        }

        public async Task<Enrollment> Update(Enrollment item)
        {
            var existingItem = await GetByKey(item.EnrollmentId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchEnrollmentFoundException();
        }
    }
}
