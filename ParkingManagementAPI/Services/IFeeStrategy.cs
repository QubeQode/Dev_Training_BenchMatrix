/*
    Simple fee calculation Interface
    - decimal CalculateFee(TimeSpan duration);

    Used by:
    - HourlyFeeStrategy
        CalculateFee(TimeSpan duration) => Math.Ceiling(duration.TotalHours) * 50;
    - DailyFeeStrategy
        CalculateFee(TimeSpan duration) => Math.Ceiling(duration.TotalDays) * 500;
    - FlatFeeStrategy => 200;
*/