using TMPro;
using UnityEngine;

public class LiveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI liveText;

    private void Awake()
    {
        liveText = GetComponent<TextMeshProUGUI>();
        LiveManager.Instance.LiveChange += LiveManager_LiveChange;
    }

    private void LiveManager_LiveChange(int live)
    {
        liveText.text = live.ToString();
    }
}
