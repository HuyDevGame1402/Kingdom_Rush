using UnityEngine;

public class SoundHeroMage : MonoBehaviour, ISoundHero
{
    public void PlaySoundHeroAttack()
    {
        if (SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioMageShoot();
        }
    }
}
