using UnityEngine;
using UnityEngine.UI;
using System;

public class OnClickButtonChooseHero : MonoBehaviour
{
    public HeroData heroData;

    public Action<Transform,HeroData> OnHeroSelected;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }
    private void OnButtonClicked()
    {
        if(SoundMenuGameManager.Instance != null)
        {
            SoundMenuGameManager.Instance.PlayAudioSourceClickUpgradesAndHeroRoom();
        }

        OnHeroSelected?.Invoke(transform, heroData);
    }
}
