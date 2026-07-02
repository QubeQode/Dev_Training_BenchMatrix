import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Priority } from '../../models/task-model';

@Component({
  selector: 'app-task-create-form',
  imports: [FormsModule],
  templateUrl: './task-create-form.html',
  styleUrl: './task-create-form.css',
})

export class TaskCreateForm {
  taskName = '';
  taskDescription = '';
  dueDate = '';
  priority = Priority.Low;

  readonly Priority = Priority;

  @Output()
  taskCreated = new EventEmitter<{
    name: string;
    description: string;
    dueDate: Date;
    priority: Priority;
  }>();

  createTask(): void {
    this.taskCreated.emit({
      name: this.taskName,
      description: this.taskDescription,
      dueDate: new Date(this.dueDate),
      priority: this.priority
    });

    this.taskName = '';
    this.taskDescription = '';
    this.dueDate = '';
    this.priority = Priority.Low;
  }
}
