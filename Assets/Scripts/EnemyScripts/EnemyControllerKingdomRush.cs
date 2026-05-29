using UnityEngine;

public class EnemyControllerKingdomRush : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMovement;
    [SerializeField] private EnemyAnimation enemyAnimation;

    private MoveDirectionType lastDirectionState;
    private bool isFirstFrame = true;


    private void Update()
    {
        HandleAnimationByWaypointData();
    }

    private void HandleAnimationByWaypointData()
    {
        // Nếu quái đứng yên, ép về Idle
        if (!enemyMovement.IsMoving)
        {
            enemyAnimation.PlayAnimationByDirection("Idle", false);
            return;
        }

        // Đọc trực tiếp Enum cấu hình từ điểm Waypoint hiện tại thông qua di chuyển
        MoveDirectionType currentDirState = enemyMovement.CurrentDirection;

        // Chỉ xử lý chuyển đổi Animation khi quái vừa bước qua Waypoint có hướng khác hướng cũ
        if (currentDirState != lastDirectionState || isFirstFrame)
        {
            isFirstFrame = false;
            lastDirectionState = currentDirState;

            string animString = "Down";
            bool flipX = false;

            switch (currentDirState)
            {
                case MoveDirectionType.Walk_Down:
                    animString = "Down";
                    flipX = false;
                    break;
                case MoveDirectionType.Walk_Up:
                    animString = "Up";
                    flipX = false;
                    break;
                case MoveDirectionType.Walk_Right:
                    animString = "Right";
                    flipX = false; // Hướng Right gốc của Sprite Atlas
                    break;
                case MoveDirectionType.Walk_Left:
                    animString = "Right"; // Dùng chung cụm dữ liệu đi ngang (Index 2)
                    flipX = true;  // Kích hoạt lật ngược hình sang bên Trái
                    break;
            }

            // Gọi lệnh ép Animation hiển thị hình ảnh
            enemyAnimation.PlayAnimationByDirection(animString, flipX);
            Debug.Log($"[Controller] Waypoint ép quái đổi hướng thành: {currentDirState}");
        }
    }
}