using Business.DTOs.Integration;
using System.Net.Http.Json;

namespace Business.TMDB
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "3a1b76eedcfccb8ed491fd77d32d6bcc";
        private readonly string _baseUrl = "https://api.themoviedb.org/3";
        public TmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<TmdbMovieDetails> GetMovieDetailsAsync(int tmdbMovieId)
        {
            var response = await _httpClient.GetFromJsonAsync<TmdbMovieDetails>(
                $"{_baseUrl}/movie/{tmdbMovieId}?api_key={_apiKey}&append_to_response=credits,videos");
            return response;
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
    }
}
