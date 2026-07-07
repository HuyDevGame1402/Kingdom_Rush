using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class TowerInfoBuy : MonoBehaviour
{
    [SerializeField] private TextMeshPro des;

    [SerializeField] private List<OnClickBuyTower> onClickBuyTowers = new List<OnClickBuyTower>();

    public float offsetXLeft = -3.5f;
    public float offsetXRight = 3.5f;
    public float offset = 5f;
    private Vector3 leftPoint;
    private Vector3 rightPoint;
    private Vector3 centerPoint;

    private void Start()
    {
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        for(int i = 0; i < onClickBuyTowers.Count; i++)
        {
            onClickBuyTowers[i].OnBuyTowerShow += TowerInfoBuy_OnBuyTowerShow;
            onClickBuyTowers[i].OnBuyTowerHide += TowerInfoBuy_OnBuyTowerHide;
        }
    }

    private void TowerInfoBuy_OnBuyTowerHide()
    {
        ActiveChildCount(false);
    }

    private void TowerInfoBuy_OnBuyTowerShow(int priceBuyTower, Transform ground)
    {
        des.text = "sell this tower and get a " + priceBuyTower.ToString() + " GP refund";
        ActiveChildCount(true);
        Debug.LogWarning(ground.name);
        centerPoint = ground.position + Vector3.right * offset;
        leftPoint = centerPoint + Vector3.right * offsetXLeft;
        rightPoint = centerPoint + Vector3.right * offsetXRight;
        if(IsInCamera(rightPoint) && IsInCamera(leftPoint) && IsInCamera(centerPoint))
        {
            transform.position = centerPoint;
        }
        else
        {
            transform.position = ground.position + Vector3.right * -offset;
        }
    }

    public void ActiveChildCount(bool isActive)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }
    public bool IsInCamera(Vector3 worldPos)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(worldPos);

        return viewportPos.z > 0 &&
               viewportPos.x >= 0 && viewportPos.x <= 1 &&
               viewportPos.y >= 0 && viewportPos.y <= 1;
    }
}
