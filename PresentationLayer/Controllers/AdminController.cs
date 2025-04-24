using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Models.Group;
using PresentationLayer.Models.Tool;
using PresentationLayer.Models.User;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController(IUnitOfWork _unitOfWork, ILogger<AuthController> logger, UserManager<ApplicationUser> userManager) : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("PremiumRequests");
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
        public IActionResult Tools(string searchTerm = "", int groupId = 0, int page = 1, string tab = "active")
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

            if (tab == "disabled")
            {
                toolsQuery = toolsQuery.Where(t => t.IsDisabled);
            }
            else
            {
                toolsQuery = toolsQuery.Where(t => !t.IsDisabled);
            }

            var tools = toolsQuery
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
                TotalPages = (int)Math.Ceiling((double)totalToolsCount / pageSize),
                Tab = tab
            };

            return View(model);
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
                Groups = groups,
                IsPremium = tool.IsPremium,
                IsDisabled = tool.IsDisabled
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
                tool.IsPremium = model.IsPremium;
                tool.IsDisabled = model.IsDisabled;

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

        public async Task<IActionResult> Users(int page = 1, string search = "", string roleFilter = "All")
        {
            const int pageSize = 10;
            var usersQuery = userManager.Users.AsQueryable();

            // Search by username or email
            if (!string.IsNullOrEmpty(search))
            {
                usersQuery = usersQuery.Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            }

            // Filter by role
            if (roleFilter != "All")
            {
                usersQuery = usersQuery.Where(u => userManager.GetRolesAsync(u).Result.Contains(roleFilter));
            }

            // Pagination
            var totalUsers = await usersQuery.CountAsync();
            var users = await usersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // Prepare the model
            var model = new UserListViewModel
            {
                Users = users,
                TotalUsers = totalUsers,
                CurrentPage = page,
                PageSize = pageSize,
                SearchQuery = search,
                RoleFilter = roleFilter,
                Roles = ["User", "Premium"]
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Contains("Admin")) return Forbid();

            if (newRole == "Premium" && !currentRoles.Contains("Premium"))
            {
                await userManager.AddToRoleAsync(user, "Premium");
                await userManager.RemoveFromRoleAsync(user, "User");
            }
            else if (newRole == "User" && !currentRoles.Contains("User"))
            {
                await userManager.AddToRoleAsync(user, "User");
                await userManager.RemoveFromRoleAsync(user, "Premium");
            }

            user.PremiumRequest = null;
            await userManager.UpdateAsync(user);

            return RedirectToAction("Users");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Users");
            }

            return View("Error", result.Errors);
        }
        public async Task<IActionResult> PremiumRequests(int page = 1)
        {
            const int pageSize = 10;
            var usersQuery = userManager.Users.Where(u => u.PremiumRequest == "Pending").AsQueryable();

            var totalUsers = await usersQuery.CountAsync();
            var users = await usersQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var model = new PremiumRequestViewModel
            {
                Users = users,
                TotalUsers = totalUsers,
                CurrentPage = page,
                PageSize = pageSize
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePremiumRequest(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Contains("Premium"))
            {
                await userManager.AddToRoleAsync(user, "Premium");
                await userManager.RemoveFromRoleAsync(user, "User");
            }

            user.PremiumRequest = "Approved";
            await userManager.UpdateAsync(user);

            return RedirectToAction("PremiumRequests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectPremiumRequest(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            user.PremiumRequest = "Rejected";
            await userManager.UpdateAsync(user);

            return RedirectToAction("PremiumRequests");
        }
    }
}
