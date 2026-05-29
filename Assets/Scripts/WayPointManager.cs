using System.Collections.Generic;
using UnityEngine;

// Định nghĩa 4 hướng mặt cơ bản
public enum MoveDirectionType
{
    Walk_Down,
    Walk_Up,
    Walk_Right,
    Walk_Left
}

[System.Serializable]
public class WaypointNode
{
    public Transform position; // Vị trí điểm mốc
    public MoveDirectionType faceDirection; // Hướng mặt quái phải quay khi đi tới đây
}

[System.Serializable]
public class WayPoint
{
    // Đổi từ List<Transform> sang List<WaypointNode> để mang theo dữ liệu hướng
    public List<WaypointNode> positionList;
}

public class WayPointManager : MonoBehaviour
{
    public static WayPointManager Instance;

    public List<WayPoint> wayPointListRoad1 = new List<WayPoint>();

    private void Awake()
    {
        Instance = this;
    }
}