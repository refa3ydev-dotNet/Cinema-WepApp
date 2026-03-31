using Business.DTOs.Schedule;
using Core.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Mapping
{
    public static class ScheduleMapping
    {
        public static ScheduleDisplayDto ToScheduleDisplayDto(this MovieSchedule schedule)
        {
            return new ScheduleDisplayDto
            {
                Id = schedule.Id,
                MovieName = schedule.Movie?.Name,
                RoomName = schedule.Room?.RoomName,
                StartTime = schedule.StartDate,
                Price = schedule.Movie.Price,
                Status = schedule.StartDate > DateTime.Now ? "Active":"Expired"
            };
        }
    }
}
