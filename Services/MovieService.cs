using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TeluqMovieForm.Models;

namespace TeluqMovieForm.Services;

public class MovieService : IMovieService
{
    private readonly HttpClient _httpClient;

    public MovieService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<MovieResult>> GetMoviesAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<SwapiResponse>("https://swapi.dev/api/films");

            if (response?.Results is null)
            {
                return Enumerable.Empty<MovieResult>();
            }

            return response.Results
                .OrderByDescending(m => m.ReleaseDate)
                .Skip(1)
                .Take(5)
                .Select(m => new MovieResult(m.Title, m.Url));
        }
        catch
        {
            return Enumerable.Empty<MovieResult>();
        }
    }

    private class SwapiResponse
    {
        [JsonPropertyName("results")]
        public List<SwapiMovie> Results { get; set; } = new();
    }

    private class SwapiMovie
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = string.Empty;
    }
}