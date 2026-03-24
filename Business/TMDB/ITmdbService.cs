using Business.DTOs.Integration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.TMDB
{
    public interface ITmdbService
    {
        Task<TmdbMovieDetails> GetMovieDetailsAsync(int movieId);
        Task<TmdbSearchResponse> SearchMoviesAsync(string query, int page = 1);
        Task<TmdbSearchResponse> GetPopularMoviesAsync(int page = 1);
    }
}
