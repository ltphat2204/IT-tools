using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Group;
using PresentationLayer.Models.Tool;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class AdminController(IUnitOfWork _unitOfWork, ILogger<AuthController> logger) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Groups()
        {
            var groups = _unitOfWork.Groups.GetAllWithTools().ToList();
            return View(groups);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(GroupCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newGroup = new Group
                {
                    GroupName = model.GroupName,
                };

                _unitOfWork.Groups.Add(newGroup);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Groups");
            }

            return View("Groups");
        }
        [HttpPost]
        public async Task<IActionResult> EditGroup(GroupEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = _unitOfWork.Groups.GetById(model.GroupId);
                if (group != null)
                {
                    group.GroupName = model.GroupName;
                    await _unitOfWork.SaveChangesAsync();
                }

                return RedirectToAction("Groups");
            }

            return View("Groups");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var group = _unitOfWork.Groups.GetById(groupId);
            if (group != null)
            {
                _unitOfWork.Groups.Remove(group);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToAction("Groups");
        }
        public IActionResult Tools()
        {
            var tools = _unitOfWork.Tools.GetAllWithGroup().ToList();
            return View(tools);
        }
        public IActionResult CreateTool()
        {
            var groups = _unitOfWork.Groups.GetAll();
            var model = new ToolCreateViewModel { Groups = groups };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTool(ToolCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = _unitOfWork.Groups.GetById(model.GroupId);
                if (group == null)
                {
                    ModelState.AddModelError("GroupId", "Group not found.");
                    model.Groups = _unitOfWork.Groups.GetAll();
                    return View(model);
                }

                var newTool = new Tool
                {
                    Name = model.Name,
                    Description = model.Description,
                    UIStringHtml = model.UIStringHtml,
                    Libraries = model.Libraries,
                    Process = model.Process,
                    GroupId = model.GroupId,
                    Group = group
                };

                _unitOfWork.Tools.Add(newTool);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Tools");
            }

            foreach (var key in ModelState.Keys)
            {
                var value = ModelState[key];
                if (value?.Errors.Count > 0)
                {
                    foreach (var error in value.Errors)
                    {
                        Debug.WriteLine($"Error in field {key}: {error.ErrorMessage}");
                    }
                }
            }

            model.Groups = _unitOfWork.Groups.GetAll();
            return View(model);
        }
        public IActionResult EditTool(int id)
        {
            var tool = _unitOfWork.Tools.GetById(id);
            if (tool == null)
            {
                return NotFound();
            }

            var groups = _unitOfWork.Groups.GetAll();

            var model = new ToolEditViewModel
            {
                ToolId = tool.ToolId,
                Name = tool.Name,
                Description = tool.Description,
                UIStringHtml = tool.UIStringHtml?.Replace("`", @"\`").Replace("$", @"\$"),
                Libraries = tool.Libraries,
                Process = tool.Process?.Replace("`", @"\`").Replace("$", @"\$"),
                GroupId = tool.GroupId,
                Groups = groups
            };


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTool(ToolEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tool = _unitOfWork.Tools.GetById(model.ToolId);
                if (tool == null)
                {
                    return NotFound();
                }

                tool.Name = model.Name;
                tool.Description = model.Description;
                tool.UIStringHtml = model.UIStringHtml;
                tool.Libraries = model.Libraries;
                tool.Process = model.Process;
                tool.GroupId = model.GroupId;

                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Tools");
            }

            foreach (var key in ModelState.Keys)
            {
                var value = ModelState[key];
                if (value?.Errors.Count > 0)
                {
                    foreach (var error in value.Errors)
                    {
                        Debug.WriteLine($"Error in field {key}: {error.ErrorMessage}");
                    }
                }
            }

            model.Groups = _unitOfWork.Groups.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTool(int toolId)
        {
            var tool = _unitOfWork.Tools.GetById(toolId);
            if (tool != null)
            {
                _unitOfWork.Tools.Remove(tool);
                await _unitOfWork.SaveChangesAsync();
            }

            return RedirectToAction("Tools");
        }
    }
}
