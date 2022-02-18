using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeamEventRegistration.Core.Models;

namespace TeamEventRegistration.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly TeamEventRegistrationDbContext _context;

        public List<Event> Events { get; set; }
        public string CurrentUserId {get; set;}

        public IndexModel(ILogger<IndexModel> logger,
            TeamEventRegistrationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (CurrentUserId != null)
            {
                Events = _context.Events
                    .Where(e => e.Active && e.RegistrationStart < DateTime.Now && e.RegistrationEnd > DateTime.Now)
                    .Include(e => e.Registrations)
                        .ThenInclude(r => r.Team)
                    .ToList();
            }
            return Page();
        }
    }
}
