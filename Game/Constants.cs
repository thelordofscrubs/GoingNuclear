public enum ItemType {
    TurbineMechanic,
    ControlRoomEngineer,
    ReactorEngineer,
    FuelCell,
    Forklift
}

public enum FuelCellTypes {
    Regular,
    Level2,
    LongLasting,
    HighTemp,
    Safety
}

public enum PowerPlantStates {    
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

public enum RandomEvents {
    NoEvent = -1,
    LightningStrike,
    NeutrinoBurst,
    Sabotage
}

public enum GamePhases {
		DayStart,
		MainGame,
		DayEnd
	}