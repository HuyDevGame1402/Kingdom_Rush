using UnityEngine;
using System.Collections;

public class Dynamite : ThrowableObject
{
    [Header("Dynamite Ice Settings")]
    [SerializeField] private int damage = 250;

    [SerializeField] private TextSO textSO;

    [SerializeField] private float offsetSpawnTextY;
    [SerializeField] private GameObject soilExpansion;
    [SerializeField] private float timeDelayDisable = 1.5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioBom;

    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnHitTarget()
    {
        soilExpansion.SetActive(true);
        if (textSO != null)
        {
            TextSpawnManager.Instance.SpawnText(transform.position + Vector3.up * offsetSpawnTextY,
                textSO.sprites[Random.Range(0, textSO.sprites.Count)]);
        }
        PlayAudioExplosion();
        base.OnHitTarget();
    }

    protected override void FuncOnCompleteAnimationExplosion()
    {
        base.FuncOnCompleteAnimationExplosion();
        StartCoroutine(CoroutineAwaitDisableGameObject());
    }

    private IEnumerator CoroutineAwaitDisableGameObject()
    {
        yield return new WaitForSeconds(timeDelayDisable);
        soilExpansion.SetActive(false);
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag(enemyTag)
            && enemyList.Contains(collision.transform) == false)
        {
            if (collision.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.TakeDamage(damage, null);
            }
            enemyList.Add(collision.transform);
        }
    }

    private void PlayAudioExplosion()
    {
        audioSource.PlayOneShot(audioBom);
    }
}
