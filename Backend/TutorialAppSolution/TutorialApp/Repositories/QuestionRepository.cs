using Microsoft.EntityFrameworkCore;
using TutorialApp.Contexts;
using TutorialApp.Exceptions.Question;
using TutorialApp.Interfaces;
using TutorialApp.Models;

namespace TutorialApp.Repositories
{
    public class QuestionRepository : IRepository<int, Question>
    {
        private readonly TutorialAppContext _context;

        public QuestionRepository(TutorialAppContext context)
        {
            _context = context;
        }

        public async Task<Question> Add(Question item)
        {
            _context.Questions.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Question> DeleteByKey(int key)
        {
            var item = await GetByKey(key);
            if (item == null)
            {
                return null;
            }
            _context.Questions.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Question>> Get()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question> GetByKey(int key)
        {
            var item = await _context.Questions.FirstOrDefaultAsync(c => c.QuestionId == key);
            return item;
        }

        public async Task<Question> Update(Question item)
        {
            var existingItem = await GetByKey(item.QuestionId);
            if (existingItem != null)
            {
                _context.Entry(existingItem).State = EntityState.Detached;
                _context.Attach(item);

                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return item;
            }
            throw new NoSuchQuestionFoundException();
        }
    }
}
