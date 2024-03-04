using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesApp.Pages
{
    public class CategoryModel : PageModel
    {
        private readonly ToDoService _service;

        public List<ToDoListModel> Items { get; set; }

        public CategoryModel(ToDoService service)
        {
            _service = service;
        }

        public ActionResult OnGet(string category)
        {
            Items = _service.GetItemsByCategory(category);

            return Page();
        }
    }
}
