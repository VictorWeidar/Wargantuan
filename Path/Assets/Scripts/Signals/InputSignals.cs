using UnityEngine;

public static class InputSignals
{
    /// <summary>
    /// pending
    /// </summary>
    public static event AxisFire1DownHandler OnAxisFire1Down;
    public delegate void AxisFire1DownHandler();
    public static void DoAxisFire1Down() { if (OnAxisFire1Down != null) OnAxisFire1Down(); }
}
