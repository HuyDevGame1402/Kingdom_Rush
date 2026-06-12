using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

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
        OnHeroSelected?.Invoke(transform, heroData);
    }
}
