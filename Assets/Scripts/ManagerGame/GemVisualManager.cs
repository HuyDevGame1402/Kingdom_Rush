using System.Collections.Generic;
using UnityEngine;

public class GemVisualManager : MonoBehaviour
{
    public static GemVisualManager Instance { get; private set; }

    [Header("Pool Configuration")]
    [SerializeField] private GemItem gemPrefab;
    [SerializeField] private int initialPoolSize = 10;

    private readonly List<GemItem> gemPool = new List<GemItem>();

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

        InitPool();
    }

    /// <summary>
    /// Khởi tạo Object Pool ban đầu với các prefab ẩn
    /// </summary>
    private void InitPool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewPoolItem();
        }
    }

    private GemItem CreateNewPoolItem()
    {
        GemItem gem = Instantiate(gemPrefab, transform);
        gem.gameObject.SetActive(false);
        gemPool.Add(gem);
        return gem;
    }

    /// <summary>
    /// Lấy một GemItem chưa sử dụng từ Pool (Tự động mở rộng nếu thiếu)
    /// </summary>
    private GemItem GetGemFromPool()
    {
        for (int i = 0; i < gemPool.Count; i++)
        {
            if (!gemPool[i].gameObject.activeInHierarchy)
            {
                return gemPool[i];
            }
        }

        // Tự động mở rộng pool nếu lượng Gem hiển thị vượt quá số lượng ban đầu
        return CreateNewPoolItem();
    }

    /// <summary>
    /// Trả GemItem về trạng thái ẩn để tái sử dụng
    /// </summary>
    private void ReturnToPool(GemItem gem)
    {
        gem.gameObject.SetActive(false);
    }

    /// <summary>
    /// Gọi hàm này từ bất kỳ Class tính toán nào để hiển thị hiệu ứng rớt Gem
    /// </summary>
    /// <param name="gemAmount">Số lượng gem hiển thị trên Text (vd: +5)</param>
    /// <param name="spawnPosition">Vị trí xuất hiện trên thế giới (World Position)</param>
    public void SpawnGemVisual(int gemAmount, Vector3 spawnPosition)
    {
        GemItem gemItem = GetGemFromPool();
        gemItem.SetupAndAnimate(gemAmount, spawnPosition, ReturnToPool);
    }
}
