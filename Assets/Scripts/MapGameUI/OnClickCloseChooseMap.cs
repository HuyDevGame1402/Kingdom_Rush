using UnityEngine;
using UnityEngine.UI;

public class OnClickCloseChooseMap : MonoBehaviour
{
    [SerializeField] private MapSelectUIGame mapSelectUIGame;
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(HideMapSelectGame);
    }
    private void HideMapSelectGame()
    {
        if(mapSelectUIGame != null) mapSelectUIGame.HideMainUI();
    }

}
