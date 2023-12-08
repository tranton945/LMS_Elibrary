using LMS_Elibrary.Data;
using LMS_Elibrary.Models;

namespace LMS_Elibrary.Services
{
    public interface IPrivateFilesRepository
    {
        public Task<List<PrivateFileDTO>> GetAll();
        public Task<FileAndPrivateFileDTO> GetById(int id);
        public Task<List<FileAndPrivateFileDTO>> Add(List<IFormFile> files);
        public Task<bool> ChangeFileName(string name, int id);
        public Task<bool> Delete(int id);

        public Task<List<PrivateFileDTO>> Search(string searchString);
        public Task<List<string>> GetAllFileType();
        public Task<List<PrivateFileDTO>> GetPrivateFileByType(List<string> types);
        public Task<List<Data.File>> DownloadFile(List<int> ints);
    }
}
