using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Quiz;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class QuizRepository : IRepository<int, Quiz>
    {
        private readonly TutorialAppContext _context;

        public QuizRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Quiz> Add(Quiz item)
        {
            _context.Quizzes.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Quiz> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                return null;
            }
            _context.Quizzes.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Quiz>> Get()
        {
            return await _context.Quizzes.ToListAsync();
        }

        public async Task<Quiz> GetByKey(int key)
        {
            var item = await _context.Quizzes.FirstOrDefaultAsync(c => c.QuizId == key);
            return item;
        }

        public async Task<Quiz> Update(Quiz item)
        {
            var existingItem = await GetByKey(item.QuizId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchQuizFoundException();
        }
    }
}
