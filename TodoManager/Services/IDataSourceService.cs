using System.Threading.Tasks;
using TodoManager.Models;

namespace TodoManager.Services
{
    public interface IDataSourceService
    {
        // Courses
        Task<bool> DoesTaskExists(int id);
       Task<int> GetTaskCount();
        Task<int?> DeleteTask(int id);
        Task<Models.Task?> GetTask(int id);
        Task<Models.Task?[]> GetTaskList(int? skip, int? take, string? search);
        Task<int?> AddTask(TaskRequest request);
        Task<Models.Task?> UpdateTask(int id ,TaskRequest request);
    }
}
 