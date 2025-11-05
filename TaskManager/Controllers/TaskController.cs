using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskManager.Data;
using TaskManager.Data.Enum;
using TaskManager.Models;
using TaskManager.Services;
using static TaskManager.Models.TaskItem.GetTaskItem;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskItemService _service;
        public TaskController(ApplicationDbContext context, ITaskItemService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _service.Get(new GetTaskItemRequest { });
            var model = new TaskItemViewModel
            {
                ActiveTasks = response.Where(x => x.State != Status.done).ToList(),
                DoneTasks = response.Where(x => x.State == Status.done).ToList()
            };
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.Delete(id);
            if (response.IsSuccess is not true)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            var model = new SaveTaskItemRequest() { };
            if (id > 0)
            {
                var response = await _service.GetById(id.GetValueOrDefault());
                if (response is not null)
                {
                    model.Id = response.Id;
                    model.Title = response.Title;
                    model.State = response.State;
                    //if (!string.IsNullOrEmpty(model.DueDateString))
                    //{
                    //    // فرض کن در View فیلد date به شکل string (مثل "1404/08/13") میاد
                    //    var persianCalendar = new System.Globalization.PersianCalendar();
                    //    var parts = model.DueDateString.Split('/');
                    //    var year = int.Parse(parts[0]);
                    //    var month = int.Parse(parts[1]);
                    //    var day = int.Parse(parts[2]);
                    //    model.DueDate = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
                    //}
                    model.DueDate = response.DueDate;
                    model.CreatedDate = response.CreatedDate;
                    model.Description = response.Description;
                    model.TagIds = response.Tags.Select(t => t.Id).ToList();
                }
            }
            ViewBag.AllTags = new MultiSelectList(
                _context.Tags.Where(t => t.IsActive == true).ToList(),
                "Id",
                "Name",
                model.TagIds
            );
            ViewBag.State = Enum.GetValues(typeof(Status))
                .Cast<Status>()
                .Select(x => new SelectListItem
                {
                    Value = ((int)x).ToString(),
                    Text = x.ToString(),
                }).ToList();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaveTaskItemRequest request)
        {
            var response = await _service.Save(request);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MarkAsDone(int id)
        {
            var response = await _service.GetById(id);
            if (response is null)
                return NotFound();

            var current = response.State;

            if (current == Status.done)
            {
                response.State = Status.newTask;
            }
            else
            {
                response.State = Status.done;
                response.DoneDate = DateTime.UtcNow;
            }

            response.UpdatedDate = DateTime.UtcNow;

            var saveRequest = new SaveTaskItemRequest
            {
                Id = response.Id,
                Title = response.Title,
                State = response.State,
                DueDate = response.DueDate,
                DoneDate = response.DoneDate,
                CreatedDate = response.CreatedDate,
                UpdatedDate = response.UpdatedDate,
                Description = response.Description,
            };

            var saveResponse = await _service.Save(saveRequest);
            return RedirectToAction(nameof(Index));
        }
    }
}