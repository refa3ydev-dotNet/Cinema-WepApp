using Business.DTOs.Integration;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Business.Services.TmdbService
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.themoviedb.org/3";

        public TmdbService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["TmdbSettings:ApiKey"]
                ?? throw new InvalidOperationException("TMDB API Key not found in configuration");
        }

        public async Task<TmdbMovieDetails> GetMovieDetailsAsync(int tmdbMovieId)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbMovieDetails>(
                $"{_baseUrl}/movie/{tmdbMovieId}?api_key={_apiKey}&append_to_response=credits,videos");
            return response;
        }

        public async Task<TmdbPersonDetails> GetPersonDetailsAsync(int personId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<TmdbPersonDetails>(
                    $"{_baseUrl}/person/{personId}?api_key={_apiKey}");
                return response;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TmdbSearchResponse> GetPopularMoviesAsync(int page = 1)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse>(
                $"{_baseUrl}/movie/popular?api_key={_apiKey}&language=en-US&page={page}");

            return response;
        }

        public async Task<TmdbSearchResponse> SearchMoviesAsync(string movieName, int page = 1)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse>
                ($"{_baseUrl}/search/movie?api_key={_apiKey}&query={movieName}&page={page}");

            return response;
        }

        public async Task<TmdbSearchResponse> GetMoviesByGenreAsync(int genreId, int page = 1)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbSearchResponse>
                ($"{_baseUrl}/discover/movie?api_key={_apiKey}&with_genres={genreId}&page={page}&language=en-US&sort_by=popularity.desc");

            return response;
        }
    }
}
