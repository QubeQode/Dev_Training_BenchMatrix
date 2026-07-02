import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../../task-service';
import { Priority } from '../../models/task-model';
import { UpdateTaskRequestDTO } from '../../models/update-task-request-dto';
import { AsyncPipe } from '@angular/common';
import { switchMap, tap, Subject, startWith, catchError, of } from 'rxjs';

@Component({
  selector: 'app-task-details',
  imports: [ReactiveFormsModule, AsyncPipe, RouterLink],
  templateUrl: './task-details.html',
  styleUrl: './task-details.css',
})

export class TaskDetails {
  private route = inject(ActivatedRoute);
  private taskService = inject(TaskService);
  private fb = inject(FormBuilder);
  private refreshtask$ = new Subject<void>();

  readonly Priority = Priority;

  errorMessage = '';
  saveMessage= '';

  taskForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(400)]],
    dueDate: ['', Validators.required],
    priority: [Priority.Low, Validators.required],
    completed: [false, Validators.required]
  });

  task$ = this.refreshtask$.pipe(
    startWith(undefined),
    switchMap(() => this.taskService.getTaskById(Number(this.route.snapshot.paramMap.get('id')))),
    tap(task => {
      const priorityEnum = Priority[task.priority as keyof typeof Priority];
  
      this.taskForm.patchValue({
        name: task.name,
        description: task.description,
        completed: task.completed,
        dueDate: task.dueDate.split('T')[0],
        priority: priorityEnum
      });
    }),
    catchError(err => {
      console.error('Failed to load task', err);
      this.errorMessage = 'Failed to load task';
      return of (null);
    })
  );
  
  saveTask(taskId: number): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    this.saveMessage = '';

    const formValue = this.taskForm.getRawValue();

    const dto: UpdateTaskRequestDTO = {
      name: formValue.name!,
      description: formValue.description!,
      completed: formValue.completed!,
      dueDate: formValue.dueDate!,
      priority: Priority[formValue.priority!]
    };

    this.taskService.updateTask(taskId, dto)
      .subscribe({
        next: () => {
          this.saveMessage = 'Saved changes';
          setTimeout(() => {
            this.saveMessage = '';            
          }, 3000);
          this.refreshtask$.next();
        },
        error: err => {
          console.error('Failed to update task', err);
        }
      });
  }

  toggleCompleted(): void {
    const currentValue = this.taskForm.controls.completed.value;

    this.taskForm.patchValue({
      completed: !currentValue
    });
  }

  get nameControl() {
    return this.taskForm.controls.name;
  }

  get descriptionControl() {
    return this.taskForm.controls.description;
  }

  get dueDateControl() {
    return this.taskForm.controls.dueDate;
  }

  get priorityControl() {
    return this.taskForm.controls.priority;
  }

  get completedControl() {
    return this.taskForm.controls.completed;
  }
}
