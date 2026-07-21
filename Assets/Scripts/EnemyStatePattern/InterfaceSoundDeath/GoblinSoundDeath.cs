using UnityEngine;

public class GoblinSoundDeath : MonoBehaviour, IHasSoundDeath
{
    private void Awake()
    {
        transform.GetComponent<EnemyController>().OnEnemyDead += GoblinSoundDeath_OnEnemyDead;       
    }

    public void PlaySoundDeath()
    {
        GoblinSoundDeath_OnEnemyDead();
    }

    private void GoblinSoundDeath_OnEnemyDead()
    {
        if(SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioGoblinDeath();
        }
    }
}
