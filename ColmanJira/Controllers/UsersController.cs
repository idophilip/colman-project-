using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ColmanJira.Data;

namespace ColmanJira.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly ApplicationDbContext _context;

        public UsersController(ILogger<UsersController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET Users/UsersRoles
        [HttpGet]
        public IEnumerable UsersRoles()
        {
            var usersRoles = (from u in _context.Users
                              let roles = (from ur in _context.UserRoles
                                           where ur.UserId.Equals(u.Id)
                                           join r in _context.Roles on ur.RoleId equals r.Id
                                           select r.Name)
                              select new { Name = u.UserName, Role = roles.First() })
                              .ToList();
            return usersRoles;
        }
    }
}
