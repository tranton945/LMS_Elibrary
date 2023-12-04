using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class FileRepository : IFileRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public FileRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }
        public async Task<Data.File> Add(IFormFile file, int docId)
        {
            var isuser = await _getUser.user();
            var _file = new Data.File
            {
                FileName = file.FileName,
                FileData = await ConvertFormFileToByteArray(file),
                FileType = file.ContentType,
                FileSize = (int)file.Length,
                Updator = isuser.UserName,
                LastUpdate = DateTime.UtcNow,
                DocumentId = docId
            };
            _context.Files.Add(_file);
            await _context.SaveChangesAsync();
            return _file;
        }
        private async Task<byte[]> ConvertFormFileToByteArray(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Files.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null) 
            { 
                return false;
            }
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Data.File>> GetAll()
        {
            var result = await _context.Files.ToListAsync();
            return result;
        }

        public async Task<Data.File> GetById(int id)
        {
            var result = await _context.Files.SingleOrDefaultAsync(i => i.Id == id);
            if (result == null)
            {
                return new Data.File();
            }
            return result;
        }

        public Task<bool> Update(IFormFile file, int id, int docId)
        {
            throw new NotImplementedException();
        }
    }
}
