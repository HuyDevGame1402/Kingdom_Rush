using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New TextSO", menuName = "ScriptableObjects/TextSO")]
public class TextSO : ScriptableObject
{
    public int textId;
    public List<Sprite> sprites;
}
