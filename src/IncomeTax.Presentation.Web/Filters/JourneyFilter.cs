using IncomeTax.Application.Journey;
using IncomeTax.Presentation.Web.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IncomeTax.Presentation.Web.Filters;

public sealed class JourneyFilter(
    OnGetJourneyValidator onGetJourneyValidator,
    OnPostJourneyValidator onPostJourneyValidator) : IPageFilter
{
    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        if (!context.HttpContext.Request.Method.Equals(HttpMethods.Get)) return;
        
        string currentPage = context.ActionDescriptor.ViewEnginePath.TrimStart("/").ToString();
        if (onGetJourneyValidator.IsValid(currentPage, out string? redirectPage)) return;
        context.Result = new RedirectToPageResult(redirectPage);
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context) { }

    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
        if (!context.HttpContext.Request.Method.Equals(HttpMethods.Post)) return;
        
        string currentPage = context.ActionDescriptor.ViewEnginePath.TrimStart("/").ToString();
        if (onPostJourneyValidator.ShouldNavigateToCheckAnswers(currentPage))
            context.Result = new RedirectToPageResult(nameof(CheckAnswers));
    }
}