using Microsoft.AspNetCore.Mvc;
using TaskManager.Services;
using static TaskManager.Models.TagModel;

namespace TaskManager.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _service;
        public TagController(ITagService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var response = await _service.Get(new GetTagRequest { });
            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> Save(int? id)
        {
            var model = new SaveTagRequest() { };
            if (id > 0)
            {
                var response = await _service.GetById(id.GetValueOrDefault());
                if (response is not null)
                {
                    model.Id = response.Id;
                    model.Name = response.Name;
                    model.IsActive = response.IsActive;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save(SaveTagRequest request)
        {
            var response = await _service.Save(request);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
