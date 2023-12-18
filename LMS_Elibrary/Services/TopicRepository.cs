using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;
using ServiceStack;

namespace LMS_Elibrary.Services
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ElibraryDbContext _context;

        public TopicRepository(ElibraryDbContext context) 
        {
            _context = context;
        }
        public async Task<Topic> Add(Topic topic)
        {
            var isDuplicate = await _context.Topics.AnyAsync(a => a.TopicName == topic.TopicName);
            if (isDuplicate)
            {
                return null;
            }
            var _topic = new Topic
            {
                TopicName = topic.TopicName,
                SubjectId = topic.SubjectId,
            };
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return _topic;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Topics.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Topic>> GetAll()
        {
            var result = await _context.Topics.ToListAsync();
            return result;
        }

        public async Task<Topic> GetById(int id)
        {
            var result = await _context.Topics.SingleOrDefaultAsync(i => i.Id == id);
            if(result == null)
            {
                return new Topic();
            }
            return result;
        }

        public async Task<List<Topic>> GetBySubjectId(int id)
        {
            var result = await _context.Topics.Where(i => i.SubjectId == id).OrderByDescending(i => i.Id).ToListAsync();
            return result;
        }

        public async Task<bool> Update(Topic topic, int id)
        {
            var result = await _context.Topics.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            //result.SubjectId = topic.SubjectId ?? result.SubjectId;
            if(topic != null)
            {
                result.TopicName = topic.TopicName ?? result.TopicName;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
