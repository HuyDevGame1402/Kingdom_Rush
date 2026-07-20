using UnityEngine;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance { get; private set; }

    [SerializeField] private bool isUseSupportOptions = false;

    private IHasLogicOption currentLogicOption;

    [SerializeField] private OnClickUseOption onClickUseOption;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(onClickUseOption == null) onClickUseOption = GetComponent<OnClickUseOption>();
        onClickUseOption.OnClick += OnClickUseOption_OnClick;
        onClickUseOption.enabled = false;
    }

    private void OnClickUseOption_OnClick(Vector3 pos)
    {
        if(currentLogicOption != null)
        {
            currentLogicOption.Execute(pos);
            currentLogicOption = null;
            SetSupportOptions(false);
        }
    }

    public void SetSupportOptions(bool isUseSupportOptions)
    {
        this.isUseSupportOptions = isUseSupportOptions;
        if (isUseSupportOptions)
        {
            onClickUseOption.enabled = true;
        }
        else
        {
            onClickUseOption.enabled = false;
        }
    }

    public void SetLogicOption(IHasLogicOption logicOption)
    {
        currentLogicOption = logicOption;
    }
    public void ResetLogicOption()
    {
        currentLogicOption = null;
    }
}
