import { Component, OnInit, inject } from '@angular/core';
import { Priority } from '../../models/task-model';
import { TaskItem } from '../task-item/task-item';
import { TaskService } from '../../task-service';
import { TaskResponseDTO } from '../../models/task-response-dto';
import { AsyncPipe } from '@angular/common';
import { catchError, Observable, of, finalize, switchMap, Subject, startWith } from 'rxjs';
import { TaskCreateForm } from '../task-create-form/task-create-form';
import { CreateTaskRequestDTO } from '../../models/create-task-request-dto';
import { UpdateTaskRequestDTO } from '../../models/update-task-request-dto';

@Component({
  selector: 'app-task-list',
  imports: [TaskItem, AsyncPipe, TaskCreateForm],
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})

export class TaskList {
  private taskService = inject(TaskService);

  private refreshTasks$ = new Subject<void>();

  errorMessage = '';

  tasks$ = this.refreshTasks$.pipe(
    startWith(undefined),
    switchMap(() => this.taskService.getAllTasks()),
    catchError(err => {
      console.error(err);
      this.errorMessage = 'Failed to load tasks'
      return of ([])
    })
  )

  onTaskCreated (taskData: {
    name: string;
    description: string;
    dueDate: Date;
    priority: Priority;
  }): void {
    const dto: CreateTaskRequestDTO = {
      name: taskData.name,
      description: taskData.description,
      dueDate: taskData.dueDate.toISOString().split('T')[0],
      priority: Priority[taskData.priority]
    };

    this.taskService.createTask(dto)
      .subscribe({
        next: () => {
          this.refreshTasks$.next();
        },
        error: err => {
          console.error('Failed to create task', err);
        }
      });
  }

  onTaskPriorityChanged (taskId: number, priority: Priority): void {
    this.errorMessage = '';

    this.taskService.getTaskById(taskId)
      .pipe(
        switchMap(task => {
          const dto: UpdateTaskRequestDTO = {
            name: task.name,
            description: task.description,
            completed: task.completed,
            dueDate: task.dueDate,
            priority: Priority[priority]
          }

          return this.taskService.updateTask(taskId, dto);
        })
      ).subscribe({
        next: () => {
          this.refreshTasks$.next();
        },
        error: err => {
          console.error('Failed to update task priority', err);
        }
      });
  }

  onCompletedTask(taskId: number): void {
    this.taskService.toggleTask(taskId)
      .subscribe({
        next: () => this.refreshTasks$.next(),
        error: err => console.error(err)
      });
  }

  onTaskDeleted(taskId: number): void {
    this.taskService.deleteTask(taskId)
      .subscribe({
        next: () => this.refreshTasks$.next(),
        error: err => console.error(err)
      });
  }
}
