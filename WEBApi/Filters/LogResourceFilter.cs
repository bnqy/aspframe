using Microsoft.AspNetCore.Mvc.Filters;

namespace WEBApi.Filters;

public class LogResourceFilter : Attribute, IResourceFilter
{
    // executed after model binding, action execution, result execution
    public void OnResourceExecuted(ResourceExecutedContext context) // contains info like result returned IActionResult infos
    {
        Console.WriteLine("Executed!");
    }

    //executed at the start of the pipline, after author filter
    public void OnResourceExecuting(ResourceExecutingContext context) // context contains HttpContext, routing details, info about current action
    {
        Console.WriteLine("Executing!!!");
    }
}
