using System;

public class GameResource {
    public int OwnedCount;

    public void ChangeOwnedCount(int changeAmount) {
        OwnedCount = Math.Max(OwnedCount + changeAmount, 0);
    }
}