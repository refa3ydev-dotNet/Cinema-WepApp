using Business.DTOs.Schedule;
using Business.Managers.Movies;
using Core.Entities.Relations;
using DataAccess.Repositories.MOVIE;
using DataAccess.Repositories.Schedule;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Managers.Schedule
{
    public class MovieScheduleManager : IMovieScheduleManager
    {
            private readonly IMovieScheduleRepository _movieScheduleRepository;
        private readonly IMovieManager _movieManager;
            public MovieScheduleManager(IMovieScheduleRepository movieScheduleRepository, IMovieManager movieManager)
            {
                _movieScheduleRepository = movieScheduleRepository;
                _movieManager = movieManager;
            }
        public async Task CreateScheduleAsync(CreateScheduleDto dto)
        {
            var movie = await _movieManager.GetMovieByIdAsync(dto.MovieId);
            // 🚨 الخطوة دي هتحميك: لو مفيش فيلم بالرقم ده، اضرب إيرور صريح ومتكملش!
            if (movie == null)
            {
                throw new Exception($"Cannot create schedule! Movie with ID {dto.MovieId} does not exist. Make sure you select a valid movie.");
            }
            int runTime = (movie!=null && movie.Runtime>0)?movie.Runtime:120;

            int CleaningBreak = 15;

            var movieSchedule = new MovieSchedule
            {
                MovieId = dto.MovieId,
                CinemaId = dto.CinemaId,
                RoomId = dto.RoomId,
                StartDate = dto.StartTime,
                EndDate = dto.StartTime.AddMinutes(runTime + CleaningBreak),
                Price = dto.Price
            };
             await _movieScheduleRepository.AddScheduleAsync(movieSchedule);
    }
    }
}
