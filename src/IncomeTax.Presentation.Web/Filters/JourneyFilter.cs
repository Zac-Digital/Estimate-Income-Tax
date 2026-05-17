using IncomeTax.Application.Journey;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IncomeTax.Presentation.Web.Filters;

public sealed class JourneyFilter(JourneyValidator journeyValidator) : IPageFilter
{
    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        if (context.HttpContext.Request.Method.Equals(HttpMethods.Post)) return;

        string currentPage = context.ActionDescriptor.ViewEnginePath.TrimStart("/").ToString();
        bool isValid = journeyValidator.IsValid(currentPage, out string? redirectPage);

        if (isValid) return;
        
        context.Result = new RedirectToPageResult(redirectPage);
    }
    
    public void OnPageHandlerSelected(PageHandlerSelectedContext context) {}
    public void OnPageHandlerExecuted(PageHandlerExecutedContext context) {}
}