using System.Text;
using System.Text.Json;
namespace EstadoCuenta_FrontEnd.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public ApiClient WithBaseUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
            return this;
        }
        public ApiClient WithHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(name, value);
            return this;
        }
        public async Task<TResponse> GetAsync<TResponse>(string endpoint, object queryParams = null)
        {
            var url = BuildUrl(endpoint, queryParams);
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        public async Task<(TResponse? Result, Dictionary<string, List<string>>? Errors)> PostAsync<TRequest, TResponse>(string endpoint, TRequest body)
        {
            var jsonBody = JsonSerializer.Serialize(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + endpoint, content);
            var errorContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var errorObject = JsonSerializer.Deserialize<JsonElement>(errorContent);
                    if (errorObject.TryGetProperty("errors", out var errors))
                    {
                        var parsedErrors = errors.EnumerateObject()
                            .ToDictionary(prop => prop.Name, prop => prop.Value.EnumerateArray().Select(e => e.GetString()!).ToList());
                        return (default, parsedErrors);
                    }
                }
                catch
                {
                    Console.WriteLine($"Error al deserializar: {errorContent}");
                }
                throw new HttpRequestException($"Error: {response.StatusCode}, Details: {errorContent}");
            }

            if (string.IsNullOrWhiteSpace(errorContent)) return (default, null);

            try
            {
                var result = JsonSerializer.Deserialize<TResponse>(errorContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return (result, null);
            }
            catch
            {
                throw new HttpRequestException($"Error al deserializar la respuesta: {errorContent}");
            }
        }




        private string BuildUrl(string endpoint, object queryParams)
        {
            if (queryParams == null) return _baseUrl + endpoint;

            var query = new StringBuilder("?");
            foreach (var prop in queryParams.GetType().GetProperties())
            {
                var value = prop.GetValue(queryParams);
                if (value != null)
                {
                    query.Append($"{prop.Name}={Uri.EscapeDataString(value.ToString())}&");
                }
            }

            return _baseUrl + endpoint + query.ToString().TrimEnd('&');
        }
    }
}
