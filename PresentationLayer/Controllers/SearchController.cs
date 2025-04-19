using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Tool;

namespace PresentationLayer.Controllers
{
    public class SearchController(IUnitOfWork _unitOfWork, ILogger<AuthController> logger) : Controller
    {
        public IActionResult Index(string searchTerm = "", int groupId = 0, int page = 1)
        {
            int pageSize = 10;

            var toolsQuery = _unitOfWork.Tools.GetAllWithGroup()
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                toolsQuery = toolsQuery.Where(t => t.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            if (groupId != 0)
            {
                toolsQuery = toolsQuery.Where(t => t.GroupId == groupId);
            }

            var tools = toolsQuery
                .Where(t => !t.IsDisabled)
                .OrderBy(t => t.ToolId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var totalToolsCount = toolsQuery.Count();

            var model = new ToolsViewModel
            {
                Tools = tools,
                Groups = _unitOfWork.Groups.GetAll().ToList(),
                CurrentSearchTerm = searchTerm,
                CurrentGroupId = groupId,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling((double)totalToolsCount / pageSize)
            };

            return View(model);
        }
    }
}
