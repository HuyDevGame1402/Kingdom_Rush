using UnityEngine;
using TMPro;

public class DiamonUIRight : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI diamondText;

    private void Start()
    {
        if(PlayerManager.Instance != null)
        {
            UpdateDiamondCount(PlayerManager.Instance.Gems);
            PlayerManager.Instance.OnUpdateGems += UpdateDiamondCount;
        }
    }
    private void OnDestroy()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnUpdateGems -= UpdateDiamondCount;
        }
    }
    public void UpdateDiamondCount(int count)
    {
        if (diamondText != null)
        {
            diamondText.text = count.ToString();
        }
    }
}
