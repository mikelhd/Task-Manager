using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Entites;
using TaskManager.Models;
using static TaskManager.Models.TagModel;

namespace TaskManager.Services
{
    public interface ITagService
    {
        Task<List<GetTagResponse>> Get(GetTagRequest request);
        Task<GetTagResponse> GetById(int id);
        Task<ResponseBase> Save(SaveTagRequest request);
        Task<ResponseBase> Delete(int id);
    }
    public class TagService : ITagService
    {
        private readonly ApplicationDbContext _context;
        public TagService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GetTagResponse>> Get(GetTagRequest request)
        {
            var query = _context.Tags.AsQueryable();
            var response = await query.Select(x => new GetTagResponse
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            }).ToListAsync();
            return response;
        }
        public async Task<GetTagResponse> GetById(int id)
        {
            var query = _context.Tags.Where(x => x.Id == id);
            var response = await query.Select(x => new GetTagResponse
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            }).FirstOrDefaultAsync();
            return response;
        }

        public async Task<ResponseBase> Save(SaveTagRequest request)
        {
            Tag find = null;
            if (request.Id > 0)
            {
                find = await _context.Tags.FindAsync(request.Id);
            }
            if (find is null)
            {
                find = new Tag
                {
                    Id = request.Id.GetValueOrDefault(),
                    Name = request.Name,
                    IsActive = request.IsActive
                };
                _context.Tags.Add(find);
            }
            else
            {
                find.Name = request.Name;
                find.IsActive = request.IsActive;
                _context.Tags.Update(find);
            }
            await _context.SaveChangesAsync();
            return new ResponseBase(true);
        }
        public async Task<ResponseBase> Delete(int id)
        {
            var tag = _context.Tags.FirstOrDefault(x => x.Id == id);
            if (tag is null)
            {
                return new ResponseBase(false);
            }
            _context.Tags.Remove(tag);
            return new ResponseBase(true);
        }
    }
}
