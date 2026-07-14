using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Frozotov : ThrowableObject
{
    [Header("Frozotov Ice Settings")]
    [SerializeField] private float slowDuration = 5.0f;

    [SerializeField] private CircleCollider2D circleCollider;
    private const string enemyTag = "EnemyKingdomRush";
    [SerializeField] private List<Transform> enemyList = new List<Transform>();

    protected override void OnHitTarget()
    {
        StartCoroutine(CoroutineTriggerEnemy());
        // Hiệu ứng đặc trưng của Frozotov: Vỡ ra tạo vùng băng giá làm chậm kẻ địch
        Debug.Log("Frozotov vỡ vụn! Tạo vùng làm chậm.");
        visualTransform.gameObject.SetActive(false);
        SpriteSheetAnimator.Instance.PlayAnimation(
        target:explosionGameObject,
        animPrefix: animationExplosion,
        startFrame: startFrame,
        endFrame: endFrame,
        eventFrame: -1,
        onEventTrigger: () => {
            // Gây sát thương ngay tại event frame (ví dụ frame 11)
        },
        offsetConfigs: animationConfigOffset,
        frameRate: frameRate,
            onComplete: () => {
                gameObject.SetActive(false);
            }
        );
    }

    private IEnumerator CoroutineTriggerEnemy()
    {
        circleCollider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        circleCollider.enabled = false;
        enemyList.Clear();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag(enemyTag)
            && enemyList.Contains(collision.transform) == false)
        {
            if(collision.TryGetComponent(out CharacterFreezing characterFreezing))
            {
                characterFreezing.StartFreezeStatus(slowDuration);
            }
            enemyList.Add(collision.transform);
        }
    }
}