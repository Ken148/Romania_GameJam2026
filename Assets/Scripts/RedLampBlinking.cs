using UnityEngine;

public class Blinking : MonoBehaviour
{
    public Light targetLight;

    [Header("Pulse Settings")]
    public float minIntensity = 0.1f;
    public float maxIntensity = 2f;
    public float pulseInterval = 1.5f;
    public float fadeOutDuration = 1.0f;

    private float timer;

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponentInChildren<Light>();
        }

        if (targetLight == null)
        {
            Debug.LogWarning("Blinking: No Light found on this object or its children.");
        }
    }

    void Update()
    {
        if (targetLight == null)
            return;

        timer += Time.deltaTime;

        if (timer >= pulseInterval)
            timer = 0f;

        if (timer <= fadeOutDuration)
        {
            float t = timer / fadeOutDuration;

            targetLight.intensity = Mathf.Lerp(
                maxIntensity,
                minIntensity,
                t
            );
        }
        else
        {
            targetLight.intensity = minIntensity;
        }
    }
}
