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
        public string SearchTerm { get; set; }
        public List<Product> Results { get; private set; }

       
        public void OnGet()
        {
            var url1 = Url.Page("ProductDetails/Index", new { name = "big-widget" });
            var url2 = _linkGenerator.GetPathByPage("/ProductDetails/Index", values: new { name = "big-widget" });
            var url3 = _linkGenerator.GetPathByPage(HttpContext, "/ProductDetails/Index", values: new { name = "big-widget" });
            var url4 = _linkGenerator.GetUriByPage(page: "/ProductDetails/Index",
                handler: null,
                values: new { id = 5},
                scheme: "https",
                host: new HostString("www.example.com"));
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Results = _service.Search(SearchTerm, StringComparison.Ordinal);
            }

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
