using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class OptionClick : MonoBehaviour
{
    [SerializeField] private List<OptionUI> reinforceUIs = new List<OptionUI>();

    private Button button;
    private OptionUI reinforceUI;

    [SerializeField] private bool isOnClick;

    [SerializeField] private IHasLogicOption logicOption;

    [SerializeField] private ReduceUITime reduceUITime;

    private void Awake()
    {
        button = GetComponent<Button>();
        reinforceUI = GetComponent<OptionUI>();
        TryGetComponent(out logicOption);
        if(reduceUITime != null)
        {
            reduceUITime.OnReduceFinish += ReduceUITime_OnReduceFinish;
        }
    }

    private void ReduceUITime_OnReduceFinish()
    {
        isOnClick = true;
    }

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        if (isOnClick == false) return;
        reinforceUI.UpdateSpriteSelected();
        DisableSelectedOtherButton();
        if(OptionManager.Instance != null && logicOption != null)
        {
            OptionManager.Instance.SetLogicOption(logicOption);
            OptionManager.Instance.SetSupportOptions(true);
        }
    }

    private void DisableSelectedOtherButton()
    {
        for(int i = 0; i < reinforceUIs.Count; i++)
        {
            reinforceUIs[i].UpdateSpriteNormal();
        }
    }

    public void SetOnClick(bool onClick)
    {
        isOnClick = onClick;
    }
}
