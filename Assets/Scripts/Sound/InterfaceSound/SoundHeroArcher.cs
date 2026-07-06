using UnityEngine;

public class SoundHeroArcher : MonoBehaviour, ISoundHero
{
    public void PlaySoundHeroAttack()
    {
        if (SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioArrowFly();
        }
    }
}
