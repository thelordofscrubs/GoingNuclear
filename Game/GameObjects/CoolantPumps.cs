using System;
using Godot;

namespace GameObjects;

public class CoolantPumps {

    // Kg per second
    public double CoolantFlowCapacity;

    // Bar
    public double TargetReactorInletPressure;

    public double RepairLevel = 1.0;

    // Bar
    public double CurrentReactorInletPressure { get => TargetReactorInletPressure * RepairLevel; }
    // Kg per second
    public double CurrentCoolantFlowCapacity { get => CoolantFlowCapacity * RepairLevel; }

    public CoolantPumps(double coolantFlowCapacity, double targetReactorInletPressure) {
        CoolantFlowCapacity = coolantFlowCapacity;
        TargetReactorInletPressure = targetReactorInletPressure;
    }

    public void Degrade(double delta) {
        RepairLevel -= 0.005 * delta;
    }
}