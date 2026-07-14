using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class ThrowableObject : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] protected Transform visualTransform;
    [SerializeField] private float rotationSpeed = 500f;

    [Header("Movement")]
    [SerializeField] private float duration = 1.2f;
    [SerializeField] private float bounceHeight = 1.5f;

    [Header("Spawn Position")]
    [Tooltip("Khoảng cách ngang từ Spawn đến Target")]
    [SerializeField] private float spawnOffsetX = -2f;

    [Tooltip("Spawn thấp hơn Target bao nhiêu")]
    [SerializeField] private float spawnOffsetY = -0.2f;

    protected Vector3 targetPosition;
    private Vector3 startPosition;

    public Transform test;

    [Header("Explosion")]
    [SerializeField] protected GameObject explosionGameObject;
    [SerializeField] protected string animationExplosion;
    [SerializeField] protected float frameRate;
    [SerializeField] protected int startFrame;
    [SerializeField] protected int endFrame;
    [SerializeField] protected List<EnemyAnimConfig> animationConfigOffset = new List<EnemyAnimConfig>();

    public virtual void InitializeFromSky(Vector3 clickPosition)
    {
        targetPosition = clickPosition;

        // Spawn dưới đất, lệch sang trái Target
        startPosition = new Vector3(
            targetPosition.x + spawnOffsetX,
            targetPosition.y + spawnOffsetY,
            targetPosition.z);

        transform.position = startPosition;

        StartCoroutine(BounceRoutine());
    }

    private IEnumerator BounceRoutine()
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);

            // Di chuyển chậm đầu và cuối
            float progress = Mathf.SmoothStep(0f, 1f, t);

            Vector3 pos = Vector3.Lerp(startPosition, targetPosition, progress);

            // Đường cong nảy
            float arc = 4f * bounceHeight * progress * (1f - progress);
            pos.y += arc;

            transform.position = pos;

            UpdateRotation();

            yield return null;
        }

        transform.position = targetPosition;

        OnHitTarget();
    }

    private void UpdateRotation()
    {
        if (visualTransform != null)
        {
            visualTransform.Rotate(
                Vector3.forward,
                rotationSpeed * Time.deltaTime);
        }
    }

    protected abstract void OnHitTarget();
}