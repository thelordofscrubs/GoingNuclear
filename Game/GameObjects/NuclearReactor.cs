using System;
using Godot;

namespace GameObjects;
public class NuclearReactor {

    // Kelvin
    public double CoreTemperature;
    // Kilograms
    public double CoreMass = 100000;
    // J/kgK
    public double CoreSpecificHeatCapacity = 320;

    public double CoreCoolantHeatTransferCoefficient = 10000;
    public double CoreCoolantSurfaceArea = 1000;

    public double FuelFreshness = 1.0;
    public int FuelRodCount = 10;
    public int ControlRodCount = 10;

    public double ControlRodDepth = 0.5;

    // 1 is ascending, 0 is stationary, -1 is descending
    public int ControlRodState = 0;

    public Coolant coolant;
    public TurbineBay turbineBay;
    public CoolantPumps coolantPumps;


    public Action TriggerMeltdown;

    public NuclearReactor(Action triggerMeltdown, double ambientTemp = 300) {
        CoreTemperature = ambientTemp;
        coolant = new Coolant(ambientTemp, 2);
        turbineBay = new TurbineBay(ambientTemp, coolant);
        coolantPumps = new CoolantPumps(600, 5);
        TriggerMeltdown = triggerMeltdown;
    }

    // Returns watts generated this tick
    public double GameTick(double delta) {

        adjustControlRodPosition(delta);

        coolant.FlowSpeed = coolantPumps.CurrentCoolantFlowCapacity;

        double extremeFactor = 1.5; // Adjust how punishing low insertion is

        // S-curve that steepens at both ends
        double controlRodEffect = 1 - Math.Pow(Math.Sin(Math.PI * (1-ControlRodDepth) / 2), extremeFactor);

        // Reactor power multiplier based on remaining fuel and rod insertion
        double reactivityMultiplier = Math.Pow(1.7, (FuelRodCount - (ControlRodCount * controlRodEffect)) * 5);
        double energyDelta = reactivityMultiplier * delta * FuelFreshness * controlRodEffect; 


        // Gradually reduce FuelFreshness without sudden jumps
        FuelFreshness -= energyDelta / (10d * 1000 * 1000 * 1000 * 1000);

        IncreaseCoreEnergy(energyDelta);
        TransferHeatToCoolant(delta);

        if (coolant.Temperature > coolant.coolantType.BoilingPoint * 2) {
            TriggerMeltdown();
        }

        double wattsGenerated = turbineBay.GameTick(delta);
        return wattsGenerated;
    }

    public void Degrade(double delta) {
        ChangeFuelFreshness(-0.0001 * delta);
        turbineBay.Degrade(delta);
        coolantPumps.Degrade(delta);
    }

    private void adjustControlRodPosition (double delta) {
        if (ControlRodState != 0) {
            ControlRodDepth = Math.Clamp(ControlRodDepth + 0.05 * delta * ControlRodState, 0, 1.0);
        }
    }

    public void ChangeFuelFreshness (double changeAmount) {
        FuelFreshness = Math.Clamp(FuelFreshness + changeAmount, 0, 1.0);
    }

    private void TransferHeatToCoolant(double delta) {
        // Calculate heat transferred
        double deltaTemp = CoreTemperature - coolant.Temperature;
        double heatTransferred = CoreCoolantHeatTransferCoefficient * CoreCoolantSurfaceArea * deltaTemp;

        // Update core temperature
        DecreaseCoreEnergy(heatTransferred * delta);

        // Update coolant temperature
        coolant.IncreaseEnergy(heatTransferred * delta);
    }

    public void IncreaseCoreEnergy(double Joules) {
        CoreTemperature += Joules / (CoreMass * CoreSpecificHeatCapacity);
    }

    public void DecreaseCoreEnergy(double Joules) {
        CoreTemperature -= Joules / (CoreMass * CoreSpecificHeatCapacity);
    }

}