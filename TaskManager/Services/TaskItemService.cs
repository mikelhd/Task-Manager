using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Data.Entites;
using TaskManager.Models;
using TaskManager.Models.TaskItem;

namespace TaskManager.Services
{
    public interface ITaskItemService
    {
        Task<List<GetTaskItem.GetTaskItemResponse>> Get(GetTaskItem.GetTaskItemRequest request);
        Task<GetTaskItem.GetTaskItemResponse> GetById(int id);
        Task<ResponseBase> Save(GetTaskItem.SaveTaskItemRequest request);
        Task<ResponseBase> Delete(int id);
    }
    public class TaskItemService : ITaskItemService
    {
        private readonly ApplicationDbContext _context;
        public TaskItemService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<GetTaskItem.GetTaskItemResponse>> Get(GetTaskItem.GetTaskItemRequest request)
        {
            var query = _context.TaskItems.AsQueryable();

            var response = await query.Select(x => new GetTaskItem.GetTaskItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                State = x.State,
                DueDate = x.DueDate,
                DoneDate = x.DoneDate,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                UpdatedDate = x.UpdatedDate,
            }).ToListAsync();
            return response;
        }

        public async Task<GetTaskItem.GetTaskItemResponse> GetById(int id)
        {
            var query = _context.TaskItems.Where(x => x.Id == id);

            var response = await query.Select(x => new GetTaskItem.GetTaskItemResponse
            {
                Id = x.Id,
                Title = x.Title,
                State = x.State,
                DueDate = x.DueDate,
                DoneDate = x.DoneDate,
                CreatedDate = x.CreatedDate,
                Description = x.Description,
                UpdatedDate = x.UpdatedDate
            }).FirstOrDefaultAsync();
            return response;
        }

        public async Task<ResponseBase> Save(GetTaskItem.SaveTaskItemRequest request)
        {
            TaskItem find = null;
            if (request.Id > 0 && request.Id is not null)
            {
                find = await _context.TaskItems.FindAsync(request.Id);
            }
            if (find is null)
            {
                find = new TaskItem
                {
                    Title = request.Title,
                    State = request.State,
                    DueDate = request.DueDate,
                    DoneDate = request.DoneDate,
                    CreatedDate = DateTime.Now,
                    Description = request.Description,
                    UpdatedDate = request.UpdatedDate,
                };
                _context.TaskItems.Add(find);
            }
            else
            {
                find.Title = request.Title;
                find.State = request.State;
                find.DueDate = request.DueDate;
                find.DoneDate = request.DoneDate;
                find.Description = request.Description;
                find.CreatedDate = DateTime.Now;
                find.UpdatedDate = request.UpdatedDate;
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
            return new ResponseBase(true);
        }
    }
}
