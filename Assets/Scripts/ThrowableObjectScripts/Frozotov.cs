using UnityEngine;

public class Frozotov : ThrowableObject
{
    [Header("Frozotov Ice Settings")]
    [SerializeField] private float slowDuration = 5.0f;


    protected override void FuncOnCompleteAnimationExplosion()
    {
        base.FuncOnCompleteAnimationExplosion();
        gameObject.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
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