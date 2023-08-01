using Haply.HardwareAPI.Unity;
using UnityEngine;

public class HapticController : MonoBehaviour
{
    [Range(-5, 5)]
    public float forceX;
    [Range(-5, 5)]
    public float forceY;
    [Range(-5, 5)]
    public float forceZ;

    void Awake()
    {
        var hapticThread = GetComponent<HapticThread>();
        hapticThread.onInitialized.AddListener(() => hapticThread.Run(ForceCalculation));
    }


    private Vector3 ForceCalculation(in Vector3 position)
    {
        return new Vector3(forceX, forceY, forceZ);
    }
}
