using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
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
        public async Task<Topic> Add(CreateTopicModel topic)
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
            _context.Topics.Add(_topic);
            await _context.SaveChangesAsync();
            return _topic;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Topics
                .Include(a => a.Lecture)
                .SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            var documents = await _context.Documents
                             .Include(a => a.Lecture)
                                 .ThenInclude(a => a.Topic)
                             .Where(s => s.Lecture.Topic.Id == id)
                             .OrderByDescending(id => id.Date)
                             .ToListAsync();
            // update all LectureID of document to null
            foreach (var document in documents)
            {
                document.LectureID = null;
            }
            var questions = await _context.Questions.Where(a => a.TopicId == id).ToListAsync();
            foreach (var question in questions)
            {
                question.TopicId = null;
            }
            _context.Lectures.RemoveRange(result.Lecture);
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Topic>> GetAll()
        {
            var result = await _context.Topics
                .Include(a => a.Lecture)
                .ThenInclude(a => a.Documents)
                .ThenInclude(a => a.File)
                .ToListAsync();
            return result;
        }

        public async Task<Topic> GetById(int id)
        {
            var result = await _context.Topics
                .Include(a => a.Lecture)
                .ThenInclude(a => a.Documents)
                .ThenInclude(a => a.File)
                .SingleOrDefaultAsync(i => i.Id == id);
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
