using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class HeroUIGame : MonoBehaviour
{
    [SerializeField] private List<OnClickButtonChooseHero> buttonList = new List<OnClickButtonChooseHero>();

    [Header("UI Reference")]
    [SerializeField] private Transform buttonHeroChoose;

    [Header("UI Description Hero")]
    [SerializeField] private Image imageHero;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Transform parameterHealth;
    [SerializeField] private Transform parameterAttack;
    [SerializeField] private Transform parameterArchery;
    [SerializeField] private Transform parameterSpeed;
    [SerializeField] private Sprite spriteYellow;
    [SerializeField] private Sprite spriteBrown;
    [SerializeField] private Color white;
    [SerializeField] private Color brown;
    [SerializeField] private OnClickButtonSelectHero buttonSelectHero;


    private void Start()
    {
        RegisterEvent();
    }

    private void OnDestroy()
    {
        UnRegisterEvent();
    }

    private void RegisterEvent()
    {
        for(int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].OnHeroSelected += OnHeroSelected;
        }
    }
    private void UnRegisterEvent()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].OnHeroSelected -= OnHeroSelected;
        }
    }

    private void OnHeroSelected(Transform buttonTransform, HeroData heroData)
    {
        if(buttonHeroChoose != null)
        {
            // ẩn đi image selected
            buttonHeroChoose.GetChild(0).gameObject.SetActive(false);
        }
        buttonHeroChoose = buttonTransform;
        buttonHeroChoose.GetChild(0).gameObject.SetActive(true); // hiển thị image selected hero
        imageHero.sprite = heroData.heroUISelect;
        descriptionText.text = heroData.description;
        SetupParameterUI(heroData);
        buttonSelectHero.SetupValueButton(heroData);
    }

    private void SetupParameterUI(HeroData heroData)
    {
        SetUpParameter(heroData.health, parameterHealth);
        SetUpParameter(heroData.attackMelee, parameterAttack);
        SetUpParameter(heroData.attackRanged, parameterArchery);
        SetUpParameter(heroData.speed, parameterSpeed);
    }

    private void SetUpParameter(int value, Transform parameterParent)
    {
        for(int i = 0; i < parameterParent.childCount; i++)
        {
            if(i < value)
            {
                UpdateParameterImageUI(parameterParent, i, spriteYellow, white);
            }
            else
            {
                UpdateParameterImageUI(parameterParent, i, spriteBrown, brown);
            }
        } 
    }
    private void UpdateParameterImageUI(Transform paramaterParent, int index,
        Sprite spriteImage, Color color)
    {
        paramaterParent.GetChild(index).GetComponent<Image>().sprite = spriteImage;
        paramaterParent.GetChild(index).GetComponent<Image>().color = color;
    }
}
