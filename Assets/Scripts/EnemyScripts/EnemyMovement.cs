using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 0.8f;
    public float targetRadius = 0.05f;

    private List<WaypointNode> currentPath;
    private int currentWaypointIndex = 0;
    private bool isMoving = false;

    // Biến lưu trữ hướng mặt hiện tại đọc từ Waypoint
    private MoveDirectionType currentDirection;

    public bool IsMoving => isMoving;
    public MoveDirectionType CurrentDirection => currentDirection;

    private void Start()
    {
        if (WayPointManager.Instance != null && WayPointManager.Instance.wayPointListRoad1.Count > 0)
        {
            SetupPath(WayPointManager.Instance.wayPointListRoad1[0].positionList);
        }
    }

    public void SetupPath(List<WaypointNode> path)
    {
        if (path == null || path.Count == 0) return;

        currentPath = path;
        isMoving = true;

        // NẾU BẢN ĐỒ CHỈ CÓ 1 ĐIỂM (Không hợp lệ)
        if (currentPath.Count < 2)
        {
            currentWaypointIndex = 0;
            currentDirection = currentPath[0].faceDirection;
            transform.position = currentPath[0].position.position;
            return;
        }

        // CHUẨN HÓA LOGIC SPARK:
        // Đặt quái vật đứng ở điểm đầu tiên (Index 0)
        transform.position = currentPath[0].position.position;

        // Nhắm ngay mục tiêu là điểm thứ hai (Index 1) để di chuyển tới
        currentWaypointIndex = 1;

        // SỬA LỖI TẠI ĐÂY: Lấy luôn hướng đi của điểm đích (Index 1) để quái quay mặt đúng ngay từ giây đầu tiên!
        currentDirection = currentPath[currentWaypointIndex].faceDirection;
    }

    private void Update()
    {
        if (!isMoving || currentPath == null || currentPath.Count == 0) return;

        MoveAlongWaypoints();
    }

    private void MoveAlongWaypoints()
    {
        Vector3 targetPosition = currentPath[currentWaypointIndex].position.position;

        // Di chuyển tịnh tiến cơ thể quái
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Kiểm tra xem đã chạm điểm mốc đang nhắm tới chưa
        if (Vector3.Distance(transform.position, targetPosition) <= targetRadius)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex < currentPath.Count)
            {
                // CẬP NHẬT HƯỚNG MỚI: Lấy hướng của điểm đích tiếp theo vừa được cập nhật
                currentDirection = currentPath[currentWaypointIndex].faceDirection;
            }
            else
            {
                isMoving = false;
                Debug.Log("Quái vật đã đi hết bản đồ!");
            }
        }
    }
}