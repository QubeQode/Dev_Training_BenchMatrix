import { Component, signal, inject, OnInit} from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { TaskList } from './components/task-list/task-list';
import { TaskCreateForm } from './components/task-create-form/task-create-form';
import { TaskService } from './task-service';
import { TaskResponseDTO } from './models/task-response-dto';
import { catchError, Observable, of, finalize, Subject, startWith, switchMap } from 'rxjs';
import { AsyncPipe } from '@angular/common';
import { Priority } from './models/task-model';
import { CreateTaskRequestDTO } from './models/create-task-request-dto';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
  standalone: true
})

export class App{}
