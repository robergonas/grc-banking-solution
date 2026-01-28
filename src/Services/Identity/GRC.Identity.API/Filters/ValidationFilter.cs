using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GRC.Identity.API.Filters;

/// <summary>
/// Filtro para validación automática de modelos
/// </summary>
public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var response = new ValidationErrorResponse
            {
                Message = "Validation failed",
                Errors = errors
            };

            context.Result = new BadRequestObjectResult(response);
        }
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}
public class ValidationErrorResponse
{
    public string Message { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }
}