using UnityEngine;

public class OrcSoundDeath : MonoBehaviour, IHasSoundDeath
{
    private void Awake()
    {
        transform.GetComponent<EnemyController>().OnEnemyDead += OrcSoundDeath_OnEnemyDead;
    }

    public void PlaySoundDeath()
    {
        OrcSoundDeath_OnEnemyDead();
    }

    private void OrcSoundDeath_OnEnemyDead()
    {
        if (SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioOrcDeath();
        }
    }
}
