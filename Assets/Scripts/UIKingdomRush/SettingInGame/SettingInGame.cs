using UnityEngine;
using UnityEngine.UI;

public class SettingInGame : MonoBehaviour
{
    public static SettingInGame Instance { get; private set; }
    private const string ANIMATION_OPEN = "Open";
    private const string ANIMATION_CLOSE = "Close";
    private const string IMAGEX = "ImageX";
    private Animator animator;

    [SerializeField] private Button buttonMusic;
    [SerializeField] private Button buttonSound;
    [SerializeField] private Button buttonVibration;

    private bool isMusic = true;
    private bool isSound = true;
    private bool isVibration = true;

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        OnRegisterEventOnClickButton();
    }
    
    private void OnRegisterEventOnClickButton()
    {
        buttonMusic.onClick.AddListener(OnClickMusic);
        buttonSound.onClick.AddListener(OnClickSound);
        buttonVibration.onClick.AddListener(OnClickVibration);
    }

    public void OpenSetting()
    {
        if(SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayTickWood();
        }
        animator.SetTrigger(ANIMATION_OPEN);
    }

    public void CloseSetting()
    {
        animator.SetTrigger(ANIMATION_CLOSE);
    }

    private void OnClickMusic()
    {
        if (SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayTickWood();
        }
        isMusic = !isMusic;
        SetImageXInButton(buttonMusic, !isMusic);
        if(MusicInGame.Instance != null) MusicInGame.Instance.SetVolume(isMusic);
    }

    private void OnClickSound()
    {
        isSound = !isSound;
        SetImageXInButton(buttonSound, !isSound);
        if (SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayTickWood();
        }
    }

    private void OnClickVibration()
    {
        if (SoundInGameManager.Instance != null)
        {
            SoundInGameManager.Instance.PlayTickWood();
        }
        isVibration = !isVibration;
        SetImageXInButton(buttonVibration, !isVibration);
    }

    private void SetImageXInButton(Button button, bool isActive)
    {
        button.transform.Find(IMAGEX).gameObject.SetActive(isActive);
    }

    public bool GetIsMusic()
    {
        return isMusic;
    }
    public bool GetIsSound()
    {
        return isSound;
    }   
}
