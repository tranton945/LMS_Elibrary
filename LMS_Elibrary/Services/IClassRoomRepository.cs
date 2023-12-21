using LMS_Elibrary.Data;

namespace LMS_Elibrary.Services
{
    public interface IClassRoomRepository
    {
        public Task<List<ClassRoom>> GetAll();
        public Task<ClassRoom> GetById(int id);
        public Task<ClassRoom> Add(ClassRoom classRoom);
        public Task<bool> Update(ClassRoom classRoom, int subId, int id);
        public Task<bool> Delete(int id);
        public Task<List<string>> GetClassRoomBySubjectId(int subjectID);
    }
}
