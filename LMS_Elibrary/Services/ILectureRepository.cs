using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace LMS_Elibrary.Services
{
    public interface ILectureRepository
    {
        public Task<List<Lecture>> GetAll();
        public Task<Lecture> GetById(int id);
        public Task<Lecture> Add(CreateLectureModel lecture);
        public Task<Lecture> AddLectureAndDocument(LectureAndDocumentInput lectureAndDocumentInput);
        public Task<bool> Update(CreateLectureModel lecture, int id);
        public Task<bool> BlockStudent(int id, bool blockStudent);
        public Task<bool> Delete(int id);
        public Task<bool> AssignDocument(int LectureId, List<string> classRooms);
        //public Task<string> GetClassRoomBySubjectId(int subjectID);
    }
}
