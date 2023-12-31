﻿namespace TodoManager.Models
{
    public class GetTaskListRequest
    {
        /// <summary>
        /// The number of courses to skip
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// The number of courses to retrieve
        /// </summary>
        public int? Take { get; set; }

        /// <summary>
        /// A partial search query string
        /// </summary>
        public string? Search { get; set; }
    }
}
