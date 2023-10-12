using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LearnApi.ExceptionHandler
{
    public class HandleCustomExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is EntityException)
            {
                context.Result = new BadRequestObjectResult(context.Exception.Message); // Use any status code you prefer
                context.ExceptionHandled = true;
            }
        }
    }
}
