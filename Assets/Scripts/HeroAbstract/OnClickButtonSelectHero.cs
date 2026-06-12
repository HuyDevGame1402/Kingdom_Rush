using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnClickButtonSelectHero : MonoBehaviour
{
    [SerializeField] private HeroData heroSelected;
    [SerializeField] private TextMeshProUGUI price;

    public void SetupValueButton(HeroData heroData)
    {
        heroSelected = heroData;
        if(heroSelected.priceHeroNumber > 0)
        {
            price.text = heroSelected.priceHeroText;
            price.gameObject.SetActive(true);
        }
        else
        {
            price.gameObject.SetActive(false);
        }
        transform.GetComponent<Image>().sprite = heroSelected.imageButtonLock;
    }
}
