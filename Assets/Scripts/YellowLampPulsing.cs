using UnityEngine;

public class GentleLightPulse : MonoBehaviour
{
    public Light targetLight;
    public float baseIntensity = 1.2f;
    public float pulseAmount = 0.1f;
    public float pulseSpeed = 1.5f;

    void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();
    }

    void Update()
    {
        if (targetLight == null)
            return;

        float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        targetLight.intensity = baseIntensity + pulse;
    }
}