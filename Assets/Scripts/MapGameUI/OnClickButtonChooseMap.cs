using UnityEngine;
using System;
using UnityEngine.UI;

public class OnClickButtonChooseMap : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    public event Action<LevelData> OnClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickChooseMapGame);
    }

    private void OnClickChooseMapGame()
    {
        OnClick?.Invoke(levelData);
    }
}
