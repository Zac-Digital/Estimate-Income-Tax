using System.Globalization;
using System.Text;
using System.Text.Json;
using IncomeTax.Domain;
using IncomeTax.Domain.Constant;
using Microsoft.AspNetCore.Http;

namespace IncomeTax.Application;

public class UserService
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly IHttpContextAccessor _contextAccessor;

    public UserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void UpdateSalary(double amount, string frequency) =>
        Serialise(Journey.Salary,
            new SalaryDto(amount, SalaryFrequencyExtensions.SalaryPageRadioOptionToEnum[frequency]));

    public void UpdateStatePension(bool isOverStatePensionAge) =>
        Serialise(Journey.StatePension, new StatePensionDto(isOverStatePensionAge));

    public string GetGrossIncome()
    {
        SalaryDto salaryDto = Deserialise<SalaryDto>(Journey.Salary);
        return
            $"{salaryDto.Amount.ToString("C", CultureInfo.CreateSpecificCulture("en-GB")).Replace(".00", "")}{SalaryFrequencyExtensions.CheckAnswersPageGrossIncomeFriendlySuffix[salaryDto.Frequency]}";
    }

    public string GetIsOverStatePensionAge() =>
        Deserialise<StatePensionDto>(Journey.StatePension).IsOverStatePensionAge ? "Yes" : "No";

    private void Serialise<T>(Journey journeyStage, T journeyDto) where T : JourneyDto
    {
        string journeyJson = JsonSerializer.Serialize(journeyDto, JsonSerializerOptions);
        byte[] journeyBytes = Encoding.UTF8.GetBytes(journeyJson);
        _contextAccessor.HttpContext.Session.Set(journeyStage.ToString(), journeyBytes);
    }

    private T Deserialise<T>(Journey journeyStage) where T : JourneyDto
    {
        _contextAccessor.HttpContext.Session.TryGetValue(journeyStage.ToString(), out byte[] journeyBytes);
        string journeyJson = Encoding.UTF8.GetString(journeyBytes);
        return JsonSerializer.Deserialize<T>(journeyJson, JsonSerializerOptions)!;
    }
}