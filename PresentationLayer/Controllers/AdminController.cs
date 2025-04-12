using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Group;

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
            var groups = _unitOfWork.Groups.GetAll();
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

    }
}
