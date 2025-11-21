using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
public class AlphaClient
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public AlphaClient(HttpClient http)
    {
        _http = http;
        _apiKey = Environment.GetEnvironmentVariable("API_KEY") ?? throw new Exception("API_KEY not found in environment variables");
    }

    public async Task<List<DayModel>> GetDailyAverages(string symbol)
    {
        var intraday = await GetIntraday(symbol);

        return intraday
            .GroupBy(c => c.Day)
            .Select(g => new DayModel
            {
                Day = g.Key,
                LowAverage = g.Average(c => c.LowAverage),
                HighAverage = g.Average(c => c.HighAverage),
                Volume = g.Sum(c => c.Volume),
            })
            .OrderByDescending(d => d.Day)
            .ToList();
    }

    public async Task<List<DayModel>> GetIntraday(string symbol)
    {
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=15min&outputsize=full&apikey={_apiKey}";


        var json = await _http.GetStringAsync(url);

        using var doc = JsonDocument.Parse(json);

        // Validate Response
        ValidateAlphaVantageResponse(doc);

        // Skip the metadata and get the time series data directly
        string timeSeriesKey = doc.RootElement
            .EnumerateObject()
            .FirstOrDefault(prop => prop.Name.StartsWith("Time Series"))
            .Name;
        var series = doc.RootElement.GetProperty(timeSeriesKey);

        // Parse the data into DayModel objects
        var days = new List<DayModel>();

        // Iterate through each timestamp entry
        foreach (var interval in series.EnumerateObject())
        {
            var timestamp = DateTime.Parse(interval.Name);
            var day = timestamp.ToString("yyyy-MM-dd");
            var data = interval.Value;

            // Assumption that there could be nulls.
            var high = GetDouble(data, "2. high");
            var low = GetDouble(data, "3. low");
            var volume = GetInt(data, "5. volume");

            days.Add(new DayModel
            {
                Day = DateOnly.Parse(day),
                LowAverage = low,
                HighAverage = high,
                Volume = volume,
            });
        }

        return days;
    }

    // Parser helper methods
    private static double GetDouble(JsonElement obj, string property)
    {
        // If the property exists and can be parsed, return the double value;
        // otherwise, return 0.0
        return obj.TryGetProperty(property, out var val)
            && val.GetString() is string str
            && double.TryParse(str , out double d)
            ? d : 0.0;
    }

    private static int GetInt(JsonElement obj, string property)
    {
        // If the property exists and can be parsed, return the int value;
        // otherwise, return 0
        return obj.TryGetProperty(property, out var val)
            && val.GetString() is string str
            && int.TryParse(str , out int i)
            ? i : 0;
    }

    // Error Response Helper
    private static void ValidateAlphaVantageResponse(JsonDocument doc)
    {
        if (doc.RootElement.TryGetProperty("Error Message", out var error))
        {
            throw new Exception($"Alpha Vantage API Error: {error.GetString()}");
        }

        if (doc.RootElement.TryGetProperty("Note", out var note))
        {
            throw new Exception($"Alpha Vantage API Note: {note.GetString()}");
        }

        if (doc.RootElement.TryGetProperty("Information", out var info))
        {
            throw new Exception($"Alpha Vantage API Information: {info.GetString()}");
        }
    }
}