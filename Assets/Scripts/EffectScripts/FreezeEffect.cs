using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class FreezeEffect : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Material defaultMaterial; // Material gốc của enemy
    [SerializeField] private Material iceMaterial;     // Material băng (mat_Enemy_Ice) đã tạo ở Bước 1

    [Header("Settings")]
    [SerializeField] private float freezeDuration = 3.0f; // Thời gian đóng băng mặc định
    [SerializeField] private float blendTime = 0.3f;       // Thời gian chuyển màu (mượt mà)

    private SpriteRenderer spriteRenderer;
    private Coroutine freezeCoroutine;
    private Material runtimeIceMaterial; // Tạo một instance riêng để không ảnh hưởng enemy khác

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Sử dụng defaultMaterial nếu không được gán, đề phòng lỗi
        if (defaultMaterial == null) defaultMaterial = spriteRenderer.material;

        // Tạo một instance mới của Material băng để có thể thay đổi thuộc tính riêng cho enemy này
        if (iceMaterial != null)
        {
            runtimeIceMaterial = new Material(iceMaterial);
        }
    }

    // Hàm gọi để đóng băng enemy
    public void ApplyFreeze(float duration = -1f)
    {
        float actualDuration = duration > 0f ? duration : freezeDuration;

        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
        }
        freezeCoroutine = StartCoroutine(FreezeRoutine(actualDuration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        // 1. Chuyển sang Material băng
        spriteRenderer.material = runtimeIceMaterial;

        // 2. Chuyển màu dần sang băng (Blending in)
        float timer = 0f;
        while (timer < blendTime)
        {
            timer += Time.deltaTime;
            float blendValue = timer / blendTime;
            // Thay đổi biến "_IceBlend" trong Shader
            runtimeIceMaterial.SetFloat("_IceBlend", blendValue);
            yield return null;
        }
        // Đảm bảo blend đạt tối đa
        runtimeIceMaterial.SetFloat("_IceBlend", 1f);

        // 3. Đợi trong thời gian đóng băng (kết hợp với hiệu ứng đóng băng bạn đã làm)
        yield return new WaitForSeconds(duration);

        // 4. Chuyển màu dần về bình thường (Blending out)
        timer = 0f;
        while (timer < blendTime)
        {
            timer += Time.deltaTime;
            float blendValue = 1f - (timer / blendTime);
            runtimeIceMaterial.SetFloat("_IceBlend", blendValue);
            yield return null;
        }
        runtimeIceMaterial.SetFloat("_IceBlend", 0f);

        // 5. Chuyển về Material gốc
        spriteRenderer.material = defaultMaterial;

        freezeCoroutine = null;
    }

    // Quan trọng: Dọn dẹp material instance để tránh memory leak
    private void OnDestroy()
    {
        if (runtimeIceMaterial != null)
        {
            Destroy(runtimeIceMaterial);
        }
    }
}