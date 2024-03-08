using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesModelBinding.Pages
{
    [IgnoreAntiforgeryToken] // to call from postman
    public class EditProductModel : PageModel
    {
        public ProductModel Product { get; set; }

        public void OnGet()
        {
        }

        public void OnPost(ProductModel product) 
        { 
            Product = product;
        }

        public void OnPostEditTwoProducts(ProductModel product1, ProductModel product2)
        {
            Product = product1;
        }

	}
}
