using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAnimation : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("GameObject chứa component Image của con thuyền")]
    [SerializeField] private GameObject boatImageObject;

    [Header("Movement Points")]
    [Tooltip("Điểm xuất phát UI (RectTransform hoặc Transform trống)")]
    [SerializeField] private Transform point1;
    [Tooltip("Điểm đích đến UI (RectTransform hoặc Transform trống)")]
    [SerializeField] private Transform point2;

    [Header("Settings")]
    [Tooltip("Tốc độ khung hình của Animation")]
    [SerializeField] private float frameRate = 12f;
    [Tooltip("Thời gian tối đa (giây) dự kiến để thuyền bơi từ Điểm 1 đến Điểm 2")]
    [SerializeField] private float travelDuration = 4f;
    [Tooltip("Thời gian chờ trước khi lặp lại vòng tuần hoàn")]
    [SerializeField] private float respawnDelay = 5f;

    private RectTransform boatRectTransform;

    private void Start()
    {
        if (boatImageObject == null || point1 == null || point2 == null)
        {
            Debug.LogError("[BoatAnimation] Vui lòng gán đầy đủ Boat Object và các điểm Point 1, Point 2!");
            return;
        }

        boatRectTransform = boatImageObject.GetComponent<RectTransform>();

        // Bắt đầu chuỗi hành động tuần hoàn ngay khi Start game
        StartCoroutine(BoatRoutineLoop());
    }

    /// <summary>
    /// Coroutine quản lý toàn bộ vòng lặp hoạt động của thuyền
    /// </summary>
    private IEnumerator BoatRoutineLoop()
    {
        while (true)
        {
            // === GIAI ĐOẠN 1: CHUẨN BỊ XUẤT PHÁT ===
            // Đặt thuyền về vị trí Điểm 1 ban đầu và hiển thị Image lên
            boatRectTransform.position = point1.position;
            boatImageObject.SetActive(true);

            // Cờ đánh dấu để kiểm tra xem quá trình Anim 01->48 đã xong chưa
            bool isStage1Finished = false;

            // Gọi chạy Animation di chuyển (Frame 1 đến 48)
            MapDecoAnimationManager.Instance.PlayAnimation(
                animName: "mapDeco_ship2",
                targetObject: boatImageObject,
                frameRate: frameRate,
                loop: false,
                startFrame: 1,
                endFrame: 48,
                onComplete: () => {
                    // Ngay khi kết thúc animation 01->48, bật cờ để ngắt di chuyển ngay lập tức
                    Debug.Log("[BoatAnimation] Animation bơi (01-48) đã xong! Ngắt di chuyển và chuyển sang lặn.");
                    isStage1Finished = true;
                }
            );

            // Tiến hành di chuyển tịnh tiến thuyền từ Điểm 1 hướng về Điểm 2
            float elapsedTime = 0f;
            Vector3 startPos = point1.position;
            Vector3 endPos = point2.position;

            // Vòng lặp di chuyển này sẽ dừng khi: Hết thời gian di chuyển HOẶC Animation 01-48 kết thúc trước
            while (elapsedTime < travelDuration && !isStage1Finished)
            {
                elapsedTime += Time.deltaTime;

                // Cập nhật vị trí tịnh tiến mượt mà
                boatRectTransform.position = Vector3.Lerp(startPos, endPos, elapsedTime / travelDuration);
                yield return null;
            }

            // Đảm bảo dừng hẳn mọi tiến trình chờ của Giai đoạn 1
            while (!isStage1Finished)
            {
                yield return null;
            }


            // === GIAI ĐOẠN 2: CHUYỂN QUA ANIMATION LẶN / SÓNG CUỘN (49-99) ===
            bool isStage2Finished = false;

            // Gọi chạy Animation hành động lặn (Frame 49 đến 99) tại vị trí hiện tại, không di chuyển nữa
            MapDecoAnimationManager.Instance.PlayAnimation(
                animName: "mapDeco_ship2",
                targetObject: boatImageObject,
                frameRate: frameRate,
                loop: false,
                startFrame: 49,
                endFrame: 99,
                onComplete: () => {
                    // Khi chạy hết hành động lặn, tiến hành dọn dẹp
                    Debug.Log("[BoatAnimation] Đã chạy xong Animation lặn (49-99). Ẩn thuyền.");

                    // Ẩn Image thuyền đi
                    boatImageObject.SetActive(false);

                    // Stop hẳn animation để giải phóng bộ nhớ
                    MapDecoAnimationManager.Instance.StopAnimation(boatImageObject);

                    isStage2Finished = true;
                }
            );

            // Chờ cho đến khi giai đoạn lặn kết thúc hoàn toàn
            while (!isStage2Finished)
            {
                yield return null;
            }


            // === GIAI ĐOẠN 3: HOÃN 5 GIÂY VÀ LẶP LẠI ===
            Debug.Log($"[BoatAnimation] Chờ {respawnDelay} giây trước khi tạo lại thuyền...");
            yield return new WaitForSeconds(respawnDelay);
        }
    }
}