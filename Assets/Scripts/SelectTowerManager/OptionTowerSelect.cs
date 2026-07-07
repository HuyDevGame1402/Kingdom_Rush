using UnityEngine;

public class OptionTowerSelect : MonoBehaviour
{
    private OnClickChooseTower onClickChooseTower;
    private void OnEnable()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            onClickChooseTower = transform.GetChild(i).GetComponent<OnClickChooseTower>();
            if (onClickChooseTower.GetTowerSO()
                .priceTower > GoldManager.Instance.GetGold())
            {
                onClickChooseTower.GetComponent<SpriteRenderer>().sprite = onClickChooseTower.GetTowerSO().towerIconGray;
            }
            else
            {
                onClickChooseTower.GetComponent<SpriteRenderer>().sprite = onClickChooseTower.GetTowerSO().towerIcon;

            }
        }
    }
}
