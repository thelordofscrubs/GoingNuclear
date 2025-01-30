
using System;

public class Employee : GameResource {
    public double Efficiency = 1.0;
    public double Satisfacton = 1.0;
    public double HourlyWage = 10;

    public Employee(double Efficiency, double HourlyWage) {
        this.Efficiency = Efficiency;
        this.HourlyWage = HourlyWage;
    }

    public bool RollToQuit() {
        return new Random().NextDouble() > Satisfacton;
    }
}