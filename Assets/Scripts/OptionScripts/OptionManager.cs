using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance { get; private set; }

    [SerializeField] private bool isUseSupportOptions = false;

    private IHasLogicOption currentLogicOption;

    [SerializeField] private OnClickUseOption onClickUseOption;

    [Header("Out Path")]
    [SerializeField] private GameObject outPathSprite;
    [SerializeField] private float offsetY;
    private Vector3 positionActive;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(onClickUseOption == null) onClickUseOption = GetComponent<OnClickUseOption>();
        onClickUseOption.OnClick += OnClickUseOption_OnClick;
        onClickUseOption.OnClickOutPath += OnClickUseOption_OnClickOutPath;
        onClickUseOption.enabled = false;
    }

    private void OnClickUseOption_OnClickOutPath(Vector3 position)
    {
        outPathSprite.SetActive(true);
        positionActive = position;
        positionActive.y += offsetY;
        outPathSprite.transform.position = positionActive;
        DecorSpriteAnimator.Instance.PlayAnimation(
            outPathSprite,
            "gui_common",
            "error_feedback",
            1,
            15,
            0.05f,
            () =>
            {
                outPathSprite.SetActive(false);
            });
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
