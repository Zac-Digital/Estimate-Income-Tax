using System.Text;
using System.Text.Json;
using IncomeTax.Domain;
using IncomeTax.Domain.Constant;
using Microsoft.AspNetCore.Http;

namespace IncomeTax.Application.Session;

public sealed class SessionService
{
    private readonly IHttpContextAccessor _accessor;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public SessionService(IHttpContextAccessor accessor) { _accessor = accessor; }
    
    public void Serialise<T>(JourneyStage journeyStage, T journeyDto) where T : JourneyDto
    {
        string journeyJson = JsonSerializer.Serialize(journeyDto, JsonSerializerOptions);
        byte[] journeyBytes = Encoding.UTF8.GetBytes(journeyJson);
        _accessor.HttpContext.Session.Set(journeyStage.ToString(), journeyBytes);
    }

    public T Deserialise<T>(JourneyStage journeyStage) where T : JourneyDto
    {
        _accessor.HttpContext.Session.TryGetValue(journeyStage.ToString(), out byte[] journeyBytes);
        string journeyJson = Encoding.UTF8.GetString(journeyBytes);
        return JsonSerializer.Deserialize<T>(journeyJson, JsonSerializerOptions)!;
    }
}