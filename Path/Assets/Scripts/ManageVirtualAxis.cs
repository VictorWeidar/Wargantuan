using UnityEngine;

public class ManageVirtualAxis : MonoBehaviour
{
    bool axisFire1Pressed = false;

    void Update()
    {
        if (Input.GetAxis("Fire1") != 0) {
            if (!axisFire1Pressed) {
                axisFire1Pressed = true;
                InputSignals.DoAxisFire1Down();
            }
        }
        else 
            axisFire1Pressed = false;
    }
}
