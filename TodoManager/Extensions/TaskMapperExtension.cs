using TodoManager.Dtos;
using TodoManager.Models;

namespace TodoManager.Extensions
{
    /// <summary>
    /// This is Custom Task To TaskDto Mappers
    /// </summary>
    public static class TaskMapperExtension
    {
        ///<Summary>
        /// <paramref name="task"/>
        /// <return>Ths extension method return TaskDto object i</return>
        ///</Summary>
        public static TaskDto MapToTaskDto(this Models.Task task)
        {
            return new TaskDto()
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                StartDate = task.StartDate,
                AllotedTime = task.AllotedTime,
                ElapsedTime = task.ElapsedTime,
                EndDate = task.StartDate.AddSeconds(task.ElapsedTime),
                DueDate = task.StartDate.AddSeconds(task.AllotedTime),
                TaskStatus = task.Status ? "CLOSED" : "PENDING",
                DaysOverdue = !task.Status ? Convert.ToInt32((task.StartDate.AddSeconds(task.ElapsedTime) - task.StartDate.AddSeconds(task.AllotedTime)).TotalDays) : 0,
                DaysLate = task.Status ? Convert.ToInt32((task.StartDate.AddSeconds(task.AllotedTime) - task.StartDate.AddSeconds(task.ElapsedTime)).TotalDays) : 0
            };
        }
    }
}
