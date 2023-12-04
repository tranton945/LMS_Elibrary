using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace LMS_Elibrary.Services
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ElibraryDbContext _context;
        private readonly GetUser _user;

        public DocumentRepository(ElibraryDbContext context, GetUser user) 
        {
            _context = context;
            _user = user;
        } 
        public async Task<Document> Add(Document document)
        {
            var isuser = await _user.user();
            var doc = new Document
            {
                Name = document.Name,
                Type = document.Type,
                Creator = isuser.UserName,
                Date= DateTime.Now,
                Approved = null,
                LectureID = document.LectureID,
            };
            _context.Add(doc);
            await _context.SaveChangesAsync();
            return doc;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                return false;
            }
            _context.Documents.Remove(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Document>> GetAll()
        {
            var result = await _context.Documents.ToListAsync();
            return result;
        }

        public async Task<Document> GetById(int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return new Document();
            }
            return result;
        }

        public async Task<bool> Update(Document document, int id)
        {
            var result = await _context.Documents.SingleOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return false;
            }
            if(document != null)
            {
                result.Name = document.Name ?? result.Name;
                result.Type = document.Type ?? result.Type;
                result.LectureID = document.LectureID ?? result.LectureID;
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
