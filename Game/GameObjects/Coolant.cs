using System;
using System.Collections.Generic;
using System.Data;
using Godot;

namespace GameObjects;

public class Coolant {

    // Kelvin
    public double Temperature;

    // Kilograms per second
    public double FlowSpeed = 0;

    // Kilograms
    public double TotalCoolantMass = 10000;

    // Bar
    public double PressureInReactor;

    public CoolantType coolantType;

    public Coolant(double ambientTemp, double normalPressure, string coolantType = "water") {
        Temperature = ambientTemp;
        PressureInReactor = normalPressure;
        this.coolantType = CoolantTypes[coolantType];
    }

    public void ReduceEnergy(double JoulesReduced) {
        double temperatureChange = JoulesReduced / (TotalCoolantMass * coolantType.HeatCapacity);
        Temperature -= temperatureChange;
    }

    public void IncreaseEnergy(double JoulesAdded) {
        double temperatureChange = JoulesAdded / (TotalCoolantMass * coolantType.HeatCapacity);
        Temperature += temperatureChange;
    }

    public static Dictionary<string, CoolantType> CoolantTypes = new Dictionary<string, CoolantType>{
        {"water" , new CoolantType(4186, 373.15, 273.15, "Water")}
    };

    public class CoolantType {
        // Specific heat capacity (Joules per Kilogram * Kelvin)
        public readonly double HeatCapacity;

        // Kelvin
        public readonly double BoilingPoint;
        
        // Kelvin
        public readonly double FreezingPoint;

        public readonly string DisplayName;
        
        public CoolantType(double heatCapacity, double boilingPoint, double freezingPoint, string name) {
            HeatCapacity = heatCapacity;
            BoilingPoint = boilingPoint;
            FreezingPoint = freezingPoint;
            DisplayName = name;
        }
    }
}