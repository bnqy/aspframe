using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesView.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private static readonly List<string> _users = new List<string>
		{
			"Bart",
			"Jimmy",
			"Robbie"
		};


		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		[BindProperty, Required]
		public string NewUser { get; set; }
		public List<String> ExistingUsers { get; set; }

		public void OnGet()
		{
			ExistingUsers = _users;
		}

		public IActionResult OnPost()
		{
			ExistingUsers = _users;

			if (!ModelState.IsValid)
			{
				return Page();
			}
			if (ExistingUsers.Contains(NewUser))
			{
				ModelState.AddModelError(nameof(NewUser), "This already exists!");
				return Page();
			}

			_users.Insert(0, NewUser);
			return RedirectToPage();
		}
	}
}