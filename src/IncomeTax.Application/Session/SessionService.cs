using System.Diagnostics.CodeAnalysis;
using IncomeTax.Domain.Journey;
using Microsoft.AspNetCore.Http;

namespace IncomeTax.Application.Session;

[ExcludeFromCodeCoverage(Justification = "Wrapper Class - Session Storage")]
public sealed class SessionService(IHttpContextAccessor accessor)
{
    public void Update(JourneyStage stage, string value) =>
        accessor.HttpContext.Session.SetString(stage.ToString(), value);

    public string? Get(JourneyStage stage) =>
        accessor.HttpContext.Session.GetString(stage.ToString());
    
    public void Remove(JourneyStage stage) => 
        accessor.HttpContext.Session.Remove(stage.ToString());
}