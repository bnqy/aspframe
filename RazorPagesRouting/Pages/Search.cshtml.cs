using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesRouting.Pages
{
    public class SearchModel : PageModel
    {
        private readonly ProductService _service;
        private readonly LinkGenerator _linkGenerator;

        public SearchModel(ProductService service, LinkGenerator linkGenerator)
        {
            _service = service;
            _linkGenerator = linkGenerator;
        }

        [BindProperty, Required]
        public string SearchTerm { get; set; }  // this is model bound
        public List<Product> Results { get; private set; }

       
        public void OnGet()
        {
            var url1 = Url.Page("ProductDetails/Index", new { name = "big-widget" });
            var url2 = _linkGenerator.GetPathByPage("/ProductDetails/Index", values: new { name = "big-widget" });
            var url3 = _linkGenerator.GetPathByPage(HttpContext, "/ProductDetails/Index", values: new { name = "big-widget" });
            var url4 = _linkGenerator.GetUriByPage(page: "/ProductDetails/Index",
                handler: null,
                values: new { name = "big-widget" },
                scheme: "https",
                host: new HostString("www.example.com"));

            Console.Write(@"{0}
{1}
{2}
{3}", url1, url2, url3, url4) ;
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)   // checks if models are valid
            {
                Results = _service.Search(SearchTerm, StringComparison.Ordinal);

                return Page();  // returns PageResult
            }

            return RedirectToPage("./Index"); // returns RedirectToPage and reditrects to /index
        }
        public void OnPostIgnoreCase()
        {
            if (ModelState.IsValid)
            {
                Results = _service.Search(SearchTerm, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
