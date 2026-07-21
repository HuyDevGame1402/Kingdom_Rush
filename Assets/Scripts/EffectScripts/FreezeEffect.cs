using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class FreezeEffect : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Material defaultMaterial; // Material gốc của enemy
    [SerializeField] private Material iceMaterial;     // Material băng (mat_Enemy_Ice)

    [Header("Settings")]
    [SerializeField] private float freezeDuration = 3.0f; // Thời gian đóng băng mặc định
    [SerializeField] private float blendTime = 0.3f;       // Thời gian chuyển đổi mượt mà

    [Tooltip("Độ phủ màu băng tối đa (Mặc định 0.2 cho giống Kingdom Rush)")]
    [Range(0f, 1f)]
    [SerializeField] private float maxIceBlend = 0.2f;     // Giới hạn Ice Blend ở 0.2f

    private SpriteRenderer spriteRenderer;
    private Coroutine freezeCoroutine;
    private Material runtimeIceMaterial; // Instance riêng cho từng Enemy

    // Tên property trong Shader Graph
    private static readonly int IceBlendID = Shader.PropertyToID("_IceBlend");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Lấy Material gốc hiện tại nếu Inspector chưa gán
        if (defaultMaterial == null)
        {
            defaultMaterial = spriteRenderer.sharedMaterial;
        }

        // Tạo instance riêng của Ice Material
        if (iceMaterial != null)
        {
            runtimeIceMaterial = new Material(iceMaterial);
        }
    }

    // Hàm gọi đóng băng từ bên ngoài
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
        if (runtimeIceMaterial != null)
        {
            spriteRenderer.material = runtimeIceMaterial;
        }

        // 2. Chuyển màu dần sang băng (Blend từ 0 -> maxIceBlend = 0.2f)
        float timer = 0f;
        while (timer < blendTime)
        {
            timer += Time.deltaTime;
            // Tính toán giá trị Blend tăng dần từ 0 đến maxIceBlend
            float blendValue = Mathf.Lerp(0f, maxIceBlend, timer / blendTime);

            if (runtimeIceMaterial != null)
            {
                runtimeIceMaterial.SetFloat(IceBlendID, blendValue);
            }
            yield return null;
        }

        // Đảm bảo blend đạt chính xác giá trị maxIceBlend (0.2f)
        if (runtimeIceMaterial != null)
        {
            runtimeIceMaterial.SetFloat(IceBlendID, maxIceBlend);
        }

        // 3. Giữ hiệu ứng trong thời gian đóng băng
        yield return new WaitForSeconds(duration);

        // 4. Trở về bình thường dần dần (Blend từ maxIceBlend = 0.2f -> 0)
        timer = 0f;
        while (timer < blendTime)
        {
            timer += Time.deltaTime;
            // Tính toán giá trị Blend giảm dần từ maxIceBlend về 0
            float blendValue = Mathf.Lerp(maxIceBlend, 0f, timer / blendTime);

            if (runtimeIceMaterial != null)
            {
                runtimeIceMaterial.SetFloat(IceBlendID, blendValue);
            }
            yield return null;
        }

        if (runtimeIceMaterial != null)
        {
            runtimeIceMaterial.SetFloat(IceBlendID, 0f);
        }

        // 5. Trả lại Material gốc ban đầu
        spriteRenderer.material = defaultMaterial;

        freezeCoroutine = null;
    }

    // Dọn dẹp bộ nhớ tránh Memory Leak
    private void OnDestroy()
    {
        if (runtimeIceMaterial != null)
        {
            Destroy(runtimeIceMaterial);
        }
    }
    // Hàm gọi để HỦY ĐÓNG BĂNG NGAY LẬP TỨC
    public void RemoveFreeze()
    {
        // 1. Dừng Coroutine đang làm mượt/đếm giờ (nếu có)
        if (freezeCoroutine != null)
        {
            StopCoroutine(freezeCoroutine);
            freezeCoroutine = null;
        }

        // 2. Reset biến _IceBlend trong Material về 0
        if (runtimeIceMaterial != null)
        {
            runtimeIceMaterial.SetFloat(IceBlendID, 0f);
        }

        // 3. Trả lại Material gốc ngay lập tức
        if (spriteRenderer != null && defaultMaterial != null)
        {
            spriteRenderer.material = defaultMaterial;
        }
    }
}