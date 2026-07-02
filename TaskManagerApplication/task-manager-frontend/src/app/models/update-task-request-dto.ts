import { Priority } from "./task-model";

export interface UpdateTaskRequestDTO
{
    name: string;
    description: string;
    completed: boolean;
    priority: string;
    dueDate: string;
}
