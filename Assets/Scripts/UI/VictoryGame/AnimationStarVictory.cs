using System.Collections.Generic;
using UnityEngine;

public class AnimationStarVictory : MonoBehaviour
{
    [Header("Prefabs & Positions")]
    [SerializeField] private List<Transform> imageStarList = new List<Transform>();
    [SerializeField] private Transform point;
    [SerializeField] private Transform parent;

    [Header("Pool Settings")]
    [SerializeField] private int totalStarsToSpawn = 35;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 40f;           // Bán kính rải nhẹ vị trí xuất phát

    [Header("Parabola Arc Settings")]
    [SerializeField] private float minUpwardY = 200f;           // Độ cao hất lên tối thiểu
    [SerializeField] private float maxUpwardY = 450f;           // Độ cao hất lên tối đa
    [SerializeField] private float minHorizontalX = -350f;      // Tản rộng sang trái
    [SerializeField] private float maxHorizontalX = 350f;       // Tản rộng sang phải

    [Header("Fall Landing Settings")]
    [SerializeField] private float fallDistanceY = 350f;        // Tầm rơi sâu xuống bên dưới
    [SerializeField] private float driftMultiplierX = 1.4f;     // Hệ số quán tính trôi ngang khi rơi (tăng số này nếu muốn sao văng xa hơn nữa)

    [Header("Lifetime Range")]
    [SerializeField] private float minDuration = 1.0f;          // Thời gian sống ngắn nhất
    [SerializeField] private float maxDuration = 2.2f;          // Thời gian sống dài nhất

    private List<StarItem> starPool = new List<StarItem>();

    private void Start()
    {
        InitializeStarPool();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayVictoryStarAnimation();
        }
    }

    private void InitializeStarPool()
    {
        if (imageStarList == null || imageStarList.Count == 0)
        {
            Debug.LogWarning("Chưa gán Prefab Star vào imageStarList!");
            return;
        }

        for (int i = 0; i < totalStarsToSpawn; i++)
        {
            Transform randomPrefab = imageStarList[Random.Range(0, imageStarList.Count)];
            Transform spawnedStar = Instantiate(randomPrefab, parent);

            StarItem starItem = spawnedStar.GetComponent<StarItem>();
            if (starItem == null)
            {
                starItem = spawnedStar.gameObject.AddComponent<StarItem>();
            }

            spawnedStar.gameObject.SetActive(false);
            starPool.Add(starItem);
        }
    }

    public void PlayVictoryStarAnimation()
    {
        Vector2 basePosition = Vector2.zero;

        if (point != null)
        {
            RectTransform pointRect = point.GetComponent<RectTransform>();
            RectTransform parentRect = parent.GetComponent<RectTransform>();

            if (pointRect != null && parentRect != null)
            {
                basePosition = parentRect.InverseTransformPoint(pointRect.position);
            }
        }

        foreach (var star in starPool)
        {
            // 1. Điểm xuất phát (rải rác nhẹ quanh point gốc)
            Vector2 startPos = basePosition + (Random.insideUnitCircle * spawnRadius);

            // 2. Tính toán Đỉnh Parabol (Apex Position)
            float offsetX = Random.Range(minHorizontalX, maxHorizontalX);
            float offsetY = Random.Range(minUpwardY, maxUpwardY);
            Vector2 apexPos = startPos + new Vector2(offsetX, offsetY);

            // 3. Tính toán Điểm Đáp (Landing Position)
            // Tiếp tục cho X trôi theo hướng văng ban đầu (quán tính) và Y rơi xuống
            float landingX = apexPos.x + (offsetX * (driftMultiplierX - 1f));
            float landingY = apexPos.y - fallDistanceY - Random.Range(0f, 150f);
            Vector2 landingPos = new Vector2(landingX, landingY);

            // 4. Thời gian chạy ngẫu nhiên cho từng sao
            float duration = Random.Range(minDuration, maxDuration);

            // 5. Khởi chạy animation
            star.LaunchParabola(startPos, apexPos, landingPos, duration);
        }
    }
}