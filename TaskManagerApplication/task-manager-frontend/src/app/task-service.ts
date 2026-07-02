import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { CreateTaskRequestDTO } from './models/create-task-request-dto';
import { UpdateTaskRequestDTO } from './models/update-task-request-dto';
import { TaskResponseDTO } from './models/task-response-dto';
import { Priority } from './models/task-model';

@Injectable({providedIn: "root"})

export class TaskService {
    private http = inject(HttpClient);
    private baseurl = "http://localhost:5099/api/Task";

    getAllTasks(): Observable<TaskResponseDTO[]> {
        return this.http.get<TaskResponseDTO[]>(this.baseurl);
    }

    getTaskById(taskId: number): Observable<TaskResponseDTO> {
        return this.http.get<TaskResponseDTO>(`${this.baseurl}/${taskId}`);
    }

    createTask(dto: CreateTaskRequestDTO): Observable<TaskResponseDTO> {
        return this.http.post<TaskResponseDTO>(this.baseurl, dto)
    }

    updateTask(id: number, dto: UpdateTaskRequestDTO): Observable<TaskResponseDTO> {
        return this.http.put<TaskResponseDTO>(`${this.baseurl}/${id}`, dto)
    }

    toggleTask(id: number): Observable<TaskResponseDTO> {
        return this.http.patch<TaskResponseDTO>(`${this.baseurl}/${id}/toggle`, {})
    }

    deleteTask(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseurl}/${id}`)
    }
}
