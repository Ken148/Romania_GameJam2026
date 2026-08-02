using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light targetLight;

    [Header("Normal State")]
    public float normalIntensity = 1.5f;

    [Header("Flicker Timing")]
    public float minTimeBetweenFlickerBursts = 0.5f;
    public float maxTimeBetweenFlickerBursts = 3f;

    [Header("Flicker Burst")]
    public int minFlickersPerBurst = 1;
    public int maxFlickersPerBurst = 6;
    public float minOffDuration = 0.05f;
    public float maxOffDuration = 0.4f;
    public float minSpikeDuration = 0.03f;
    public float maxSpikeDuration = 0.15f;
    public float flickerSpikeIntensity = 3f;

    [Header("Randomness")]
    [Range(0f, 1f)]
    public float chanceOfExtendedDarkness = 0.2f;
    public float extendedDarknessDuration = 1.5f;

    void Start()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light>();

        targetLight.intensity = normalIntensity;
        StartCoroutine(FlickerLoop());
    }

    IEnumerator FlickerLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenFlickerBursts, maxTimeBetweenFlickerBursts);
            yield return new WaitForSeconds(waitTime);

            yield return StartCoroutine(DoFlickerBurst());
        }
    }

    IEnumerator DoFlickerBurst()
    {
        int flickerCount = Random.Range(minFlickersPerBurst, maxFlickersPerBurst + 1);

        for (int i = 0; i < flickerCount; i++)
        {
            targetLight.intensity = 0f;
            float offDur = Random.Range(minOffDuration, maxOffDuration);
            yield return new WaitForSeconds(offDur);

            bool spike = Random.value > 0.5f;
            targetLight.intensity = spike ? flickerSpikeIntensity : normalIntensity;
            float onDur = Random.Range(minSpikeDuration, maxSpikeDuration);
            yield return new WaitForSeconds(onDur);
        }

        if (Random.value < chanceOfExtendedDarkness)
        {
            targetLight.intensity = 0f;
            yield return new WaitForSeconds(extendedDarknessDuration);
        }

        targetLight.intensity = normalIntensity;
    }
}