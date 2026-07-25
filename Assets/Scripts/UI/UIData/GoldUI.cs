using UnityEngine;
using TMPro;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private void Awake()
    {
        goldText = GetComponent<TextMeshProUGUI>();
        GoldManager.Instance.GoldChange += GoldManager_GoldChange;
    }

    private void GoldManager_GoldChange(int gold)
    {
        goldText.text = gold.ToString();
    }
}
