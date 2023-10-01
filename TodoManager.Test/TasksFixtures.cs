using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoManager.Test
{
    public static class TasksFixtures
    {
        public static List<Models.Task> GetTaskTestLists => new List<Models.Task>()
            {
                new Models.Task {
                 Id = 1,
                Name = "Name 1",
                Description  = "Test Description",
                StartDate = DateTime.Now,
                AllotedTime = 64055500,
                ElapsedTime = 80077440,
                Status = false
                } ,
                new Models.Task {
                 Id = 2,
                Name = "Name 2",
                Description  = "Test Description 2",
                StartDate = DateTime.Now.AddDays(-2),
                AllotedTime = 67262000,
                ElapsedTime = 877002220,
                Status = true
                },

                new Models.Task {
                 Id = 3,
                Name = "Name 3",
                Description  = "Test Description 3",
                StartDate = DateTime.Now.AddDays(-1),
                AllotedTime = 672727200,
                ElapsedTime = 287442220,
                Status = true
                },
            };


    }   
}
