import { Component, EventEmitter, Output, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Priority } from '../../models/task-model';

@Component({
  selector: 'app-task-create-form',
  imports: [ReactiveFormsModule],
  templateUrl: './task-create-form.html',
  styleUrl: './task-create-form.css',
})

export class TaskCreateForm {
  private fb = inject(FormBuilder);
  readonly Priority = Priority;

  taskCreatedMessage = '';

  @Output()
  taskCreated = new EventEmitter<{
    name: string;
    description: string;
    dueDate: Date;
    priority: Priority;
  }>();

  taskForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(400)]],
    dueDate: ['', Validators.required],
    priority: [Priority.Low, Validators.required]
  });

  createTask(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    const formValue = this.taskForm.getRawValue();

    this.taskCreated.emit({
      name: formValue.name!,
      description: formValue.description!,
      dueDate: new Date(formValue.dueDate!),
      priority: formValue.priority!
    });

    this.taskCreatedMessage = 'Task Created';

    setTimeout(() => {
      this.taskCreatedMessage = '';            
    }, 3000);

    this.taskForm.reset({
      name: '',
      description: '',
      dueDate: '',
      priority: Priority.Low
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
}
