export enum Priority{
    Low,
    Medium,
    High
}

export interface Task {
    Id: number;
    Name: string;
    Description: string;
    Completed: boolean;
    Priority: Priority,
    DueDate: Date,
    CreatedAt: Date
}
