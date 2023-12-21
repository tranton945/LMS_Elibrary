using LMS_Elibrary.Data;
using LMS_Elibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using ServiceStack;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LMS_Elibrary.Services
{
    public class PrivateFilesRepository : IPrivateFilesRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _getUser;

        public PrivateFilesRepository(ElibraryDbContext context, GetUser getUser) 
        {
            _context = context;
            _getUser = getUser;
        }
        public async Task<List<FileAndPrivateFileDTO>> Add(List<IFormFile> files)
        {
            var issuer = await _getUser.user();
            // list kết quả để trả về sau khi add
            List<FileAndPrivateFileDTO> result = new List<FileAndPrivateFileDTO>();
            foreach (var file in files)
            {
                // tạo privateFile
                var privateFile = new PrivateFile
                {
                    Updator = issuer.Name,
                    LastUpdate = DateTime.Now
                };
                // lưu lại privateFile để lấy privateFileID
                _context.PrivateFiles.Add(privateFile);
                await _context.SaveChangesAsync();

                var _file = new Data.File
                {
                    FileName = file.FileName,
                    FileData = await ConvertFormFileToByteArray(file),
                    FileType = Path.GetExtension(file.FileName),
                    FileSize = (int)file.Length,
                    DocumentId = null,
                    PrivateFilesId = privateFile.Id
                };
                _context.Files.Add(_file);
                await _context.SaveChangesAsync();
                // tạo đối tượng fileAndPrivateFileDTO
                var fileAndPrivateFileDTO = new FileAndPrivateFileDTO
                {
                    File = _file,
                    PrivateFile = privateFile
                };

                // thêm fileAndPrivateFileDTO vừa tạo vào result
                result.Add(fileAndPrivateFileDTO);

            }

            return result;

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
            var result = await _context.PrivateFiles
                                        .Include(a => a.File)
                                        .SingleOrDefaultAsync(a => a.Id == id);
            if(result == null)
            {
                return false;
            }
            _context.RemoveRange(result.File);
            _context.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Data.File>> DownloadFile(List<int> ints)
        {
            var result = await _context.Files
                                .Include(a => a.PrivateFile)
                                .Where(f => ints.Contains(f.PrivateFilesId ?? 0) && f.FileData != null && f.FileData.Length > 0)
                                .OrderByDescending(f => f.PrivateFile.LastUpdate)
                                .ToListAsync();

            return result;
        }

        public async Task<List<PrivateFileDTO>> GetAll()
        {
            var result = await _context.PrivateFiles
                                        .Include(a => a.File)
                                        .Where(f => (f.File.FileName != null || f.File != null) && f.File.PrivateFile != null)
                                        .Select(a => new PrivateFileDTO
                                        {
                                            PrivateFileId = a.Id,
                                            Type = a.File.FileType,
                                            Name = a.File.FileName,
                                            Updator = a.Updator,
                                            Date = a.LastUpdate,
                                            Size = a.File.FileSize 
                                        })
                                        .OrderByDescending(a => a.Date)
                                        .ToListAsync();         
            return result;
        }

        public async Task<FileAndPrivateFileDTO> GetById(int id)
        {
            var result = await _context.PrivateFiles
                                    .Include(a => a.File)
                                    .SingleOrDefaultAsync(i => i.Id == id);
            if(result == null)
            {
                return new FileAndPrivateFileDTO();
            }
            var dto = new FileAndPrivateFileDTO
            {
                File = result.File,
                PrivateFile = result
            };
            return dto;
        }
        public async Task<List<string>> GetAllFileType()
        {
            var result = await _context.PrivateFiles
                                    .Include(a => a.File)
                                    .Where(f => f.File.FileType != null)
                                    .Select(s => s.File.FileType)
                                    .Where(fileType => !string.IsNullOrEmpty(fileType))
                                    .Distinct()
                                    .ToListAsync();
            return result;
        }
        public async Task<List<PrivateFileDTO>> GetPrivateFileByType(List<string> types)
        {
            var result = await _context.PrivateFiles
                                    .Include(a => a.File)
                                    .Where(f => types.Contains(f.File.FileType))
                                    .Select(x => new PrivateFileDTO
                                    {
                                        PrivateFileId = x.Id,
                                        Type = x.File.FileType,
                                        Name = x.File.FileName,
                                        Updator = x.Updator,
                                        Date = x.LastUpdate,
                                        Size = x.File.FileSize
                                    })
                                    .OrderByDescending(a => a.Date)
                                    .ToListAsync();
            return result;
        }

        public async Task<List<PrivateFileDTO>> Search(string searchString)
        {
            var result = await _context.PrivateFiles
                        .Include(a => a.File)
                        .Where(s => s.File.FileName.Contains(searchString) ||
                                    s.Updator.Contains(searchString))
                        .Select(s => new PrivateFileDTO
                        {
                            PrivateFileId = s.Id,
                            Type = s.File.FileType,
                            Name = s.File.FileName,
                            Updator = s.Updator,
                            Date = s.LastUpdate,
                            Size = s.File.FileSize
                        })
                        .OrderByDescending(d => d.Date)
                        .ToListAsync();
            //if (result == null)
            //{
            //    return new List<PrivateFileDTO>();
            //}
            return result;
        }

        public async Task<bool> ChangeFileName(string name, int id)
        {
            var result = await _context.PrivateFiles
                                    .Include(a => a.File)
                                    .SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            }
            var extension = result.File.FileType;
            result.File.FileName = $"{name}{extension}";

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
