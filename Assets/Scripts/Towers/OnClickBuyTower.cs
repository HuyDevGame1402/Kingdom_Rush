using UnityEngine;
using System;

public class OnClickBuyTower : MonoBehaviour
{
    public Transform tower;
    private int priceBuyTower;
    private bool confirmSale = false;

    [SerializeField] private Sprite spriteTick;
    [SerializeField] private Sprite spriteMoney;
    private SpriteRenderer spriteRender;

    [Range(0, 1f)]
    [SerializeField] private float percentageLoss = 0.2f;

    public event Action<int, Transform> OnBuyTowerShow;
    public event Action OnBuyTowerHide;

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        priceBuyTower = CalculateSellingPrice();
        if (confirmSale == false)
        {
            confirmSale = true;
            OnBuyTowerShow?.Invoke(priceBuyTower, tower.GetComponent<TowerLevelUp>().groundTower);
            spriteRender.sprite = spriteTick;
            if (SoundInGameManager.Instance != null)
            {
                SoundInGameManager.Instance.PlayMouseOverTowerIcon();
            }
        }
        else
        {
            if (GameManager.Instance == null || GameManager.Instance.currentGameState
            == GameManager.GameState.Instruction || GameManager.Instance.
            currentGameState == GameManager.GameState.FinishLevel || GoldManager.Instance == null
            || tower == null) return;

            SetupSpriteMoney();
            GoldManager.Instance.AddGold(priceBuyTower);
            tower.GetComponent<TowerLevelUp>().groundTower.GetComponent<BuildPlot>().EnableCapsualCollider();
            tower.gameObject.SetActive(false);
            tower = null;
            confirmSale = false;
            OnBuyTowerHide?.Invoke();
            if (SoundInGameManager.Instance != null)
            {
                SoundInGameManager.Instance.PlayTowerSell();
            }
            if (SelectTowerManager.Instance != null)
            {
                SelectTowerManager.Instance.DisableViewTower();
            }
        }
    }

    public void SetupSpriteMoney()
    {
        if (spriteRender == null) return;
        spriteRender.sprite = spriteMoney;
    }

    public void SetConfirmSale(bool confirmSale)
    {
        this.confirmSale = confirmSale;
    }

    private int CalculateSellingPrice()
    {
        if (tower == null) return 0;
        if (GameManager.Instance.currentGameState == GameManager.GameState.Prepare)
        {
            return tower.GetComponent<TowerLevelUp>().GetCurrentBaseTowerSO().priceBuyTower;
        }
        else if (GameManager.Instance.currentGameState == GameManager.GameState.Playing)
        {
            return tower.GetComponent<TowerLevelUp>().GetCurrentBaseTowerSO().priceBuyTower -= (int)(percentageLoss * priceBuyTower);
        }
        return 0;
    }
}
