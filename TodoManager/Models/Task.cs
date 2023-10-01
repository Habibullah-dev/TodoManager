namespace TodoManager.Models
{
    public class Task
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public int AllotedTime { get; set; }

        public int ElapsedTime { get; set; }

        public bool Status { get; set; } 


    }
}
