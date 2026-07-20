using System.Collections;
using UnityEngine;

public class MeteoriteProjectile : MonoBehaviour
{
    [Header("Animation Frames")]
    [SerializeField] private string animExplosionName = "fireball_explosion_";

    [Header("Sprite")]
    [SerializeField] private Transform spriteMateorite;
    [SerializeField] private Transform explosionTransform;
    [SerializeField] private Transform soilExpansionTransform;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 10f;

    private Vector3 targetPos;
    private float damage;
    private bool isInitialized = false;
    private Vector3 moveDirection;

    public Transform posTest1;

    [Header("Shadow Settings (Prefab)")]
    // Bạn kéo file Prefab cái bóng từ cửa sổ Project vào ô này nhé
    [SerializeField] private GameObject shadowPrefab;

    // Biến nội bộ dùng để lưu trữ và quản lý cái bóng được sinh ra trong Game
    private GameObject spawnedShadow;
    private SpriteRenderer[] shadowRenderers;
    private Vector3 startPos;
    private float totalDistance;

    [SerializeField] private MeteoriteSound meteoriteSound;

    //private void Start()
    //{
    //    Initialize(posTest1.position, 10f);
    //}

    public void Initialize(Vector3 targetPosition, float damageAmount)
    {
        this.targetPos = targetPosition;
        this.damage = damageAmount;
        this.startPos = transform.position;

        totalDistance = Vector3.Distance(startPos, targetPos);

        // --- TỰ ĐỘNG TẠO BÓNG TỪ PREFAB ---
        if (shadowPrefab != null)
        {
            // Sinh cái bóng ra ngay tại vị trí đích (mặt đất)
            // Ép góc xoay Quaternion.identity để nó luôn nằm ngang chuẩn chỉ
            spawnedShadow = Instantiate(shadowPrefab, targetPosition, Quaternion.identity);

            // Lấy tất cả SpriteRenderer của các GameObject con nằm trong cái bóng vừa tạo
            shadowRenderers = spawnedShadow.GetComponentsInChildren<SpriteRenderer>();

            // Khởi tạo trạng thái ban đầu cho bóng: nhỏ và mờ
            UpdateShadowVisual(0f);
        }

        // Tính toán hướng di chuyển
        moveDirection = (targetPos - transform.position).normalized;

        RotateTowardsTarget();
        StartCoroutine(CoroutineTest());
        meteoriteSound.PlayAudioLoop();
    }

    private IEnumerator CoroutineTest()
    {
        yield return new WaitForSeconds(1f);
        isInitialized = true;
    }

    private void Update()
    {
        if (!isInitialized) return;

        // Di chuyển thiên thạch hướng về phía TargetPos
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // --- CẬP NHẬT LOGIC BÓNG TRÊN MẶT ĐẤT ---
        if (spawnedShadow != null)
        {
            // Tính toán tiến trình bay (từ 0 đến 1) dựa trên khoảng cách còn lại
            float currentDistance = Vector3.Distance(transform.position, targetPos);
            float progress = Mathf.Clamp01(1f - (currentDistance / totalDistance));

            // Cập nhật kích thước và độ mờ cho bóng theo tiến trình thiên thạch rơi xuống
            UpdateShadowVisual(progress);
        }

        // Kiểm tra khoảng cách tới mục tiêu
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            isInitialized = false;
            OnHitTarget();
        }
    }

    private void UpdateShadowVisual(float progress)
    {
        if (spawnedShadow == null) return;

        // Phóng to dần scale từ nhỏ (0.2) lên kích thước gốc (1.0) khi thiên thạch lao xuống gần đất
        float currentScale = Mathf.Lerp(0.2f, 1.0f, progress);
        spawnedShadow.transform.localScale = new Vector3(currentScale, currentScale, 1f);

        // Chỉnh độ đậm nhạt (Alpha) cho cả 3 sprite con của cái bóng vừa tạo
        if (shadowRenderers != null)
        {
            float currentAlpha = Mathf.Lerp(0.1f, 0.7f, progress);

            foreach (SpriteRenderer sr in shadowRenderers)
            {
                if (sr != null)
                {
                    Color color = sr.color;
                    color.a = currentAlpha;
                    sr.color = color;
                }
            }
        }
    }

    private void RotateTowardsTarget()
    {
        if (moveDirection == Vector3.zero) return;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (spriteMateorite != null) spriteMateorite.rotation = targetRotation;
    }

    private void OnHitTarget()
    {
        Debug.Log($"Thiên thạch đã đánh trúng mục tiêu! Gây {damage} sát thương.");

        // --- XỬ LÝ ẨN HOẶC XÓA BÓNG KHI NỔ ---
        if (spawnedShadow != null)
        {
            // Cách 1: Phá hủy luôn cái bóng vì đã nổ xong (Khuyên dùng)
            Destroy(spawnedShadow);

            // Cách 2: Nếu bạn muốn ẩn đi thay vì xóa thì dùng dòng dưới:
            // spawnedShadow.SetActive(false); 
        }

        meteoriteSound.PlayAudioHit();

        spriteMateorite.gameObject.SetActive(false);
        explosionTransform.gameObject.SetActive(true);
        soilExpansionTransform.gameObject.SetActive(true);

        SpriteSheetAnimator.Instance.PlayAnimation(
            target: explosionTransform.gameObject,
            animPrefix: animExplosionName,
            frameRate: 0.05f,
            onComplete: () =>
            {
                explosionTransform.gameObject.SetActive(false);
                StartCoroutine(CoroutineDisable());
            }
        );
    }

    private IEnumerator CoroutineDisable()
    {
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }
}