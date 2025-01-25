using System;
using Godot;

namespace GameObjects;
public class TurbineBay {

    public int TurbineCount = 3;

    public double TotalHeatCapacity { get => HeatCapacityPerTurbine * TurbineCount; }

    // Kelvin per second
    public double HeatCapacityPerTurbine = 200;

    public double AmbientTemperature;

    public double RepairLevel = 1.0;

    public Coolant coolant;

    public TurbineBay(double ambientTemp, Coolant coolant) {
        AmbientTemperature = ambientTemp;
        this.coolant = coolant;
    }

    // Returns watts generated this tick
    public double GameTick(double delta) {
        // Heat dissipated is limited by both the coolant's capacity and the reactor's heat capacity
        double temperatureDifference = coolant.Temperature - AmbientTemperature;
        double maxDissipation = TotalHeatCapacity * delta; // Total system heat limit
        double coolantDissipation = coolant.FlowSpeed * coolant.coolantType.HeatCapacity * temperatureDifference * delta;

        // Actual heat dissipated is the smaller of these two limits
        double heatDissipated = Math.Min(coolantDissipation, maxDissipation);
        
        // Update coolant temperature
        coolant.ReduceEnergy(heatDissipated);

        // Convert dissipated heat to electrical power, modulated by repair level
        double powerOutput = heatDissipated * RepairLevel;

        // Return the power generated in Watts
        return powerOutput;
    }

    public void Degrade(double delta) {
        // Degenerate the repair level
        RepairLevel -= 0.01 * delta;
    }

}