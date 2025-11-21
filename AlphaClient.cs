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

    public async Task<Dictionary<string, object>> GetIntraday(string symbol)
    {
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=15min&outputsize=full&apikey={_apiKey}";

        
        var json = await _http.GetStringAsync(url);
        return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
    }
}