using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesModelBinding.Pages
{
    public class CalculatorModel : PageModel
    {
        public int Square { get; set; }
        public int Input { get; set; }
        public void OnGet(int number)
        {
            Square = number * number;
            Input = number;
        }
    }
}
