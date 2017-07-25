using UnityEngine;

// a class used to enable the main camera to automatically position and orient itself in such a way that the entire play field is clearly seen
public class ManageCamera : MonoBehaviour
{
    [Tooltip("the camera is automatically positioned and oriented in such a way that it can see the entire play field if this property is ticked")]
    [SerializeField] bool convenientMode;
    [Tooltip("The width of the additional space between any of the four camera boundaries and the corresponding play field boundary")]
    [SerializeField] float additionalSpaceWidth = 2;

    void Awake()
    {
        GameEventSignals.OnTerrainGenerated += OnTerrainGenerated;
    }

    void OnDestroy()
    {
        GameEventSignals.OnTerrainGenerated -= OnTerrainGenerated;
    }

    void OnTerrainGenerated()
    {
        if (!convenientMode) return;

        float radianHalfVerticalFOV = Mathf.Deg2Rad * Camera.main.fieldOfView * 0.5f;
        float distance0 = (ManageTerrain.Ins.ColLength * 0.5f + additionalSpaceWidth) / Mathf.Tan(radianHalfVerticalFOV);
        float distance1 = (ManageTerrain.Ins.rowLength * 0.5f + additionalSpaceWidth) / (Camera.main.aspect * Mathf.Tan(radianHalfVerticalFOV));
        var gridCenterPos = new Vector3(ManageTerrain.Ins.rowLength / 2, ManageTerrain.Ins.gridYPos, -ManageTerrain.Ins.ColLength / 2);
        transform.position = gridCenterPos;
        transform.position += Vector3.up * Mathf.Max(distance0, distance1);
        transform.LookAt(gridCenterPos);
    }
}
