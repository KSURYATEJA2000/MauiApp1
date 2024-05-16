namespace MauiApp1.Services.ApiCallServices;

public interface IApiCall
{
    public const string ApiExecuteQuery = "ApiExecuteQuery";
    public const string ApiGetDataSet = "ApiGetDataSet";
    public const string ApiGetDataTable = "ApiGetDataTable";

    public Task<string> CallOneAsync(string sqlQuery, string apiName, string tableName = "");

    public Task<string> CallTwoAsync(Dictionary<string, object> spParameters, string spName);

    public T Deserialize<T>(string jsonString);

    public List<T> DeserializeToListObject<T>(string jsonString);

    public IReadOnlyList<T> DeserializeToListObjectV2<T>(string jsonString);

    public T DeserializeToObject<T>(string jsonString);

    public T DeserializeToObjectV2<T>(string jsonString);

    public Tuple<int, string, string> ReturnVal(string jsonString);
}