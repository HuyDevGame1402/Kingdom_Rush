using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ImageGlareUI : MonoBehaviour
{
    public static ImageGlareUI Instance { get; private set; }

    [Header("Reference")]
    [SerializeField] private Image glareImage;

    [Header("Effect")]
    [SerializeField] private float duration = 2f;

    [SerializeField] private Color startColor = Color.white;

    // Màu vàng kem giống ánh sáng sau vụ nổ
    [SerializeField]
    private Color endColor = new Color(1f, 0.92f, 0.75f, 0f);

    private Coroutine glareRoutine;

    private void Awake()
    {
        Instance = this;
        if (glareImage == null)
            glareImage = GetComponent<Image>();

        Color c = startColor;
        c.a = 0;
        glareImage.color = c;

        glareImage.gameObject.SetActive(false);
    }

    public void ShowGlare()
    {
        if (glareRoutine != null)
            StopCoroutine(glareRoutine);

        glareRoutine = StartCoroutine(GlareRoutine());
    }

    private IEnumerator GlareRoutine()
    {
        glareImage.gameObject.SetActive(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            // Mượt hơn Linear
            float smooth = Mathf.SmoothStep(0f, 1f, t);

            Color color = Color.Lerp(startColor, endColor, smooth);

            // Alpha giảm dần
            color.a = Mathf.Lerp(1f, 0f, smooth);

            glareImage.color = color;

            yield return null;
        }

        Color end = endColor;
        end.a = 0f;

        glareImage.color = end;
        glareImage.gameObject.SetActive(false);
    }
}