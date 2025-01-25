using System;
using Godot;

namespace GameObjects;
public class NuclearReactor {

    // Kelvin
    public double Heat;

    public double FuelFreshness = 1.0;
    public int FuelRods = 10;
    public int ControlRods = 10;

    public double ControlRodDepth = 0.9;

    public Coolant coolant;
    public TurbineBay turbineBay;
    public CoolantPumps coolantPumps;


    public Action TriggerMeltdown;

    public NuclearReactor(Action triggerMeltdown, double ambientTemp = 300) {
        Heat = ambientTemp;
        coolant = new Coolant(ambientTemp, 2);
        turbineBay = new TurbineBay(ambientTemp, coolant);
        coolantPumps = new CoolantPumps(600, 5);
        TriggerMeltdown = triggerMeltdown;
    }

    // Returns watts generated this tick
    public double GameTick(double delta) {        
        

        coolant.FlowSpeed = coolantPumps.CurrentCoolantFlowCapacity;

        // Temp heat calc
        double heatDelta = Math.Pow(2, FuelRods) * delta * FuelFreshness - Math.Pow(1.8, ControlRods) * delta * ControlRodDepth;

        coolant.IncreaseEnergy(heatDelta);

        if (coolant.Temperature > coolant.coolantType.BoilingPoint) {
            TriggerMeltdown();
            return 9999999999999999 * delta;
        }

        double wattsGenerated = turbineBay.GameTick(delta);
        return wattsGenerated;
    }

    public void Degrade(double delta) {
        FuelFreshness -= 0.01 * delta;
        turbineBay.Degrade(delta);
        coolantPumps.Degrade(delta);
    }

}