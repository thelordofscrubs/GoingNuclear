using System;
using Godot;

namespace GameObjects;
public class NuclearReactor {

    // Kelvin
    public double CoreTemperature;
    // Kilograms
    public double CoreMass = 300000;
    // J/kgK
    public double CoreSpecificHeatCapacity = 320;

    public double CoreCoolantHeatTransferCoefficient = 10000;
    public double CoreCoolantSurfaceArea = 1000;

    public double FuelFreshness = 1.0;
    public int FuelRodCount = 10;
    public int ControlRodCount = 10;

    public double ControlRodDepth = 0.8;

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

        double energyDelta = calculateHeatGeneration(delta);


        // reduce fuel freshness by the portion of "Stored" joules consumed
        FuelFreshness -= energyDelta / (10d * 1000 * 1000 * 1000 * 1000);

        IncreaseCoreEnergy(energyDelta);
        TransferHeatToCoolant(delta);

        if (coolant.Temperature > coolant.coolantType.BoilingPoint * 2) {
            TriggerMeltdown();
        }

        double wattsGenerated = turbineBay.GameTick(delta);
        return wattsGenerated;
    }

    private double calculateHeatGeneration(double deltaTime)
    {
        // 1) Baseline for thermal power (roughly the reactor's "rated" power):
        //    For example, 3 gigawatts for a commercial-scale reactor.
        double baseThermalPower = 1.0e9;  // 3e9 J/s

        // 2) Exponential shape parameter.
        //    - Large beta => steeper ramp near x=0 (fully out).
        //    - If you want less punishment, reduce beta.
        double beta = 2.5;

        // 3) fraction of insertion:
        //    0 => rods fully out => potential meltdown territory
        //    1 => rods fully in => zero heat
        double x = ControlRodDepth;

        // 4) Combine base power with your other parameters:
        //    - FuelRodCount vs total rods (or skip if you always use all rods).
        //    - FuelFreshness (1.0 = fresh, <1.0 = partially burned).
        double rodFraction = (double)FuelRodCount / (double)ControlRodCount;
        double hMax = baseThermalPower * rodFraction * FuelFreshness;

        // 5) Shifted exponential, unnormalized:
        //    Heat(x) = hMax * ( e^(beta*(1-x)) - 1 )
        //
        //    At x=1 => e^(beta*(0)) - 1 => 1 - 1 = 0 => minimal heat
        //    At x=0 => e^(beta) - 1, which can exceed hMax
        //              (i.e., if e^beta is large, you punish going fully out)
        double exponentTerm = Math.Exp(beta * (1.0 - x));
        double instantaneousPower = hMax * (exponentTerm - 1.0);

        // 6) Multiply by deltaTime to get total Joules in this tick/frame
        double heatThisTick = instantaneousPower * deltaTime;

        // Optionally clamp if negative (shouldn’t happen unless x>1 or numeric issues)
        if (heatThisTick < 0.0) heatThisTick = 0.0;

        return heatThisTick;
    }


    public void Degrade(double delta) {
        ChangeFuelFreshness(-0.0001 * delta);
        turbineBay.Degrade(delta);
        coolantPumps.Degrade(delta);
    }

    private void adjustControlRodPosition (double delta) {
        if (ControlRodState != 0) {
            ControlRodDepth = Math.Clamp(ControlRodDepth + 0.01 * delta * ControlRodState, 0, 1.0);
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