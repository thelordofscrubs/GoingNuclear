using System;
using Godot;

namespace GameObjects;
public class NuclearReactor {

    // Kelvin
    public double Heat;

    // Kilograms per second
    public double CoolantCapacity = 500;
    public int FuelRods = 10;
    public int ControlRods = 10;

    public Coolant coolant;

    public NuclearReactor(double ambientTemp = 300) {
        Heat = ambientTemp;
        coolant = new Coolant(ambientTemp, 2);
    }

    // Returns watts generated this tick
    public double GameTick(double delta) {        
        double wattsGenerated = 0.0;

        // Temp heat calc
        double heatDelta = Math.Pow(2, FuelRods) * delta - Math.Pow(1.8, ControlRods) * delta;

        Heat += heatDelta;



        return 0.0;
    }

}