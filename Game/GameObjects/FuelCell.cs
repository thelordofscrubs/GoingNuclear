
using System;

namespace GameObjects;
public class FuelCell {
    public FuelCellType Type;
    public double FuelLevel = 1.0;

    // fraction degraded per second in use
    public double FlatDegradationSpeed;

    // fraction degraded per megajoule of heat generated
    public double DegradationPerMegajoule;

    public void ChangeFuelLevel(double changeAmount) {
        FuelLevel = Math.Clamp(FuelLevel + changeAmount, 0.0, 1.0);
    }

    public void DegradeCellTime(double delta) {
        ChangeFuelLevel(-FlatDegradationSpeed * delta);
    }

    public void DegradeCellJoules(double megajoules) {
        ChangeFuelLevel(-DegradationPerMegajoule * megajoules);
    }

    public bool CurrentlyInserted = false;

    public FuelCell(FuelCellType type) {
        Type = type;
        if (type == FuelCellType.LongLasting) {
            FlatDegradationSpeed = 0.0005;
            DegradationPerMegajoule = .00001;
        }
        else {
            FlatDegradationSpeed = 0.001;
            DegradationPerMegajoule = .00002;
        }
    }
}