using Haply.HardwareAPI.Unity;
using UnityEngine;

public class HapticController : MonoBehaviour
{
    [Range(0, 600)]
    public float stiffness = 2f;
    [Range(-4, 4)]
    public float forceX;
    [Range(-4, 4)]
    public float forceY;
    [Range(-4, 4)]
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
