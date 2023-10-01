using System.ComponentModel.DataAnnotations;

namespace TodoManager.Models
{
    public class TaskRequest
    {
     
        /// <summary>
        /// The name of the Task (ex: "Do My Homework")
        /// </summary>
        public string Name { get; set; }  = string.Empty;

        /// <summary>
        /// The task description (ex: "Solving complex data structure problems")
        /// </summary>
        public string Description { get; set; } = string.Empty; 

        /// <summary>
        /// The task start date (ex: "Chemistry")
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The Alloted time span in seconds
        /// </summary>
        public int AllotedTime { get; set; }

        /// <summary>
        /// The Elapsed time span in seconds
        /// </summary>
        public int ElapsedTime { get; set; }

        /// <summary>
        /// The Status bool (TRUE , CLOSED) (FALSE, PENDING) 
        /// </summary>
        public bool Status { get; set; }



    }
}


