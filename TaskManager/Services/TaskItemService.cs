using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TaskManager.Data;
using TaskManager.Data.Entites;
using TaskManager.Models;
using static TaskManager.Models.TagModel;
using static TaskManager.Models.TaskItem.GetTaskItem;

namespace TaskManager.Services
{
    public interface ITaskItemService
    {
        Task<List<GetTaskItemResponse>> Get(GetTaskItemRequest request);
        Task<GetTaskItemResponse> GetById(int id);
        Task<ResponseBase> Save(SaveTaskItemRequest request);
        Task<ResponseBase> Delete(int id);
    }
    public class TaskItemService : ITaskItemService
    {
        private readonly ApplicationDbContext _context;
        public TaskItemService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<GetTaskItemResponse>> Get(GetTaskItemRequest request)
        {
            var query = _context.TaskItems.Include(t => t.Tags).AsQueryable();

            var response = await query.Select(x => new GetTaskItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                State = x.State,
                DueDate = x.DueDate,
                DoneDate = x.DoneDate,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                UpdatedDate = x.UpdatedDate,
                Tags = x.Tags.Select(t => new GetTagResponse
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            }).ToListAsync();
            return response;
        }

        public async Task<GetTaskItemResponse> GetById(int id)
        {
            var query = _context.TaskItems.Where(x => x.Id == id);

            var response = await query.Select(x => new GetTaskItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                State = x.State,
                DueDate = x.DueDate,
                DoneDate = x.DoneDate,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                UpdatedDate = x.UpdatedDate,
                Tags = x.Tags.Select(t => new GetTagResponse
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            }).FirstOrDefaultAsync();
            return response;
        }

        public async Task<ResponseBase> Save(SaveTaskItemRequest request)
        {
            DateTime? dueDateMiladi = null;

            if (!string.IsNullOrEmpty(request.DueDate))
            {
                try
                {
                    // پشتیبانی از انواع جداکننده (/, -)
                    var parts = request.DueDate.Split('/', '-', '.');
                    int year = int.Parse(parts[0]);
                    int month = int.Parse(parts[1]);
                    int day = int.Parse(parts[2]);

                    var persian = new PersianCalendar();
                    dueDateMiladi = persian.ToDateTime(year, month, day, 0, 0, 0, 0);
                }
                catch
                {
                    dueDateMiladi = null;
                }
            }
            TaskItem find = null;
            if (request.Id > 0 && request.Id is not null)
            {
                find = await _context.TaskItems.Include(t => t.Tags).FirstOrDefaultAsync(t => t.Id == request.Id);
            }
            if (find is null)
            {
                find = new TaskItem
                {
                    Title = request.Title,
                    State = request.State,
                    DueDate = dueDateMiladi.GetValueOrDefault(),
                    CreatedDate = DateTime.Now,
                    Description = request.Description,
                };
                if (request.TagIds is not null && request.TagIds.Any())
                {
                    var tags = await _context.Tags.Where(t => request.TagIds.Contains(t.Id)).ToListAsync();
                    find.Tags = tags;
                }
                _context.TaskItems.Add(find);
            }
            else
            {
                find.Title = request.Title;
                find.State = request.State;
                find.DueDate = dueDateMiladi.GetValueOrDefault();
                find.Description = request.Description;
                find.UpdatedDate = DateTime.Now;
                find.Tags.Clear();
                if (request.TagIds is not null && request.TagIds.Any())
                {
                    var tags = await _context.Tags.Where(t => request.TagIds.Contains(t.Id)).ToListAsync();
                    foreach (var tag in tags)
                    {
                        find.Tags.Add(tag);
                    }
                }
                _context.TaskItems.Update(find);
            }
            await _context.SaveChangesAsync();
            return new ResponseBase(true);
        }
        public async Task<ResponseBase> Delete(int id)
        {
            var task = _context.TaskItems.FirstOrDefault(x => x.Id == id);
            if (task is null)
            {
                return new ResponseBase(false);
            }
            _context.TaskItems.Remove(task);
            _context.SaveChangesAsync();
            return new ResponseBase(true);
        }
    }
}
