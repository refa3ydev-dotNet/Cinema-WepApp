using Business.DTOs.Schedule;
using Business.Managers.Movies;
using Business.Mapping;
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

        public async Task<bool> DeleteScheduleAsync(int scheduleId, int cinemaId)
        {
            var schedule=await _movieScheduleRepository.GetScheduleByIdAndCinemaIdAsync(scheduleId, cinemaId);
            if (schedule == null)
            {
                return false;

            }
            // ToDo : Teckits should be deleted
            schedule.IsDeleted = true;
            await _movieScheduleRepository.UpdateScheduleAsync(schedule);
            return true;
        }

        public async Task<IEnumerable<ScheduleDisplayDto>> GetCinemaSchedulesAsync(int cinemaId)
        {
            var schedules=await _movieScheduleRepository.GetSchedulesByCinemaIdAsync(cinemaId);
            return schedules.Select(s=>s.ToScheduleDisplayDto()).ToList();
        }

        public async Task<ScheduleDisplayDto> GetScheduleByIdAsync(int scheduleId, int cinemaId)
        {
            var schedule =await _movieScheduleRepository.GetScheduleByIdAndCinemaIdAsync(scheduleId, cinemaId);
            if (schedule == null)
            {
                return null;
            }
            return schedule.ToScheduleDisplayDto();
        }

        public async Task UpdateScheduleAsync(UpdateScheduleDto dto)
        {
            var schedule = await _movieScheduleRepository.GetScheduleByIdAndCinemaIdAsync(dto.Id, dto.CinemaId);
            if (schedule == null)
            {
                return;
            }
            schedule.StartDate = dto.StartTime;
            schedule.EndDate = dto.StartTime.AddMinutes(dto.RunTime + 15);
            schedule.RoomId = dto.RoomId;
            schedule.Price = dto.Price;
            await _movieScheduleRepository.UpdateScheduleAsync(schedule);
        }
    }
}
