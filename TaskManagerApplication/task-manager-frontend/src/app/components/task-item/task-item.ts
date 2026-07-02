import { 
  Component, 
  EventEmitter, 
  Input, 
  Output
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { Priority } from "../../models/task-model";
import { TaskResponseDTO } from '../../models/task-response-dto';

@Component({
  selector: 'app-task-item',
  templateUrl: './task-item.html',
  imports: [DatePipe],
  styleUrl: './task-item.css'
})

export class TaskItem {
  @Input({required: true}) task!: TaskResponseDTO;

  @Output() priorityChanged = new EventEmitter
    <{priority: Priority, taskId: number}>();
  @Output() completedToggle = new EventEmitter<number>();
  @Output() taskDeleted = new EventEmitter<number>();

  readonly Priority = Priority;

  setPriority(priority: Priority): void {
    this.priorityChanged.emit({
      priority: priority,
      taskId: this.task.id
    });
  }

  toggleCompleted(): void {
    this.completedToggle.emit(this.task.id);
  }

  deleteTask(): void {
    this.taskDeleted.emit(this.task.id);
  }

  isPrioritySelected(priority: Priority): boolean {
    return this.task.priority === Priority[priority];
  }

  getPriorityLabel(): string {
    return this.task.priority;
  }
}
