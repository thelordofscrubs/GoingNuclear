using System.Collections.Generic;
using System.Linq;
using GameObjects;


public class GameStatus {
    public int CurrentDay = 0;
    public double Money = 1000;
	public Dictionary<EmployeeTypes, int> EmployeeCounts = new Dictionary<EmployeeTypes, int>{
        {EmployeeTypes.ControlRoomEngineer, 0},
        {EmployeeTypes.TurbineMechanic, 0},
        {EmployeeTypes.ReactorEngineer, 0}
    };
    public List<FuelCell> FuelCells = new List<FuelCell>();
    public List<FuelCell> CurrentlyActiveFuelCells {get => FuelCells.Where( fc => fc.CurrentlyInserted ).ToList();}


    public double WattHoursGeneratedToday = 0;
    public double TotalWattHoursGenerated = 0;
    public double WattHourRequirementToday = 0;
    public RandomEventsDaily CurrentRandomDaily = RandomEventsDaily.NoEvent;
    
}