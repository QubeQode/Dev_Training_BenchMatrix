/*
    Calculates the Parking Fee:
    - Constructed with DI of
        - _hourly = HourlyFeeStrategy hourly;
        - _daily = DailyFeeStrategy daily;
        - _flat = FlatFeeStrategy flat;
    - Delegates fee calculation
        - (ParkingTicket ticket)
        - duration = Conclusion - Issuance
        - IFeeStrategy strategy = ticket.FeeStrategyType switch
            - .Hourly => _hourly
            - .Daily => _daily
            - .flat => _flat
            - _ => throw new InvalidFeeStrategyException(ticket.FeeStrategyType)
        - return strategy.CalculateFee(duration)
*/


