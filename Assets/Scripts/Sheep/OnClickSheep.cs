using UnityEngine;

public class OnClickSheep : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (SoundGameAttackManager.Instance != null) SoundGameAttackManager.Instance.PlayAudioSheep();
    }
}
