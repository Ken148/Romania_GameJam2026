using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CircularTimer : MonoBehaviour
{
    [Header("References")]
    public Image fillImage;
    public TextMeshProUGUI timerText;

    [Header("Colors")]
    public Color normalColor = new Color(0.3f, 0.85f, 0.4f);
    public Color warningColor = new Color(0.95f, 0.8f, 0.2f);
    public Color dangerColor = new Color(0.9f, 0.2f, 0.2f);

    [Header("Thresholds (в секундах)")]
    public float warningThreshold = 5f;
    public float dangerThreshold = 3f;

    [Header("Blink")]
    public float blinkSpeed = 6f;

    private float timeLimit;
    private float timeLeft;



    public void SetTime(float current, float max)
    {
        timeLimit = max;
        timeLeft = Mathf.Max(0f, current);
        UpdateVisual();
    }

    void UpdateVisual()
    {
        float fill = timeLimit > 0f ? timeLeft / timeLimit : 0f;
        fillImage.fillAmount = fill;


        int seconds = Mathf.CeilToInt(timeLeft);
        timerText.text = seconds.ToString();


        Color targetColor;
        if (timeLeft <= dangerThreshold)
        {

            float blink = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;
            targetColor = Color.Lerp(dangerColor, Color.white, blink * 0.5f);
        }
        else if (timeLeft <= warningThreshold)
        {
            targetColor = warningColor;
        }
        else
        {
            targetColor = normalColor;
        }

        fillImage.color = targetColor;
        timerText.color = targetColor;
    }
}