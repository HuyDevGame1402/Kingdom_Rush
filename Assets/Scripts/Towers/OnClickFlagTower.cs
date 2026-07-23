using UnityEngine;

public class OnClickFlagTower : MonoBehaviour
{
    private const string NAMEGAMEOBJECTCIRCLEFLAG = "CircleFlag";

    private void OnMouseDown()
    {
        if(SelectTowerManager.Instance != null && SelectTowerManager.Instance.GetTowerSelected() != null)
        {
            SelectTowerManager.Instance.GetTowerSelected().Find(NAMEGAMEOBJECTCIRCLEFLAG).
                gameObject.SetActive(true);
            SelectTowerManager.Instance.Hide();
        }
    }

}
