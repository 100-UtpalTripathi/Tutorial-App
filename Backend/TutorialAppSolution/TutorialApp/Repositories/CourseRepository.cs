using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Course;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class CourseRepository : IRepository<int, Course>
    {
        private readonly TutorialAppContext _context;

        public CourseRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Course> Add(Course item)
        {
            _context.Courses.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Course> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                return null;
            }
            _context.Courses.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Course>> Get()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course> GetByKey(int key)
        {
            var item = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == key);
            return item;
        }

        public async Task<Course> Update(Course item)
        {
            var existingItem = await GetByKey(item.CourseId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchCourseFoundException();
        }
    }
}
