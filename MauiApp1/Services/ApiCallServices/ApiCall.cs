using System.Text;
using System.Text.Json.Nodes;
using MauiApp1.Services.ApplicationConfigurations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MauiApp1.Services.ApiCallServices;

public class ApiCall(IOptions<AppConfig> appConfig) : IApiCall
{
    private const string ApiExecuteSp = "ApiExecuteSp";

    private static readonly HttpClient HttpClient = new(new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (_, _, _, _) => true
    });

    private readonly AppConfig AppConfig = appConfig.Value;

    public async Task<string> CallOneAsync(string sqlQuery, string apiName, string tableName = "")
    {
        Dictionary<string, object> apiParameters = new()
        {
            { "SQLQuery", sqlQuery },
            { "ClientName", AppConfig.ClientName },
            { "TableName", tableName }
        };

        return await ApiJsonAsync(apiName, apiParameters);
    }

    public async Task<string> CallTwoAsync(Dictionary<string, object> spParameters, string spName)
    {
        Dictionary<string, object> apiParameters = new()
        {
            { "SPParameter", spParameters },
            { "SPName", spName },
            { "ClientName", AppConfig.ClientName }
        };

        return await ApiJsonAsync(ApiExecuteSp, apiParameters);
    }

    public T Deserialize<T>(string jsonString)
    {
        try
        {
            var returnValue = JObject.Parse(jsonString);
            var item1 = returnValue!["Item1"]!?.ToString();
            var item2 = returnValue!["Item2"]!?.ToString();

            if (string.IsNullOrEmpty(item1))
            {
                throw new(item2);
            }

            return JsonConvert.DeserializeObject<T>(item1);
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }
    }

    public List<T> DeserializeToListObject<T>(string jsonString)
    {
        try
        {
            var returnValue = JObject.Parse(jsonString);
            var item1 = returnValue!["Item1"]!?.ToString();
            var item2 = returnValue!["Item2"]!?.ToString();

            if (string.IsNullOrEmpty(item1))
            {
                throw new(item2);
            }

            return JsonConvert.DeserializeObject<List<T>>(item1);
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }
    }

    public IReadOnlyList<T> DeserializeToListObjectV2<T>(string jsonString)
    {
        try
        {
            var returnValue = JsonNode.Parse(jsonString);
            var item1 = returnValue!["Item1"]!?.ToString();
            var item2 = returnValue!["Item2"]!?.ToString();

            if (string.IsNullOrEmpty(item1))
            {
                throw new(item2);
            }

            return System.Text.Json.JsonSerializer.Deserialize<IReadOnlyList<T>>(item1);
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }
    }

    public T DeserializeToObject<T>(string jsonString)
    {
        try
        {
            var returnValue = JObject.Parse(jsonString);
            var item1 = returnValue!["Item1"]!?.ToString();
            var item2 = returnValue!["Item2"]!?.ToString();

            if (string.IsNullOrEmpty(item1))
            {
                throw new(item2);
            }

            return JsonConvert.DeserializeObject<T>(item1);
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }
    }

    public T DeserializeToObjectV2<T>(string jsonString)
    {
        try
        {
            var returnValue = JsonNode.Parse(jsonString);
            var item1 = returnValue!["Item1"]!?.ToString();
            var item2 = returnValue!["Item2"]!?.ToString();

            if (string.IsNullOrEmpty(item1))
            {
                throw new(item2);
            }

            return System.Text.Json.JsonSerializer.Deserialize<T>(item1);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public Tuple<int, string, string> ReturnVal(string jsonString)
    {
        var returnValue = JObject.Parse(jsonString);
        return Tuple.Create(Convert.ToInt32(returnValue["Item1"]), returnValue["Item2"]?.ToString(),
            returnValue["Item3"]?.ToString().Replace("'", ""));
    }

    private async Task<string> ApiJson(string apiName, Dictionary<string, object> apiParameters)
    {
        using HttpClientHandler httpClientHandler = new();
        httpClientHandler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
        HttpClient httpClient = new(httpClientHandler)
        {
            DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
        };

        HttpResponseMessage httpResponseMessage;

        var json = JsonConvert.SerializeObject(apiParameters);

        StringContent data = new(json, Encoding.UTF8, "application/json");

        try
        {
            httpResponseMessage = await httpClient.PostAsync(string.Concat(AppConfig.ApiUrl, apiName), data);
            httpResponseMessage.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw ex.InnerException!;
        }

        httpClient.Dispose();

        return await httpResponseMessage.Content.ReadAsStringAsync();
    }

    private async Task<string> ApiJsonAsync(string apiName, Dictionary<string, object> apiParameters)
    {
        try
        {
            var json = JsonConvert.SerializeObject(apiParameters);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponseMessage = await HttpClient.PostAsync($"{AppConfig.ApiUrl}/{apiName}", data);
            httpResponseMessage.EnsureSuccessStatusCode();

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
        catch (HttpRequestException ex)
        {
            throw ex.InnerException!;
        }
    }
}