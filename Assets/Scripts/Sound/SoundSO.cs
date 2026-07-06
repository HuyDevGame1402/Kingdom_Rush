using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObjects/SoundSO")]
public class SoundSO : ScriptableObject
{
    public List<AudioClip> audioClips = new List<AudioClip>();
}
