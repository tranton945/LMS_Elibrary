using LMS_Elibrary.Data;
using Microsoft.EntityFrameworkCore;

namespace LMS_Elibrary.Services
{
    public class SubjectGroupRepository : ISubjectGroupRepository
    {
        private readonly ElibraryDbContext _context;

        public SubjectGroupRepository(ElibraryDbContext context) 
        {
            _context = context;
        }
        public async Task<SubjectGroup> Add(SubjectGroup SubjectGroup)
        {
            var a = new SubjectGroup
            {
                Name = SubjectGroup.Name,
            };
            _context.SubjectGroups.Add(a);
            await _context.SaveChangesAsync();
            return SubjectGroup;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Id == id);
            if(result == null) 
            {
                return false;
            }
            _context.SubjectGroups.Remove(result);
            return true;
        }

        public async Task<List<SubjectGroup>> GetAll()
        {
            var result = await _context.SubjectGroups.ToListAsync();
            return result;
        }

        public async Task<SubjectGroup> GetById(int id)
        {
            var result = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return new SubjectGroup();
            }
            return result;
        }

        public Task<List<SubjectGroup>> GetBySubjectGrouptId(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(SubjectGroup SubjectGroup, int id)
        {
            var result = await _context.SubjectGroups.SingleOrDefaultAsync(a => a.Id == id);
            if (result == null)
            {
                return false;
            }
            result.Name = SubjectGroup.Name ?? result.Name;
            return true;
        }
    }
}
