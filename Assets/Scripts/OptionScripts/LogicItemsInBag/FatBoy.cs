using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class FatBoy : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] private float flightTime = 1.8f;
    [SerializeField] private float curveStrength = 2.2f;

    public float FlightTime => flightTime;

    private Coroutine dropRoutine;

    [SerializeField] private SpriteRenderer spriteBomb;
    [SerializeField] private GameObject explosionGameObject;
    [SerializeField] private string animationName;
    [SerializeField] private float frameRate;
    [SerializeField] private int startFrame;
    [SerializeField] private int endFrame;
    [SerializeField] private List<EnemyAnimConfig> configsAnimation = new List<EnemyAnimConfig>();
    [SerializeField] private TextSO textSO;
    [SerializeField] private float offsetSpawnTextY;

    private Transform parent;
    private Vector3 offset = new Vector3(0.4f, -0.5300002f, 0f);
    private float rotation = -10.997f;

    private int damage = 3000;

    private void Awake()
    {
        parent = transform.parent;
    }

    public void Drop(Transform target)
    {
        if (dropRoutine != null)
            StopCoroutine(dropRoutine);

        dropRoutine = StartCoroutine(DropRoutine(target));
    }

    private IEnumerator DropRoutine(Transform target)
    {
        Vector3 start = transform.position;
        Vector3 end = target.position;

        float elapsed = 0f;

        while (elapsed < flightTime)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / flightTime);

            // X luôn đi tuyến tính
            float x = Mathf.Lerp(start.x, end.x, t);

            // Đường cong chỉ đi xuống, không bao giờ đi lên
            float curveT = Mathf.Pow(t, curveStrength);

            float y = Mathf.Lerp(start.y, end.y, curveT);

            Vector3 newPos = new Vector3(x, y, start.z);

            // Xoay đầu bom theo hướng di chuyển
            Vector3 dir = newPos - transform.position;

            if (dir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            transform.position = newPos;

            yield return null;
        }

        transform.position = end;

        OnHitTarget();
    }

    private void OnHitTarget()
    {
        StartCoroutine(CoroutineExplosion());
    }

    private IEnumerator CoroutineExplosion()
    {
        spriteBomb.enabled = false;
        transform.rotation = Quaternion.identity;
        explosionGameObject.SetActive(true);
        SpriteSheetAnimator.Instance.PlayAnimation(
        target: explosionGameObject,
        animPrefix: animationName,
        startFrame: startFrame,
        endFrame: endFrame,
        eventFrame: -1,
        onEventTrigger: () => {
            // Gây sát thương ngay tại event frame (ví dụ frame 11)
        },
        offsetConfigs: configsAnimation,
        frameRate: frameRate,
            onComplete: () => {
                explosionGameObject.SetActive(false);
            }
        );
        if (textSO != null)
        {
            TextSpawnManager.Instance.SpawnText(transform.position + Vector3.up * offsetSpawnTextY,
                textSO.sprites[Random.Range(0, textSO.sprites.Count)]);
        }
        yield return new WaitForSeconds(0.3f);
        if (ImageGlareUI.Instance != null) ImageGlareUI.Instance.ShowGlare();
        TakeDamageUpAllEnemy();
        yield return new WaitForSeconds(0.5f);
        if(ExplosionFatBoyDeco.Instance != null)
        {
            ExplosionFatBoyDeco.Instance.PlayExplosion();
        }
        ResetToParent();
    }
    private void ResetToParent()
    {
        transform.SetParent(parent, false);
        transform.localPosition = offset;
        transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
        spriteBomb.enabled = true;
        gameObject.SetActive(false);
    }

    private void TakeDamageUpAllEnemy()
    {
        if (LevelEnemySpawner.Instance != null && LevelEnemySpawner.Instance.enemyInGame.Count > 0)
        {
            for (int i = 0; i < LevelEnemySpawner.Instance.enemyInGame.Count; i++)
            {
                if (LevelEnemySpawner.Instance.enemyInGame[i].TryGetComponent(out EnemyController
                    enemyController))
                {
                    enemyController.TakeDamage(damage, null);
                }
            }
        }
    }
}