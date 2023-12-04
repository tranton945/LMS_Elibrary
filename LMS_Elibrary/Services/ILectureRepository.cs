using LMS_Elibrary.Data;

namespace LMS_Elibrary.Services
{
    public interface ILectureRepository
    {
        public Task<List<Lecture>> GetAll();
        public Task<Lecture> GetById(int id);
        public Task<Lecture> Add(Lecture lecture);
        public Task<bool> Update(Lecture lecture, int id);
        public Task<bool> Delete(int id);
    }
}
