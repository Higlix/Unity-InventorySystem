using UnityEngine;
using UnityEngine.UI;

public class BreathingImage : MonoBehaviour
{
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minAlpha = 0.5f; // Minimum alpha
    [SerializeField] private float maxAlpha = 1f;   // Maximum alpha

    private RectTransform rectTransform;
    private Image image;
    private Vector3 originalScale;
    private float originalAlpha;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        originalScale = rectTransform.localScale;
        originalAlpha = image.color.a;
    }

    void Update()
    {
        // Oscillate scale
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
        rectTransform.localScale = originalScale * scale;

        // Oscillate alpha
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * speed) + 1f) / 2f);
        SetAlpha(alpha);
    }

    private void SetAlpha(float newAlpha)
    {
        Color currentColor = image.color;
        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
    }
}