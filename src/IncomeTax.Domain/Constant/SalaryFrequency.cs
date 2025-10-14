using System.Collections.Immutable;

namespace IncomeTax.Domain.Constant;

public enum SalaryFrequency
{
    Yearly,
    Monthly,
    EveryFourWeeks,
    Weekly,
    Daily,
    Hourly
}

public static class SalaryFrequencyExtensions
{
    private const string Yearly = "Yearly";
    private const string Monthly = "Monthly";
    private const string EveryFourWeeks = "Every 4 weeks";
    private const string Weekly = "Weekly";
    private const string Daily = "Daily";
    private const string Hourly = "Hourly";

    public static readonly ImmutableDictionary<string, SalaryFrequency> SalaryPageRadioOptionToEnum =
        ImmutableDictionary.CreateRange<string, SalaryFrequency>([
            new KeyValuePair<string, SalaryFrequency>(Yearly, SalaryFrequency.Yearly),
            new KeyValuePair<string, SalaryFrequency>(Monthly, SalaryFrequency.Monthly),
            new KeyValuePair<string, SalaryFrequency>(EveryFourWeeks, SalaryFrequency.EveryFourWeeks),
            new KeyValuePair<string, SalaryFrequency>(Weekly, SalaryFrequency.Weekly),
            new KeyValuePair<string, SalaryFrequency>(Daily, SalaryFrequency.Daily),
            new KeyValuePair<string, SalaryFrequency>(Hourly, SalaryFrequency.Hourly)
        ]);

    public static readonly ImmutableDictionary<SalaryFrequency, string> CheckAnswersPageGrossIncomeFriendlySuffix =
        ImmutableDictionary.CreateRange<SalaryFrequency, string>([
            new KeyValuePair<SalaryFrequency, string>(SalaryFrequency.Yearly, " a year"),
            new KeyValuePair<SalaryFrequency, string>(SalaryFrequency.Monthly, " a month"),
            new KeyValuePair<SalaryFrequency, string>(SalaryFrequency.EveryFourWeeks, " every 4 weeks"),
            new KeyValuePair<SalaryFrequency, string>(SalaryFrequency.Weekly, " a week"),
            new KeyValuePair<SalaryFrequency, string>(SalaryFrequency.Daily, " a day"),
            new KeyValuePair<SalaryFrequency, string>(SalaryFrequency.Hourly, " an hour")
        ]);

    public static readonly string[] SalaryPageRadioSet =
    [
        Yearly,
        Monthly,
        EveryFourWeeks,
        Weekly,
        Daily,
        Hourly
    ];
}