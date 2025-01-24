using System;
using Godot;

namespace GameObjects;
public class TurbineBay {

    public int TurbineCount = 3;

    // Kilograms per second
    public double CoolantCapacityPerTurbine = 200;

    public double AmbientTemperature;

    public TurbineBay(double ambientTemp) {
        AmbientTemperature = ambientTemp;
    }

    public double GameTick(double delta, double coolantFlow, double coolantTemp) {
        
        return 0.0;
    }

}