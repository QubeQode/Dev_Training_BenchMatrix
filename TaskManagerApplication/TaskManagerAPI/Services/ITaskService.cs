using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDTO>> GetAllTasksAsync();

    Task<TaskResponseDTO> GetTaskByIdAsync (int taskId, TaskRequestDTO dto);

    Task<TaskResponseDTO> CreateTaskAsync(TaskRequestDTO dto);

    Task<TaskResponseDTO> UpdateTaskAsync(int taskId, TaskRequestDTO dto);

    Task<TaskResponseDTO> ToggleTaskAsync(int taskId, TaskRequestDTO dto);

    Task DeleteTaskAsync(int taskId, TaskRequestDTO dto);
}
