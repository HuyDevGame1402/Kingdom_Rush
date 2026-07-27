using UnityEngine;
using UnityEngine.UI;

public class OnClickChooseHeroInGame : MonoBehaviour
{
    private Button buttonHeroGUI;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private OnClickMovePlayerHero onClickMovePlayerHero;
    private bool isSelected;

    private void Awake()
    {
        buttonHeroGUI = GetComponent<Button>();
        buttonHeroGUI.onClick.AddListener(OnClickChooseHero);
        onClickMovePlayerHero.OnClickMoveToFlag += ResetSelected;
    }

    private void ResetSelected(Vector3 pos)
    {
        isSelected = false;
        selectedGameObject.SetActive(false);
        onClickMovePlayerHero.enabled = false;
    }

    private void OnClickChooseHero()
    {
        isSelected = !isSelected;
        if(isSelected)
        {
            if(MapPathManager.Instance != null)
            {
                MapPathManager.Instance.ActivePolygonCollider2D();
            }
            onClickMovePlayerHero.enabled = true;
            selectedGameObject.SetActive(true);
        }
        else
        {
            if (MapPathManager.Instance != null)
            {
                MapPathManager.Instance.DisablePolygonCollider2D();
            }
            selectedGameObject.SetActive(false);
        }
    }
}
