using UnityEngine;

public abstract class BaseProjectile : MonoBehaviour
{
    protected Transform targetEnemy;
    protected Vector3 startPosition;
    protected float speed;
    protected bool isFlying = false;

    public bool isParabol = true;

    // Hàm khởi tạo chung mà Tháp nào cũng gọi được
    public virtual void Launch(Transform enemy, float projectileSpeed)
    {
        if (enemy == null)
        {
            gameObject.SetActive(false);
            return;
        }

        targetEnemy = enemy;
        startPosition = transform.position;
        speed = projectileSpeed;
        isFlying = true;
        gameObject.SetActive(true);
    }

    protected virtual void Update()
    {
        if (!isFlying) return;

        if (targetEnemy == null)
        {
            OnTargetLost();
            return;
        }

        MoveLogic();
    }

    // Mỗi loại đạn sẽ tự viết cách bay của riêng mình ở đây (Tính đa hình)
    protected abstract void MoveLogic();

    // Logic khi quái chết giữa đường
    protected virtual void OnTargetLost()
    {
        isFlying = false;
        gameObject.SetActive(false);
    }

    // Logic chung khi trúng mục tiêu
    protected virtual void OnHitTarget()
    {
        isFlying = false;
        Debug.Log($"<color=yellow>[Projectile]</color> Đã bắn trúng: {targetEnemy.name}");
        // Xử lý trừ máu quái tại đây...

        gameObject.SetActive(false); // Ẩn đi để tối ưu Object Pooling
    }
}