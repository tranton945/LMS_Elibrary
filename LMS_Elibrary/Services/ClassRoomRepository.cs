using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class ClassRoomRepository : IClassRoomRepository
    {
        private readonly ElibraryDbContext _context;

        public ClassRoomRepository(ElibraryDbContext context)
        {
            _context = context;
        }

        public async Task<ClassRoom> Add(ClassRoom classRoom)
        {
            var newClass = new ClassRoom
            {
                ClassRoomName = classRoom.ClassRoomName,
                ClassRoomId = classRoom.ClassRoomId,
                SubjectId = classRoom.SubjectId,
            };
            _context.Add(newClass);
            await _context.SaveChangesAsync();
            return newClass;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.ClassRooms.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClassRoom>> GetAll()
        {
            var result = await _context.ClassRooms.ToListAsync();
            return result;
        }

        public async Task<ClassRoom> GetById(int id)
        {
            var result = await _context.ClassRooms.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return new ClassRoom();
            }
            return result;
        }

        public async Task<List<string>> GetClassRoomBySubjectId(int subjectID)
        {
            var result = await _context.ClassRooms.Where(a => a.SubjectId == subjectID).ToListAsync();
            var className = result.Select(a => a.ClassRoomName).ToList();
            className.Insert(0, "Tùy chon lớp học");
            return className;
        }

        public async Task<bool> Update(ClassRoom classRoom, int SubjectId, int id)
        {
            var result = await _context.ClassRooms.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            }
            result.ClassRoomName = classRoom.ClassRoomName ?? result.ClassRoomName;
            result.ClassRoomId = classRoom.ClassRoomId ?? result.ClassRoomId;
            result.SubjectId = (SubjectId != null) ? SubjectId : result.SubjectId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
