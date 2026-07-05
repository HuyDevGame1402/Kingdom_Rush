using UnityEngine;
using System.Collections.Generic;
using DG.Tweening; // Nhớ add thêm namespace này nhé bạn

public class TextSpawnManager : MonoBehaviour
{
    public static TextSpawnManager Instance { get; private set; }

    [Header("Pool Settings")]
    public GameObject textPrefab;
    public int initialPoolSize = 10;
    private List<GameObject> poolList = new List<GameObject>();

    [Header("Animation Settings")]
    public float duration = 0.6f;
    public Vector3 targetPunchScale = new Vector3(1.3f, 1.3f, 1.3f);
    public float minRotationZ = -15f;
    public float maxRotationZ = 15f;
    public float moveUpDistance = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Khởi tạo pool sẵn để tránh giật lag lúc đang đánh nhau cao trào
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(textPrefab, transform);
            obj.SetActive(false);
            poolList.Add(obj);
        }
    }

    // Hàm lấy object từ pool ra tái sử dụng
    private GameObject GetPooledText()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            if (!poolList[i].activeInHierarchy)
            {
                return poolList[i];
            }
        }

        // Nếu thiếu thì tự nở pool thêm
        GameObject obj = Instantiate(textPrefab, transform);
        obj.SetActive(false);
        poolList.Add(obj);
        return obj;
    }

    public void SpawnText(Vector3 positionSpawn, Sprite textSprite)
    {
        if (textPrefab == null) return;

        // 1. Lấy text từ pool và set vị trí
        GameObject textObj = GetPooledText();
        textObj.transform.position = positionSpawn;

        // Reset lại scale gốc (tránh việc dùng lại bị lệch scale cũ)
        textObj.transform.localScale = Vector3.one;

        // 2. Setup Sprite cho SpriteRenderer
        SpriteRenderer spriteRenderer = textObj.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = textSprite;
            // Reset độ mờ về tối đa đề phòng hiệu ứng trước đó làm mờ đi
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }

        // 3. Tạo độ nghiêng ngẫu nhiên chuẩn vị Kingdom Rush (Z rotation)
        float randomZ = Random.Range(minRotationZ, maxRotationZ);
        textObj.transform.rotation = Quaternion.Euler(0, 0, randomZ);

        // Kích hoạt object lên trước khi chạy Tween
        textObj.SetActive(true);

        // 4. THỰC HIỆN ANIMATION BẰNG DOTWEEN

        // Di chuyển chữ nhẹ lên phía trên tạo cảm giác bay bổng
        textObj.transform.DOMoveY(positionSpawn.y + moveUpDistance, duration).SetEase(Ease.OutQuad);

        // Tạo hiệu ứng Punch Scale (Phóng to đột ngột rồi đàn hồi lại)
        textObj.transform.DOPunchScale(targetPunchScale - Vector3.one, duration * 0.5f, 5, 0.5f);

        // Làm mờ dần (Fade out) và Ẩn object đi khi kết thúc
        if (spriteRenderer != null)
        {
            spriteRenderer.DOFade(0f, duration)
                .SetDelay(duration * 0.3f) // Chờ một chút rồi mới bắt đầu mờ đi
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    // Tắt active để trả về pool khi xong hiệu ứng
                    textObj.SetActive(false);
                });
        }
        else
        {
            // Backup trường hợp prefab không có SpriteRenderer thì dùng DOVirtual để delay tắt active
            DOVirtual.DelayedCall(duration, () => textObj.SetActive(false));
        }
    }
}