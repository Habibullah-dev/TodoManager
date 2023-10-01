using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoManager.Dtos;
using TodoManager.Extensions;
using TodoManager.Models;
using TodoManager.Services;

namespace TodoManager.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class TasksController : Controller
    {
        private readonly IDataSourceService dataSourceService;

        public TasksController(IDataSourceService dataSourceService)
        {
            this.dataSourceService = dataSourceService;
        }


        /// <summary>
        /// Gets a Todo Task from the DataStore
        /// </summary>
        /// <param name="id">The Id of the Todo Task</param>
        /// <returns></returns>
        /// <response code="404">When the task does not exist</response>
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // api/courses/2
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var task = await dataSourceService.GetTask(id);

            if (task == null)
            {
                return NotFound();
            }

            //A Customer Mapper Extension Method For Mapping Task To TaskDTO

            var taskDto = task.MapToTaskDto();

            return Ok(taskDto);
        }



        /// <summary>
        /// Gets a pageable list of Tasks
        /// that match the query from the DataStore 
        /// </summary>
        /// <param name="request">Query parameters</param>
        /// <returns>A list of Tasks</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TaskDto[]))]
        public async Task<IActionResult> Get([FromQuery] GetTaskListRequest request)
        {
            Models.Task?[] tasks = await dataSourceService.GetTaskList(request.Skip , request.Take, request.Search);

            if(tasks is null)
            {
                return Ok(Array.Empty<TaskDto>());
            }

             var taskDtos = tasks.Select(task =>
                {
                    return task.MapToTaskDto();

                }).ToArray();
           
            return Ok(taskDtos);
        }

        /// <summary>
        /// Adds a task to the DataSource
        /// </summary>
        /// <param name="request">The details of the new task</param>
        /// <returns>The newly added task</returns>
        /// <response code="422">When there is a validation error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Models.Task))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] TaskRequest request)
        {
              //Add Task to mock service
            int? id = await dataSourceService.AddTask(request);

            if (id != null)
            {
                var task = await dataSourceService.GetTask(id.Value);

                return CreatedAtAction(nameof(Get), new { id = id }, task);
            }
            
            return BadRequest();
           
        }



        /// <summary>
        /// Updates a Task in the DataStore
        /// </summary>
        /// <param name="id">The id of the Task to update</param>
        /// <param name="request">The details of the Task to be updated</param>
        /// <response code="422">When there is a validation error</response>
        /// <response code="404">When the task was not found by its id</response>
        [HttpPut]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] TaskRequest request)
        {
            if (!await dataSourceService.DoesTaskExists(id))
            {
                return NotFound();
            }

             var task = await dataSourceService.UpdateTask(id ,request);

            return Accepted();
        }

        /// <summary>
        /// Deletes a Task from the DataStore
        /// </summary>
        /// <param name="id">The id of the Task to delete</param>
        /// <response code="404">When the task was not found by its id</response>
        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (! await dataSourceService.DoesTaskExists(id))
            {
                return NotFound();
            }

            int? taskId = await dataSourceService.DeleteTask(id);

            return Ok(taskId);
        }

   

    }
}
