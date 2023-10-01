
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NuGet.Frameworks;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using TodoManager.Controllers;
using TodoManager.Dtos;
using TodoManager.Services;

namespace TodoManager.Test
{
    public class TasksControllerTest : IAsyncLifetime
    {
        //Mocking IDataSourceService
        private readonly Mock<IDataSourceService> _dataSourceServciceMock = new();

        private HttpClient _httpClient = null!;

        private List<Models.Task> testTaskModelList;

        public TasksControllerTest()
        {
            var application = new WebApplicationFactory<Program>();

            testTaskModelList = new();

            _httpClient = application.CreateClient();
        }


        public Task InitializeAsync()
        {
            testTaskModelList = TasksFixtures.GetTaskTestLists;

            return Task.FromResult(testTaskModelList);
        }


        [Fact]
        public async Task Get_Task_OnSuccessTest()
        {
            //Arrange
      
            var task = testTaskModelList[0];

            _dataSourceServciceMock.Setup( mockService => mockService.GetTask(task.Id))
                .ReturnsAsync(task);

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (OkObjectResult)await taskController.Get(task.Id);

            //************** Assert  ********************************/

            //Should return StatusCode 200, on Success
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);

            //Verifies that dataSourceServiceMock calls the GetTask once
            _dataSourceServciceMock.Verify(service => service.GetTask(task.Id), Times.Once);

            //Assets that the result is not Null
            Assert.NotNull(result);

            //Ensures the task model is mapped to taskDTO
            result.Value.Should().BeOfType<TaskDto>();

        }

        [Fact]
        public async Task Get_Task_OnFailureTest_404NotFund()
        {
            //Arrange

            var task = testTaskModelList[0];

            _dataSourceServciceMock.Setup(mockService => mockService.GetTask(It.IsAny<int>()))
                .ReturnsAsync(value: null);

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (NotFoundResult) await taskController.Get(100);


            //************** Assert ********************************/

            //Should return StatusCode 404, on failure
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

        }


        [Fact]
        public async Task Get_TaskList_OnSuccessTest()
        {

            //Arrange
            _dataSourceServciceMock.Setup(mockService => mockService.GetTaskList(0, null,null))
                .ReturnsAsync(testTaskModelList.ToArray);

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (OkObjectResult) await taskController.Get(new Models.GetTaskListRequest { Skip = 0, Take = testTaskModelList.Count,Search = null});


            //************** Assert ********************************/

            //Should return StatusCode 200, on Success

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            //Assets that the result is not Null
            Assert.NotNull(result);

            //Ensures the task[] model is mapped to taskDTO[]
            result.Value.Should().BeOfType<TaskDto[]>();


        }

        [Fact]
        public async Task Get_TaskList_OnFailureTest()
        {
            //Arrange
            _dataSourceServciceMock.Setup(mockService => mockService.GetTaskList(0, null, null))
                .ReturnsAsync(value: null);

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (OkObjectResult)await taskController.Get(new Models.GetTaskListRequest { Skip = 0, Take = 0, Search = null });

            //************** Assert ********************************/

            //Should return StatusCode 200, on Success

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            //Assets that the result is not Null
            Assert.Empty((System.Collections.IEnumerable) result.Value);

        }


        [Fact]
        public async Task  Post_Task_OnSuccessTest()
        {

            //Arrange
            var task = new Models.TaskRequest
            {
                Name = "Name 4",
                Description = "Test Description 3",
                StartDate = DateTime.Now.AddDays(-1),
                AllotedTime = 672727200,
                ElapsedTime = 287442220,
                Status = true
            };

            _dataSourceServciceMock.Setup(mockService => mockService.AddTask(task))
                .ReturnsAsync(testTaskModelList.Count + 1);

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (CreatedAtActionResult) await taskController.Post(task);

            //************** Assert ********************************/

            //Should return StatusCode 200, on Success
;
             Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);
    
        }

        [Fact]
        public async Task Put_Task_OnSuccessTest()
        {

            //Arrange
            var task = new Models.TaskRequest
            {
                Name = "Name 4b",
                Description = "Test Description 4",
            };

            _dataSourceServciceMock.Setup(mockService => mockService.DoesTaskExists(4))
            .ReturnsAsync(true);

            _dataSourceServciceMock.Setup(mockService => mockService.UpdateTask(4, task))
                .ReturnsAsync(testTaskModelList.Where(x => x.Id == 4).FirstOrDefault());

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (AcceptedResult) await taskController.Put(4,task);

            //************** Assert ********************************/

            //Should return StatusCode 202, on Success
            ;
            Assert.Equal((int)HttpStatusCode.Accepted, result.StatusCode);

        }

        [Fact]
        public async Task Delete_Task_OnSuccessTest()
        {
            //Arrange

            _dataSourceServciceMock.Setup(mockService => mockService.DoesTaskExists(4))
            .ReturnsAsync(true);

            _dataSourceServciceMock.Setup(mockService => mockService.DeleteTask(4))
            .ReturnsAsync(4);

            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (OkObjectResult )await taskController.Delete(4);

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            //Assets that the result is not Null
            Assert.NotNull(result);

            Assert.Equal(result.Value, 4);
        }

        [Fact]
        public async Task Delete_Task_OnFailureTest_404NotFount()
        {
            //Arrange

            _dataSourceServciceMock.Setup(mockService => mockService.DoesTaskExists(int.MaxValue))
            .ReturnsAsync(false);


            var taskController = new TasksController(_dataSourceServciceMock.Object);

            //************** Act ********************************/

            var result = (NotFoundResult)await taskController.Delete(4);

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);

          
        }


        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
