using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RazorPagesModelBinding.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public UserBindingModel Input { get; set; }
        
        public void OnGet()
        {

        }

        // no need to add parameter UserBindingModel
        // bc [BindProperty] public UserBindingModel Input { get; set; } added
        public IActionResult OnPost()
        {
            // here goes invalid model validation
            // if validation fails it returns errors
            if (!ModelState.IsValid)
            {
                return Page();  // if is not valid the form redisplays
            }
            // takeaway is to check modelstate is valid or not and then
            // transaction afterwards

            //return Page();
            return RedirectToPage("Success");
        }

        public class UserBindingModel
        {
            [Required]
            [StringLength(100)]
            [Display(Name = "Your name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(100)]
            [Display(Name = "Last name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}
