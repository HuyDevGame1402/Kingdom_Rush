using UnityEngine;

public class HeroBaseSoundAttack : MonoBehaviour, IHasSoundAttack
{
    public void PlaySoundAttack()
    {
        if (SoundGameAttackManager.Instance != null)
        {
            SoundGameAttackManager.Instance.PlayAudioSoliderAttack();
        }
    }
}
