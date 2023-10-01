namespace TodoManager.Dtos
{
    /// <summary>
    /// This is TaskDTO class for mapping into prefered Response
    /// </summary>
    public class TaskDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public int AllotedTime { get; set; }

        public int ElapsedTime { get; set; }


       public DateTime EndDate { get; set; }

        public DateTime DueDate { get; set; }   

        public int DaysOverdue { get; set; }

        public int DaysLate { get; set; }

        public string TaskStatus { get; set; }



    }
}
