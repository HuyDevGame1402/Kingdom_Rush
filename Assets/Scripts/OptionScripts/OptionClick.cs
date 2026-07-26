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

    [SerializeField] protected bool isSelectedOption;

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
        isSelectedOption = false;
    }

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    protected virtual void OnClickButton()
    {
        if (isOnClick == false) return;

        if(isSelectedOption == false)
        {
            if (SoundInGameManager.Instance != null)
            {
                SoundInGameManager.Instance.PlayOnClickOption();
            }
            reinforceUI.UpdateSpriteSelected();
            DisableSelectedOtherButton();
            if (OptionManager.Instance != null && logicOption != null)
            {
                OptionManager.Instance.SetLogicOption(logicOption);
                OptionManager.Instance.SetSupportOptions(true);
            }
            isSelectedOption = true;

            if(MapPathManager.Instance != null)
            {
                MapPathManager.Instance.ActivePolygonCollider2D();
            }

        }
        else
        {
            reinforceUI.UpdateSpriteNormal();
            if (OptionManager.Instance != null && logicOption != null)
            {
                OptionManager.Instance.ResetLogicOption();
                OptionManager.Instance.SetSupportOptions(false);
            }
            isSelectedOption = false;
        }
    }

    private void DisableSelectedOtherButton()
    {
        for(int i = 0; i < reinforceUIs.Count; i++)
        {
            reinforceUIs[i].UpdateSpriteNormal();

            if (reinforceUIs[i].TryGetComponent(out OptionClick optionClick))
            {
                optionClick.ResetIsSelectedOption();
            }
        }
    }

    public void SetOnClick(bool onClick)
    {
        isOnClick = onClick;
    }

    public void ResetIsSelectedOption()
    {
        isSelectedOption = false;
    }
}
