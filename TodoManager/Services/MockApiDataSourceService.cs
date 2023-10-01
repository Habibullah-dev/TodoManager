using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using TodoManager.Models;

namespace TodoManager.Services
{
    public class MockApiDataSourceService : IDataSourceService
    {
        private const string baseUrl = "https://651599f2dc3282a6a3ceaf28.mockapi.io/api/";

        private HttpClient httpClient;
        public MockApiDataSourceService(HttpClient client)
        { 
            httpClient = client;

        }


        public async Task<int?> AddTask(TaskRequest request)
        {
            var url = baseUrl + $"tasks";

            var requestJson = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(requestJson);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var responseMessage = await httpClient.PostAsync(url, httpContent, CancellationToken.None);

            if (responseMessage.StatusCode == HttpStatusCode.Created)
            {
                var responseBody = await responseMessage.Content
                    .ReadAsStringAsync(CancellationToken.None);
                var task = JsonSerializer.Deserialize<Models.Task>(responseBody,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                return task?.Id;
            }

            return null;
        }

        public async Task<int?> DeleteTask(int id)
        {
            var url = baseUrl + $"tasks/{id}";

            var responseMessage = await httpClient.DeleteAsync(url , CancellationToken.None);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await responseMessage.Content
                    .ReadAsStringAsync(CancellationToken.None);
                var task = JsonSerializer.Deserialize<Models.Task>(responseBody,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                return task?.Id;
            }

            return null;
        }

        public async Task<bool> DoesTaskExists(int id)
        {
            var task =  await this.GetTask(id);

            return task != null ? true : false;
        }

        public async Task<Models.Task?> GetTask(int id)
        {
            var url = baseUrl + $"tasks/{id}";

            var responseMessage = await httpClient.GetAsync(url, CancellationToken.None);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await responseMessage.Content
                    .ReadAsStringAsync(CancellationToken.None);

                var task = JsonSerializer.Deserialize<Models.Task>(responseBody,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                return task;
            }

            if (responseMessage.StatusCode == HttpStatusCode.NotFound || responseMessage.StatusCode == HttpStatusCode.InternalServerError)
            {
                return null;
            }

            return null;
        }

        public async Task<int> GetTaskCount()
        {
            return (await this.GetTaskList(skip : 0, take: null, null))?.Length ?? 0;
        }

        public async Task<Models.Task?[]> GetTaskList(int? skip, int? take, string? search)
        {
            var url = baseUrl + $"tasks";

            var responseMessage = await httpClient.GetAsync(url, CancellationToken.None);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await responseMessage.Content
                    .ReadAsStringAsync(CancellationToken.None);

                var taskList = JsonSerializer.Deserialize<Models.Task[]>(responseBody,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                if (taskList == null)
                {
                    return null;
                }

                return taskList.OrderBy(o => o.Id)
                            .Where(o => string.IsNullOrWhiteSpace(search)
                            || (o.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                || o.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                                || o.StartDate.ToString().Contains(search, StringComparison.InvariantCultureIgnoreCase)
                                || o.AllotedTime.ToString().Contains(search, StringComparison.InvariantCultureIgnoreCase)))
                            .Skip(skip ?? 0).Take(take ?? taskList.Length).ToArray();
            }

            return null;
        }

        public async Task<Models.Task?> UpdateTask(int id , TaskRequest request)
        {
            var url = baseUrl + $"tasks/{id}";

            var requestJson = JsonSerializer.Serialize(request);

            var httpContent = new StringContent(requestJson);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var responseMessage = await httpClient.PutAsync(url, httpContent, CancellationToken.None);

            if (responseMessage.StatusCode == HttpStatusCode.OK)
            {
                var responseBody = await responseMessage.Content
                    .ReadAsStringAsync(CancellationToken.None);
                var task = JsonSerializer.Deserialize<Models.Task>(responseBody,
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                return task;
            }

            return null;

        }

      
    }
}
