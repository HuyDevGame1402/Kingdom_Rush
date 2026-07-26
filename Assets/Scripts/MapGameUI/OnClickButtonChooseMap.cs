using UnityEngine;
using System;
using UnityEngine.UI;

public class OnClickButtonChooseMap : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    public event Action<LevelData> OnClickChooseMap;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickChooseMapGame);
    }

    private void OnClickChooseMapGame()
    {

        if(SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceClickButton();
        }

        OnClickChooseMap?.Invoke(levelData);
    }
}
