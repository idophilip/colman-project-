using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ColmanJira.Data;
using ColmanJira.Models;

namespace ColmanJira.Controllers
{
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ApplicationDbContext _context;

        public TasksController(ILogger<TasksController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET /Tasks
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] long? typeId, [FromQuery] long? statusId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            await GetAuth();
            List<IdentityUser> users = new List<IdentityUser>();
            List<IUser> extendedUsers = new List<IUser>();
            users = ViewBag.users;
            extendedUsers = ViewBag.extendedUsers;

            ViewBag.extendedUsers = extendedUsers;
            List<TaskType> taskTypes = await _context.TaskTypes.ToListAsync();
            List<Models.TaskStatus> taskStatuses = await _context.TaskStatuses.ToListAsync();
            List<Models.Task> tasks = await _context.Tasks.ToListAsync();

            ViewBag.users = users;
            ViewBag.taskTypes = taskTypes;
            ViewBag.taskStatuses = taskStatuses;
            for (int currTaskIndex = 0; currTaskIndex < tasks.Count; currTaskIndex++)
            {
                for (int currUserIndex = 0; currUserIndex < users.Count; currUserIndex++)
                {
                    if (tasks[currTaskIndex].UserId == users[currUserIndex].Id)
                    {
                        tasks[currTaskIndex].UserId = users[currUserIndex].Id;
                        tasks[currTaskIndex].User.UserName = users[currUserIndex].UserName;
                        tasks[currTaskIndex].UserId = users[currUserIndex].Id;
                    }
                }
            }

            ViewBag.tasks = tasks;
            bool isQuery = false;

            IEnumerable<Models.Task> filteredTasks = _context.Tasks;

            if (typeId.HasValue)
            {
                filteredTasks = _context.Tasks.Where(t => t.TypeId == typeId);
                filteredTasks = filteredTasks.ToList();
                isQuery = true;
            }

            if (statusId.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.StatusId == statusId);
                filteredTasks = filteredTasks.ToList();
                isQuery = true;
            }

            if (isQuery)
            {
                ViewBag.tasks = filteredTasks;
            }

            return View();
        }

        // GET Tasks/Filter?taskId=X&statusId=Y
        [HttpGet]
        public IEnumerable<Models.Task> Filter([FromQuery]long? typeId, [FromQuery] long? statusId)
        {
            IEnumerable<Models.Task> tasks = _context.Tasks;

            if (typeId.HasValue)
            {
                tasks = _context.Tasks.Where(t => t.TypeId == typeId);
            }

            if (statusId.HasValue)
            {
                tasks = tasks.Where(t => t.StatusId == statusId);
            }

            return tasks;
        }

        // GET Tasks/TasksCountByStatus
        [HttpGet]
        public IEnumerable TasksCountByStatus()
        {
            var statusesCount = _context.Tasks
                .GroupBy(t => new { Id = t.StatusId, Name = t.Status.Name })
                .Select(g => new { Name = g.Key.Name, Count = g.Count() })
                .ToList();

            return statusesCount;
        }

        // GET Tasks/TasksCountByUser
        [HttpGet]
        public IEnumerable TasksCountByUser()
        {
            var usersCount = _context.Tasks
                .GroupBy(t => new { Id = t.UserId, Name = t.User.UserName })
                .Select(g => new { Name = g.Key.Name, Value = g.Count() })
                .ToList();

            return usersCount;
        }

        // GET Tasks/TasksCountByType
        [HttpGet]
        public IEnumerable TasksCountByType()
        {

            var usersCount = _context.Tasks
                .GroupBy(t => new { Id = t.TypeId, Name = t.Type.Name })
                .Select(g => new { Name = g.Key.Name, Value = g.Count() })
                .ToList();

            return usersCount;
        }

        // GET /Tasks/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Models.Task task = await _context.Tasks.Include(e => e.Status).FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST /Tasks
        [HttpPost]
        public async Task<IActionResult> Create(Models.Task task)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // PUT /Tasks/Update
        [HttpPost]
        public async Task<IActionResult> Update(Models.Task task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE /Tasks/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            Models.Task task = new Models.Task() { Id = Id };

            if (Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Statistics()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            return View();
        }


        public async Task<List<IUser>> GetAuth()
        {
            List<IdentityRole> roles = await _context.Roles.ToListAsync();

            ViewBag.roles = roles;

            List<IdentityUserRole<string>> userRoles = await _context.UserRoles.ToListAsync();
            List<IdentityUser> users = await _context.Users.ToListAsync();
            List<IUser> extendedUsers = new List<IUser>();

            for (int currUser = 0; currUser < users.Count; currUser++)
            {
                for (int currUserRole = 0; currUserRole < userRoles.Count; currUserRole++)
                {
                    if (users[currUser].Id == userRoles[currUserRole].UserId)
                    {
                        for (int currRole = 0; currRole < roles.Count; currRole++)
                        {
                            if (roles[currRole].Id == userRoles[currUserRole].RoleId)
                            {
                                bool isManager = roles[currRole].Id == "1";

                                extendedUsers.Add(new IUser(userRoles[currUserRole].RoleId, userRoles[currUserRole].UserId, users[currUser].Email, roles[currRole].Name, isManager));
                            }
                        }
                    }
                }
            }
            for (int currUser = 0; currUser < extendedUsers.Count; currUser++)
            {
                if (User.Identity.Name == extendedUsers[currUser].Email)
                {
                    if (extendedUsers[currUser].isManager)
                    {
                        ViewBag.isManager = true;
                        HttpContext.Session.SetString("isManager", "1");
                    }
                    else
                    {
                        ViewBag.isManager = false;
                        HttpContext.Session.SetString("isManager", "0");
                    }
                }
            }

            ViewBag.users = users;
            ViewBag.extendedUsers = extendedUsers;
            return extendedUsers;
        }
    }
}
