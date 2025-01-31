public enum ItemType {
    TurbineMechanic,
    ControlRoomEngineer,
    ReactorEngineer,
    FuelCell,
    Forklift
}

public enum FuelCellType {
    Regular,
    Level2,
    LongLasting,
    HighTemp,
    Safety
}

public enum PowerPlantState {    
    Dead,
    LowPower,
    Normal,
    HighPower,
    Critical,
    Meltdown
}

public enum RandomEventsDaily {
    NoEvent = -1,
    DeadStart,
    ColdDay,
    HotDay,
    Protest,
    CheapFuel,
    CheapHires,
    FuelUpgrade,
    ExpensiveFuel,
    ExpensiveHires
}

public enum RandomEvent {
    NoEvent = -1,
    LightningStrike,
    NeutrinoBurst,
    Sabotage
}

public enum GamePhase {
    DayStart,
    MainGame,
    DayEnd
}

public enum DayResult {
    Success,
    Meltdown,
    OutOfTime
}