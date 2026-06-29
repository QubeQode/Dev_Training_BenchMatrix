enum Priority{
    Low,
    Medium,
    High
}

interface Task {
    Id: number;
    Name: string;
    Description: string;
    Completed: boolean;
    Priority: Priority,
    DueDate: Date,
    CreatedAt: Date
}

const generateId = (previousId: number): number => {
    if (previousId = 0) {
        return 0;
    }
    else {
        return previousId + 1;
    }
};

const calculateDateDiff = (completeDate: Date, createDate: Date): number =>
     completeDate.getTime() - createDate.getTime();

const firstItem =  <T>(array: T[]): T | undefined => array[0];
