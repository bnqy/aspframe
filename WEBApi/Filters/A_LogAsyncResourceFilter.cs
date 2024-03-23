using Microsoft.AspNetCore.Mvc.Filters;

namespace WEBApi.Filters
{
    public class A_LogAsyncResourceFilter : Attribute, IAsyncResourceFilter
    {
        // Execured at the start of the pipline after author
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, 
            ResourceExecutionDelegate next) // encapsulates the remainder of the pipline
        {
            Console.WriteLine("Executing async!!!");

            // executes rest of the pipe and gets ResourceExecutedContext
            ResourceExecutedContext resourceExecutedContext = await next();
            Console.WriteLine("Executed async!"); // called at the end
        }
    }
}
