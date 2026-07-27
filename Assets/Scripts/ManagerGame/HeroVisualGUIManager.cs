using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HeroVisualGUIManager : MonoBehaviour
{
    public static HeroVisualGUIManager Instance { get; private set; }

    [SerializeField] private Transform playerHero;
    [Header("GUI")]
    [SerializeField] private Transform heroGUI;
    [SerializeField] private Image imageHeroGUI;
    [SerializeField] private Image healthGUI;
    private RectTransform healthRectTransform;
    [SerializeField] private Image expGUI;
    private RectTransform expRectTransform;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    private Vector3 localScaleHealth = Vector3.one;
    private float percentHealth;
    private Vector3 localScaleEXP = Vector3.one;
    private float percentExp;

    private void Awake()
    {
        Instance = this;
        healthRectTransform = healthGUI.GetComponent<RectTransform>();
        expRectTransform = expGUI.GetComponent<RectTransform>();
    }

    public void SetPlayerHero(Transform playerHero)
    {
        this.playerHero = playerHero;
        InitHeroGUI();
        RegisterEventPlayerHero();
    }

    private void InitHeroGUI()
    {
        if (playerHero == null) return;
        heroGUI.gameObject.SetActive(true);
        imageHeroGUI.sprite = playerHero.GetComponent<BaseUnitStateMachine>().unitData.characterGUI;
        healthRectTransform.localScale = localScaleHealth;
        localScaleEXP.x = 0;
        expRectTransform.localScale = localScaleEXP;
    }

    private void RegisterEventPlayerHero()
    {
        if(playerHero.TryGetComponent(out HealthHero healthPlayerHero))
        {
            healthPlayerHero.OnHitDamage += HealthPlayerHero_OnHitDamage;
        }
        if(playerHero.TryGetComponent(out HeroDataInGame heroDataInGame))
        {
            heroDataInGame.OnChangeExpEvent += HeroDataInGame_OnChangeExpEvent;
            heroDataInGame.OnLevelUpEvent += HeroDataInGame_OnLevelUpEvent;
        }
    }

    private void HeroDataInGame_OnLevelUpEvent(int currentLevel)
    {
        currentLevelText.text = currentLevel.ToString();
    }

    private void HeroDataInGame_OnChangeExpEvent(int currentExp, int maxExp)
    {
        percentExp = (float)currentExp / maxExp;
        localScaleEXP.x = percentExp;
        expRectTransform.localScale = localScaleEXP;
    }

    private void HealthPlayerHero_OnHitDamage(int currentHealth, int maxHealth)
    {
        percentHealth = (float)currentHealth / maxHealth;
        localScaleHealth.x = percentHealth;
        healthRectTransform.localScale = localScaleHealth;
    }
}
