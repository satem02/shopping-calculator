using System.Globalization;

public static class CurrencyExtention
{
    public static string ToCurrencyString(this decimal value)
    {
        return value < 100 ? $"{(int) (value)}p" : (((decimal) value) / 100).ToString("C2",CultureInfo.CreateSpecificCulture("en-GB"));           
    }
}
