using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class MapSelectUIGame : MonoBehaviour
{
    [SerializeField] private List<OnClickButtonChooseMap> listButtonChooseMap = new List<OnClickButtonChooseMap>();

    [SerializeField] private Image imageIcon;
    [SerializeField] private TextMeshProUGUI nameMapTxt;
    [SerializeField] private TextMeshProUGUI typeMapTxt;
    [SerializeField] private Animator bookVersion;
    [SerializeField] private Transform mainUI;
    [Header("UI Refirence Map Version 1")]
    [SerializeField] private TextMeshProUGUI desVersion1;
    [SerializeField] private Transform uiDesMapVersion1;

    private void Start()
    {
        RegisterEventChooseMap();
    }
    private void OnDestroy()
    {
        UnRegisterEventChooseMap();
    }
    private void RegisterEventChooseMap()
    {
        for(int i = 0; i < listButtonChooseMap.Count; i++)
        {
            listButtonChooseMap[i].OnClickChooseMap += MapSelectUIGame_OnClick;
        }
    }
    private void UnRegisterEventChooseMap()
    {
        for (int i = 0; i < listButtonChooseMap.Count; i++)
        {
            listButtonChooseMap[i].OnClickChooseMap -= MapSelectUIGame_OnClick;
        }
    }
    private void MapSelectUIGame_OnClick(LevelData obj)
    {
        if(obj == null) return;
        if(obj.levelType == LevelType.Campaign)
        {
            UpdateMapVersion1(obj);
        }
        mainUI.gameObject.SetActive(true);
    }
    private void UpdateMapVersion1(LevelData obj)
    {
        UpdateUICommonMap(obj.levelName, obj.levelType.ToString(), obj.spriteIcon);
        desVersion1.text = obj.description;
        uiDesMapVersion1.gameObject.SetActive(true);
        if (bookVersion != null)
        {
            StartCoroutine(WaitTimeForAnimationBook());
        }
    }
    private IEnumerator WaitTimeForAnimationBook()
    {
        yield return new WaitForSeconds(0.5f);
        bookVersion.SetTrigger("Up");
    }
    private void UpdateUICommonMap(string mapName, string mapType, Sprite mapIcon)
    {
        nameMapTxt.text = mapName;
        typeMapTxt.text = mapType;
        imageIcon.sprite = mapIcon;
    }

    public void HideMainUI()
    {
        StartCoroutine(WaitTimeForAnimationBookIdle());
    }
    private IEnumerator WaitTimeForAnimationBookIdle()
    {
        bookVersion.SetTrigger("Idle");
        yield return new WaitForSeconds(0.1f);
        mainUI.gameObject.SetActive(false);
    }
}
