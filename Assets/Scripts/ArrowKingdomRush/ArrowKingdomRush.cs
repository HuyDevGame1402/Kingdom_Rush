using UnityEngine;

public class ArrowKingdomRush : BaseProjectile
{
    private float arcHeight;
    private float progress = 0f;

    // Ghi đè lại hàm Launch của cha để lấy thêm thông số độ cao cầu vồng
    public void LaunchWithArc(Transform enemy, float arrowSpeed, float arrowArcHeight)
    {
        base.Launch(enemy, arrowSpeed); // Gọi logic cơ bản của cha
        arcHeight = arrowArcHeight;
        progress = 0f;
    }

    protected override void MoveLogic()
    {
        // Tính toán khoảng cách tuyến tính ban đầu để quy ra progress
        float distance = Vector3.Distance(startPosition, targetEnemy.position);
        if (distance > 0)
        {
            progress += (speed / distance) * Time.deltaTime;
        }
        else
        {
            progress = 1f;
        }

        progress = Mathf.Clamp01(progress);

        // Tính Parabol trục Y
        Vector3 currentLinearPosition = Vector3.Lerp(startPosition, targetEnemy.position, progress);
        float heightOffset = Mathf.Sin(progress * Mathf.PI) * arcHeight;
        Vector3 finalPosition = new Vector3(currentLinearPosition.x, currentLinearPosition.y + heightOffset, currentLinearPosition.z);

        // Xoay mũi tên theo hướng bay
        Vector3 moveDirection = finalPosition - transform.position;
        if (moveDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        transform.position = finalPosition;

        if (progress >= 1f)
        {
            OnHitTarget();
        }
    }
}