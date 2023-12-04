using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class LectureRepository : ILectureRepository
    {
        private readonly ElibraryDbContext _context;

        public LectureRepository(ElibraryDbContext context) 
        {
            _context = context;
        }
        public async Task<Lecture> Add(Lecture lecture)
        {
            var _lecture = new Lecture
            {
                Title= lecture.Title,
                Descriptions=lecture.Descriptions,
                TopicId=lecture.TopicId,
            };
            _context.Add(lecture);
            await _context.SaveChangesAsync();
            return lecture;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Lectures.SingleOrDefaultAsync(i => i.Id == id);
            if(result == null) 
            {
                return false;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Lecture>> GetAll()
        {
            var result = await _context.Lectures.ToListAsync();
            return result;
        }

        public async Task<Lecture> GetById(int id)
        {
            var result = await _context.Lectures.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return new Lecture();;
            }
            return result;
        }

        public async Task<bool> Update(Lecture lecture, int id)
        {
            var result = await _context.Lectures.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return false;
            }
            if(lecture != null) 
            {
                result.Title = lecture.Title ?? result.Title;
                result.Descriptions = lecture.Descriptions ?? result.Descriptions;
                result.TopicId = (lecture.TopicId != null) ? lecture.TopicId : result.TopicId;
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
