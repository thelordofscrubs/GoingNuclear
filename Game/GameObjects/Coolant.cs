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

    // Bar
    public double PressureInReactor;

    // Bar
    public double ReactorInletPressure;

    public CoolantType coolantType;

    public Coolant(double ambientTemp, double normalPressure, string coolantType = "water") {
        Temperature = ambientTemp;
        PressureInReactor = normalPressure;
        ReactorInletPressure = normalPressure;
        this.coolantType = CoolantTypes[coolantType];
    }

    public static Dictionary<string, CoolantType> CoolantTypes = new Dictionary<string, CoolantType>{
        {"water" , new CoolantType(4.186, 373.15, 273.15, "Water")}
    };

    public class CoolantType {
        // Specific heat capacity (Joules per gram*C)
        public double HeatCapacity;

        // Kelvin
        public double BoilingPoint;
        
        // Kelvin
        public double FreezingPoint;

        public string DisplayName;
        
        public CoolantType(double heatCapacity, double boilingPoint, double freezingPoint, string name) {
            HeatCapacity = heatCapacity;
            BoilingPoint = boilingPoint;
            FreezingPoint = freezingPoint;
            DisplayName = name;
        }
    }
}