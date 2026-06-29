using TaskManagerAPI.DTOs;

namespace TaskManagerAPI.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDTO>> GetAllTasksAsync();

    Task<TaskResponseDTO> GetTaskByIdAsync (int taskId);

    Task<TaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO dto);

    Task<TaskResponseDTO> UpdateTaskAsync(int taskId, UpdateTaskRequestDTO dto);

    Task<TaskResponseDTO> ToggleTaskAsync(int taskId);

    Task DeleteTaskAsync(int taskId);
}
