using System.Net.Http.Json;

public class SubjectService
{
    private readonly HttpClient _http;

    public SubjectService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Subject>> GetSubjectsAsync()
    {
        return await _http.GetFromJsonAsync<List<Subject>>("api/subjects") ?? new List<Subject>();
    }
}
