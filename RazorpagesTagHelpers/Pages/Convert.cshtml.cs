using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RazorpagesTagHelpers.Pages
{
    public class ConvertModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public SelectListItem[] CurrencyCodes { get; } =
        {
            new SelectListItem{Text="GBP", Value = "GBP"},
            new SelectListItem{Text="USD", Value = "USD"},
            new SelectListItem{Text="CAD", Value = "CAD"},
            new SelectListItem{Text="EUR", Value = "EUR"},
        };

        public void OnGet()
        {
            Input = new InputModel();
        }

        public IActionResult OnPost()
        {
            if (Input.CurrencyFrom == Input.CurrencyTo)
            {
                ModelState.AddModelError(string.Empty, "Can not convert currency to itself");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            return RedirectToPage("Success");
        }

        public class InputModel
        {
            [Required]
            [StringLength(3, MinimumLength = 3)]
            [Display(Name = "Currency From")]
            [CurrencyCode("GBP", "USD", "CAD", "EUR")]
            public string CurrencyFrom { get; set; }

            [Required]
            [CurrencyCode("GBP", "USD", "CAD", "EUR")]
            [Display(Name = "Currency To")]
            [StringLength(3, MinimumLength = 3)]
            public string CurrencyTo { get; set; }

            [Required]
            [Range(1, 1000)]
            public int Quantity { get; set; }
        }
    }
}
