using UnityEngine;

public class Blinking : MonoBehaviour
{
    public Light targetLight;

    [Header("Pulse Settings")]
    public float minIntensity = 0.1f;
    public float maxIntensity = 2f;
    public float pulseInterval = 1.5f; // время между вспышками
    public float fadeOutDuration = 1.0f; // сколько длится затухание

    private float timer;

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= pulseInterval)
            timer = 0f;

        if (timer <= fadeOutDuration)
        {
            // Плавное затухание от max к min
            float t = timer / fadeOutDuration;
            targetLight.intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }
        else
        {
            targetLight.intensity = minIntensity;
        }
    }
}