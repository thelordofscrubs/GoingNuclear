using System;
using Godot;

namespace GameObjects;
public class TurbineBay {

    public int TurbineCount = 5;

    public double TotalHeatCapacity { get => EnergyCapacityPerTurbine * TurbineCount; }

    // Watts
    public double EnergyCapacityPerTurbine = 200 * 1000 * 1000;

    public double AmbientTemperature;

    public double RepairLevel = 1.0;

    public double TurbineEfficiency = 2.5;

    public Coolant coolant;

    public TurbineBay(double ambientTemp, Coolant coolant) {
        AmbientTemperature = ambientTemp;
        this.coolant = coolant;
    }

    // Returns Joules generated this tick
    public double GameTick(double delta) {
        // Heat dissipated is limited by both the coolant's capacity and the reactor's heat capacity
        double temperatureDifference = coolant.Temperature - AmbientTemperature;
        if (temperatureDifference <= 0) {
            return 0;
        }

        // Define a "smooth threshold" for dissipation
        double effectiveTemperatureDifference = Math.Max(temperatureDifference - 10, 0) * 0.3;

        // Calculate max dissipation and coolant dissipation
        double maxDissipation = TotalHeatCapacity * delta; // Total system heat limit
        double coolantDissipation = coolant.FlowSpeed * coolant.coolantType.HeatCapacity * effectiveTemperatureDifference * delta;

        // Actual heat dissipated is the smaller of these two limits * repair level
        double heatDissipated = Math.Min(coolantDissipation, maxDissipation) * RepairLevel;
        
        // Update coolant temperature
        coolant.ReduceEnergy(heatDissipated);

        // Convert dissipated heat to electrical power, modulated by efficiency
        double powerOutput = heatDissipated * TurbineEfficiency;

        ChangeRepairLevel(-effectiveTemperatureDifference * delta / (10d * 1000));

        // Return the power generated in Joules
        return powerOutput;
    }

    public void ChangeRepairLevel(double changeAmount) {
        RepairLevel = Math.Clamp(RepairLevel + changeAmount, 0.0, 1.0);
    }

    public void Degrade(double delta) {
        // Degenerate the repair level
        ChangeRepairLevel(-0.0001 * delta);
    }

}